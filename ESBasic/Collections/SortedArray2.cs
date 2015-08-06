using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;

namespace ESBasic.Collections
{
    /// <summary>
    /// SortedArray 有序的数组，其中Key是不允许重复的。如果单个添加重复的key，则将覆盖旧数据。如果是批添加出现重复，则批添加将全部失败。   
    /// 该实现是线程安全的。
    /// </summary>
    [Serializable]
    public class SortedArray2<TKey, TVal>
    {
        private List<TVal> lazyCopy = null;
        protected IComparer<TKey> comparer4Key = null;
        private int minCapacityForShrink = 32;
        private TKey[] keyArray = new TKey[32];
        private TVal[] valArray = new TVal[32];
        private int validCount = 0;

        #region SmartRWLocker
        [NonSerialized]
        private SmartRWLocker smartRWLocker = new SmartRWLocker();
        private SmartRWLocker SmartRWLocker
        {
            get
            {
                if (smartRWLocker == null)
                {
                    smartRWLocker = new SmartRWLocker();
                }

                return smartRWLocker;
            }
        }
        #endregion

        #region Ctor
        public SortedArray2() { }
        public SortedArray2(IComparer<TKey> _comparer4Key) :this(_comparer4Key ,null)
        {                  
        }       

        public SortedArray2(IComparer<TKey> _comparer4Key ,IDictionary<TKey, TVal> dictionary)
        {
            this.comparer4Key = _comparer4Key;
            this.Rebuild(dictionary);           
        }

        public void Rebuild(IDictionary<TKey, TVal> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return;
            }

            this.keyArray = new TKey[dictionary.Count];
            this.valArray = new TVal[dictionary.Count];

            dictionary.Keys.CopyTo(this.keyArray, 0);
            dictionary.Values.CopyTo(this.valArray, 0);
            Array.Sort<TKey, TVal>(this.keyArray, this.valArray, this.comparer4Key);
            this.validCount = dictionary.Count;
        }
        #endregion

        #region Property      

        #region Count
        public int Count
        {
            get
            {
                return this.validCount;
            }
        }
        #endregion

        #region Capacity
        private int Capacity
        {
            get
            {
                return this.keyArray.Length;
            }
        }
        #endregion

        #region LastReadTime
        public DateTime LastReadTime
        {
            get { return this.SmartRWLocker.LastRequireReadTime; }
        }
        #endregion

        #region LastWriteTime
        public DateTime LastWriteTime
        {
            get { return this.SmartRWLocker.LastRequireWriteTime; }
        }
        #endregion
        #endregion

        #region ContainsKey
        public bool ContainsKey(TKey t)
        {
            return this.IndexOfKey(t) >= 0;
        }
        #endregion

        #region Index
        public TVal this[TKey key]
        {
            get
            {
                using (this.SmartRWLocker.Lock(AccessMode.Read))
                {
                    int index = this.IndexOfKey(key);
                    if (index < 0)
                    {
                        throw new Exception(string.Format("SortedArray doesn't contain the key [{0}]", key));
                    }

                    return this.valArray[index];
                }
            }
        }
        #endregion

