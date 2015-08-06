using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// IAgileLogger 用于记录日志信息。log方法不会抛出异常！
    /// 通常可以通过ESFramework.Common.AdvancedFunction.SetProperty 方法来简化组件的日志记录器的装配。  
    /// zhuweisky
    /// </summary>
    public interface IAgileLogger
    {       
        void LogWithTime(string msg);
        void Log(string errorType, string msg, string location, ErrorLevel level);
        void Log(Exception ee, string location, ErrorLevel level);

        /// <summary>
        /// LogSimple 不记录异常的堆栈位置
        /// </summary>       
        void LogSimple(Exception ee, string location, ErrorLevel level);

        bool Enabled { set;}       
    }

    #region EmptyAgileLogger
    public sealed class EmptyAgileLogger : IAgileLogger
    {
        #region ILogger 成员

        public void Log(string errorType, string msg, string location, ErrorLevel level)
        {
            //Do Nothing !
        }

        public bool Enabled
        {
            set
            {
                // TODO:  添加 EmptyEsfLogger.Enabled setter 实现
            }
        }      

        #endregion

        #region IAgileLogger 成员
        public void Log(Exception ee, string location, ErrorLevel level)
        {
          
        }

        public void LogSimple(Exception ee, string location, ErrorLevel level)
        {
           
        }       

        public void LogWithTime(string msg)
        {
            
        }

        #endregion
    }
    #endregion	
}
