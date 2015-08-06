using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using ESBasic.Helpers;

namespace ESBasic.Emit.ForEntity
{
    public class PropertyQuickerEmitter
    {
        private int number = 0;
        private bool saveFile = false;
        private string assemblyName = "DynamicPropertyQuickerAssembly";
        private string theFileName;
        private AssemblyBuilder dynamicAssembly;
        private ModuleBuilder moduleBuilder;
        private IDictionary<Type, Type> dicPropertyQuickerType = new Dictionary<Type, Type>(); //TEntity -- PropertyQuicker Type          

        #region Ctor
        public PropertyQuickerEmitter() :this(false)
        {
        }

        public PropertyQuickerEmitter(bool save)
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

        #region CreatePropertyQuickerType
        /// <summary>
        /// CreatePropertyQuickerType 为EntityType发射一个实现了IPropertyQuicker[TEntity]接口的类型。
        /// </summary>       
        public Type CreatePropertyQuickerType(Type entityType)
        {
            lock (this.dicPropertyQuickerType)
            {
                if (this.dicPropertyQuickerType.ContainsKey(entityType))
                {
                    return this.dicPropertyQuickerType[entityType];
                }

                Type orMappingType = this.DoCreateORMappingType(entityType);
                this.dicPropertyQuickerType.Add(entityType, orMappingType);

                return orMappingType;
            }
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
        
        #region DoCreateORMappingType
        private Type DoCreateORMappingType(Type entityType)
        {
            try
            {
                System.Threading.Interlocked.Increment(ref this.number);
                Type parentGenericType = typeof(IPropertyQuicker<>);
                Type parentClosedType = parentGenericType.MakeGenericType(entityType);

                TypeBuilder typeBuilder = moduleBuilder.DefineType("ESBasic.DynaAssembly." + TypeHelper.GetClassSimpleName(entityType) + "ORMapping" + this.number.ToString(), TypeAttributes.Public | TypeAttributes.Class);
                typeBuilder.AddInterfaceImplementation(parentClosedType);

                #region Emit Ctor
                ConstructorBuilder ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
                ILGenerator ctorGen = ctor.GetILGenerator();
                ctorGen.Emit(OpCodes.Ret);
                #endregion

                MethodInfo baseMethod1 = ReflectionHelper.SearchMethod(parentClosedType, "GetPropertyValue", new Type[] { entityType, typeof(string) });
                this.EmitGetPropertyValueMethod(typeBuilder, baseMethod1, entityType);

                MethodInfo baseMethod2 = parentClosedType.GetMethod("SetPropertyValue");// ReflectionHelper.SearchMethod(parentClosedType, "SetPropertyValue", new Type[] { entityType, typeof(string), typeof(object) });
                this.EmitSetPropertyValueMethod(typeBuilder, baseMethod2, entityType);

                MethodInfo baseMethod4 = ReflectionHelper.SearchMethod(parentClosedType, "GetValue", new Type[] { typeof(object), typeof(string) });
                this.EmitGetValueMethod(typeBuilder, baseMethod4, entityType, baseMethod1);

                MethodInfo baseMethod5 = ReflectionHelper.SearchMethod(parentClosedType, "SetValue", new Type[] { typeof(object), typeof(string), typeof(object) });
                this.EmitSetValueMethod(typeBuilder, baseMethod5, entityType, baseMethod2);


                Type target = typeBuilder.CreateType();

                return target;
            }
            catch (Exception ee)
            {
                throw new Exception(string.Format("Error Emitting ORMapping for {0}" ,entityType),ee);
            }
        }
        #endregion       

        #region Emit
        #region EmitGetPropertyValueMethod
        private void EmitGetPropertyValueMethod(TypeBuilder typeBuilder, MethodInfo baseMethod, Type entityType)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetPropertyValue", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            MethodInfo compareStringMethod = typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) });
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.DeclareLocal(typeof(object));
            ilGenerator.Emit(OpCodes.Nop);

            PropertyInfo[] tempPros = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);// ESBasic.Helpers.TypeHelper.ConvertListToArray<PropertyInfo>(columnList);
            IList<PropertyInfo> proList = new List<PropertyInfo>();
            foreach (PropertyInfo propertyInfo in tempPros)
            {
                if (propertyInfo.CanWrite && propertyInfo.CanRead)
                {
                    proList.Add(propertyInfo);
                }
            }
            PropertyInfo[] pros = ESBasic.Collections.CollectionConverter.ConvertListToArray<PropertyInfo>(proList);

