using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// 将安全日志记录到文本文件。
    /// </summary>
    public class SecurityFileLogger :ISecurityLogger
    {
        #region Ctor
        public SecurityFileLogger() { }
        public SecurityFileLogger(IAgileLogger logger)
        {
            this.agileLogger = logger;
        } 
        #endregion

        #region AgileLogger
        private IAgileLogger agileLogger;
        public IAgileLogger AgileLogger
        {
            set { agileLogger = value; }
        }
        #endregion

        public void Log(string userID, string source, string taskType, string comment)
        {
            string msg = string.Format("User:{0},Source:{1},TaskType:{2},Comment:{3}.", userID, source, taskType, comment);
            this.agileLogger.LogWithTime(msg);
        }
    }
}
