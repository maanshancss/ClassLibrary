using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using ESBasic.Helpers;

namespace ESBasic.Emit.DynamicProxy
{
    /// <summary>
    /// EfficientAopProxyEmitter ���ɵĶ�̬�������ͽ��̳�TInterface�ӿڣ����Ҷ�̬������һ������ΪoriginType���������     
    /// ��AopProxyEmitter��ȣ���Ҫ��EfficientAopProxyEmitter���ɵĶ�̬������ֱ�ӵ��ñ��ػ�ķ����ģ���AopProxyEmitter���ɵĶ�̬������ͨ��������ñ��ػ�ķ����ġ�
    /// ע�⣺
    /// (1)��̬�������TInterface�ӿڵ�����ʵ�ֽ�ת������Ctor�����originTypeʵ����ɡ�originType������public���εġ�
    /// (2)TInterfaceֻ֧�ַǷ��ͽӿڣ����ӿ��п��԰������ͷ�����
    /// (3)֧�ַ�����ref/out������   
    /// zhuweisky ���һ������2007.08.02
    /// </summary>
    public class EfficientAopProxyEmitter //:BaseProxyEmitter
    {
        private bool saveFile = false;
        private string assemblyName = "DynamicProxyAssembly";
        private string theFileName;
        private AssemblyBuilder dynamicAssembly;
        protected ModuleBuilder moduleBuilder;
        private IDictionary<string, Type> proxyTypeDictionary = new Dictionary<string, Type>();//DynamicTypeName -- DynamicType

        #region Ctor
        public EfficientAopProxyEmitter() :this(false)
        {
        }

        public EfficientAopProxyEmitter(bool save)
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

        #region EmitProxyType
        public Type EmitProxyType(Type interfaceType, Type originType)
        {
            if (!interfaceType.IsInterface)
            {
                throw new Exception("TInterface must be interface type !");
            }

            if (interfaceType.ContainsGenericParameters)
            {
                throw new Exception("TInterface can't be generic !");
            }

            string dynamicTypeName = this.GetDynamicTypeName(interfaceType, originType);
            if (this.proxyTypeDictionary.ContainsKey(dynamicTypeName))
            {
                return this.proxyTypeDictionary[dynamicTypeName];
            }

            Type target = this.DoEmitProxyType(interfaceType, originType);
            this.proxyTypeDictionary.Add(dynamicTypeName, target);

            return target;
        } 

        public Type EmitProxyType<TInterface>(Type originType)
        {
            Type interfaceType = typeof(TInterface);
            return this.EmitProxyType(interfaceType, originType);
        } 
        #endregion

