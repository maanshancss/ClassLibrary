using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// SamePriorityObjectManager 同一优先级对象管理器的参考实现。
    /// </summary>    
    public class SamePriorityObjectManager<T> : ISamePriorityObjectManager<T>
    {
        private LinkedList<T> waiterList = new LinkedList<T>();
        private SmartRWLocker smartRWLocker = new SmartRWLocker();
        public event CbGeneric<T> WaiterDiscarded;

        #region Capacity
        private int capacity = int.MaxValue;
        public int Capacity
        {
            get { return capacity; }
        } 
        #endregion

        #region Count
        public int Count
        {
            get
            {
                using (this.smartRWLocker.Lock(AccessMode.Read))
                {
                    return this.waiterList.Count;
                }
            }
        }
        #endregion    
   
        #region Full
        public bool Full
        {
            get
            {
                return this.Count >= this.capacity;
            }
        } 
        #endregion

        #region ActionTypeOnAddOverflow
        private ActionTypeOnAddOverflow actionTypeOnAddOverflow = ActionTypeOnAddOverflow.Wait;
        public ActionTypeOnAddOverflow ActionTypeOnAddOverflow
        {
            get { return actionTypeOnAddOverflow; }            
        } 
        #endregion

        #region DetectSpanInMSecsOnWait
        private int detectSpanInMSecsOnWait = 10;
        public int DetectSpanInMSecsOnWait
        {
            get { return detectSpanInMSecsOnWait; }
            set 
            {                 
                detectSpanInMSecsOnWait = value <= 0 ? 1 : value; 
            }
        }        
        #endregion

        #region Ctor
        public SamePriorityObjectManager()
        {
            this.WaiterDiscarded += delegate { };
        }
        public SamePriorityObjectManager(int _capacity, ActionTypeOnAddOverflow actionType, int _detectSpanInMSecsOnWait)
            : this()
        {
            this.capacity = _capacity;
            this.actionTypeOnAddOverflow = actionType;
            this.detectSpanInMSecsOnWait = _detectSpanInMSecsOnWait;
        }

        public SamePriorityObjectManager(int _capacity, ActionTypeOnAddOverflow actionType):this(_capacity,actionType,10)
        {
        }

        public SamePriorityObjectManager(PriorityManagerPara para)
            : this(para.Capacity, para.ActionTypeOnAddOverflow, para.DetectSpanInMSecsOnWait)
        {
        }
        #endregion

        #region AddWaiter
        public void AddWaiter(T waiter)
        {
            if (this.waiterList.Count < this.capacity)
            {
                using (this.smartRWLocker.Lock(AccessMode.Write))
                {
                    this.waiterList.AddLast(waiter);
                    return;
                }
            }

            if (this.actionTypeOnAddOverflow == ActionTypeOnAddOverflow.DiscardCurrent)
            {
                this.WaiterDiscarded(waiter);
                return;
            }

            if (this.actionTypeOnAddOverflow == ActionTypeOnAddOverflow.DiscardLatest)
            {
                T discarded = default(T);
                using (this.smartRWLocker.Lock(AccessMode.Write))
                {
                    if (this.waiterList.Count > 0)
                    {
                        discarded = this.waiterList.Last.Value;
                        this.waiterList.RemoveLast();
                    }
                    this.waiterList.AddLast(waiter);                    
                }
                this.WaiterDiscarded(discarded);
                return;
            }

            if (this.actionTypeOnAddOverflow == ActionTypeOnAddOverflow.DiscardOldest)
            {
                T discarded = default(T);
                using (this.smartRWLocker.Lock(AccessMode.Write))
                {
                    if (this.waiterList.Count > 0)
                    {
                        discarded = this.waiterList.First.Value;
                        this.waiterList.RemoveFirst();
                    }
                    this.waiterList.AddLast(waiter);                    
                }
                this.WaiterDiscarded(discarded);
                return;
            }
           
            //this.actionTypeOnAddOverflow == ActionTypeOnAddOverflow.Wait
            while (this.waiterList.Count >= this.capacity)
            {
                System.Threading.Thread.Sleep(this.detectSpanInMSecsOnWait);
            }

            using (this.smartRWLocker.Lock(AccessMode.Write))
            {                
                this.waiterList.AddLast(waiter);                
            }
        } 
        #endregion

        #region Contains
        public bool Contains(T waiter)
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))
            {
                return this.waiterList.Contains(waiter);
            }
        } 
        #endregion

        #region RemoveWaiter
        public void RemoveWaiter(T waiter)
        {
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                if (!this.waiterList.Contains(waiter))
                {
                    return;
                }

                this.waiterList.Remove(waiter);
            }
        } 
        #endregion        

        #region GetNextWaiter
        public T GetNextWaiter()
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))           
            {
                if (this.waiterList.Count == 0)
                {
                    return default(T);
                }

                return this.waiterList.First.Value;    
            }
        } 
        #endregion

        #region PopNextWaiter
        public T PopNextWaiter()
        {
            using (this.smartRWLocker.Lock(AccessMode.Write))
            {
                if (this.waiterList.Count == 0)
                {
                    return default(T);
                }

                T target = this.waiterList.First.Value;
                this.waiterList.RemoveFirst();
                return target;
            }
        }
        #endregion

        #region GetWaitersByPriority
        public T[] GetWaitersByPriority()
        {
            using (this.smartRWLocker.Lock(AccessMode.Read))           
            {
                T[] waiters = new T[this.Count];
                if (waiters.Length == 0)
                {
                    return waiters;
                }

                LinkedListNode<T> firstNode = this.waiterList.First;
                waiters[0] = firstNode.Value;

                LinkedListNode<T> temp = firstNode;
                for (int i = 1; i < waiters.Length; i++)
                {
                    temp = temp.Next;
                    waiters[i] = temp.Value;
                }

                return waiters;
            }
        } 
        #endregion      

        #region Clear
        public void Clear()
        {
            using (this.smartRWLocker.Lock(AccessMode.Write))           
            {
                this.waiterList.Clear();
            }
        } 
        #endregion        
    }
}
