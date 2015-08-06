using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement
{
    /// <summary>
    /// FixedQueue 固定大小的队列，当到达Capacity时，缓存新object，则最先缓存的object会被删除掉。
    /// FixedQueue是线程安全的。
    /// </summary>  
    public class FixedQueue<T> 
    {
        private Queue<T> queue = new Queue<T>();
        private object locker = new object();
        public event CbGeneric<T> ObjectDiscarded;

        #region Ctor
        public FixedQueue()
            : this(int.MaxValue)
        {
        }

        public FixedQueue(int _capacity)
        {
            this.capacity = capacity;
            this.ObjectDiscarded += delegate { };
        }
        
        #endregion

        #region Capacity
        private int capacity = int.MaxValue;
        public int Capacity
        {
            get { return capacity; }
            set 
            {
                if (value < 0)
                {
                    throw new Exception("The capacity of FixedQueue can't be less than 0 !");
                }

                lock (this.locker)
                {
                    if (value == 0)
                    {
                        this.queue.Clear();
                        this.capacity = value;
                        return;
                    }

                    if (value > 0)
                    {
                        this.capacity = value;

                        while (this.queue.Count > this.capacity)
                        {
                            this.ObjectDiscarded(this.queue.Dequeue());
                        }
                    }
                }
            }
        } 
        #endregion

        #region Count
        public int Count
        {
            get
            {
                return this.queue.Count;
            }
        } 
        #endregion

        #region Enqueue
        public void Enqueue(T obj)
        {
            if (this.capacity <= 0)
            {
                return;
            }

            lock (this.locker)
            {
                if (this.queue.Count >= this.capacity)
                {
                    this.ObjectDiscarded(this.queue.Dequeue());
                }

                this.queue.Enqueue(obj);
            }
        }

        #endregion

        #region Dequeue
        public T Dequeue()
        {
            lock (this.locker)
            {
                return this.queue.Dequeue();
            }
        }
        #endregion

        #region Peek
        public T Peek()
        {
            lock (this.locker)
            {
                return this.queue.Peek();
            }
        } 
        #endregion

        #region Remove
        public void Remove(T obj)
        {
            lock (this.locker)
            {
                Queue<T> newQueue = new Queue<T>();
                while (this.queue.Count > 0)
                {
                    T tmp = this.queue.Dequeue();
                    if (!tmp.Equals(obj))
                    {
                        newQueue.Enqueue(tmp);
                    }
                }
                this.queue = newQueue;
            }
        } 
        #endregion

        #region GetObjectArrayCopy
        public T[] GetObjectArrayCopy()
        {
            lock (this.locker)
            {
                return this.queue.ToArray();
            }
        } 
        #endregion

        #region Clear
        public void Clear()
        {
            lock (this.locker)
            {
                this.queue.Clear();
            }
        }
        #endregion

    }
}
