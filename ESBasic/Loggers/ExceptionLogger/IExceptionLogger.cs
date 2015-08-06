using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// IExceptionLogger ���ڼ�¼�쳣����ϸ��Ϣ��
    /// ע��DataRabbit�ṩ�˼�¼�����ݱ��ʵ�֡�
    /// </summary>
    public interface IExceptionLogger
    {
        /// <summary>
        /// ��¼�쳣��   
        /// </summary>
        /// <param name="ee">�쳣</param>
        /// <param name="methodPath">�׳��쳣��Ŀ�귽����</param>
        /// <param name="genericTypes">Ŀ�귽�������Ͳ��������Ϊ�Ƿ��ͷ���������null</param>
        /// <param name="argumentNames">���÷����ĸ�Parameters�����ơ��������û�в���������null</param>
        /// <param name="argumentValues">���÷����ĸ�Parameters��ֵ���������û�в���������null</param>
        void Log(Exception ee, string methodPath, Type[] genericTypes, string[] argumentNames, object[] argumentValues);      
    }

    public class EmptyExceptionLogger : IExceptionLogger
    {       
        public void Log(Exception ee, string methodPath, Type[] genericTypes, string[] argumentNames, object[] argumentValues) 
        {
            
        }        
    }

}
