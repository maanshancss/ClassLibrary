using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Emit.DynamicBridge
{
    /// <summary>
    /// IEventBridge 事件桥，用于将A对象的事件与B对象提供的处理器方法桥接起来。
    /// 要求：事件的参数与处理器方法的参数完全一致。
    /// zhuweisky 2010.04.01
    /// </summary>
    public interface IEventBridge
    {
        /// <summary>
        /// Initialize 用于完成A对象的事件与B对象处理器的关联。
        /// </summary>
        void Initialize();
    }
}
