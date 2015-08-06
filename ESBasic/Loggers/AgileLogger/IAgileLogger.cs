using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// IAgileLogger ���ڼ�¼��־��Ϣ��log���������׳��쳣��
    /// ͨ������ͨ��ESFramework.Common.AdvancedFunction.SetProperty ���������������־��¼����װ�䡣  
    /// zhuweisky
    /// </summary>
    public interface IAgileLogger
    {       
        void LogWithTime(string msg);
        void Log(string errorType, string msg, string location, ErrorLevel level);
        void Log(Exception ee, string location, ErrorLevel level);

        /// <summary>
        /// LogSimple ����¼�쳣�Ķ�ջλ��
        /// </summary>       
        void LogSimple(Exception ee, string location, ErrorLevel level);

        bool Enabled { set;}       
    }

    #region EmptyAgileLogger
    public sealed class EmptyAgileLogger : IAgileLogger
    {
        #region ILogger ��Ա

        public void Log(string errorType, string msg, string location, ErrorLevel level)
        {
            //Do Nothing !
        }

        public bool Enabled
        {
            set
            {
                // TODO:  ��� EmptyEsfLogger.Enabled setter ʵ��
            }
        }      

        #endregion

        #region IAgileLogger ��Ա
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
