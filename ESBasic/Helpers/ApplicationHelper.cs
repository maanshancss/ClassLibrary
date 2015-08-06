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
        /// StartApplication ����һ��Ӧ�ó���/����
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
        /// IsAppInstanceExist Ŀ��Ӧ�ó����Ƿ��Ѿ�������ͨ�������жϵ�ʵ��Ӧ�á���ռ������
        /// </summary>       
        public static bool IsAppInstanceExist(string instanceName)
        {
            
            bool createdNew = false;
            ApplicationHelper.MutexForSingletonExe = new Mutex(false, instanceName, out createdNew);
            
            return (!createdNew);
        }

        private static System.Threading.Mutex MutexForSingletonExe = null;

        /// <summary>
        /// �ͷ���IsAppInstanceExistռ�õ�����
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
        /// OpenUrl ��������д�wsUrl����
        /// </summary>        
        public static void OpenUrl(string url)
        {
            Process.Start(url);
        } 
        #endregion        
    }
}
