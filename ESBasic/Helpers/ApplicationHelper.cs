using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace ESBasic.Helpers
{
    public static class ApplicationHelper
    {
        #region StartApplication 
        /// <summary>
        /// StartApplication 启动一个应用程序/进程
        /// </summary>       
        public static void StartApplication(string appFilePath)
        {
            Process downprocess = new Process();
            downprocess.StartInfo.FileName = appFilePath;
            downprocess.Start();
        }
        #endregion

        #region IsAppInstanceExist
        /// <summary>
        /// IsAppInstanceExist 目标应用程序是否已经启动。通常用于判断单实例应用。将占用锁。
        /// </summary>       
        public static bool IsAppInstanceExist(string instanceName)
        {
            
            bool createdNew = false;
            ApplicationHelper.MutexForSingletonExe = new Mutex(false, instanceName, out createdNew);
            
            return (!createdNew);
        }

        private static System.Threading.Mutex MutexForSingletonExe = null;

        /// <summary>
        /// 释放由IsAppInstanceExist占用的锁。
        /// </summary>        
        public static void ReleaseAppInstance(string instanceName)
        {
            if (ApplicationHelper.MutexForSingletonExe != null)
            {                
                ApplicationHelper.MutexForSingletonExe.Close();
                ApplicationHelper.MutexForSingletonExe = null;
            }
        }
        #endregion

        #region OpenUrl
        /// <summary>
        /// OpenUrl 在浏览器中打开wsUrl链接
        /// </summary>        
        public static void OpenUrl(string url)
        {
            Process.Start(url);
        } 
        #endregion        
    }
}
