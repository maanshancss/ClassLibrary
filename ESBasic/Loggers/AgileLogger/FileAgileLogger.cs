using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Helpers;

namespace ESBasic.Loggers
{
    /// <summary>
    /// FileAgileLogger ����־��¼���ı��ļ����̰߳�ȫ��
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
        /// ����־�ļ����ӵ�һ���Ĵ�Сʱ��������һ���µ��ļ���¼��־��
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

        #region FileAgileLogger ��Ա
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

                string ss = string.Format("\n{0} : {1} ���� {2} ����������:{3}��λ�ã�{4}", DateTime.Now.ToString(), EnumDescription.GetFieldText(level), msg, errorType, location);

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

        #region IDisposable ��Ա

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
