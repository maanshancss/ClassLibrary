using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// IMethodTimeLogger 该接口用于记录方法的执行时间。
    /// zhuweisky 2008.06.13
    /// </summary>
    public interface IMethodTimeLogger
    {
        /// <summary>
        /// 记录方法执行的时间。   
        /// </summary>        
        /// <param name="methodPath">抛出异常的目标方法。</param>
        /// <param name="genericTypes">目标方法的类型参数。如果为非泛型方法，则传入null</param>
        /// <param name="argumentNames">调用方法的各Parameters的名称。如果方法没有参数，则传入null</param>
        /// <param name="argumentValues">调用方法的各Parameters的值。如果方法没有参数，则传入null</param>
        /// <param name="millisecondsConsumed">方法执行的时间，毫秒</param>
        void Log(string methodPath, Type[] genericTypes, string[] argumentNames, object[] argumentValues, double millisecondsConsumed);
    }

    public class EmptyMethodTimeLogger : IMethodTimeLogger
    {
        public void Log(string methodPath, Type[] genericTypes, string[] argumentNames, object[] argumentValues, double millisecondsConsumed)
        {
            
        }
    }

}
