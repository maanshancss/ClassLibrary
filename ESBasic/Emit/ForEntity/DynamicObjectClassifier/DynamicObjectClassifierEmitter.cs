using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using ESBasic.Helpers;
using ESBasic.Collections;
using System.Collections;
using ESBasic.Arithmetic;

namespace ESBasic.Emit.ForEntity
{
    /// <summary>
    /// DynamicObjectClassifierEmitter 用于发射实现了IObjectClassifier接口的对象分类器类型。
    /// 发射的动态对象分类器内部采用N层嵌套字典实现。
    /// </summary>
    public class DynamicObjectClassifierEmitter
    {
        private bool saveFile = false;
        public const string AssemblyName = "DynamicObjectClassifierAssembly";
        private string theFileName;
        private AssemblyBuilder dynamicAssembly;
        protected ModuleBuilder moduleBuilder;   

        #region Ctor
        public DynamicObjectClassifierEmitter() :this(false)
        {
        }

        public DynamicObjectClassifierEmitter(bool save)
        {
            this.saveFile = save;
            this.theFileName = DynamicObjectClassifierEmitter.AssemblyName + ".dll";

            AssemblyBuilderAccess assemblyBuilderAccess = this.saveFile ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run;
            this.dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(DynamicObjectClassifierEmitter.AssemblyName), assemblyBuilderAccess);
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