            Label loadNullLabel = ilGenerator.DefineLabel();
            Label retLabel = ilGenerator.DefineLabel();
            Label[] labels = new Label[pros.Length + 1];
            for (int i = 0; i < pros.Length; i++)
            {
                labels[i] = ilGenerator.DefineLabel();
            }
            labels[pros.Length] = loadNullLabel;

            for (int i = 0; i < pros.Length; i++)
            {
                PropertyInfo property = pros[i];
                ilGenerator.MarkLabel(labels[i]);
                ilGenerator.Emit(OpCodes.Ldarg_2);
                string proName = property.Name;
                ilGenerator.Emit(OpCodes.Ldstr, proName);
                ilGenerator.EmitCall(OpCodes.Call, compareStringMethod, new Type[] { typeof(string), typeof(string) });
                ilGenerator.Emit(OpCodes.Brfalse, labels[i + 1]);

                ilGenerator.Emit(OpCodes.Nop);
                if (entityType.IsValueType)
                {
                    ilGenerator.Emit(OpCodes.Ldarga ,1);
                }
                else
                {
                    ilGenerator.Emit(OpCodes.Ldarg_1);
                }
                MethodInfo getPropertyMethod = entityType.GetMethod("get_" + proName, new Type[] { });
                if (entityType.IsValueType)
                {
                    ilGenerator.EmitCall(OpCodes.Call, getPropertyMethod, new Type[] { });
                }
                else
                {
                    ilGenerator.EmitCall(OpCodes.Callvirt, getPropertyMethod, new Type[] { });
                }
                if (property.PropertyType.IsValueType)
                {
                    ilGenerator.Emit(OpCodes.Box, property.PropertyType);
                }

                ilGenerator.Emit(OpCodes.Stloc_0);
                ilGenerator.Emit(OpCodes.Br, retLabel);
            }

            ilGenerator.MarkLabel(loadNullLabel);
            ilGenerator.Emit(OpCodes.Ldnull);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.Emit(OpCodes.Br, retLabel);

