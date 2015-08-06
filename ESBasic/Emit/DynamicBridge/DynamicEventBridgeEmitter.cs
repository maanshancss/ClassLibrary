using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using ESBasic.Helpers;

namespace ESBasic.Emit.DynamicBridge
{
    /// <summary>
    /// DynamicEventBridgeEmitter 用于发射实现了IEventBridge接口的事件桥类型。
    /// 发射得到的动态类型的构造参数将接收两个类型的参数：typeOfEventPublisher，typeOfEventHandler
    /// </summary>
    public class DynamicEventBridgeEmitter
    {
        private bool saveFile = false;
        private string assemblyName = "DynamicEventBridgeAssembly";
        private string theFileName;
        private AssemblyBuilder dynamicAssembly;
        protected ModuleBuilder moduleBuilder;

        private IDictionary<string, Type> eventBridgeTypeDictionary = new Dictionary<string, Type>();//DynamicTypeName -- DynamicType

        #region Ctor
        public DynamicEventBridgeEmitter() :this(false)
        {
        }

        public DynamicEventBridgeEmitter(bool save)
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

        #region EmitEventBridgeType
        /// <summary>
        /// EmitEventBridgeType 发射实现了IEventBridge接口的事件桥类型
        /// </summary>
        /// <param name="typeOfEventPublisher">发布事件的类型</param>
        /// <param name="typeOfEventHandler">包含了事件处理器方法的类型</param>
        /// <param name="eventHandlerNamePrefix">处理器方法的名称的前缀（即该前缀加上事件名称就得到处理器方法的名称）</param>
        /// <returns>实现了IEventBridge接口的事件桥类型，其构造参数为：typeOfEventPublisher，typeOfEventHandler</returns>
        public Type EmitEventBridgeType(Type typeOfEventPublisher, Type typeOfEventHandler ,string eventHandlerNamePrefix)
        {
            Dictionary<string, string> eventAndHanlerMapping = new Dictionary<string, string>();
            foreach (EventInfo eventInfo in typeOfEventPublisher.GetEvents())
            {               
                string handlerName = eventHandlerNamePrefix + eventInfo.Name ;
                MethodInfo method = typeOfEventHandler.GetMethod(handlerName) ;
                if (method == null)
                {
                    throw new Exception(string.Format("Can't find proper handler for {0}.{1} event in {2}!", typeOfEventPublisher, eventInfo.Name, typeOfEventHandler));
                }

                Type[] argTypes = EmitHelper.GetParametersType(method);
                Type[] eventArgTypes = EmitHelper.GetParametersType(eventInfo.EventHandlerType.GetMethod("Invoke"));
                if (argTypes.Length != eventArgTypes.Length)
                {
                    throw new Exception(string.Format("Can't find proper handler for {0}.{1} event in {2}!", typeOfEventPublisher, eventInfo.Name, typeOfEventHandler));          
                }

                for (int i = 0; i < argTypes.Length; i++)
                {
                    if (argTypes[i] != eventArgTypes[i])
                    {
                        throw new Exception(string.Format("Can't find proper handler for {0}.{1} event in {2}!", typeOfEventPublisher, eventInfo.Name, typeOfEventHandler));                      
                    }
                }

                eventAndHanlerMapping.Add(eventInfo.Name, handlerName);
            }

            return this.EmitEventBridgeType(typeOfEventPublisher, typeOfEventHandler ,eventAndHanlerMapping);
        }

        /// <summary>
        /// EmitEventBridgeType 发射实现了IEventBridge接口的事件桥类型
        /// </summary>
        /// <param name="typeOfEventPublisher">发布事件的类型</param>
        /// <param name="typeOfEventHandler">包含了事件处理器方法的类型</param>
        /// <param name="eventAndHanlerMapping">事件名称与处理器方法名称的映射</param>
        /// <returns>实现了IEventBridge接口的事件桥类型，其构造参数为：typeOfEventPublisher，typeOfEventHandler</returns>
        public Type EmitEventBridgeType(Type typeOfEventPublisher, Type typeOfEventHandler, IDictionary<string, string> eventAndHanlerMapping)
        {
            string typeName = string.Format("{0}_{1}_DynamicEventBridge", TypeHelper.GetClassSimpleName(typeOfEventPublisher), TypeHelper.GetClassSimpleName(typeOfEventHandler));
            if (this.eventBridgeTypeDictionary.ContainsKey(typeName))
            {
                return this.eventBridgeTypeDictionary[typeName];
            }

            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);
            typeBuilder.AddInterfaceImplementation(typeof(IEventBridge));
            //定义成员
            FieldInfo handlerField = typeBuilder.DefineField("handler", typeOfEventHandler, FieldAttributes.Private);
            FieldInfo eventPublisherField = typeBuilder.DefineField("eventPublisher", typeOfEventPublisher, FieldAttributes.Private);