        #region EmitDynamicNTierDictionaryType
        public Type EmitDynamicNTierDictionaryType<TObject>(Type[] nestedKeyTypes)
        {
            Type entityType = typeof(TObject);
            int tierNumber = nestedKeyTypes.Length;

            string typeContactStr = entityType.ToString();
            for (int i = 0; i < nestedKeyTypes.Length; i++)
            {  
                typeContactStr += "_" + TypeHelper.GetClassSimpleName(nestedKeyTypes[i]);
            }

            Type baseType = typeof(IObjectClassifier<TObject>);
            string typeName = string.Format("{0}_Classifier", typeContactStr);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);
            typeBuilder.AddInterfaceImplementation(baseType);
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(SerializableAttribute).GetConstructor(Type.EmptyTypes),new object[]{})) ;

            //定义成员
            Type innerDicType = typeof(Dictionary<,>).MakeGenericType(nestedKeyTypes[tierNumber - 1], typeof(IObjectContainer<TObject>));
            if (tierNumber > 1)
            {
                for (int i = nestedKeyTypes.Length - 2; i >= 0; i--)
                {
                    innerDicType = typeof(Dictionary<,>).MakeGenericType(nestedKeyTypes[i], innerDicType);
                }
            }


            FieldBuilder dicField = typeBuilder.DefineField("dic", innerDicType, FieldAttributes.Private);
            FieldBuilder columns4ClassifyField = typeBuilder.DefineField("columns4Classify", typeof(string[]), FieldAttributes.Private);
            FieldBuilder containerListField = typeBuilder.DefineField("containerList", typeof(List<IObjectContainer<TObject>>), FieldAttributes.Private);
                        
            FieldBuilder containerCreatorField = typeBuilder.DefineField("containerCreator", typeof(IObjectContainerCreator<TObject>),FieldAttributes.Private);
            containerCreatorField.SetCustomAttribute(new CustomAttributeBuilder(typeof(NonSerializedAttribute).GetConstructor(Type.EmptyTypes), new object[] { }));
            FieldBuilder propertyQuickerField = typeBuilder.DefineField("propertyQuicker", typeof(IPropertyQuicker<TObject>), FieldAttributes.Private);
            propertyQuickerField.SetCustomAttribute(new CustomAttributeBuilder(typeof(NonSerializedAttribute).GetConstructor(Type.EmptyTypes), new object[] { }));
            
            Dictionary<int, FieldBuilder> distinctArrayFieldInfoDic = new Dictionary<int, FieldBuilder>();
            for (int i = 0; i < nestedKeyTypes.Length;i++ )
            {
                distinctArrayFieldInfoDic.Add(i, typeBuilder.DefineField("distinctArray" + i.ToString(), typeof(SortedArray<>).MakeGenericType(nestedKeyTypes[i]), FieldAttributes.Private));
            }

            #region Emit Ctor
            ConstructorBuilder ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] {typeof(string[]) });
            ILGenerator ctorGen = ctor.GetILGenerator();

            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Newobj, innerDicType.GetConstructor(Type.EmptyTypes)); //值类型用 OpCodes.Initobj          
            ctorGen.Emit(OpCodes.Stfld, dicField);

            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Newobj, typeof(List<IObjectContainer<TObject>>).GetConstructor(Type.EmptyTypes));        
            ctorGen.Emit(OpCodes.Stfld, containerListField);

            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes)); //调用基类的构造函数           

            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Ldarg_1);
            ctorGen.Emit(OpCodes.Stfld, columns4ClassifyField);

            for (int i = 0; i < nestedKeyTypes.Length; i++)
            {
                ctorGen.Emit(OpCodes.Ldarg_0);
                ctorGen.Emit(OpCodes.Newobj, typeof(SortedArray<>).MakeGenericType(nestedKeyTypes[i]).GetConstructor(Type.EmptyTypes));
                ctorGen.Emit(OpCodes.Stfld, distinctArrayFieldInfoDic[i]);
            }

            ctorGen.Emit(OpCodes.Ret);
            #endregion

            this.EmitInitializeMethod<TObject>(baseType, typeBuilder ,containerCreatorField ,propertyQuickerField);
            this.EmitAddMethod<TObject>(tierNumber, entityType, baseType, innerDicType, typeBuilder, nestedKeyTypes, dicField, columns4ClassifyField ,containerListField , distinctArrayFieldInfoDic ,containerCreatorField,propertyQuickerField);
            MethodBuilder doGetContainerMethodBuilder = this.EmitDoGetContainerMethod<TObject>(tierNumber, entityType, baseType, innerDicType, typeBuilder, nestedKeyTypes, dicField, columns4ClassifyField);
            this.EmitGetContainersMethod<TObject>(baseType, typeBuilder, nestedKeyTypes, distinctArrayFieldInfoDic, doGetContainerMethodBuilder);
            this.EmitGetAllContainersMethod(baseType, typeBuilder, containerListField);
            this.EmitGetDistinctValuesMethod(baseType, typeBuilder,nestedKeyTypes ,columns4ClassifyField, distinctArrayFieldInfoDic);
            this.EmitProperties4ClassifyProperty(baseType, typeBuilder, columns4ClassifyField);
            
            Type target = typeBuilder.CreateType();

            return target;
        }

        #region EmitInitializeMethod
        private void EmitInitializeMethod<TObject>(Type baseType, TypeBuilder typeBuilder, FieldBuilder containerCreatorField, FieldBuilder propertyQuickerField)
        {
            MethodInfo baseMethod = ReflectionHelper.SearchMethod(baseType, "Initialize", new Type[] { typeof(IObjectContainerCreator<TObject>) });
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("Initialize", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            ILGenerator ilGen = methodBuilder.GetILGenerator();

            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldarg_1);
            ilGen.Emit(OpCodes.Stfld, containerCreatorField);

            ilGen.Emit(OpCodes.Ldarg_0);
            MethodInfo openMethod = typeof(PropertyQuickerFactory).GetMethod("CreatePropertyQuicker" ,new Type[]{});
            MethodInfo createPropertyQuickerMethod = openMethod.MakeGenericMethod(typeof(TObject));
            ilGen.Emit(OpCodes.Call, createPropertyQuickerMethod);
            ilGen.Emit(OpCodes.Stfld, propertyQuickerField);

             
            ilGen.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);
        }
        #endregion

        #region EmitAddMethod
        private void EmitAddMethod<TEntity>(int tierNumber, Type entityType, Type baseType, Type nestedDicType, TypeBuilder typeBuilder, Type[] nestedKeyTypes, FieldBuilder dicField, FieldBuilder columns4ClassifyField,
            FieldBuilder containerListField, Dictionary<int, FieldBuilder> distinctArrayFieldInfoDic, FieldBuilder containerCreatorField, FieldBuilder propertyQuickerField)
        {
            MethodInfo addBaseMethod = ReflectionHelper.SearchMethod(baseType, "Add", new Type[] { entityType});
            MethodBuilder addMethodBuilder = typeBuilder.DefineMethod("Add", addBaseMethod.Attributes & ~MethodAttributes.Abstract, addBaseMethod.CallingConvention, addBaseMethod.ReturnType, EmitHelper.GetParametersType(addBaseMethod));
            ILGenerator ilGen = addMethodBuilder.GetILGenerator();

            MethodInfo getPropertyValueMethod = typeof(IPropertyQuicker<TEntity>).GetMethod("GetPropertyValue");
            MethodInfo createNewContainerMethod = typeof(IObjectContainerCreator<TEntity>).GetMethod("CreateNewContainer");
            MethodInfo containerAddMethod = typeof(IObjectContainer<TEntity>).GetMethod("Add");
            MethodInfo createInstanceMethod = typeof(Activator).GetMethod("CreateInstance", new Type[] { typeof(Type) });
            MethodInfo addListMethod = typeof(List<IObjectContainer<TEntity>>).GetMethod("Add");            

            LocalBuilder[] valLocalBuilders = new LocalBuilder[nestedKeyTypes.Length];
            Type tempType = nestedDicType;
            LocalBuilder curDicBuilder = ilGen.DeclareLocal(tempType);
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldfld, dicField);
            ilGen.Emit(OpCodes.Stloc, curDicBuilder);

            for (int i = 0; i < tierNumber; i++)
            {
                MethodInfo dicContainsKeyMethod = tempType.GetMethod("ContainsKey");
                MethodInfo get_ItemMethod = tempType.GetMethod("get_Item");
                MethodInfo dicAddMethod = tempType.GetMethod("Add");
                MethodInfo addSortedArrayMethod = typeof(SortedArray<>).MakeGenericType(nestedKeyTypes[i]).GetMethod("Add" ,new Type[]{nestedKeyTypes[i]});
                              
                Label nextLable = ilGen.DefineLabel();                
                valLocalBuilders[i] = ilGen.DeclareLocal(nestedKeyTypes[i]);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld ,propertyQuickerField);               
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, columns4ClassifyField);
                ilGen.Emit(OpCodes.Ldc_I4, i);
                ilGen.Emit(OpCodes.Ldelem_Ref);
                ilGen.Emit(OpCodes.Callvirt, getPropertyValueMethod);
                ilGen.Emit(OpCodes.Unbox_Any, nestedKeyTypes[i]);
                ilGen.Emit(OpCodes.Stloc, valLocalBuilders[i]);

                //this.ageDistinctArray.Add(provali);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, distinctArrayFieldInfoDic[i]);
                ilGen.Emit(OpCodes.Ldloc, valLocalBuilders[i]);
                ilGen.Emit(OpCodes.Call, addSortedArrayMethod);

                ilGen.Emit(OpCodes.Ldloc, curDicBuilder);
                ilGen.Emit(OpCodes.Ldloc, valLocalBuilders[i]);
                ilGen.Emit(OpCodes.Callvirt, dicContainsKeyMethod);
                ilGen.Emit(OpCodes.Brtrue, nextLable);
                ilGen.Emit(OpCodes.Nop);

                ilGen.Emit(OpCodes.Ldloc, curDicBuilder);
                ilGen.Emit(OpCodes.Ldloc, valLocalBuilders[i]);

                if (i == tierNumber - 1) //this.entityObjectContainerCreator.CreateNewContainer()
                {
                    LocalBuilder containerLocalBuilder = ilGen.DeclareLocal(typeof(IObjectContainer<TEntity>));
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, containerCreatorField);                     
                    ilGen.Emit(OpCodes.Callvirt, createNewContainerMethod);
                    ilGen.Emit(OpCodes.Stloc, containerLocalBuilder);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, containerListField);
                    ilGen.Emit(OpCodes.Ldloc, containerLocalBuilder);
                    ilGen.Emit(OpCodes.Callvirt, addListMethod);                    
                    ilGen.Emit(OpCodes.Ldloc, containerLocalBuilder);
                    ilGen.Emit(OpCodes.Callvirt, dicAddMethod);
                    ilGen.Emit(OpCodes.Nop);
                    ilGen.MarkLabel(nextLable);
                }
                else
                {
                    tempType = tempType.GetGenericArguments()[1];
                    ilGen.Emit(OpCodes.Newobj, tempType.GetConstructor(Type.EmptyTypes));
                    ilGen.Emit(OpCodes.Callvirt, dicAddMethod);
                    ilGen.Emit(OpCodes.Nop);
                    ilGen.MarkLabel(nextLable);

                    ilGen.Emit(OpCodes.Ldloc, curDicBuilder);
                    ilGen.Emit(OpCodes.Ldloc, valLocalBuilders[i]);
                    ilGen.Emit(OpCodes.Callvirt, get_ItemMethod);
                    curDicBuilder = ilGen.DeclareLocal(tempType);
                    ilGen.Emit(OpCodes.Stloc, curDicBuilder);                    
                }                            
            }

            // this.dic[proval0][proval1].Add(entity);
            Type tempType2 = nestedDicType;
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldfld, dicField);
            for (int i = 0; i < tierNumber; i++)
            {
                MethodInfo get_ItemMethod = tempType2.GetMethod("get_Item");
                ilGen.Emit(OpCodes.Ldloc, valLocalBuilders[i]);
                ilGen.Emit(OpCodes.Callvirt, get_ItemMethod);

                tempType2 = tempType2.GetGenericArguments()[1];
            }

            ilGen.Emit(OpCodes.Ldarg_1);
            ilGen.Emit(OpCodes.Callvirt, containerAddMethod);
            ilGen.Emit(OpCodes.Nop);

            ilGen.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(addMethodBuilder, addBaseMethod);
        }
        #endregion 

        #region EmitGetContainersMethod
        private void EmitGetContainersMethod<TEntity>(Type baseType, TypeBuilder typeBuilder, Type[] nestedKeyTypes, Dictionary<int, FieldBuilder> distinctArrayFieldInfoDic, MethodBuilder doGetContainerMethodBuilder)
        {
            MethodInfo baseMethod = ReflectionHelper.SearchMethod(baseType, "GetContainers", new Type[] { typeof(object[]) });
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetContainers", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            ILGenerator ilGen = methodBuilder.GetILGenerator();

            LocalBuilder distinctValListLocalBuilder = ilGen.DeclareLocal(typeof(IList[]));
            ilGen.Emit(OpCodes.Ldc_I4, nestedKeyTypes.Length);
            ilGen.Emit(OpCodes.Newarr, typeof(IList));
            ilGen.Emit(OpCodes.Stloc, distinctValListLocalBuilder);
            

            for (int i = 0; i < nestedKeyTypes.Length; i++)
            {
                MethodInfo getAllMethod = typeof(SortedArray<>).MakeGenericType(nestedKeyTypes[i]).GetMethod("GetAll", new Type[] { });

                ilGen.Emit(OpCodes.Ldloc, distinctValListLocalBuilder);
                ilGen.Emit(OpCodes.Ldc_I4, i);                
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, distinctArrayFieldInfoDic[i]);
                ilGen.Emit(OpCodes.Call, getAllMethod);
                ilGen.Emit(OpCodes.Stelem_Ref);
            }

            LocalBuilder mappingListLocalBuilder = ilGen.DeclareLocal(typeof(List<object[]>)); //
            LocalBuilder containerListLocalBuilder = ilGen.DeclareLocal(typeof(List<IObjectContainer<TEntity>>));
            LocalBuilder containerLocalBuilder = ilGen.DeclareLocal(typeof(IObjectContainer<TEntity>));
            LocalBuilder countLocalBuilder = ilGen.DeclareLocal(typeof(int)); //for循环计数
            Label nextLabel = ilGen.DefineLabel();
            Label onNullLabel = ilGen.DefineLabel();
            Label judgeLabel = ilGen.DefineLabel();

            MethodInfo adjustMappingValuesMethod = typeof(DynamicObjectClassifierEmitter).GetMethod("AdjustMappingValues" , BindingFlags.Static | BindingFlags.Public);
            MethodInfo getItemValuesMethod = typeof(List<object[]>).GetMethod("get_Item" ,new Type[]{typeof(int)});
            //MethodInfo DoGetContainerMethod = typeBuilder.GetMethod("DoGetContainer") ;
            MethodInfo addListMethod = typeof(List<IObjectContainer<TEntity>>).GetMethod("Add");
            MethodInfo getCountMethod = typeof(List<object[]>).GetMethod("get_Count");

            ilGen.Emit(OpCodes.Ldarg_1);
            ilGen.Emit(OpCodes.Ldloc, distinctValListLocalBuilder);
            ilGen.Emit(OpCodes.Call, adjustMappingValuesMethod);
            ilGen.Emit(OpCodes.Stloc, mappingListLocalBuilder);

            ilGen.Emit(OpCodes.Newobj, typeof(List<IObjectContainer<TEntity>>).GetConstructor(Type.EmptyTypes));
            ilGen.Emit(OpCodes.Stloc, containerListLocalBuilder);

            ilGen.Emit(OpCodes.Ldc_I4_0);
            ilGen.Emit(OpCodes.Stloc, countLocalBuilder);
            ilGen.Emit(OpCodes.Br, judgeLabel);
            ilGen.MarkLabel(nextLabel);
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldloc, mappingListLocalBuilder);
            ilGen.Emit(OpCodes.Ldloc, countLocalBuilder);
            ilGen.Emit(OpCodes.Callvirt, getItemValuesMethod);
            ilGen.Emit(OpCodes.Call, doGetContainerMethodBuilder);
            ilGen.Emit(OpCodes.Stloc, containerLocalBuilder);
            ilGen.Emit(OpCodes.Ldloc, containerLocalBuilder);
            ilGen.Emit(OpCodes.Ldnull);
            ilGen.Emit(OpCodes.Ceq);
            ilGen.Emit(OpCodes.Brtrue, onNullLabel);
            ilGen.Emit(OpCodes.Ldloc, containerListLocalBuilder);
            ilGen.Emit(OpCodes.Ldloc, containerLocalBuilder);
            ilGen.Emit(OpCodes.Callvirt, addListMethod);

            ilGen.Emit(OpCodes.Nop);
            ilGen.MarkLabel(onNullLabel);
            ilGen.Emit(OpCodes.Nop);
            ilGen.Emit(OpCodes.Ldloc, countLocalBuilder);
            ilGen.Emit(OpCodes.Ldc_I4_1);
            ilGen.Emit(OpCodes.Add);
            ilGen.Emit(OpCodes.Stloc, countLocalBuilder);

            ilGen.MarkLabel(judgeLabel);
            ilGen.Emit(OpCodes.Ldloc, countLocalBuilder);
            ilGen.Emit(OpCodes.Ldloc, mappingListLocalBuilder);
            ilGen.Emit(OpCodes.Callvirt, getCountMethod);
            ilGen.Emit(OpCodes.Clt);
            ilGen.Emit(OpCodes.Brtrue, nextLabel);

            ilGen.Emit(OpCodes.Ldloc, containerListLocalBuilder);
            ilGen.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);
            
        }
        #endregion

        #region EmitDoGetContainerMethod
        private MethodBuilder EmitDoGetContainerMethod<TEntity>(int tierNumber, Type entityType, Type baseType, Type nestedDicType, TypeBuilder typeBuilder, Type[] nestedKeyTypes, FieldBuilder dicField, FieldBuilder columns4ClassifyField)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("DoGetContainer", MethodAttributes.Private, typeof(IObjectContainer<TEntity>), new Type[] { typeof(object[]) });
            ILGenerator ilGen = methodBuilder.GetILGenerator();

            LocalBuilder returnValBuilder = ilGen.DeclareLocal(typeof(IObjectContainer<TEntity>));
            Label returnLable = ilGen.DefineLabel();

            LocalBuilder[] valLocalBuilders = new LocalBuilder[nestedKeyTypes.Length];
            Type tempType = nestedDicType;

            LocalBuilder curDicBuilder = ilGen.DeclareLocal(tempType);
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldfld, dicField);
            ilGen.Emit(OpCodes.Stloc, curDicBuilder);

            for (int i = 0; i < nestedKeyTypes.Length; i++)
            {
                Label nextLable = ilGen.DefineLabel();

                MethodInfo dicContainsKeyMethod = tempType.GetMethod("ContainsKey");
                MethodInfo get_ItemMethod = tempType.GetMethod("get_Item");

                valLocalBuilders[i] = ilGen.DeclareLocal(nestedKeyTypes[i]);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Ldc_I4, i);
                ilGen.Emit(OpCodes.Ldelem_Ref);
                ilGen.Emit(OpCodes.Unbox_Any, nestedKeyTypes[i]);
                ilGen.Emit(OpCodes.Stloc, valLocalBuilders[i]);
                ilGen.Emit(OpCodes.Ldloc, curDicBuilder);
                ilGen.Emit(OpCodes.Ldloc, valLocalBuilders[i]);
                ilGen.Emit(OpCodes.Callvirt, dicContainsKeyMethod);
                ilGen.Emit(OpCodes.Brtrue, nextLable);
                ilGen.Emit(OpCodes.Nop);
                ilGen.Emit(OpCodes.Ldnull);
                ilGen.Emit(OpCodes.Stloc, returnValBuilder);
                ilGen.Emit(OpCodes.Br, returnLable);

                ilGen.MarkLabel(nextLable);

                if (i < nestedKeyTypes.Length - 1)
                {
                    tempType = tempType.GetGenericArguments()[1];

                    ilGen.Emit(OpCodes.Ldloc, curDicBuilder);
                    ilGen.Emit(OpCodes.Ldloc, valLocalBuilders[i]);
                    ilGen.Emit(OpCodes.Callvirt, get_ItemMethod);

                    curDicBuilder = ilGen.DeclareLocal(tempType);
                    ilGen.Emit(OpCodes.Stloc, curDicBuilder);
                }
            }

            Type tempType2 = nestedDicType;
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldfld, dicField);
            for (int i = 0; i < nestedKeyTypes.Length; i++)
            {
                MethodInfo get_ItemMethod = tempType2.GetMethod("get_Item");
                ilGen.Emit(OpCodes.Ldloc, valLocalBuilders[i]);
                ilGen.Emit(OpCodes.Callvirt, get_ItemMethod);

                tempType2 = tempType2.GetGenericArguments()[1];
            }

            ilGen.Emit(OpCodes.Stloc, returnValBuilder);

            ilGen.MarkLabel(returnLable);
            ilGen.Emit(OpCodes.Ldloc, returnValBuilder);
            ilGen.Emit(OpCodes.Ret);

            return methodBuilder;
        }
        #endregion

        #region EmitGetAllContainersMethod
        private void EmitGetAllContainersMethod(Type baseType,TypeBuilder typeBuilder, FieldBuilder containerListField)
        {
            MethodInfo baseMethod = ReflectionHelper.SearchMethod(baseType, "GetAllContainers", new Type[] { });
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetAllContainers", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            ILGenerator ilGen = methodBuilder.GetILGenerator();

            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldfld, containerListField);
            ilGen.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);
        } 
        #endregion

        #region EmitGetDistinctValuesMethod
        private void EmitGetDistinctValuesMethod(Type baseType, TypeBuilder typeBuilder ,Type[] nestedKeyTypes,FieldBuilder columns4ClassifyField, Dictionary<int, FieldBuilder> distinctArrayFieldInfoDic)
        {
            MethodInfo baseMethod = ReflectionHelper.SearchMethod(baseType, "GetDistinctValues", new Type[] {typeof(string) });
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetDistinctValues", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            ILGenerator ilGen = methodBuilder.GetILGenerator();

            MethodInfo equalMethod = typeof(string).GetMethod("Equals" ,new Type[]{typeof(object)});
            LocalBuilder resultLocalBuilder = ilGen.DeclareLocal(typeof(IList));
            
            ilGen.Emit(OpCodes.Ldnull);
            ilGen.Emit(OpCodes.Stloc, resultLocalBuilder);

            for (int i = 0; i < nestedKeyTypes.Length; i++)
            {
                MethodInfo getAllMethod = typeof(SortedArray<>).MakeGenericType(nestedKeyTypes[i]).GetMethod("GetAll", new Type[]{ });

                Label nextLabel = ilGen.DefineLabel();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, columns4ClassifyField);
                ilGen.Emit(OpCodes.Ldc_I4, i);
                ilGen.Emit(OpCodes.Ldelem_Ref);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Callvirt, equalMethod);
                ilGen.Emit(OpCodes.Brfalse, nextLabel);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, distinctArrayFieldInfoDic[i]);
                ilGen.Emit(OpCodes.Call, getAllMethod);
                ilGen.Emit(OpCodes.Stloc, resultLocalBuilder);

                ilGen.MarkLabel(nextLabel);
            }           

            ilGen.Emit(OpCodes.Ldloc, resultLocalBuilder);
            ilGen.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodBuilder, baseMethod);
        }
        #endregion

        #region EmitProperties4ClassifyProperty
        private void EmitProperties4ClassifyProperty(Type baseType, TypeBuilder typeBuilder, FieldBuilder columns4ClassifyField)
        {           
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty("Properties4Classify", PropertyAttributes.None, typeof(string[]), null);

            //定义属性的get方法
            MethodInfo baseMethod = ReflectionHelper.SearchMethod(baseType, "get_Properties4Classify", Type.EmptyTypes);
            MethodBuilder getPropertyBuilder = typeBuilder.DefineMethod("get_Properties4Classify", baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.CallingConvention, baseMethod.ReturnType, EmitHelper.GetParametersType(baseMethod));
            ILGenerator ilGen = getPropertyBuilder.GetILGenerator();

            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldfld, columns4ClassifyField);
            ilGen.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(getPropertyBuilder, baseMethod);
            propertyBuilder.SetGetMethod(getPropertyBuilder);
        }
        #endregion

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

        #region AdjustMappingValues static
        /// <summary>
        /// AdjustMappingValues 当属性匹配值为null时，可以将其替换为所有的区分值，并与其它属性匹配值做笛卡尔交集，得到多组匹配条件。
        /// 辅助以简化Emit实现。
        /// </summary>       
        public static List<object[]> AdjustMappingValues(object[] propertyValues4Classify, IList[] distinctValList)
        {
            List<object[]> resultList = new List<object[]>();
            Dictionary<int, IList<object>> dic = new Dictionary<int, IList<object>>();
            for (int i = 0; i < propertyValues4Classify.Length; i++)
            {
                if (propertyValues4Classify[i] == null)
                {
                    List<object> copy = new List<object>();
                    foreach (object obj in distinctValList[i])
                    {
                        copy.Add(obj);
                    }
                    dic.Add(i, copy);
                }
                else
                {
                    IList<object> list = new List<object>();
                    list.Add(propertyValues4Classify[i]);
                    dic.Add(i, list);
                }
            }

            SimpleCrossJoiner<object> crossJoiner = new SimpleCrossJoiner<object>();
            for (int i = 0; i < propertyValues4Classify.Length; i++)
            {
                crossJoiner.CrossJoin(dic[i]);
            }

            foreach (CrossJoinPath<object> path in crossJoiner.Result)
            {
                resultList.Add(path.Path.ToArray());
            }

            return resultList;
        } 
        #endregion
    }      
}
