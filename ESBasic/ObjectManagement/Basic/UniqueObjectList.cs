using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement
{  
    /// <summary>
    /// UniqueObjectList 保证list中的每个object都是唯一的，不会重复。该接口的实现保证是线程安全的。
    /// </summary>   
    public class UniqueObjectList<T>
    {        
        private IList<T> innerList = new List<T>();
        private object locker = new object();

        #region Add ,Remove
        public void Add(T obj)
        {
            lock (this.locker)
            {
                if (!this.innerList.Contains(obj))
                {
                    this.innerList.Add(obj);
                }
            }
        }

        public void Remove(T obj)
        {
            lock (this.locker)
            {
                this.innerList.Remove(obj);
            }
        }
        #endregion

        #region List
        public IList<T> GetListCopy()
        {
            lock(this.locker)
            {
                return ESBasic.Collections.CollectionConverter.CopyAllToList<T>(this.innerList);
            }
        }
        #endregion
    }
}
