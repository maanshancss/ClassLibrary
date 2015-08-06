using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// ILogger 用于日志记录的基础接口，线程安全的
    /// </summary>
    public interface ILogger :IDisposable
    {
        void Log(string msg);
        void LogWithTime(string msg);
        bool Enabled { get;set; }       
    }

    public class EmptyLogger : ILogger
    {     
        #region ILogger 成员

        #region Log
        public void Log(string msg)
        {
          
        } 
        #endregion

        #region LogWithTime
        public void LogWithTime(string msg)
        {
          
        } 
        #endregion

        #endregion

        #region Enabled    
        public bool Enabled
        {
            get { return true; }
            set { }
        } 
        #endregion       

        #region IDisposable 成员

        public void Dispose()
        {
            
        }

        #endregion
    }
}
