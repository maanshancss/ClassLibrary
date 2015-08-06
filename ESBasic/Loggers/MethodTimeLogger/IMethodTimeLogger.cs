using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// IMethodTimeLogger �ýӿ����ڼ�¼������ִ��ʱ�䡣
    /// zhuweisky 2008.06.13
    /// </summary>
    public interface IMethodTimeLogger
    {
        /// <summary>
        /// ��¼����ִ�е�ʱ�䡣   
        /// </summary>        
        /// <param name="methodPath">�׳��쳣��Ŀ�귽����</param>
        /// <param name="genericTypes">Ŀ�귽�������Ͳ��������Ϊ�Ƿ��ͷ���������null</param>
        /// <param name="argumentNames">���÷����ĸ�Parameters�����ơ��������û�в���������null</param>
        /// <param name="argumentValues">���÷����ĸ�Parameters��ֵ���������û�в���������null</param>
        /// <param name="millisecondsConsumed">����ִ�е�ʱ�䣬����</param>
        void Log(string methodPath, Type[] genericTypes, string[] argumentNames, object[] argumentValues, double millisecondsConsumed);
    }

    public class EmptyMethodTimeLogger : IMethodTimeLogger
    {
        public void Log(string methodPath, Type[] genericTypes, string[] argumentNames, object[] argumentValues, double millisecondsConsumed)
        {
            
        }
    }

}
