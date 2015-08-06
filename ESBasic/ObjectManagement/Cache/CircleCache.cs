using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Cache
{
    /// <summary>
    /// CircleCache 循环缓存。循环使用缓存中的每个对象。线程安全。
    /// </summary>  
    public class CircleCache<T>
    {
        private object locker = new object();
        private Circle<T> circle = new Circle<T>() ;

        #region Ctor
        public CircleCache() { }
        public CircleCache(ICollection<T> collection)
        {
            if (collection != null && collection.Count > 0)
            {
                foreach (T t in collection)
                {
                    this.circle.Append(t);
                }

            }
        } 
        #endregion

        #region Add
        public void Add(T t)
        {
            lock (this.locker)
            {
                this.circle.Append(t);
            }
        }        
        #endregion

        #region Get
        public T Get()
        {
            lock (this.locker)
            {
                this.circle.MoveNext();
                return this.circle.Current;
            }
        } 
        #endregion
    }
}