        #region TryGet
        public bool TryGet(TKey key, out TVal val)
        {
            val = default(TVal);
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                int index = this.IndexOfKey(key);
                if (index < 0)
                {
                    return false;
                }

                val = this.valArray[index];
                return true;
            }
        } 
        #endregion

        #region IndexOfKey
        public int IndexOfKey(TKey t)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    return -1;
                }

                int index = Array.BinarySearch<TKey>(this.keyArray, 0, this.validCount, t ,this.comparer4Key);

                return (index < 0) ? -1 : index;
            }
        }
        #endregion

        #region AddSafely
        /// <summary>
        /// 安全添加。如果已经存在相同的key，则直接返回。
        /// </summary>      
        public void AddSafely(TKey key, TVal val)
        {
            if (key == null)
            {
                throw new Exception("Target Key can't be null !");
            }

            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                if (this.ContainsKey(key))
                {
                    return;
                }

                this.Add(key, val);
            }
        } 
        #endregion

        #region Add
        public void Add(TKey key, TVal val)
        {
            if (key == null)
            {
                throw new Exception("Target Key can't be null !");
            }

            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                this.lazyCopy = null;
                int index = Array.BinarySearch<TKey>(this.keyArray, 0, this.validCount, key ,this.comparer4Key);
                if (index >= 0)
                {
                    this.valArray[index] = val;
                    return;
                }                

                this.AdjustCapacity(1);

                int posIndex = ~index;

                Array.Copy(this.keyArray, posIndex, this.keyArray, posIndex + 1, this.validCount - posIndex);
                Array.Copy(this.valArray, posIndex, this.valArray, posIndex + 1, this.validCount - posIndex);
                this.keyArray[posIndex] = key;
                this.valArray[posIndex] = val;

                ++this.validCount;               
            }
        }

        public void Add(IDictionary<TKey, TVal> dic)
        {
            if (dic == null || dic.Count == 0)
            {
                return;
            }

            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                this.lazyCopy = null;
                if (this.validCount == 0)
                {
                    this.Rebuild(dic);
                    return;
                }

                foreach (TKey key in dic.Keys)
                {
                    if (this.ContainsKey(key))
                    {
                        throw new Exception(string.Format("The Key [{0}] has existed in SortedArray !", key));
                    }
                }

                this.AdjustCapacity(dic.Count);

                foreach (TKey key in dic.Keys)
                {
                    this.keyArray[this.validCount] = key;
                    this.valArray[this.validCount] = dic[key];
                    ++this.validCount;
                }

                Array.Sort<TKey, TVal>(this.keyArray, this.valArray, 0, this.validCount, this.comparer4Key);
            }
        }
        #endregion

        #region Remove
        public void Remove(TKey t)
        {
            this.RemoveByKey(t);
        }

        #region RemoveByKey           
        public void RemoveByKey(TKey t)
        {
            if (t == null)
            {
                return;
            }

            int index = this.IndexOfKey(t);
            if (index >= 0)
            {
                this.RemoveAt(index);
            }

        }
        #endregion

        #region RemoveByKeys
        public void RemoveByKeys(ICollection<TKey> keyCollection)
        {
            if (keyCollection == null || keyCollection.Count == 0)
            {
                return;
            }

            this.lazyCopy = null;
            Dictionary<TKey, TKey> dic = new Dictionary<TKey, TKey>();
            foreach (TKey key in keyCollection)
            {
                dic.Add(key, key);
            }

            IDictionary<TKey, TVal> newDic = new Dictionary<TKey, TVal>();
            for (int i = 0; i < this.validCount; i++)
            {
                if (!dic.ContainsKey(this.keyArray[i]))
                {
                    newDic.Add(this.keyArray[i], this.valArray[i]);
                }
            }

            this.Clear();
            this.Add(newDic);
        }
        #endregion

        #region RemoveAt
        public void RemoveAt(int index)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                if (index < 0 || (index >= this.validCount))
                {
                    return;
                }

                this.lazyCopy = null;
                Array.Copy(this.keyArray, index + 1, this.keyArray, index, this.validCount - index - 1);
                Array.Copy(this.valArray, index + 1, this.valArray, index, this.validCount - index - 1);
                --this.validCount;
            }
        }
        #endregion

        #region RemoveBetween
        public void RemoveBetween(int minIndex, int maxIndex)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                minIndex = minIndex < 0 ? 0 : minIndex;
                maxIndex = maxIndex >= this.validCount ? this.validCount - 1 : maxIndex;

                if (maxIndex < minIndex)
                {
                    return;
                }

                this.lazyCopy = null;
                Array.Copy(this.keyArray, maxIndex + 1, this.keyArray, minIndex, this.validCount - maxIndex - 1);
                Array.Copy(this.valArray, maxIndex + 1, this.valArray, minIndex, this.validCount - maxIndex - 1);

                this.validCount -= (maxIndex - minIndex + 1);
            }
        }
        #endregion

        #region RemoveBetween
        public void RemoveBetween(TKey minKey, bool minClosed, TKey maxKey, bool maxClosed)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                if (this.comparer4Key.Compare(minKey, maxKey) > 0)
                {                    
                    return;
                }

                this.lazyCopy = null;
                int minIndex = 0;
                int maxIndex = 0;

                bool hitMin = this.HitKey(minKey, out minIndex);
                bool hitMax = this.HitKey(maxKey, out maxIndex);

                minIndex = minClosed ? (minIndex) : (hitMin ? minIndex + 1 : minIndex);
                maxIndex = maxClosed ? (hitMax ? maxIndex : maxIndex - 1) : (maxIndex - 1);

                this.RemoveBetween(minIndex, maxIndex);
            }
        }
        #endregion

        #endregion

        #region GetBetween
        public void GetBetween(int minIndex, int maxIndex, out TKey[] resultKey, out TVal[] resultVal)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                minIndex = minIndex < 0 ? 0 : minIndex;
                maxIndex = maxIndex >= this.validCount ? this.validCount - 1 : maxIndex;

                if (maxIndex < minIndex)
                {
                    resultKey = new TKey[0];
                    resultVal = new TVal[0];
                    return;
                }

                int count = maxIndex - minIndex + 1;
                resultKey = new TKey[count];
                resultVal = new TVal[count];

                Array.Copy(this.keyArray, minIndex, resultKey, 0, count);
                Array.Copy(this.valArray, minIndex, resultVal, 0, count);
            }
        }
        #endregion

        #region HitKey
        private bool HitKey(TKey key, out int posIndex)
        {
            posIndex = Array.BinarySearch<TKey>(this.keyArray, 0, this.validCount, key ,this.comparer4Key);
            if (posIndex >= 0)
            {
                return true;
            }

            posIndex = ~posIndex;
            return false;
        }
        #endregion

        #region KeyValuePair
        public KeyValuePair<TKey, TVal> GetAt(int index)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (index < 0 || index >= this.validCount)
                {
                    throw new Exception("Index out of the range !");
                }

                return new KeyValuePair<TKey, TVal>(this.keyArray[index], this.valArray[index]);
            }
        }
        #endregion

        #region GetByKeyScope
        /// <summary>
        /// GetByKeyScope 获取minKey-maxKey范围内的键和值的有序数组。
        /// </summary>       
        public void GetByKeyScope(TKey minKey, bool minClosed, TKey maxKey, bool maxClosed, out TKey[] resultKey, out TVal[] resultVal)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.comparer4Key.Compare(minKey ,maxKey) > 0)
                {
                    resultKey = new TKey[0];
                    resultVal = new TVal[0];
                    return;
                }

                int minIndex = 0;
                int maxIndex = 0;

                bool hitMin = this.HitKey(minKey, out minIndex);
                bool hitMax = this.HitKey(maxKey, out maxIndex);

                minIndex = minClosed ? (minIndex) : (hitMin ? minIndex+1 : minIndex );
                maxIndex = maxClosed ? (hitMax ? maxIndex : maxIndex - 1) : (maxIndex - 1);


                this.GetBetween(minIndex, maxIndex, out resultKey, out resultVal);
            }
        }

        /// <summary>
        /// GetByKeyScope 获取minKey-maxKey范围内的值的有序数组。
        /// </summary>
        public TVal[] GetByKeyScope(TKey minKey, bool minClosed, TKey maxKey, bool maxClosed)
        {
            TKey[] resultKey = null;
            TVal[] resultVal = null;

            this.GetByKeyScope(minKey, minClosed, maxKey, maxClosed, out resultKey, out resultVal);

            return resultVal;
        }
        #endregion

        #region GetGreater
        public void GetGreater(TKey minKey, bool closed, out TKey[] resultKey, out TVal[] resultVal)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                TKey maxKey = this.GetMaxKey();
                if (this.comparer4Key.Compare(minKey ,maxKey) > 0)
                {
                    resultKey = new TKey[0];
                    resultVal = new TVal[0];
                    return;
                }

                int minIndex = 0;

                bool hitMin = this.HitKey(minKey, out minIndex);

                if (hitMin && !closed)
                {
                    minIndex += 1;
                }

                this.GetBetween(minIndex, this.validCount - 1, out resultKey, out resultVal);
            }
        }

        public TVal[] GetGreater(TKey minKey, bool includEqual)
        {
            TKey[] resultKey = null;
            TVal[] resultVal = null;

            this.GetGreater(minKey, includEqual, out resultKey, out resultVal);

            return resultVal;
        }
        #endregion

        #region GetSmaller
        public void GetSmaller(TKey maxKey, bool closed, out TKey[] resultKey, out TVal[] resultVal)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                TKey minKey = this.GetMinKey();
                if (this.comparer4Key.Compare(minKey ,maxKey) > 0)
                {
                    resultKey = new TKey[0];
                    resultVal = new TVal[0];
                    return;
                }

                int maxIndex = 0;

                bool hitMax = this.HitKey(maxKey, out maxIndex);

                if (hitMax && !closed)
                {
                    maxIndex -= 1;
                }

                this.GetBetween(0, maxIndex, out resultKey, out resultVal);
            }
        }

        public TVal[] GetSmaller(TKey maxKey, bool includEqual)
        {
            TKey[] resultKey = null;
            TVal[] resultVal = null;

            this.GetSmaller(maxKey, includEqual, out resultKey, out resultVal);

            return resultVal;
        }
        #endregion

        #region GetValNotInKeyScope
        public TVal[] GetValNotInKeyScope(TKey minKey, TKey maxKey)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.comparer4Key.Compare(minKey ,maxKey) > 0)
                {
                    return new TVal[0];
                }

                int minIndex = 0;
                int maxIndex = 0;

                bool hitMin = this.HitKey(minKey, out minIndex);
                bool hitMax = this.HitKey(maxKey, out maxIndex);

                minIndex = hitMin ? minIndex - 1 : minIndex;
                maxIndex = hitMax ? maxIndex + 1 : maxIndex;

                //(-$,minIndex) + [maxIndex ,+$)

                int count = minIndex + this.validCount - maxIndex;

                TVal[] result = new TVal[count];

                Array.Copy(this.valArray, result, minIndex);
                Array.Copy(this.valArray, maxIndex, result, minIndex, this.validCount - maxIndex);
                return result;
            }
        }
        #endregion

        #region GetByKeys
        public List<TVal> GetByKeys(IEnumerable<TKey> collection)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (collection == null)
                {
                    return new List<TVal>();
                }

                List<TVal> list = new List<TVal>();

                foreach (TKey key in collection)
                {
                    int index = this.IndexOfKey(key);
                    if (index >= 0)
                    {
                        list.Add(this.valArray[index]);
                    }
                }

                return list;
            }
        }
        #endregion

        #region GetValNotInKeys
        public List<TVal> GetValNotInKeys(IEnumerable<TKey> collection)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (collection == null)
                {
                    return new List<TVal>();
                }

                List<TKey> keyList = new List<TKey>(collection);
                keyList.Sort();
                TKey[] ary = keyList.ToArray();

                List<TVal> resultList = new List<TVal>();
                for (int i = 0; i < this.validCount; i++)
                {
                    int index = Array.BinarySearch<TKey>(ary, this.keyArray[i] ,this.comparer4Key);
                    if (index < 0)
                    {
                        resultList.Add(this.valArray[i]);
                    }
                }

                return resultList;
            }
        }
        #endregion

        #region Shrink
        /// <summary>
        /// Shrink 将内部数组收缩到最小，释放内存。
        /// </summary>
        public void Shrink()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                if (this.keyArray.Length == this.validCount)
                {
                    return;
                }


                int len = this.validCount >= this.minCapacityForShrink ? this.validCount : this.minCapacityForShrink;

                TKey[] newkeyAry = new TKey[len];
                TVal[] newValAry = new TVal[len];

                Array.Copy(this.keyArray, 0, newkeyAry, 0, this.validCount);
                Array.Copy(this.valArray, 0, newValAry, 0, this.validCount);
                this.keyArray = newkeyAry;
                this.valArray = newValAry;
            }
        }
        #endregion

        #region AdjustCapacity
        private void AdjustCapacity(int newCount)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                int totalCount = this.validCount + newCount;
                if (this.keyArray.Length >= totalCount)
                {
                    return;
                }

                int newCapacity = this.keyArray.Length;
                while (newCapacity < totalCount)
                {
                    newCapacity *= 2;
                }

                TKey[] newKeyAry = new TKey[newCapacity];
                TVal[] newValAry = new TVal[newCapacity];
                Array.Copy(this.keyArray, 0, newKeyAry, 0, this.validCount);
                Array.Copy(this.valArray, 0, newValAry, 0, this.validCount);
                this.keyArray = newKeyAry;
                this.valArray = newValAry;
            }
        }
        #endregion

        #region GetAllReadonly
        public List<TVal> GetAllReadonly()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.lazyCopy == null)
                {
                    this.lazyCopy = this.GetAllValueList();
                }
                return this.lazyCopy;
            }
        } 
        #endregion

        #region GetAll
        public List<TVal> GetAll()
        {
            return this.GetAllValueList();
        } 
        #endregion

        #region GetAllValueList
        public List<TVal> GetAllValueList()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                List<TVal> list = new List<TVal>();
                for (int i = 0; i < this.validCount; i++)
                {
                    list.Add(this.valArray[i]);
                }

                return list;
            }
        }
        #endregion

        #region GetAllKeyList
        public List<TKey> GetAllKeyList()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                List<TKey> list = new List<TKey>();
                for (int i = 0; i < this.validCount; i++)
                {
                    list.Add(this.keyArray[i]);
                }

                return list;
            }
        }
        #endregion

        #region GetMaxKey
        public TKey GetMaxKey()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    throw new Exception("SortedArray is Empty !");
                }

                return this.keyArray[this.validCount - 1];
            }
        }
        #endregion

        #region Get
        /// <summary>
        /// 如果key不存在，则返回default(TVal)。
        /// </summary>       
        public TVal Get(TKey key)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                int index = this.IndexOfKey(key);
                if (index < 0)
                {
                    return default(TVal);
                }

                return this.valArray[index];
            }
        } 
        #endregion

        #region TryGetMaxKey
        public bool TryGetMaxKey(out TKey key)
        {
            key = default(TKey);
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    return false;
                }

                key = this.keyArray[this.validCount - 1];
                return true;
            }
        }

        public bool TryGetMaxKey(out TKey key ,out TVal val)
        {
            key = default(TKey);
            val = default(TVal);
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    return false;
                }

                key = this.keyArray[this.validCount - 1];
                val = this.valArray[this.validCount - 1];
                return true;
            }
        }
        #endregion

        #region TryGetMinKey
        public bool TryGetMinKey(out TKey key)
        {
            key = default(TKey);
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    return false;
                }

                key = this.keyArray[0];
                return true;
            }
        }

        public bool TryGetMinKey(out TKey key, out TVal val)
        {
            key = default(TKey);
            val = default(TVal);
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    return false;
                }

                key = this.keyArray[0];
                val = this.valArray[0];
                return true;
            }
        }
        #endregion

        #region GetMinKey
        public TKey GetMinKey()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    throw new Exception("SortedArray is Empty !");
                }

                return this.keyArray[0];
            }
        }
        #endregion

        #region Clear
        public void Clear()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                this.lazyCopy = null;
                this.keyArray = new TKey[this.minCapacityForShrink];
                this.valArray = new TVal[this.minCapacityForShrink];
                this.validCount = 0;
            }
        }
        #endregion

        #region ToDictionary
        public Dictionary<TKey, TVal> ToDictionary()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                Dictionary<TKey, TVal> dic = new Dictionary<TKey, TVal>(this.validCount);
                for (int i = 0; i < this.validCount; i++)
                {
                    dic.Add(this.keyArray[i], this.valArray[i]);
                }

                return dic;
            }
        } 
        #endregion

        #region ToString
        public override string ToString()
        {
            return string.Format("Count:{0} ,Capacity:{1}", this.validCount, this.keyArray.Length);
        }
        #endregion
    }
}
