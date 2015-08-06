using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Collections;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement.Cache
{
    /// <summary>
    /// BidirectionalMapping 双向映射。即Key和Value都是唯一的，在这种情况下使用BidirectionalMapping可提升依据Value查找Key的速度。
    /// 该实现是线程安全的。2008.08.20
    /// </summary>    
    public class BidirectionalMapping<T1,T2>
    {
        private IDictionary<T1, T2> dictionary = new Dictionary<T1, T2>();
        private IDictionary<T2, T1> reversedDictionary = new Dictionary<T2, T1>();
        private SmartRWLocker smartRWLocker = new SmartRWLocker();

        #region Count
        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        } 
        #endregion

        #region Add
        /// <summary>
        /// Add 添加映射对。如果已经有相同的key/value存在，则会覆盖。
        /// </summary>    
        public void Add(T1 t1, T2 t2)
        {
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                if (this.dictionary.ContainsKey(t1))
                {
                    this.dictionary.Remove(t1);
                }

                this.dictionary.Add(t1, t2);

                if (this.reversedDictionary.ContainsKey(t2))
                {
                    this.reversedDictionary.Remove(t2);
                }

                this.reversedDictionary.Add(t2, t1);
            }
        } 
        #endregion

        #region RemoveByT1
        public void RemoveByT1(T1 t1)
        {
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                if (!this.dictionary.ContainsKey(t1))
                {
                    return;
                }

                T2 t2 = this.dictionary[t1];
                this.dictionary.Remove(t1);
                this.reversedDictionary.Remove(t2);
            }
        } 
        #endregion

        #region RemoveByT2
        public void RemoveByT2(T2 t2)
        {
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                if (!this.reversedDictionary.ContainsKey(t2))
                {
                    return;
                }

                T1 t1 = this.reversedDictionary[t2];
                this.reversedDictionary.Remove(t2);
                this.dictionary.Remove(t1);
            }
        }
        #endregion

        #region GetT2
        public T2 GetT2(T1 t1)
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                if (!this.dictionary.ContainsKey(t1))
                {
                    return default(T2);
                }

                return this.dictionary[t1];
            }
        } 
        #endregion

        #region GetT1
        public T1 GetT1(T2 t2)
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                if (!this.reversedDictionary.ContainsKey(t2))
                {
                    return default(T1);
                }

                return this.reversedDictionary[t2];
            }
        }
        #endregion

        #region ContainsT1
        public bool ContainsT1(T1 t1)
        {
            return this.dictionary.ContainsKey(t1);
        } 
        #endregion

        #region ContainsT2
        public bool ContainsT2(T2 t2)
        {
            return this.reversedDictionary.ContainsKey(t2);
        } 
        #endregion

        #region GetAllT1ListCopy
        /// <summary>
        /// GetAllT1ListCopy 返回T1类型元素列表的拷贝。
        /// </summary>  
        public IList<T1> GetAllT1ListCopy()
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                return CollectionConverter.CopyAllToList<T1>(this.dictionary.Keys);
            }
        } 
        #endregion

        #region GetAllT2ListCopy
        /// <summary>
        /// GetAllT2ListCopy 返回T2类型元素列表的拷贝。
        /// </summary>    
        public IList<T2> GetAllT2ListCopy()
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                return CollectionConverter.CopyAllToList<T2>(this.reversedDictionary.Keys);
            }
        } 
        #endregion
            
    }
}
