using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Engines;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// RefreshableCacheManager 用于管理多个可被刷新的缓存对象，并能定时刷新所管理的缓存。
    /// </summary>
    public class RefreshableCacheManager : IRefreshableCacheManager ,IEngineActor
    {
        private AgileCycleEngine agileCycleEngine;
        private object locker = new object();        
        public event CbCacheException CacheRefreshFailed;

        #region Ctor
        public RefreshableCacheManager()
        {
            this.CacheRefreshFailed += delegate { };
        } 
        #endregion

        #region CacheList
        private IList<IRefreshableCache> cacheList = new List<IRefreshableCache>();
        public IList<IRefreshableCache> CacheList
        {
            set { cacheList = value ?? new List<IRefreshableCache>(); }
        } 
        #endregion

        #region RefreshSpanInSecs
        private int refreshSpanInSecs = 60;
        public int RefreshSpanInSecs
        {          
            set { refreshSpanInSecs = value; }
        } 
        #endregion

        #region Initialize
        public void Initialize()
        {
            if (this.refreshSpanInSecs <= 0)
            {
                throw new Exception("RefreshSpanInSecs Property must be greater than 0 !");
            }

            foreach (IRefreshableCache cache in this.cacheList)
            {
                cache.LastRefreshTime = DateTime.Now;
            }

            this.agileCycleEngine = new AgileCycleEngine(this);
            this.agileCycleEngine.DetectSpanInSecs = 1;            
            this.agileCycleEngine.Start();     
         }

        public void RefreshNow()
        {
            this.EngineAction();
        }       
        #endregion

        #region AddCache
        public void AddCache(IRefreshableCache cache)
        {
            lock (this.locker)
            {
                this.cacheList.Add(cache);
            }
        } 
        #endregion

        #region RemoveCache
        public void RemoveCache(IRefreshableCache cache)
        {
            lock (this.locker)
            {
                this.cacheList.Remove(cache);
            }
        }
        #endregion      

        #region IEngineActor 成员

        public bool EngineAction()
        {
            lock (this.locker)
            {
                foreach (IRefreshableCache cache in this.cacheList)
                {
                    try
                    {
                        int spanInSecs = cache.RefreshSpanInSecs;
                        if (spanInSecs <= 0)
                        {
                            spanInSecs = this.refreshSpanInSecs;
                        }

                        TimeSpan span = DateTime.Now - cache.LastRefreshTime;
                        if (span.TotalSeconds >= spanInSecs)
                        {
                            cache.Refresh();
                            cache.LastRefreshTime = DateTime.Now;
                        }
                    }
                    catch (Exception ee)
                    {
                        this.CacheRefreshFailed(cache, ee);
                    }
                }

                return true;
            }
        }

        #endregion
    }
}
