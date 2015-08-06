using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using ESBasic.Helpers;

namespace ESBasic.Emit.DynamicDispatcher
{
    /// <summary>
    /// DynamicDispatcherEmitter 用于发射中转调用的分发器类型。
    /// 发射得到的动态类型的构造参数将接收参数：IExecuterProvider<TIDispatch>
    /// 动态类型将实现TIDispatch接口，并将所有针对该接口方法的调用全部转发给IExecuterProvider.GetExecuters()方法返回的TIDispatch对象。
    /// zhuweisky 2010.04.02
    /// </summary>
    public class DynamicDispatcherEmitter
    {
        private bool saveFile = false;
        private string assemblyName = "DynamicDispatcherAssembly";
        private string theFileName;
        private AssemblyBuilder dynamicAssembly;
        protected ModuleBuilder moduleBuilder;
        private IDictionary<Type, Type> dicDispatcherType = new Dictionary<Type, Type>(); //typeOfIDispatcher -- DispatcherType      

        #region Ctor
        public DynamicDispatcherEmitter() :this(false)
        {
        }

        public DynamicDispatcherEmitter(bool save)
        {
            this.saveFile = save;
            this.theFileName = this.assemblyName + ".dll";

            AssemblyBuilderAccess assemblyBuilderAccess = this.saveFile ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run;
            this.dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(this.assemblyName), assemblyBuilderAccess);
            if (this.saveFile)
            {
                this.moduleBuilder = dynamicAssembly.DefineDynamicModule("MainModule", this.theFileName);
            }
            else
            {
                this.moduleBuilder = dynamicAssembly.DefineDynamicModule("MainModule");
            }
        } 
        #endregion 

        #region CreateDispatcherType          
        public Type CreateDispatcherType(Type typeOfIDispatcher)
        {
            lock (this.dicDispatcherType)
            {
                if (this.dicDispatcherType.ContainsKey(typeOfIDispatcher))
                {
                    return this.dicDispatcherType[typeOfIDispatcher];
                }

                Type orMappingType = this.EmitDispatcherType(typeOfIDispatcher);
                this.dicDispatcherType.Add(typeOfIDispatcher, orMappingType);

                return orMappingType;
            }
        }
        #endregion

        #region EmitDispatcherType      

        public Type EmitDispatcherType(Type typeOfIDispatcher)
        {
            string typeName = string.Format("{0}_DynamicDispatcher", TypeHelper.GetClassSimpleName(typeOfIDispatcher));
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);
            typeBuilder.AddInterfaceImplementation(typeOfIDispatcher);
            //定义成员           
            Type executersType = typeof(IEnumerable<>).MakeGenericType(typeOfIDispatcher);
            FieldBuilder executersField = typeBuilder.DefineField("executers", executersType, FieldAttributes.Private);

            #region Emit Ctor
            ConstructorBuilder ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { executersType });
            ILGenerator ctorGen = ctor.GetILGenerator();
            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[] { })); //调用基类的构造函数
            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Ldarg_1);
            ctorGen.Emit(OpCodes.Stfld, executersField);

            ctorGen.Emit(OpCodes.Ret);
            #endregion
          
            MethodInfo getEnumeratorMethod = typeof(IEnumerable<>).MakeGenericType(typeOfIDispatcher).GetMethod("GetEnumerator");
            MethodInfo getCurrentMethod = typeof(IEnumerator<>).MakeGenericType(typeOfIDispatcher).GetMethod("get_Current");
            MethodInfo moveNextMethod = typeof(IEnumerator).GetMethod("MoveNext");
            MethodInfo disposeMethod = typeof(IDisposable).GetMethod("Dispose");

            #region 实现IDispatcher接口
            foreach (MethodInfo baseMethod in ESBasic.Helpers.ReflectionHelper.GetAllMethods(typeOfIDispatcher))
            {
                Type[] argTypes = EmitHelper.GetParametersType(baseMethod);
                MethodBuilder methodBuilder = EmitHelper.DefineDerivedMethodSignature(typeBuilder, baseMethod);

                ILGenerator methodGen = methodBuilder.GetILGenerator();
                LocalBuilder enumeratorLocalBuilder = methodGen.DeclareLocal(typeof(IEnumerator<>).MakeGenericType(typeOfIDispatcher));
                LocalBuilder executorLocalBuilder = methodGen.DeclareLocal(typeOfIDispatcher);
                Label continueLabel = methodGen.DefineLabel();
                Label moveNextLabel = methodGen.DefineLabel();
                Label retLabel = methodGen.DefineLabel();
                Label endFinallyLabel = methodGen.DefineLabel();

                methodGen.Emit(OpCodes.Ldarg_0);
                methodGen.Emit(OpCodes.Ldfld, executersField);               
                methodGen.Emit(OpCodes.Callvirt, getEnumeratorMethod);
                methodGen.Emit(OpCodes.Stloc, enumeratorLocalBuilder);

                //开始try块
                Label beginExceptionLabel = methodGen.BeginExceptionBlock();

                methodGen.Emit(OpCodes.Br, moveNextLabel);
                methodGen.MarkLabel(continueLabel);
                methodGen.Emit(OpCodes.Ldloc, enumeratorLocalBuilder);
                methodGen.Emit(OpCodes.Callvirt, getCurrentMethod);
                methodGen.Emit(OpCodes.Stloc, executorLocalBuilder);
                methodGen.Emit(OpCodes.Ldloc, executorLocalBuilder);

                int coventIndex = 0;
                foreach (ParameterInfo pi in baseMethod.GetParameters())
                {
                    EmitHelper.LoadArg(methodGen, coventIndex + 1);
                    coventIndex++;
                }

                methodGen.Emit(OpCodes.Callvirt, baseMethod);
                methodGen.Emit(OpCodes.Nop);

                methodGen.MarkLabel(moveNextLabel);
                methodGen.Emit(OpCodes.Ldloc, enumeratorLocalBuilder);
                methodGen.Emit(OpCodes.Callvirt, moveNextMethod);
                methodGen.Emit(OpCodes.Brtrue, continueLabel);
                methodGen.Emit(OpCodes.Leave, retLabel);

                //开始finally块
                methodGen.BeginFinallyBlock();

                methodGen.Emit(OpCodes.Ldloc, enumeratorLocalBuilder);
                methodGen.Emit(OpCodes.Ldnull);
                methodGen.Emit(OpCodes.Ceq);
                methodGen.Emit(OpCodes.Brtrue, endFinallyLabel);
                methodGen.Emit(OpCodes.Ldloc, enumeratorLocalBuilder);
                methodGen.Emit(OpCodes.Callvirt, disposeMethod);
                methodGen.Emit(OpCodes.Nop);
                methodGen.MarkLabel(endFinallyLabel);
                //结束try...finally块
                methodGen.EndExceptionBlock();
                methodGen.Emit(OpCodes.Nop);

                methodGen.MarkLabel(retLabel);
                methodGen.Emit(OpCodes.Ret);

                typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);
            }
            #endregion

            Type target = typeBuilder.CreateType();
            return target;
        }        
        #endregion

        #region Save
        public void Save()
        {
            if (this.saveFile)
            {
                this.dynamicAssembly.Save(theFileName);
            }
        }
        #endregion
    }
}
