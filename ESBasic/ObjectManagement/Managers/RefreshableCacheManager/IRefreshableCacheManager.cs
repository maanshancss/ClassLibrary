using System;
using System.Collections.Generic;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// IRefreshableCacheManager 用于管理多个可被刷新的缓存对象，并能定时刷新所管理的缓存。
    /// zhuweisky 2007.07.07
    /// </summary>
    public interface IRefreshableCacheManager
    {
        /// <summary>
        /// RefreshSpanInSecs 定时刷新缓存的时间间隔。
        /// </summary>
        int RefreshSpanInSecs { set; }

        IList<IRefreshableCache> CacheList { set; }

        void Initialize();

        /// <summary>
        /// RefreshNow 手动刷新被管理的所有缓存。
        /// </summary>
        void RefreshNow();

        /// <summary>
        /// AddCache 动态添加缓存。
        /// </summary>       
        void AddCache(IRefreshableCache cache);

        /// <summary>
        /// RemoveCache 动态移除缓存。
        /// </summary>       
        void RemoveCache(IRefreshableCache cache);      

        /// <summary>
        /// CacheRefreshFailed 当某个缓存刷新抛出异常时，将触发该事件。
        /// </summary>
        event CbCacheException CacheRefreshFailed;
    }

    public delegate void CbCacheException(IRefreshableCache cache ,Exception ee) ;
}
