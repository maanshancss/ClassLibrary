using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using ESBasic.Helpers;

namespace ESBasic.Emit.ForEntity
{
    /// <summary>
    /// DynamicEntityEmitter 根据每个字段的名称/类型，发射对应的Entity类型。
    /// </summary>
    public class DynamicEntityEmitter
    {
        private bool saveFile = false;
        private string assemblyName = "DynamicEntityAssembly";
        private string theFileName;
        private AssemblyBuilder dynamicAssembly;              
        protected ModuleBuilder moduleBuilder;   

        #region Ctor
        public DynamicEntityEmitter() :this(false)
        {
        }

        public DynamicEntityEmitter(bool save)
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

        #region EmitEntityType
        public Type EmitEntityType(string entityTypeName, IDictionary<string, Type> entityPropertyDic)
        {
            string typeName = string.Format("{0}.{1}", this.assemblyName, entityTypeName);
            TypeBuilder typeBuilder = this.moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);
            //typeBuilder.AddInterfaceImplementation(interfaceType);

            List<FieldBuilder> fieldBuilderList = new List<FieldBuilder>();

            foreach (string propertyName in entityPropertyDic.Keys)
            {
                #region 定义属性
                Type propertyType = entityPropertyDic[propertyName];
                FieldBuilder fieldBuilder = typeBuilder.DefineField("m_" + propertyName, propertyType, FieldAttributes.Private);
                fieldBuilderList.Add(fieldBuilder);
                fieldBuilder.SetConstant(TypeHelper.ChangeType(propertyType, TypeHelper.GetDefaultValue(propertyType)));
                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);

                //定义属性的get方法
                MethodBuilder getPropertyBuilder = typeBuilder.DefineMethod("get", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                                                                              propertyType, Type.EmptyTypes);

                ILGenerator getAIL = getPropertyBuilder.GetILGenerator();
                getAIL.Emit(OpCodes.Ldarg_0);
                getAIL.Emit(OpCodes.Ldfld, fieldBuilder);
                getAIL.Emit(OpCodes.Ret);

                //定义属性A的set方法
                MethodBuilder setPropertyABuilder = typeBuilder.DefineMethod("set", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                                                                              null, new Type[] { propertyType });
                ILGenerator setAIL = setPropertyABuilder.GetILGenerator();
                setAIL.Emit(OpCodes.Ldarg_0);
                setAIL.Emit(OpCodes.Ldarg_1);
                setAIL.Emit(OpCodes.Stfld, fieldBuilder);
                setAIL.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getPropertyBuilder);
                propertyBuilder.SetSetMethod(setPropertyABuilder);
                #endregion
            }

            #region Emit Ctor
            ConstructorBuilder ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            ILGenerator ctorGen = ctor.GetILGenerator();
            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[] { }));

            foreach (FieldBuilder fieldBuilder in fieldBuilderList)
            {
                if (fieldBuilder.FieldType == typeof(string))
                {
                    ctorGen.Emit(OpCodes.Ldarg_0);
                    ctorGen.Emit(OpCodes.Ldstr, "");
                    ctorGen.Emit(OpCodes.Stfld, fieldBuilder);
                }
            }

            ctorGen.Emit(OpCodes.Ret);
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