            #region Emit Ctor
            ConstructorBuilder ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeOfEventPublisher, typeOfEventHandler });
            ILGenerator ctorGen = ctor.GetILGenerator();
            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[] { })); //调用基类的构造函数
            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Ldarg_1);
            ctorGen.Emit(OpCodes.Stfld, eventPublisherField);

            ctorGen.Emit(OpCodes.Ldarg_0);
            ctorGen.Emit(OpCodes.Ldarg_2);
            ctorGen.Emit(OpCodes.Stfld, handlerField);

            ctorGen.Emit(OpCodes.Ret);
            #endregion

            #region 定义current的事件处理方法
            //定义事件处理方法            
            Dictionary<string, MethodInfo> handlerMethodInfoDic = new Dictionary<string, MethodInfo>();
            foreach (string eventName in eventAndHanlerMapping.Keys)
            {
                string handlerMethodName = eventAndHanlerMapping[eventName];
                MethodInfo handlerMethod = typeOfEventHandler.GetMethod(handlerMethodName);
                Type[] argTypes = EmitHelper.GetParametersType(handlerMethod);

                MethodBuilder methodBuilder = typeBuilder.DefineMethod("On" + eventName, MethodAttributes.Private, typeof(void), argTypes);
                ILGenerator methodGen = methodBuilder.GetILGenerator();
                methodGen.Emit(OpCodes.Ldarg_0);
                methodGen.Emit(OpCodes.Ldfld, handlerField);
                int argIndex = 0;
                foreach (Type argType in argTypes)
                {
                    EmitHelper.LoadArg(methodGen, argIndex + 1);
                    argIndex++;
                }

                methodGen.Emit(OpCodes.Callvirt, handlerMethod);
                methodGen.Emit(OpCodes.Ret);

                handlerMethodInfoDic.Add("On" + eventName, methodBuilder);
            }
            #endregion

            #region 定义Initialize方法
            //定义Initialize方法
            MethodInfo baseIniMethod = typeof(IEventBridge).GetMethod("Initialize");
            MethodBuilder iniMethodBuilder = EmitHelper.DefineDerivedMethodSignature(typeBuilder, baseIniMethod);
            ILGenerator iniMethodGen = iniMethodBuilder.GetILGenerator();
            //预定事件
            foreach (string eventName in eventAndHanlerMapping.Keys)
            {
                EventInfo eventInfo = typeOfEventPublisher.GetEvent(eventName, BindingFlags.Public | BindingFlags.Default | BindingFlags.Instance);
                MethodInfo addMethod = eventInfo.GetAddMethod();
                string handlerMethodName = "On" + eventName;
                iniMethodGen.Emit(OpCodes.Ldarg_0);
                iniMethodGen.Emit(OpCodes.Ldfld, eventPublisherField);
                iniMethodGen.Emit(OpCodes.Ldarg_0);
                iniMethodGen.Emit(OpCodes.Ldftn, handlerMethodInfoDic[handlerMethodName]);
                iniMethodGen.Emit(OpCodes.Newobj, eventInfo.EventHandlerType.GetConstructor(new Type[] { typeof(object), typeof(IntPtr) }));
                iniMethodGen.Emit(OpCodes.Callvirt, addMethod);
            }
            iniMethodGen.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(iniMethodBuilder, baseIniMethod);
            #endregion

            Type target = typeBuilder.CreateType();

            this.eventBridgeTypeDictionary.Add(typeName, target);
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
