using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic
{
    /// <summary>
    /// IUrgentExceptionReporter �����쳣�����ߡ�
    /// </summary>
    public interface IUrgentExceptionReporter
    {
        void CommitUrgentException(string description, string location, Exception ee);
    }

    #region EmptyUrgentExceptionReporter
    public sealed class EmptyUrgentExceptionReporter : IUrgentExceptionReporter
    {
        #region IUrgentExceptionReporter ��Ա
        public void CommitUrgentException(string description, string location, Exception ee)
        {
        }
        #endregion
    } 
    #endregion
}
