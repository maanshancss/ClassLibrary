using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;
using ESBasic.Threading.Engines;
using ESBasic.Collections;

namespace ESBasic.ObjectManagement.Cache
{
    public class HotCache<TKey, TObject> : IEngineActor, IHotCache<TKey,TObject> where TObject :class 
    {
        private IDictionary<TKey, CachePackage<TKey,TObject>> dictionary = new Dictionary<TKey, CachePackage<TKey ,TObject>>();
        private object locker = new object();
        private AgileCycleEngine agileCycleEngine;

        public event CbSimple CacheContentChanged;

        public HotCache()
        {
            this.CacheContentChanged += delegate { };
        }

        #region Property
        #region ObjectRetriever
        private IObjectRetriever<TKey, TObject> objectRetriever;
        public IObjectRetriever<TKey, TObject> ObjectRetriever
        {
            set { objectRetriever = value; }
        }
        #endregion

        #region MaxCachedCount
        private int maxCachedCount = int.MaxValue;
        public int MaxCachedCount
        {
            get { return maxCachedCount; }
            set { maxCachedCount = value; }
        } 
        #endregion

        #region MaxMuteSpanInMinutes
        private int maxMuteSpanInMinutes = 10;
        public int MaxMuteSpanInMinutes
        {
            set { maxMuteSpanInMinutes = value; }
        }
        #endregion

        #region DetectSpanInSecs
        private int detectSpanInSecs = 600;
        public int DetectSpanInSecs
        {
            set { detectSpanInSecs = value; }
        }
        #endregion

        #region Count
        /// <summary>
        /// Count 容器中缓存对象的个数。
        /// </summary>
        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }
        #endregion 

        #region RequestCount
        private long requestCount = 0;
        public long RequestCount
        {
            get { return requestCount; }
        } 
        #endregion

        #region HitCount
        private long hitCount = 0;
        public long HitCount
        {
            get { return hitCount; }
        } 
        #endregion

        #region NonexistentCount
        private long nonexistentCount = 0;
        /// <summary>
        /// NonexistentCount Get方法返回为null的次数。
        /// </summary>
        public long NonexistentCount
        {
            get { return nonexistentCount; }
        } 
        #endregion

        #region LastReadTime
        private DateTime lastReadTime = DateTime.Now;
        public DateTime LastReadTime
        {
            get { return lastReadTime; }           
        } 
        #endregion
 
        #endregion

        #region Initialize
        public void Initialize()
        {
            if (this.maxMuteSpanInMinutes > 0)
            {
                this.agileCycleEngine = new AgileCycleEngine(this);
                this.agileCycleEngine.DetectSpanInSecs = detectSpanInSecs;
                this.agileCycleEngine.Start();
            }
        } 
        #endregion

        #region Get
        public TObject Get(TKey id)
        {
            lock (this.locker)
            {
                this.lastReadTime = DateTime.Now;

                ++this.requestCount;

                if (this.dictionary.ContainsKey(id))
                {
                    ++this.hitCount;
                    return this.dictionary[id].Target;
                }

                TObject target = this.objectRetriever.Retrieve(id);
                if (target == null)
                {
                    ++this.nonexistentCount;
                }

                if ((target != null) && (this.dictionary.Count < this.maxCachedCount))
                {
                    this.dictionary.Add(id, new CachePackage<TKey ,TObject>(id, target));
                    this.CacheContentChanged();
                }

                return target;
            }
        } 
        #endregion

        #region GetAll
        public IList<TObject> GetAll()
        {
            this.lastReadTime = DateTime.Now;
            return CollectionConverter.ConvertAll<CachePackage<TKey, TObject>, TObject>(this.dictionary.Values, delegate(CachePackage<TKey, TObject> package) { return package.Target; });
        } 
        #endregion

        #region Clear
        public void Clear()
        {
            lock (this.locker)
            {
                this.dictionary.Clear();
                this.CacheContentChanged();
            }
        } 
        #endregion

        #region Add
        public void Add(TKey id, TObject obj)
        {
            lock (this.locker)
            {
                if (this.dictionary.ContainsKey(id))
                {
                    this.dictionary.Remove(id);
                }

                this.dictionary.Add(id, new CachePackage<TKey, TObject>(id, obj));
                this.CacheContentChanged();
            }
        } 
        #endregion

        #region Remove
        public void Remove(TKey id)
        {
            lock (this.locker)
            {
                if (this.dictionary.ContainsKey(id))
                {
                    this.dictionary.Remove(id);
                    this.CacheContentChanged();
                }
            }
        }  
        #endregion

        #region IEngineActor 成员
        public bool EngineAction()
        {
            lock (this.locker)
            {
                DateTime now = DateTime.Now ;
                IList<TKey> expiredList = new List<TKey>();
                foreach (CachePackage<TKey, TObject> obj in this.dictionary.Values)
                {
                    TimeSpan span = now - obj.LastAccessTime;
                    if (span.TotalMinutes > this.maxMuteSpanInMinutes)
                    {
                        expiredList.Add(obj.ID);
                    }
                }

                foreach (TKey id in expiredList)
                {
                    this.dictionary.Remove(id);
                }

                if (expiredList.Count > 0)
                {
                    this.CacheContentChanged();
                }
    
            }

            return true;
        }
        #endregion

        #region CachePackage
        sealed class CachePackage<TKey, TObject>
        {
            #region Ctor
            public CachePackage(TKey _iD, TObject _target)
            {
                this.iD = _iD;
                this.target = _target;
            }
            
            #endregion

            #region ID
            private TKey iD;
            public TKey ID
            {
                get { return iD; }               
            }
            #endregion

            #region Target
            private TObject target;
            public TObject Target
            {
                get
                {
                    this.lastAccessTime = DateTime.Now;
                    return target;
                }              
            }
            #endregion

            #region LastAccessTime
            private DateTime lastAccessTime = DateTime.Now;
            public DateTime LastAccessTime
            {
                get { return lastAccessTime; }               
            }
            #endregion
        }
        #endregion
    }

    
}
