using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Loggers;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// RefreshableCacheExceptionLogBridge 使用IAgileLogger来记录IRefreshableCacheManager刷新失败的日志。
    /// </summary>
    public class RefreshableCacheExceptionLogBridge
    {
        #region AgileLogger
        private IAgileLogger agileLogger;
        public IAgileLogger AgileLogger
        {
            set { agileLogger = value; }
        } 
        #endregion

        #region RefreshableCacheManager
        private IRefreshableCacheManager refreshableCacheManager;
        public IRefreshableCacheManager RefreshableCacheManager
        {
            set { refreshableCacheManager = value; }
        } 
        #endregion

        public void Initialize()
        {
            this.refreshableCacheManager.CacheRefreshFailed += new CbCacheException(cacheManager_CacheRefreshFailed);
        }

        void cacheManager_CacheRefreshFailed(IRefreshableCache cache, Exception ee)
        {
            string location = string.Format("ESBasic.ObjectManagement.Managers.RefreshableCacheManager.EngineAction : [{0}]", cache.GetType().ToString());
            this.agileLogger.LogSimple(ee, location, ErrorLevel.Standard);
        }
    }
}
