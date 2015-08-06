using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic
{
    /// <summary>
    /// IUrgentExceptionReporter 紧急异常报告者。
    /// </summary>
    public interface IUrgentExceptionReporter
    {
        void CommitUrgentException(string description, string location, Exception ee);
    }

    #region EmptyUrgentExceptionReporter
    public sealed class EmptyUrgentExceptionReporter : IUrgentExceptionReporter
    {
        #region IUrgentExceptionReporter 成员
        public void CommitUrgentException(string description, string location, Exception ee)
        {
        }
        #endregion
    } 
    #endregion
}
