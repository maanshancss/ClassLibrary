using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;

namespace ESBasic.Collections
{
    /// <summary>
    /// SortedArray 有序的数组，其中Key是不允许重复的。如果添加重复的key，则将抛出异常。如果是批添加出现重复，则批添加将全部失败。   
    /// </summary>
    [Serializable]
    public class SortedArray<TKey ,TVal> where TKey : IComparable
    {
        private int minCapacityForShrink = 32;
        private TKey[] keyArray = new TKey[32];
        private TVal[] valArray = new TVal[32];
        private int validCount = 0;
        private SmartRWLocker smartRWLocker = new SmartRWLocker();

        #region Ctor
        public SortedArray() { }      

        public SortedArray(IDictionary<TKey, TVal> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return;
            }

            this.keyArray = new TKey[dictionary.Count];
            this.valArray = new TVal[dictionary.Count];

            dictionary.Keys.CopyTo(this.keyArray, 0);
            dictionary.Values.CopyTo(this.valArray, 0);
            Array.Sort<TKey, TVal>(this.keyArray, this.valArray);
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
            get { return this.smartRWLocker.LastRequireReadTime; }
        }
        #endregion

        #region LastWriteTime
        public DateTime LastWriteTime
        {
            get { return this.smartRWLocker.LastRequireWriteTime; }
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
                using (this.smartRWLocker.Lock(AccessMode.Read))
                {
                    int index = this.IndexOfKey(key) ;
                    if (index < 0)
                    {
                        throw new Exception(string.Format("SortedArray doesn't contain the key [{0}]" ,key));
                    }

                    return this.valArray[index];
                }
            }
        }
        #endregion

        #region IndexOfKey
        public int IndexOfKey(TKey t)
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    return -1;
                }

                int index = Array.BinarySearch<TKey>(this.keyArray, 0, this.validCount, t);

                return (index < 0) ? -1 : index;
            }
        }
        #endregion

        #region Add
        public void Add(TKey key ,TVal val)
        {
            if (key == null)
            {
                throw new Exception("Target Key can't be null !");
            }

            using (this.smartRWLocker.Lock(AccessMode.Write))
            {                
                int index = Array.BinarySearch<TKey>(this.keyArray, 0, this.validCount, key);
                if (index >= 0)
                {
                    throw new Exception(string.Format("The Key [{0}] has existed in SortedArray !" ,key));
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
       
        public void Add(IDictionary<TKey ,TVal> dic)
        {
            if (dic == null || dic.Count == 0)
            {
                return;
            }

            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
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

                Array.Sort<TKey ,TVal>(this.keyArray,this.valArray , 0, this.validCount);
            }
        }
        #endregion

        #region Remove
        #region RemoveByKey
        /// <summary>
        /// Remove 删除数组中所有值为t的元素。
        /// </summary>      
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

            SortedArray<TKey> ary = new SortedArray<TKey>(keyCollection);

            IDictionary<TKey, TVal> newDic = new Dictionary<TKey, TVal>();
            for (int i = 0; i < this.validCount; i++)
            {
                if (!ary.Contains(this.keyArray[i]))
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
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                if (index < 0 || (index >= this.validCount))
                {
                    return;
                }

                Array.Copy(this.keyArray, index + 1, this.keyArray, index, this.validCount - index - 1);
                Array.Copy(this.valArray, index + 1, this.valArray, index, this.validCount - index - 1);
                --this.validCount;
            }
        }
        #endregion

        #region RemoveBetween
        public void RemoveBetween(int minIndex, int maxIndex)
        {
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                minIndex = minIndex < 0 ? 0 : minIndex;
                maxIndex = maxIndex >= this.validCount ? this.validCount - 1 : maxIndex;

                if (maxIndex < minIndex)
                {
                    return;
                }

                Array.Copy(this.keyArray, maxIndex + 1, this.keyArray, minIndex, this.validCount - maxIndex - 1);
                Array.Copy(this.valArray, maxIndex + 1, this.valArray, minIndex, this.validCount - maxIndex - 1);

                this.validCount -= (maxIndex - minIndex + 1);
            }
        }
        #endregion

        #endregion

        #region GetBetween
        public void GetBetween(int minIndex, int maxIndex, out TKey[] resultKey ,out TVal[] resultVal)
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
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
            posIndex = Array.BinarySearch<TKey>(this.keyArray, 0, this.validCount, key);
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
            using (this.smartRWLocker.Lock(AccessMode.Read))
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
        /// GetByKeyScope 获取[minKey,maxKey]闭集范围内的键和值的有序数组。
        /// </summary>       
        public void GetByKeyScope(TKey minKey,bool minClosed, TKey maxKey,bool maxClosed, out TKey[] resultKey, out TVal[] resultVal) 
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                if (minKey.CompareTo(maxKey) > 0)
                {
                    resultKey = new TKey[0];
                    resultVal = new TVal[0];
                    return;
                }

                int minIndex = 0;
                int maxIndex = 0;

                bool hitMin = this.HitKey(minKey, out minIndex);
                bool hitMax = this.HitKey(maxKey, out maxIndex);

                minIndex = minClosed ? (hitMin ? minIndex : minIndex + 1) : (minIndex + 1);
                maxIndex = maxClosed ? (hitMax ? maxIndex : maxIndex - 1) : (maxIndex);
                

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

            this.GetByKeyScope(minKey,minClosed, maxKey,maxClosed, out resultKey, out resultVal);

            return resultVal;
        } 
        #endregion

        #region GetGreater     
        public void GetGreater(TKey minKey, bool includEqual, out TKey[] resultKey, out TVal[] resultVal)
        {            
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                TKey maxKey = this.GetMaxKey();
                if (minKey.CompareTo(maxKey) > 0)
                {
                    resultKey = new TKey[0];
                    resultVal = new TVal[0];
                    return;
                }

                int minIndex = 0;

                bool hitMin = this.HitKey(minKey, out minIndex);

                if (hitMin && !includEqual)
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
        public void GetSmaller(TKey maxKey, bool includEqual, out TKey[] resultKey, out TVal[] resultVal)
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                TKey minKey = this.GetMinKey();
                if (minKey.CompareTo(maxKey) > 0)
                {
                    resultKey = new TKey[0];
                    resultVal = new TVal[0];
                    return;
                }

                int maxIndex = 0;

                bool hitMax = this.HitKey(maxKey, out maxIndex);

                if (hitMax && !includEqual)
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
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                if (minKey.CompareTo(maxKey) > 0)
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
            using (this.smartRWLocker.Lock(AccessMode.Read))
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
            using (this.smartRWLocker.Lock(AccessMode.Read))
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
                    int index = Array.BinarySearch<TKey>(ary, this.keyArray[i]);
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
            using (this.smartRWLocker.Lock(AccessMode.Write))
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
            using (this.smartRWLocker.Lock(AccessMode.Write))
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

        #region GetAllValueList
        public List<TVal> GetAllValueList()
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
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
            using (this.smartRWLocker.Lock(AccessMode.Read))
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
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    throw new Exception("SortedArray is Empty !");
                }

                return this.keyArray[this.validCount - 1];
            }
        } 
        #endregion

        #region GetMinKey
        public TKey GetMinKey()
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
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
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                this.keyArray = new TKey[this.minCapacityForShrink];
                this.valArray = new TVal[this.minCapacityForShrink];
                this.validCount = 0;
            }
        } 
        #endregion
    }
}
