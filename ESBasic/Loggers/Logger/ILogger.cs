using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// ILogger ������־��¼�Ļ����ӿڣ��̰߳�ȫ��
    /// </summary>
    public interface ILogger :IDisposable
    {
        void Log(string msg);
        void LogWithTime(string msg);
        bool Enabled { get;set; }       
    }

    public class EmptyLogger : ILogger
    {     
        #region ILogger ��Ա

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

        #region IDisposable ��Ա

        public void Dispose()
        {
            
        }

        #endregion
    }
}
