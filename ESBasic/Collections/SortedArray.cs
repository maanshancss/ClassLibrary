using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;

namespace ESBasic.Collections
{
    /// <summary>
    /// 有序的数组，SortedArray 中的元素是不允许重复的。如果添加数组中已经存在的元素，将会被忽略。
    /// 该实现是线程安全的。
    /// </summary>
    [Serializable]
    public class SortedArray<T> : SortedArray2<T>, IComparer<T> where T : IComparable
    {
        public SortedArray()           
        {
            base.comparer4Key = this;
        }

        public SortedArray(ICollection<T> collection)
        {
            base.comparer4Key = this;
            base.Rebuild(collection);
        }

        #region IComparer<TKey> 成员

        public int Compare(T x, T y)
        {
            return x.CompareTo(y);
        }

        #endregion
    }

    /// <summary>
    /// 有序的数组，SortedArray 中的元素是不允许重复的。如果添加数组中已经存在的元素，将会被忽略。
    /// 该实现是线程安全的。
    /// </summary>
    [Serializable]
    public class SortedArray2<T> 
    {
        private List<T> lazyCopy = null;
        private int minCapacityForShrink = 32;
        private T[] array = new T[32] ;
        private int validCount = 0;
        protected IComparer<T> comparer4Key = null;

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
        protected SortedArray2() 
        {           
        }
        public SortedArray2(IComparer<T> comparer)
        {
            this.comparer4Key = comparer;
        }

        public SortedArray2(IComparer<T> comparer ,ICollection<T> collection)
        {
            this.comparer4Key = comparer;        
            Rebuild(collection);      
        }

        protected void Rebuild(ICollection<T> collection)
        {
            if (collection == null || collection.Count == 0)
            {
                return;
            }
            this.array = new T[collection.Count];
            collection.CopyTo(this.array, 0);
            Array.Sort(this.array, this.comparer4Key);
            this.validCount = collection.Count;
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
                return this.array.Length;
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

        #region Contains
        public bool Contains(T t)
        {
            return this.IndexOf(t) >= 0;
        } 
        #endregion

        #region Index
        public T this[int index]
        {
            get
            {
                using (this.SmartRWLocker.Lock(AccessMode.Read))
                {
                    if (index < 0 || (index >= this.validCount))
                    {
                        throw new Exception("Index out of the range !");
                    }

                    return this.array[index];
                }
            }
        } 
        #endregion

        #region IndexOf
        public int IndexOf(T t)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    return -1;
                }

                int index = Array.BinarySearch<T>(this.array, 0, this.validCount, t ,this.comparer4Key);

