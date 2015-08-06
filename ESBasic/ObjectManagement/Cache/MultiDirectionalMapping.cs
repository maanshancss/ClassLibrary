using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Collections;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement.Cache
{
    /// <summary>
    /// MultiDirectionalMapping 多向映射。用于管理类似好友群落的多向关系。
    /// 该实现是线程安全的。2010.06.01
    /// </summary>
    public class MultiDirectionalMapping<T> where T : IComparable
    {
        private Dictionary<T, SortedArray<T>> mapping = new Dictionary<T, SortedArray<T>>();
        private SmartRWLocker smartRWLocker = new SmartRWLocker();

        #region Add
        /// <summary>
        /// Add 添加映射对。如果已经有相同的映射对存在，则会覆盖。
        /// </summary>    
        public void Add(T t1, T t2)
        {
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                if (!this.mapping.ContainsKey(t1))
                {
                    this.mapping.Add(t1, new SortedArray<T>());
                }

                this.mapping[t1].Add(t2);

                if (!this.mapping.ContainsKey(t2))
                {
                    this.mapping.Add(t2, new SortedArray<T>());
                }

                this.mapping[t2].Add(t1);
            }
        } 
        #endregion

        #region Remove
        /// <summary>
        /// Remove 删除映射对。
        /// </summary>       
        public void Remove(T t1, T t2)
        {
            using(this.smartRWLocker.Lock(AccessMode.Write))
            {
                if (this.mapping.ContainsKey(t1))
                {
                    this.mapping[t1].Remove(t2);
                }

                if (this.mapping.ContainsKey(t2))
                {
                    this.mapping[t2].Remove(t1);
                }
            }
        }

        /// <summary>
        /// Remove 删除所有与t相关的映射对。
        /// </summary>       
        public void Remove(T t)
        {
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                if (this.mapping.ContainsKey(t))
                {
                    this.mapping.Remove(t);
                }

                foreach (SortedArray<T> ary in this.mapping.Values)
                {
                    ary.Remove(t);
                }
            }
        } 
        #endregion

        #region Map
        /// <summary>
        /// Map 容器中是否存在映射对
        /// </summary>     
        public bool Map(T t1, T t2)
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                if (!this.mapping.ContainsKey(t1))
                {
                    return false;
                }

                return this.mapping[t1].Contains(t2);
            }
        } 
        #endregion

        #region GetMappingList
        /// <summary>
        /// GetMappingList 获取所有与t相关的映射对象的集合。
        /// </summary>       
        public List<T> GetMappingList(T t)
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                if (this.mapping.ContainsKey(t))
                {
                    return this.mapping[t].GetAll();
                }

                return new List<T>();
            }
        } 
        #endregion

        #region Clear
        public void Clear()
        {
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                this.mapping.Clear();
            }
        } 
        #endregion

    }
}
