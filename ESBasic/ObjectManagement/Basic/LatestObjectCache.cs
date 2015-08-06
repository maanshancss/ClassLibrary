using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement
{
    /// <summary>
    /// 固定大小的最新N个对象缓存器。线程安全。
    /// 当容器满时，新加入的对象会挤掉最老的对象。
    /// zhuweisky 2011.01.23
    /// </summary>
    public class LatestObjectCache<T>
    {
        private T[] array = null;
        private int latestObjectIndex = -1; //最后加入对象的索引。
        private object locker = new object();
        private bool initialized = false;        

        #region LatestObjectCache
        public LatestObjectCache() { }
        public LatestObjectCache(int _capacity)
        {
            this.capacity = _capacity;
        } 
        #endregion

        #region Capacity
        private int capacity = 10;
        /// <summary>
        /// 最多容纳多少个最新对象。
        /// </summary>
        public int Capacity
        {
            get { return capacity; }
            set 
            {
                if (this.initialized && value != this.capacity)
                {
                    throw new Exception("Can't change the value of Capacity property after initialized.");
                }
                capacity = value; 
            }
        } 
        #endregion

        #region IsFull
        private bool isFull = false;
        /// <summary>
        /// 缓存是否已经满了？
        /// </summary>
        public bool IsFull
        {
            get { return isFull; }
        } 
        #endregion

        #region LastObjectAddedTime
        private DateTime lastObjectAddedTime = DateTime.Now;
        /// <summary>
        /// 最后一个对象加入的时间。
        /// </summary>
        public DateTime LastObjectAddedTime
        {
            get { return lastObjectAddedTime; }
        } 
        #endregion

        #region ValidObjectCount
        /// <summary>
        /// 缓存中对象的个数。
        /// </summary>
        private int ValidObjectCount
        {
            get
            {
                if (this.array == null)
                {
                    return 0;
                }

                if (this.isFull)
                {
                    return this.array.Length;
                }

                return this.latestObjectIndex + 1;
            }
        } 
        #endregion

        #region Initialize
        public void Initialize()
        {
            if (this.capacity <= 0)
            {
                throw new Exception("Capacity must be greater than 0.");
            }

            this.array = new T[this.capacity];
            this.latestObjectIndex = -1;
            this.initialized = true;
        } 
        #endregion

        #region Add
        /// <summary>
        /// 向缓存中添加最新的对象。
        /// </summary>        
        public void Add(T t)
        {
            lock (this.locker)
            {
                this.lastObjectAddedTime = DateTime.Now;
                this.latestObjectIndex = (this.latestObjectIndex + 1) % this.array.Length;
                this.array[this.latestObjectIndex] = t;
                if ((!this.isFull) && (this.latestObjectIndex == this.array.Length - 1))
                {
                    this.isFull = true;
                }
            }
        } 
        #endregion

        #region GetLatestObjects
        /// <summary>
        /// 按照由老到新的顺序获取所有缓存内的对象。
        /// </summary>       
        public List<T> GetLatestObjects()
        {
            lock (this.locker)
            {
                List<T> list = new List<T>();
                if (!this.isFull)
                {
                    for (int i = 0; i <= this.latestObjectIndex; i++)
                    {
                        list.Add(this.array[i]);
                    }

                    return list;
                }

                int startPosition = (this.latestObjectIndex + 1) % this.array.Length;
                for (int i = 0; i < this.array.Length; i++)
                {
                    int index = (startPosition + i) % this.array.Length;
                    list.Add(this.array[index]);
                }

                return list;
            }
        } 
        #endregion
    }
}