                return (index < 0) ? -1 : index;
            }
        } 
        #endregion

        #region Add
        public void Add(T t)
        {
            int posIndex = 0;
            this.Add(t, out posIndex);
        }

        /// <summary>
        /// Add 将一个元素添加到数组中。如果数组中已存在目标元素，则忽略。无论哪种情况，posIndex都会被赋予正确的值。
        /// </summary>        
        public void Add(T t, out int posIndex)
        {
            if (t == null)
            {
                throw new Exception("Target can't be null !");
            }

            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                int index = Array.BinarySearch<T>(this.array, 0, this.validCount, t, this.comparer4Key);
                if (index >= 0)
                {
                    posIndex = index;
                    return ;
                }

                this.AdjustCapacity(1);
                posIndex = ~index;
                Array.Copy(this.array, posIndex, this.array, posIndex + 1, this.validCount - posIndex);
                this.array[posIndex] = t;

                ++this.validCount;               
            }

            this.lazyCopy = null;
        }

        public void Add(ICollection<T> collection )
        {
            this.Add(collection, true);
        }

        /// <summary>
        /// Add 如果能保证collection中的元素不会与现有的元素重复，则checkRepeat可以传入false。
        /// </summary>       
        public void Add(ICollection<T> collection, bool checkRepeat)
        {
            if (collection == null || collection.Count == 0)
            {
                return;
            }

            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                ICollection<T> resultCollection = collection;

                #region checkRepeat
                if (checkRepeat)
                {
                    Dictionary<T, T> dic = new Dictionary<T, T>();
                    foreach (T t in collection)
                    {
                        if (dic.ContainsKey(t) || this.Contains(t))
                        {
                            continue;
                        }

                        dic.Add(t, t);
                    }

                    resultCollection = dic.Keys;
                }
                #endregion

                if (resultCollection.Count == 0)
                {
                    return;
                }

                this.AdjustCapacity(resultCollection.Count);

                foreach (T t in resultCollection)
                {                    
                    this.array[this.validCount] = t;
                    ++this.validCount;
                }

                Array.Sort<T>(this.array, 0, this.validCount, this.comparer4Key);
            }

            this.lazyCopy = null;
        }
        #endregion

        #region Remove
        #region Remove
        /// <summary>
        /// Remove 删除数组中所有值为t的元素。
        /// </summary>      
        public void Remove(T t)
        {
            if (t == null)
            {
                return;
            }

            int index = -1;
            do
            {
                index = this.IndexOf(t);
                if (index >= 0)
                {
                    this.RemoveAt(index);
                }

            } while (index >= 0);

            this.lazyCopy = null;
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

                if (index == this.validCount - 1)
                {
                    this.array[index] = default(T);
                }
                else
                {
                    Array.Copy(this.array, index + 1, this.array, index, this.validCount - index - 1);
                }
                --this.validCount;
            }

            this.lazyCopy = null;
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

                Array.Copy(this.array, maxIndex + 1, this.array, minIndex, this.validCount - maxIndex - 1);

                this.validCount -= (maxIndex - minIndex + 1);
            }

            this.lazyCopy = null;
        } 
        #endregion

        #endregion

        #region GetBetween
        public T[] GetBetween(int minIndex, int maxIndex)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                minIndex = minIndex < 0 ? 0 : minIndex;
                maxIndex = maxIndex >= this.validCount ? this.validCount - 1 : maxIndex;

                if (maxIndex < minIndex)
                {
                    return new T[0];
                }

                int count = maxIndex - minIndex - 1;
                T[] result = new T[count];

                Array.Copy(this.array, minIndex, result, 0, count);
                return result;
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
                if (this.array.Length == this.validCount)
                {
                    return;
                }


                int len = this.validCount >= this.minCapacityForShrink ? this.validCount : this.minCapacityForShrink;

                T[] newAry = new T[len];              

                Array.Copy(this.array,0, newAry,0, this.validCount);
                this.array = newAry;
            }
        } 
        #endregion

        #region AdjustCapacity
        private void AdjustCapacity(int newCount)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                int totalCount = this.validCount + newCount;
                if (this.array.Length >= totalCount)
                {
                    return;
                }

                int newCapacity = this.array.Length;
                while (newCapacity < totalCount)
                {
                    newCapacity *= 2;
                }

                T[] newAry = new T[newCapacity];
                Array.Copy(this.array, 0, newAry, 0, this.validCount);
                this.array = newAry;
            }
        } 
        #endregion

        #region GetMax
        public T GetMax()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    throw new Exception("SortedArray is Empty !");
                }

                return this.array[this.validCount - 1];
            }
        }
        #endregion

        #region GetMin
        public T GetMin()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                if (this.validCount == 0)
                {
                    throw new Exception("SortedArray is Empty !");
                }

                return this.array[0];
            }
        }
        #endregion

        #region GetAll
        public List<T> GetAll()
        {
            List<T> list = new List<T>();
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                for (int i = 0; i < this.validCount; i++)
                {
                    list.Add(this.array[i]);
                }
            }
            return list;
        } 
        #endregion

        #region GetAllReadonly
        /// <summary>
        /// 注意，内部使用了Lazy缓存，返回的集合不可被修改。
        /// </summary>        
        public List<T> GetAllReadonly()
        {
            if (this.lazyCopy != null)
            {
                return this.lazyCopy;
            }

            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                List<T> list = new List<T>();
                for (int i = 0; i < this.validCount; i++)
                {
                    list.Add(this.array[i]);
                }

                this.lazyCopy = list;
                return this.lazyCopy;
            }
        } 
        #endregion

        #region Clear
        public void Clear()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                this.array = new T[this.minCapacityForShrink];              
                this.validCount = 0;
            }

            this.lazyCopy = null;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return string.Format("Count:{0} ,Capacity:{1}", this.validCount, this.array.Length);
        } 
        #endregion
    }


}