            ilGenerator.MarkLabel(retLabel);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);
        }
        #endregion        

        #region EmitSetPropertyValueMethod
        private void EmitSetPropertyValueMethod(TypeBuilder typeBuilder, MethodInfo baseMethod, Type entityType)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("SetPropertyValue", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            MethodInfo compareStringMethod = typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) });  
            MethodInfo changeTypeMethod = typeof(TypeHelper).GetMethod("ChangeType", new Type[] { typeof(Type), typeof(object) });
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
           
            ilGenerator.Emit(OpCodes.Nop);

            PropertyInfo[] tempPros = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);// ESBasic.Helpers.TypeHelper.ConvertListToArray<PropertyInfo>(columnList);
            IList<PropertyInfo> proList = new List<PropertyInfo>();
            foreach (PropertyInfo propertyInfo in tempPros)
            {
                if (propertyInfo.CanWrite && propertyInfo.CanRead)
                {
                    proList.Add(propertyInfo);
                }
            }
            PropertyInfo[] pros = ESBasic.Collections.CollectionConverter.ConvertListToArray<PropertyInfo>(proList);

            Label retLabel = ilGenerator.DefineLabel();
            Label[] labels = new Label[pros.Length + 1];
            for (int i = 0; i < pros.Length; i++)
            {
                labels[i] = ilGenerator.DefineLabel();
            }
            labels[pros.Length] = retLabel;

            for (int i = 0; i < pros.Length; i++)
            {
                PropertyInfo property = pros[i];
                ilGenerator.MarkLabel(labels[i]);
                ilGenerator.Emit(OpCodes.Ldarg_2);
                string proName = property.Name;
                ilGenerator.Emit(OpCodes.Ldstr, proName);
                ilGenerator.EmitCall(OpCodes.Call, compareStringMethod, new Type[] { typeof(string), typeof(string) });
                ilGenerator.Emit(OpCodes.Brfalse, labels[i + 1]);

                ilGenerator.Emit(OpCodes.Nop);

                ilGenerator.Emit(OpCodes.Ldarg_1);
                EmitHelper.LoadType(ilGenerator, property.PropertyType);                
                ilGenerator.Emit(OpCodes.Ldarg_3);
                ilGenerator.EmitCall(OpCodes.Call,changeTypeMethod, new Type[] { typeof(Type), typeof(object) }); //先将object转换到正确的类型，即使还是一个object

                #region 类型转换 这一段是必须的，否则会导致内存状态损坏
                //注意：TypeHelper.ChangeType返回的是object。
                if (property.PropertyType.IsValueType) //值类型，则拆箱
                {
                    ilGenerator.Emit(OpCodes.Unbox_Any, property.PropertyType);
                }
                else if (property.PropertyType == typeof(byte[]) || property.PropertyType == typeof(string))
                {
                    ilGenerator.Emit(OpCodes.Castclass, property.PropertyType);
                }  
                else if (property.PropertyType == typeof(object) || property.PropertyType.IsClass || property.PropertyType.IsGenericType)
                {
                    //do nothing .对应sql_variant
                }
                else
                {
                    MethodInfo toStringMethod = typeof(object).GetMethod("ToString");
                    ilGenerator.EmitCall(OpCodes.Callvirt, toStringMethod, null);

                    //类型转换
                    if (property.PropertyType != typeof(string))
                    {
                        MethodInfo parseMethod = property.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });
                        ilGenerator.EmitCall(OpCodes.Callvirt, parseMethod, new Type[] { typeof(string) });
                    }
                }
                #endregion

                MethodInfo setPropertyMethod = entityType.GetMethod("set_" + proName, new Type[] {property.PropertyType});
                if (entityType.IsValueType)
                {
                    ilGenerator.EmitCall(OpCodes.Call, setPropertyMethod, new Type[] { property.PropertyType });
                }
                else
                {
                    ilGenerator.EmitCall(OpCodes.Callvirt, setPropertyMethod, new Type[] { property.PropertyType });
                }
                
                ilGenerator.Emit(OpCodes.Br, retLabel);
            }           

            ilGenerator.MarkLabel(retLabel);           
            ilGenerator.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);
        }
        #endregion                

        #region EmitSetValueMethod
        private void EmitSetValueMethod(TypeBuilder typeBuilder, MethodInfo baseMethod, Type entityType, MethodInfo setPropertyValueMethod)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("SetValue", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Castclass, entityType);
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Ldarg_3);
            ilGenerator.Emit(OpCodes.Callvirt, setPropertyValueMethod);

            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);
        } 
        #endregion

        #region EmitGetValueMethod
        private void EmitGetValueMethod(TypeBuilder typeBuilder, MethodInfo baseMethod, Type entityType, MethodInfo getPropertyValueMethod)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetValue", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);            
            ilGenerator.Emit(OpCodes.Ldarg_1);

            if (entityType.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Unbox_Any, entityType);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Castclass, entityType);
            }

            
            ilGenerator.Emit(OpCodes.Ldarg_2);          
            ilGenerator.Emit(OpCodes.Callvirt, getPropertyValueMethod);
           
            ilGenerator.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);
        }
        #endregion
        #endregion
    }
}
