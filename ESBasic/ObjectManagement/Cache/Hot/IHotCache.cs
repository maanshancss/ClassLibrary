using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;
using ESBasic.Threading.Engines;

namespace ESBasic.ObjectManagement.Cache
{
    /// <summary>
    /// IHotCache 用于缓存那些活跃的对象，并定时删除不活跃的对象。该接口的实现必须是线程安全的。
    /// </summary>    
    public interface IHotCache<TKey, TObject> where TObject : class
    {
        /// <summary>
        /// DetectSpanInSecs 多长时间检测一次对象是否活跃，单位：秒。
        /// </summary>
        int DetectSpanInSecs { set; }

        /// <summary>
        /// MaxMuteSpanInMinutes 对象最大的沉默时间（分钟）。如果一个对象在MaxMuteSpanInMinutes时间间隔内都不被访问，则将被从缓存中清除。
        /// 如果该属性的值被设置为小于或等于0，则表示永远不会从缓存中清除。
        /// </summary>
        int MaxMuteSpanInMinutes { set; }

        /// <summary>
        /// MaxCachedCount 最多缓存的对象个数。当超过此个数时，不再缓存新的对象。
        /// </summary>
        int MaxCachedCount { get; set; }

        IObjectRetriever<TKey, TObject> ObjectRetriever { set; }
        int Count { get; }
        long RequestCount { get; } //请求次数
        long HitCount { get; }//命中的次数
        DateTime LastReadTime { get; }        

        void Initialize();
        void Clear();
        void Add(TKey id, TObject obj);
        void Remove(TKey id);
        
        /// <summary>
        /// Get 如果缓存中存在目标则直接返回，否则通过ObjectRetriever提取对象并缓存。
        /// </summary>      
        TObject Get(TKey id);

        IList<TObject> GetAll();

        event CbSimple CacheContentChanged;
    }
}
