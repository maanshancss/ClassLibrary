using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ESBasic.Emit.DynamicBridge
{
    public static class DynamicEventBridgeFactory
    {
        private static DynamicEventBridgeEmitter DynamicEventBridgeEmitter = new DynamicEventBridgeEmitter(false);

        /// <summary>
        /// CreateEventBridge 创建实现了IEventBridge接口的实例。注意返回实例后，要调用其Initialize方法完成初始化。
        /// </summary>
        /// <param name="eventPublisher">发布事件的对象</param>
        /// <param name="eventHandler">包含了事件处理器方法的对象</param>
        /// <param name="eventHandlerNamePrefix">处理器方法的名称的前缀（即该前缀加上事件名称就得到处理器方法的名称）</param>
        /// <returns>实现了IEventBridge接口的实例。</returns>
        public static IEventBridge CreateEventBridge(object eventPublisher ,object eventHandler ,string eventHandlerNamePrefix)
        {
            lock (DynamicEventBridgeFactory.DynamicEventBridgeEmitter)
            {
                Type eventBridgeType = DynamicEventBridgeFactory.DynamicEventBridgeEmitter.EmitEventBridgeType(eventPublisher.GetType(), eventHandler.GetType(), eventHandlerNamePrefix);
                IEventBridge bridge = (IEventBridge)Activator.CreateInstance(eventBridgeType, eventPublisher, eventHandler);
                return bridge;
            }
        }

        /// <summary>
        /// CreateEventBridge 创建实现了IEventBridge接口的实例。注意返回实例后，要调用其Initialize方法完成初始化。
        /// </summary>
        /// <typeparam name="TPublisher">发布事件的类型（通常为接口），暴露了要匹配的事件</typeparam>
        /// <typeparam name="THandler">包含了事件处理方法的接口类型</typeparam>
        /// <param name="eventPublisher">发布事件的对象</param>
        /// <param name="eventHandler">包含了事件处理器方法的对象</param>
        /// <param name="eventHandlerNamePrefix">处理器方法的名称的前缀（即该前缀加上事件名称就得到处理器方法的名称）</param>
        /// <returns>实现了IEventBridge接口的实例。</returns>       
        public static IEventBridge CreateEventBridge<TPublisher, THandler>(TPublisher eventPublisher, THandler eventHandler, string eventHandlerNamePrefix)
        {
            lock (DynamicEventBridgeFactory.DynamicEventBridgeEmitter)
            {
                Type eventBridgeType = DynamicEventBridgeFactory.DynamicEventBridgeEmitter.EmitEventBridgeType(typeof(TPublisher), typeof(THandler), eventHandlerNamePrefix);
                //DynamicEventBridgeFactory.DynamicEventBridgeEmitter.Save();
                ConstructorInfo ctor = eventBridgeType.GetConstructors()[0];
                IEventBridge bridge =  (IEventBridge)ctor.Invoke(new object[]{(TPublisher)eventPublisher, (THandler)eventHandler});//(IEventBridge)Activator.CreateInstance(eventBridgeType, (TPublisher)eventPublisher, (THandler)eventHandler);
                return bridge;
            }
        }

        /// <summary>
        /// CreateEventBridge 创建实现了IEventBridge接口的实例。注意返回实例后，要调用其Initialize方法完成初始化。
        /// </summary>
        /// <param name="eventPublisher">发布事件的对象</param>
        /// <param name="eventHandler">包含了事件处理器方法的对象</param>
        /// <param name="eventAndHanlerMapping">事件名称与处理器方法名称的映射</param>
        /// <returns>实现了IEventBridge接口的实例。</returns>
        public static IEventBridge CreateEventBridge(object eventPublisher, object eventHandler, IDictionary<string, string> eventAndHanlerMapping)
        {
            lock (DynamicEventBridgeFactory.DynamicEventBridgeEmitter)
            {
                Type eventBridgeType = DynamicEventBridgeFactory.DynamicEventBridgeEmitter.EmitEventBridgeType(eventPublisher.GetType(), eventHandler.GetType(), eventAndHanlerMapping);
                IEventBridge bridge = (IEventBridge)Activator.CreateInstance(eventBridgeType, eventPublisher, eventHandler);
                return bridge;
            }
        }

        /// <summary>
        /// CreateEventBridge 创建实现了IEventBridge接口的实例。注意返回实例后，要调用其Initialize方法完成初始化。
        /// </summary>
        /// <typeparam name="TPublisher">发布事件的类型（通常为接口），暴露了要匹配的事件</typeparam>
        /// <typeparam name="THandler">包含了事件处理方法的接口类型</typeparam>
        /// <param name="eventPublisher">发布事件的对象</param>
        /// <param name="eventHandler">包含了事件处理器方法的对象</param>
        /// <param name="eventAndHanlerMapping">事件名称与处理器方法名称的映射</param>
        /// <returns>实现了IEventBridge接口的实例。</returns>
        public static IEventBridge CreateEventBridge<TPublisher, THandler>(TPublisher eventPublisher, THandler eventHandler, IDictionary<string, string> eventAndHanlerMapping)
        {
            lock (DynamicEventBridgeFactory.DynamicEventBridgeEmitter)
            {
                Type eventBridgeType = DynamicEventBridgeFactory.DynamicEventBridgeEmitter.EmitEventBridgeType(typeof(TPublisher), typeof(THandler), eventAndHanlerMapping);
                ConstructorInfo ctor = eventBridgeType.GetConstructors()[0];
                IEventBridge bridge = (IEventBridge)ctor.Invoke(new object[] { (TPublisher)eventPublisher, (THandler)eventHandler });//(IEventBridge)Activator.CreateInstance(eventBridgeType, (TPublisher)eventPublisher, (THandler)eventHandler);
                return bridge;
            }
        }      
    }
}
