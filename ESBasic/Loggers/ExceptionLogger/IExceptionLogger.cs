using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// IExceptionLogger 用于记录异常的详细信息。
    /// 注：DataRabbit提供了记录到数据表的实现。
    /// </summary>
    public interface IExceptionLogger
    {
        /// <summary>
        /// 记录异常。   
        /// </summary>
        /// <param name="ee">异常</param>
        /// <param name="methodPath">抛出异常的目标方法。</param>
        /// <param name="genericTypes">目标方法的类型参数。如果为非泛型方法，则传入null</param>
        /// <param name="argumentNames">调用方法的各Parameters的名称。如果方法没有参数，则传入null</param>
        /// <param name="argumentValues">调用方法的各Parameters的值。如果方法没有参数，则传入null</param>
        void Log(Exception ee, string methodPath, Type[] genericTypes, string[] argumentNames, object[] argumentValues);      
    }

    public class EmptyExceptionLogger : IExceptionLogger
    {       
        public void Log(Exception ee, string methodPath, Type[] genericTypes, string[] argumentNames, object[] argumentValues) 
        {
            
        }        
    }

}
