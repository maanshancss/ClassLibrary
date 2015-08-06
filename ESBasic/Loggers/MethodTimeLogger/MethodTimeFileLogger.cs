using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// 将方法的执行时间记录到文本文件。
    /// </summary>
    public class MethodTimeFileLogger : IMethodTimeLogger
    {
        #region ctor
        public MethodTimeFileLogger() { }
        public MethodTimeFileLogger(IAgileLogger logger, int _minSpanInMSecsToLog)
        {
            this.agileLogger = logger;
            this.minSpanInMSecsToLog = _minSpanInMSecsToLog;
        }
        #endregion

        #region AgileLogger
        private IAgileLogger agileLogger;
        public IAgileLogger AgileLogger
        {
            set { agileLogger = value; }
        }
        #endregion

        #region MinSpanInMSecsToLog
        private int minSpanInMSecsToLog = 0;
        public int MinSpanInMSecsToLog
        {
            set { minSpanInMSecsToLog = value; }
            get { return minSpanInMSecsToLog; }
        }
        #endregion             

        #region IMethodTimeLogger 成员      

        public void Log(string methodPath, Type[] genericTypes, string[] argumentNames, object[] argumentValues, double millisecondsConsumed)
        {
            if (millisecondsConsumed >= this.minSpanInMSecsToLog)
            {
                StringBuilder methodPathBuilder = new StringBuilder(methodPath);
                if (genericTypes != null && genericTypes.Length > 0)
                {
                    methodPathBuilder.Append("<");
                    for (int i = 0; i < genericTypes.Length; i++)
                    {
                        methodPathBuilder.Append(genericTypes[i].ToString());
                        if (i != (genericTypes.Length - 1))
                        {
                            methodPathBuilder.Append(",");
                        }
                    }
                    methodPathBuilder.Append(">");
                }

                string location = methodPathBuilder.ToString();
                if (argumentNames != null && argumentNames.Length > 0)
                {
                    StringBuilder methodParaInfoBuilder = new StringBuilder("<Parameters>");
                    for (int i = 0; i < argumentNames.Length; i++)
                    {
                        methodParaInfoBuilder.Append(string.Format("<{0}>{1}</{0}>", argumentNames[i], argumentValues[i] == null ? "NULL" : argumentValues[i].ToString()));
                    }
                    methodParaInfoBuilder.Append("</Parameters>");

                    location += "@" + methodParaInfoBuilder.ToString();
                }

                string msg = string.Format("{0}，耗时:{1}ms", location, millisecondsConsumed);
                this.agileLogger.LogWithTime(msg);
            }
        }

        #endregion
    } 
}
