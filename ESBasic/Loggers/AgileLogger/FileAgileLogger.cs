using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Helpers;

namespace ESBasic.Loggers
{
    /// <summary>
    /// FileAgileLogger 将日志记录到文本文件。线程安全。
    /// </summary>
    public sealed class FileAgileLogger : IAgileLogger, IDisposable
    {
        private FileLogger fileLogger;

        #region FilePath
        private string filePath = "";
        public string FilePath
        {
            get
            {
                return this.filePath;
            }
            set
            {
                this.filePath = value;
            }
        }
        #endregion

        #region MaxLength
        private long maxLength = long.MaxValue;
        /// <summary>
        /// 当日志文件增加到一定的大小时，将创建一个新的文件记录日志。
        /// </summary>
        public long MaxLength4ChangeFile
        {
            get { return maxLength; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("MaxLength4ChangeFile must greater than 0.");
                }
                maxLength = value;
                if (this.fileLogger != null)
                {
                    this.fileLogger.MaxLength4ChangeFile = value;
                }
            }
        }
        #endregion

        #region Ctor
        public FileAgileLogger()
        {
        }

        public FileAgileLogger(string file_Path)
        {
            this.filePath = file_Path;
        }
        #endregion

        #region FileLogger
        private FileLogger FileLogger
        {
            get
            {
                if (this.fileLogger == null)
                {
                    this.fileLogger = new FileLogger(this.filePath);
                    this.fileLogger.MaxLength4ChangeFile = this.maxLength;
                }

                return this.fileLogger;
            }
        }
        #endregion

        #region FileAgileLogger 成员
        public void LogWithTime(string msg)
        {
            try
            {
                if (!this.enabled)
                {
                    return;
                }

                this.FileLogger.LogWithTime(msg);
            }
            catch (Exception ee)
            {
                ee = ee;
            }
        }

        public void Log(string errorType, string msg, string location, ErrorLevel level)
        {
            try
            {
                if (!this.enabled)
                {
                    return;
                }

                string ss = string.Format("\n{0} : {1} －－ {2} 。错误类型:{3}。位置：{4}", DateTime.Now.ToString(), EnumDescription.GetFieldText(level), msg, errorType, location);

                this.FileLogger.Log(ss);
            }
            catch (Exception ee)
            {
                ee = ee;
            }
        }

        public void Log(Exception ee, string location, ErrorLevel level)
        {
            string methodAddress = "";
            if (ee is NullReferenceException)
            {
                try
                {
                    methodAddress += "[" + NullReferenceHelper.GetExceptionMethodAddress(ee) + "] at ";
                }
                catch { }
            }


            string msg = ee.Message + " [:] " + methodAddress + ee.StackTrace;
            this.Log(ee.GetType().ToString(), msg, location, level);
        }

        public void LogSimple(Exception ee, string location, ErrorLevel level)
        {
            this.Log(ee.GetType().ToString(), ee.Message, location, level);
        }

        #region Enabled
        private bool enabled = true;
        public bool Enabled
        {
            set
            {
                this.enabled = value;
            }
        }
        #endregion     

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (this.fileLogger != null)
            {
                this.fileLogger.Dispose();
            }
        }

        #endregion
    }
}
