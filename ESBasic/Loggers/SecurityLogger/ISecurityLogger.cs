using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// 用于记录安全日志。比如用户的登陆/退出、进入/注销等日志。
    /// 注：DataRabbit提供了记录到数据表的实现。
    /// zhuweisky 2011.02.21
    /// </summary>
    public interface ISecurityLogger
    {
        /// <summary>
        /// 记录安全日志。
        /// </summary>
        /// <param name="userID">进行安全操作的用户编号</param>
        /// <param name="source">来源（比如用户的IP）</param>
        /// <param name="taskType">安全操作的类型</param>
        /// <param name="comment">备注</param>
        void Log(string userID, string source, string taskType ,string comment);
    }

    public class EmptySecurityLogger : ISecurityLogger
    {
        public void Log(string userID, string source, string taskType, string comment)
        {           
        }
    }

}
