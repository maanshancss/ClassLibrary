using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Loggers
{
    /// <summary>
    /// FileLogger 将日志记录到文本文件。FileLogger是线程安全的。
    /// </summary>
    public class FileLogger :ILogger
    {
        private StreamWriter writer;
        private string iniPath;

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
            }
        } 
        #endregion

        #region Ctor
        public FileLogger(string filePath)
        {
            if (!File.Exists(filePath))
            {
                FileStream fs = File.Create(filePath);
                fs.Close();
            }

            this.iniPath = filePath;
            this.writer = new StreamWriter(File.Open(filePath, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write, FileShare.Read));            
        }

        ~FileLogger()
        {
            this.Close();
        }
        #endregion

        #region ILogger 成员

        #region Log
        public void Log(string msg)
        {
            if (!this.enabled)
            {
                return;
            }

            lock (this.writer)
            {
                this.writer.WriteLine(msg + "\n");
                this.writer.Flush();
                this.CheckAndChangeNewFile();
            }
        }

        private void CheckAndChangeNewFile()
        {
            if (this.writer.BaseStream.Length >= this.maxLength)
            {                
                this.writer.Close();
                this.writer = null;

                string fileName = ESBasic.Helpers.FileHelper.GetFileNameNoPath(this.iniPath);
                string dir = ESBasic.Helpers.FileHelper.GetFileDirectory(this.iniPath);
                int pos = fileName.LastIndexOf('.');
                string extendName = null;
                string pureName = fileName;
                if (pos >=0)
                {
                    extendName = fileName.Substring(pos+1);
                    pureName = fileName.Substring(0, pos);
                }

                string newPath = null;
                for(int i=1;i<1000;i++)
                {
                    string newName = pureName + "_" + i.ToString("000");
                    if (extendName != null)
                    {
                        newName += "." + extendName;
                    }
                    newPath = dir + "\\" + newName;
                    if (!File.Exists(newPath))
                    {
                        break;
                    }
                }                
                this.writer = new StreamWriter(File.Open(newPath, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write, FileShare.Read));  
            }
        }
        #endregion

        #region LogWithTime
        public void LogWithTime(string msg)
        {
            string formatMsg = string.Format("{0}:{1}", DateTime.Now.ToString(), msg);
            this.Log(formatMsg);
        } 
        #endregion

        #region Close
        private void Close()
        {            
            if (this.writer != null)
            {
                try
                {
                    this.writer.Close();
                    this.writer = null;
                }
                catch { }
            }
        } 
        #endregion

        #region Enabled
        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        } 
        #endregion       

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            this.Close();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
