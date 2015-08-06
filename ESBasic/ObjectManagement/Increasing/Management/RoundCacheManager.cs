using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Collections;

namespace ESBasic.ObjectManagement.Increasing.Management
{
    /// <summary>
    /// RoundCacheManager 用于管理历史的RoundCache和当前增量的IRoundIncreasingCache。该类是线程安全的。
    /// 当一个Round结束后，RoundCacheManager会将对应的RoundCache转移到历史并将其持久化。
    /// 关于异常处理：
    /// (1)本模型的驱动源在于IIncreaseAutoRetriever的循环引擎，如果其抛出异常将会停止运行，同时本RoundCacheManager也就丧失了驱动力，将再无任何增量进入。
    /// (2)调用AddRoundCache、RemoveRoundCache方法时也要捕获异常，因为持久化可能出错。
    /// </summary>
    /// <typeparam name="TSourceToken">如果有多处数据源，则TSourceToken用于标识每个数据源</typeparam>
    /// <typeparam name="TRoundID">每一个Round的标志，比如用天作为Round，则TRoundID可以是int (TheDate)。（</typeparam>
    /// <typeparam name="TRoundCache">某一Round的完整的数据缓存。可用于序列化存储。</typeparam>
    /// <typeparam name="TRoundIncreasingCache">用于存储当前Round数据的增量缓存</typeparam>
    /// <typeparam name="TKey">每个增量的唯一标志的类型，假设增量标志的值是逐渐递增的。</typeparam>
    /// <typeparam name="TObject">代表增量的类型</typeparam>
    public abstract class RoundCacheManager<TSourceToken ,TRoundID, TRoundCache,TRoundIncreasingCache, TKey ,TObject>
        where TRoundCache : class ,IRoundCache<TRoundID>
        where TRoundIncreasingCache : class ,IRoundIncreasingCache<TRoundID,TRoundCache, TObject>         
    {
        private TRoundIncreasingCache currentRoundCache = null; //Current Increasing
        private IDictionary<TRoundID, TRoundCache> roundCacheDictionary = null; //History

        #region event NewRoundStarted
        /// <summary>
        /// NewRoundStarted 新的一个Round开始。事件参数为新Round的ID。
        /// </summary>
        public event CbGeneric<TRoundID> NewRoundStarted;
        
        #endregion

        #region Ctor
        public RoundCacheManager()
        {
            this.NewRoundStarted += delegate { };
        } 
        #endregion.

        #region Property
        #region IncreaseAutoRetriever
        private IIncreaseAutoRetriever<TSourceToken, TRoundID, TKey, TObject> increaseAutoRetriever;
        public IIncreaseAutoRetriever<TSourceToken, TRoundID, TKey, TObject> IncreaseAutoRetriever
        {
            set { increaseAutoRetriever = value; }
        }
        #endregion

        #region RoundCachePersister
        private IRoundCachePersister<TRoundID, TRoundCache> roundCachePersister;
        public IRoundCachePersister<TRoundID, TRoundCache> RoundCachePersister
        {
            set { roundCachePersister = value; }
        }
        #endregion

        #region CurrentRoundCache readonly
        protected TRoundIncreasingCache CurrentRoundCache
        {
            get
            {
                return this.currentRoundCache;
            }
        }
        #endregion

        #region LastRefreshTime
        private DateTime lastRefreshTime = DateTime.Now;
        public DateTime LastRefreshTime
        {
            get { return lastRefreshTime; }
        } 
        #endregion

        #region MaxHistoryCountInMemory
        private int maxHistoryCountInMemory = 1;
        /// <summary>
        /// MaxHistoryCountInMemory 保存在内存中的最大的History个数。
        /// </summary>
        public int MaxHistoryCountInMemory
        {
            get { return maxHistoryCountInMemory; }
            set { maxHistoryCountInMemory = value; }
        } 
        #endregion

        #region CurrentRoundID
        public TRoundID CurrentRoundID
        {
            get
            {
                return this.currentRoundCache.RoundID;
            }
        } 
        #endregion
        #endregion

        #region Initialize
        public virtual void Initialize()
        {
            this.increaseAutoRetriever.IncreasementRetrieved += new CbIncreasementRetrieved<TRoundID, TObject>(increaseAutoRetriever_IncreasementRetrieved);

            this.roundCacheDictionary = this.roundCachePersister.LoadCaches(this.maxHistoryCountInMemory);
            if (this.roundCacheDictionary == null)
            {
                this.roundCacheDictionary = new Dictionary<TRoundID, TRoundCache>();
            }            
        } 
        #endregion

        #region increaseAutoRetriever_IncreasementRetrieved
        void increaseAutoRetriever_IncreasementRetrieved(List<TObject> list, TRoundID currentRoundID, bool isLastPhaseOfRound)
        {
            if (this.currentRoundCache == null)
            {
                this.currentRoundCache = this.CreateNewRoundIncreasingCache(currentRoundID);
            }

            this.currentRoundCache.AddIncreasement(list);

            if (isLastPhaseOfRound)
            {
                TRoundCache roundCache = this.currentRoundCache.CreateRoundCache();
                TRoundID newRoundID = this.GetNextRoundID(currentRoundID);
                this.currentRoundCache = this.CreateNewRoundIncreasingCache(newRoundID);

                //存储到内存History
                this.AddRoundCache(roundCache ,true);

                this.NewRoundStarted(newRoundID);
            }

            this.lastRefreshTime = DateTime.Now;
        } 
        #endregion

        #region AddRoundCache
        /// <summary>
        /// AddRoundCache 也有可能是重新计算某个Round的历史缓存后再重新加入
        /// </summary>
        public void AddRoundCache(TRoundCache roundCache ,bool toPersist)
        {
            if (toPersist)
            {
                this.roundCachePersister.Delete(roundCache.RoundID);
                this.roundCachePersister.Persist(roundCache);
            }

            //使用新的Dic替换旧的Dic，而不是在旧的Dic添加/移除，是为了保证线程安全的前提下最大可能的减少lock，提高效率。

            IDictionary<TRoundID, TRoundCache> newDic = new Dictionary<TRoundID, TRoundCache>();
            foreach (TRoundID roundID in this.roundCacheDictionary.Keys)
            {
                newDic.Add(roundID, this.roundCacheDictionary[roundID]);
            }

            if (newDic.ContainsKey(roundCache.RoundID))
            {
                newDic.Remove(roundCache.RoundID);
            }
            newDic.Add(roundCache.RoundID, roundCache);

            this.roundCacheDictionary = newDic;


            //删除过期的History
            IList<TRoundID> expiredList = this.GetExpiredHistoryList();
            if (expiredList != null)
            {
                foreach (TRoundID id in expiredList)
                {
                    this.RemoveRoundCache(id, false);
                }
            }
        } 
        #endregion

        #region RemoveRoundCache
        public void RemoveRoundCache(TRoundID targetRoundID ,bool removeFromPersist)
        {
            if (removeFromPersist)
            {
                this.roundCachePersister.Delete(targetRoundID);
            }

            if (!this.roundCacheDictionary.ContainsKey(targetRoundID))
            {
                return;
            }

            IDictionary<TRoundID, TRoundCache> newDic = new Dictionary<TRoundID, TRoundCache>();
            foreach (TRoundID roundID in this.roundCacheDictionary.Keys)
            {
                newDic.Add(roundID, this.roundCacheDictionary[roundID]);
            }

            newDic.Remove(targetRoundID);

            this.roundCacheDictionary = newDic;            
        } 
        #endregion

        #region GetHistoryRoundIDList
        /// <summary>
        /// GetHistoryRoundIDList 获取缓存的RoundID列表，不包括当前正在增量的RoundID
        /// </summary>        
        public IList<TRoundID> GetHistoryRoundIDList()
        {
            return CollectionConverter.CopyAllToList<TRoundID>(this.roundCacheDictionary.Keys);
        } 
        #endregion

        #region GetRoundCache
        protected TRoundCache GetHistoryRoundCache(TRoundID roundID)
        {
            if (!this.roundCacheDictionary.ContainsKey(roundID))
            {
                return null;
            }

            return this.roundCacheDictionary[roundID];
        } 
        #endregion

        #region ContainsHistory
        public bool ContainsHistory(TRoundID roundID)
        {
            return this.roundCacheDictionary.ContainsKey(roundID);
        } 
        #endregion

        protected abstract TRoundIncreasingCache CreateNewRoundIncreasingCache(TRoundID newRoundID);
        protected abstract TRoundID GetNextRoundID(TRoundID curRoundID);
        protected abstract IList<TRoundID> GetExpiredHistoryList();        
    }
}
