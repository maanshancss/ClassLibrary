using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    public class ExceptionFileLogger : IExceptionLogger
    {
        public ExceptionFileLogger() { }
        public ExceptionFileLogger(IAgileLogger logger)
            : this(logger, ErrorLevel.Standard)
        {

        }

        public ExceptionFileLogger(IAgileLogger logger, ErrorLevel _errorLevel)
        {
            this.agileLogger = logger;
            this.errorLevel = _errorLevel;
        }

        #region AgileLogger
        protected IAgileLogger agileLogger;
        public IAgileLogger AgileLogger
        {
            set { agileLogger = value; }
        }
        #endregion

        #region ErrorLevel
        protected ErrorLevel errorLevel = ErrorLevel.Standard;
        public ErrorLevel ErrorLevel
        {
            set { errorLevel = value; }
        }
        #endregion

        public void Log(Exception ee, string methodPath, Type[] genericTypes, string[] argumentNames, object[] argumentValues)
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

            this.agileLogger.Log(ee, location, this.errorLevel);
        }     
    } 
}
