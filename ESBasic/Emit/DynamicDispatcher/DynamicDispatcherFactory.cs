using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Emit.DynamicDispatcher
{
    public static class DynamicDispatcherFactory
    {
        private static DynamicDispatcherEmitter DynamicDispatcherEmitter = new DynamicDispatcherEmitter();    

        /// <summary>
        /// CreateDispatcher 创建实现了TIDispatch接口的动态分发器。
        /// </summary>
        /// <typeparam name="TIDispatch">要分发调用的接口类型</typeparam>
        /// <param name="excuters">分发器实例集合</param>
        /// <returns>实现了TIDispatch接口的动态分发器</returns>
        public static TIDispatch CreateDispatcher<TIDispatch>(IEnumerable<TIDispatch> excuters)
        {
            lock (DynamicDispatcherFactory.DynamicDispatcherEmitter)
            {
                Type dispatcherType = DynamicDispatcherFactory.DynamicDispatcherEmitter.CreateDispatcherType(typeof(TIDispatch));
                //DynamicDispatcherFactory.DynamicDispatcherEmitter.Save();
                TIDispatch dispatcher = (TIDispatch)Activator.CreateInstance(dispatcherType, excuters);
                return dispatcher;
            }
        }        
    }
}