        #region GetDynamicTypeName
        /// <summary>
        /// ��ȡҪ��̬���ɵ����͵����ơ�ע�⣬����һ��Ҫʹ�ñ��������õ���̬���͵����ơ�
        /// </summary>    
        private string GetDynamicTypeName(Type interfaceType, Type originType)
        {
            return string.Format("{0}.{1}_{2}_EfficientAopProxy", this.assemblyName, originType.ToString(), interfaceType.ToString());           
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

        #region EmitProxyType     
        private Type DoEmitProxyType(Type interfaceType ,Type originType)
        {                   
            string typeName = this.GetDynamicTypeName(interfaceType, originType);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);
            typeBuilder.SetParent(typeof(MarshalByRefObject));
            typeBuilder.AddInterfaceImplementation(interfaceType);

            //�����Ա�����ڱ��洫��originTypeʵ����
            FieldBuilder targetField = typeBuilder.DefineField("target", originType, FieldAttributes.Private);
            FieldBuilder aopInterceptorField = typeBuilder.DefineField("aopInterceptor", typeof(IAopInterceptor), FieldAttributes.Private);

            #region Emit Ctor
            ConstructorBuilder ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { originType, typeof(IAopInterceptor) });
            ILGenerator ctorGen = ctor.GetILGenerator();
            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[] { })); //���û���Ĺ��캯��
            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Ldarg_1);
            ctorGen.Emit(OpCodes.Stfld, targetField);

            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Ldarg_2);
            ctorGen.Emit(OpCodes.Stfld, aopInterceptorField);
           
            ctorGen.Emit(OpCodes.Ret);
            #endregion

            this.EmitInitializeLifetimeServiceMethod(typeBuilder);
            foreach (MethodInfo baseMethod in ESBasic.Helpers.ReflectionHelper.GetAllMethods(interfaceType))
            {
                this.EmitMethod(originType, typeBuilder, baseMethod, targetField, aopInterceptorField);
            }           

            Type target = typeBuilder.CreateType();

            return target;
        }
        #endregion

        private void EmitInitializeLifetimeServiceMethod(TypeBuilder typeBuilder)
        {
            MethodInfo baseMethod = ReflectionHelper.SearchMethod(typeof(MarshalByRefObject), "InitializeLifetimeService", new Type[] { });
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(baseMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual, baseMethod.ReturnType, new Type[] { });

            ILGenerator methodGen = methodBuilder.GetILGenerator();
            methodGen.Emit(OpCodes.Ldnull);
            methodGen.Emit(OpCodes.Ret);
        }

        #region EmitMethod
        private void EmitMethod(Type originType, TypeBuilder typeBuilder, MethodInfo baseMethod, FieldBuilder targetField ,FieldBuilder aopInterceptorField)
        {
            Type[] argTypes = EmitHelper.GetParametersType(baseMethod);
            MethodInfo originMethod = ReflectionHelper.SearchMethod(originType, baseMethod.Name, argTypes);
            MethodBuilder methodBuilder = EmitHelper.DefineDerivedMethodSignature(typeBuilder, baseMethod);
             
            ILGenerator methodGen = methodBuilder.GetILGenerator();

            //Type[] genericTypes = new Type[1] ;
            LocalBuilder genericTypesLocalBuilder = methodGen.DeclareLocal(typeof(Type[]));
            if (originMethod.IsGenericMethod)
            {               
                Type[] genericTypes = originMethod.GetGenericArguments();                
                methodGen.Emit(OpCodes.Ldc_I4, genericTypes.Length);
                methodGen.Emit(OpCodes.Newarr, typeof(Type));
                methodGen.Emit(OpCodes.Stloc, genericTypesLocalBuilder);
                for (int i = 0; i < genericTypes.Length; i++)
                {
                    methodGen.Emit(OpCodes.Ldloc, genericTypesLocalBuilder);
                    methodGen.Emit(OpCodes.Ldc_I4, i);
                    EmitHelper.LoadType(methodGen, genericTypes[i]); 
                    methodGen.Emit(OpCodes.Stelem_Ref);
                }
            }

            ParameterInfo[] paras = originMethod.GetParameters();

            
            LocalBuilder paramNamesLocalBuilder = methodGen.DeclareLocal(typeof(string[]));
            LocalBuilder paramValuesLocalBuilder = methodGen.DeclareLocal(typeof(object[]));
            if (paras.Length > 0)
            {
                //���� string[] paraNames = new string[] {"Add" ,"Insert" };   
                methodGen.Emit(OpCodes.Ldc_I4, paras.Length);
                methodGen.Emit(OpCodes.Newarr, typeof(string));
                methodGen.Emit(OpCodes.Stloc, paramNamesLocalBuilder);
                for (int i = 0; i < paras.Length; i++)
                {
                    methodGen.Emit(OpCodes.Ldloc, paramNamesLocalBuilder);
                    methodGen.Emit(OpCodes.Ldc_I4, i);
                    methodGen.Emit(OpCodes.Ldstr, paras[i].Name);
                    methodGen.Emit(OpCodes.Stelem_Ref);
                }

                //���� object[] paras = new object[] {a ,b };              
                methodGen.Emit(OpCodes.Ldc_I4, paras.Length);
                methodGen.Emit(OpCodes.Newarr, typeof(object));
                methodGen.Emit(OpCodes.Stloc, paramValuesLocalBuilder);
                for (int i = 0; i < paras.Length; i++)
                {
                    methodGen.Emit(OpCodes.Ldloc, paramValuesLocalBuilder);
                    methodGen.Emit(OpCodes.Ldc_I4, i);
                    methodGen.Emit(OpCodes.Ldarg, i + 1);

                    if (paras[i].ParameterType.IsByRef) //�����ref/out��������ȥ��ַ
                    {
                        EmitHelper.Ldind(methodGen, paras[i].ParameterType);
                        methodGen.Emit(OpCodes.Box, paras[i].ParameterType.GetElementType());
                    }
                    else if (paras[i].ParameterType.IsValueType)
                    {
                        methodGen.Emit(OpCodes.Box, paras[i].ParameterType);
                    }
                    else
                    {
                    }

                    methodGen.Emit(OpCodes.Stelem_Ref);
                }
            }

            //����  InterceptedMethod method = new InterceptedMethod(this.target, "InsertMuch", paras);
            LocalBuilder interceptedMethodLocalBuilder = methodGen.DeclareLocal(typeof(InterceptedMethod));
            methodGen.Emit(OpCodes.Ldarg_0);
            methodGen.Emit(OpCodes.Ldfld, targetField);
            methodGen.Emit(OpCodes.Ldstr, originMethod.Name);
            methodGen.Emit(OpCodes.Ldloc, genericTypesLocalBuilder);
            methodGen.Emit(OpCodes.Ldloc, paramNamesLocalBuilder);
            methodGen.Emit(OpCodes.Ldloc, paramValuesLocalBuilder);
            methodGen.Emit(OpCodes.Newobj, typeof(InterceptedMethod).GetConstructor(new Type[] { typeof(object), typeof(string), typeof(Type[]),typeof(string[]), typeof(object[]) }));
            methodGen.Emit(OpCodes.Stloc, interceptedMethodLocalBuilder);

            //����this.aopInterceptor.PreProcess(interceptedMethod);
            MethodInfo preProcessMethodInfo = typeof(IAopInterceptor).GetMethod("PreProcess", new Type[] { typeof(InterceptedMethod) });
            methodGen.Emit(OpCodes.Ldarg_0);
            methodGen.Emit(OpCodes.Ldfld, aopInterceptorField);
            methodGen.Emit(OpCodes.Ldloc, interceptedMethodLocalBuilder);
            methodGen.Emit(OpCodes.Callvirt, preProcessMethodInfo);

            //���� IArounder arounder = this.aopInterceptor.NewArounder();
            LocalBuilder arounderLocalBuilder = methodGen.DeclareLocal(typeof(IArounder));
            MethodInfo newArounderMethodInfo = typeof(IAopInterceptor).GetMethod("NewArounder") ;
            methodGen.Emit(OpCodes.Ldarg_0);
            methodGen.Emit(OpCodes.Ldfld, aopInterceptorField);
            methodGen.Emit(OpCodes.Callvirt, newArounderMethodInfo);
            methodGen.Emit(OpCodes.Stloc, arounderLocalBuilder);

            //���� arounder.BeginAround(method);
            MethodInfo beginAroundMethodInfo = typeof(IArounder).GetMethod("BeginAround");
            methodGen.Emit(OpCodes.Ldloc, arounderLocalBuilder);
            methodGen.Emit(OpCodes.Ldloc, interceptedMethodLocalBuilder);
            methodGen.Emit(OpCodes.Callvirt, beginAroundMethodInfo);

            //���� object returnVal = null;
            LocalBuilder retLocalBuilder = null; //�Ƿ��з���ֵ
            if (originMethod.ReturnType != typeof(void))
            {
                retLocalBuilder = methodGen.DeclareLocal(originMethod.ReturnType);
            }

            LocalBuilder exceptionLocalBuilder = methodGen.DeclareLocal(typeof(Exception));

            // try
            Label beginExceptionLabel = methodGen.BeginExceptionBlock(); 

            //����Ŀ�귽��
            methodGen.Emit(OpCodes.Ldarg_0);
            methodGen.Emit(OpCodes.Ldfld, targetField);

            int coventIndex = 0;
            foreach (ParameterInfo pi in originMethod.GetParameters())
            {
                EmitHelper.LoadArg(methodGen, coventIndex + 1);
                EmitHelper.ConvertTopArgType(methodGen, argTypes[coventIndex], pi.ParameterType);
                coventIndex++;
            }           

            methodGen.Emit(OpCodes.Callvirt, originMethod);
            if (retLocalBuilder != null)
            {
                methodGen.Emit(OpCodes.Stloc, retLocalBuilder);
            }

            // catch
            methodGen.BeginCatchBlock(typeof(Exception));
            methodGen.Emit(OpCodes.Stloc, exceptionLocalBuilder); //�洢Exception��local

            //���� arounder.OnException(interceptedMethod ,exception);
            MethodInfo onExceptionMethodInfo = typeof(IArounder).GetMethod("OnException");
            methodGen.Emit(OpCodes.Ldloc, arounderLocalBuilder);
            methodGen.Emit(OpCodes.Ldloc, interceptedMethodLocalBuilder);
            methodGen.Emit(OpCodes.Ldloc, exceptionLocalBuilder);
            methodGen.Emit(OpCodes.Callvirt, onExceptionMethodInfo);           

            methodGen.Emit(OpCodes.Nop);
            methodGen.Emit(OpCodes.Rethrow);
            methodGen.Emit(OpCodes.Nop);
            methodGen.EndExceptionBlock();

            //���� arounder.EndAround(returnVal);
            MethodInfo endAroundMethodInfo = typeof(IArounder).GetMethod("EndAround");
            methodGen.Emit(OpCodes.Ldloc, arounderLocalBuilder);
            if (retLocalBuilder != null)
            {
                if (originMethod.ReturnType.IsValueType) //����ֵ�����ֵ���ͣ���װ����ٵ���EndAround
                {
                    methodGen.Emit(OpCodes.Ldloc, retLocalBuilder);
                    methodGen.Emit(OpCodes.Box, originMethod.ReturnType);
                }
                else
                {
                    methodGen.Emit(OpCodes.Ldloc, retLocalBuilder);
                }
            }
            else
            {
                methodGen.Emit(OpCodes.Ldnull);
            }
            methodGen.Emit(OpCodes.Callvirt, endAroundMethodInfo);
            methodGen.Emit(OpCodes.Nop);

            //����this.aopInterceptor.PostProcess(method ,returnVal);
            MethodInfo postProcessMethodInfo = typeof(IAopInterceptor).GetMethod("PostProcess", new Type[] { typeof(InterceptedMethod),typeof(object) });
            methodGen.Emit(OpCodes.Ldarg_0);
            methodGen.Emit(OpCodes.Ldfld, aopInterceptorField);
            methodGen.Emit(OpCodes.Ldloc, interceptedMethodLocalBuilder);
            if (retLocalBuilder != null)
            {
                if (originMethod.ReturnType.IsValueType)
                {
                    methodGen.Emit(OpCodes.Ldloc, retLocalBuilder);
                    methodGen.Emit(OpCodes.Box, originMethod.ReturnType);
                }
                else
                {
                    methodGen.Emit(OpCodes.Ldloc, retLocalBuilder);
                }
            }
            else
            {
                methodGen.Emit(OpCodes.Ldnull);
            }
            methodGen.Emit(OpCodes.Callvirt, postProcessMethodInfo);
            methodGen.Emit(OpCodes.Nop);

            if (retLocalBuilder != null)
            {
                methodGen.Emit(OpCodes.Ldloc, retLocalBuilder);
            }
            methodGen.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);
        } 
        #endregion      
    }

    
}
