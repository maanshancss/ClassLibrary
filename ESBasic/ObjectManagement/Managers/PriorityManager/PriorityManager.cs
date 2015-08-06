using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// PriorityManager 具有优先级的对象的管理器。该实现是线程安全的。
    /// 注意：在PriorityManager中，优先等级是用int表示的，是从0开始连续的一串整数，整数值越小，表明优先级越高。
    /// 当Initialize方法被执行后，优先等级的范围就被固定下来。比如PriorityLevelCount值设为4，则PriorityManager所支持的优先等级即为：0，1，2，3
    /// </summary>
    /// <typeparam name="T">被管理的对象的类型，必须从IPriorityObject继承。</typeparam>   
    public class PriorityManager<T> where T : class ,IPriorityObject 
    {
        private ISamePriorityObjectManager<T>[] spObjectManagerAry = null;        
        public event CbGeneric<T> WaiterDiscarded;
      
        #region PriorityLevelCount
        private int priorityLevelCount = 1;
        /// <summary>
        /// PriorityLevelCount 优先级分为几个等级。
        /// </summary>
        public int PriorityLevelCount
        {
            get { return priorityLevelCount; }            
        } 
        #endregion

        #region Ctor
        public PriorityManager()
        {
            this.WaiterDiscarded += delegate { };
        } 
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize 初始化。所有级别的管理器都使用默认配置（Capacity为int.MaxValue）。
        /// </summary>
        /// <param name="_priorityLevelCount">支持的优先级等级个数。比如PriorityLevelCount值设为4，则所支持的优先等级即为：0，1，2，3</param>        
        public void Initialize(int _priorityLevelCount)
        {
            if (_priorityLevelCount < 1)
            {
                throw new Exception("Parameter _priorityLevelCount must > 0 !");
            }

            this.Initialize(_priorityLevelCount, new PriorityManagerPara[_priorityLevelCount]);        
        }

        /// <summary>
        /// Initialize 初始化。
        /// </summary>
        /// <param name="_priorityLevelCount">支持的优先级等级个数。比如PriorityLevelCount值设为4，则所支持的优先等级即为：0，1，2，3</param>
        /// <param name="paraAry">每个优先级管理器对应的配置，该参数不能为null，且其长度必须与_priorityLevelCount相等。如果某个优先级的管理器采用默认配置，则对应的数组元素可以为null。</param>
        public void Initialize(int _priorityLevelCount ,params PriorityManagerPara[] paraAry)
        {
            if (_priorityLevelCount < 1)
            {
                throw new Exception("Parameter _priorityLevelCount must > 0 !");
            }

            if (paraAry.Length != _priorityLevelCount)
            {
                throw new Exception("Parameter _priorityLevelCount must equal the length of paraAry !");
            }

            this.priorityLevelCount = _priorityLevelCount;
            this.spObjectManagerAry = new ISamePriorityObjectManager<T>[this.priorityLevelCount];

            for (int i = 0; i < this.spObjectManagerAry.Length; i++)
            {
                if (paraAry[i] == null)
                {
                    this.spObjectManagerAry[i] = new SamePriorityObjectManager<T>();
                }
                else
                {
                    this.spObjectManagerAry[i] = new SamePriorityObjectManager<T>(paraAry[i]);
                }

                this.spObjectManagerAry[i].WaiterDiscarded += new CbGeneric<T>(PriorityManager_WaiterDiscarded);
            }
        }

        void PriorityManager_WaiterDiscarded(T obj)
        {
            this.WaiterDiscarded(obj);
        } 
        #endregion

        #region AddWaiter
        public void AddWaiter(T waiter)
        {
            if ((waiter.PriorityLevel < 0) || (waiter.PriorityLevel >= this.priorityLevelCount))
            {
                throw new Exception("Current PriorityManager instance don't support the PriorityLevel of the target.");
            }

            this.spObjectManagerAry[waiter.PriorityLevel].AddWaiter(waiter);
        } 
        #endregion

        #region Count
        public int Count
        {
            get
            {
                int count = 0;
                for (int i = 0; i < this.spObjectManagerAry.Length; i++)
                {
                    count += this.spObjectManagerAry[i].Count;
                }

                return count;
            }
        } 
        #endregion

        #region GetCount
        public int GetCount(int priorityLevel)
        {
            if ((priorityLevel < 0) || (priorityLevel >= this.priorityLevelCount))
            {
                return 0;
            }

            return this.spObjectManagerAry[priorityLevel].Count;
        } 
        #endregion

        #region GetNextWaiter
        /// <summary>
        /// GetNextWaiter 返回优先级别最高且等待时间最长的waiter。
        /// 注意，返回时并不会从等待列表中删除waiter。如果要删除某个等待者，请调用RemoveWaiter。
        /// </summary>       
        public T GetNextWaiter()
        {
            for (int i = 0; i < this.spObjectManagerAry.Length; i++)
            {
                if (this.spObjectManagerAry[i].Count > 0)
                {
                    return this.spObjectManagerAry[i].GetNextWaiter();
                }
            }

            return null;
        }
        #endregion

        #region PopNextWaiter
        /// <summary>
        /// PopNextWaiter 弹出优先级别最高且等待时间最长的waiter。注意，此方法将会从等待列表中删除waiter。       
        /// </summary>       
        public T PopNextWaiter()
        {
            for (int i = 0; i < this.spObjectManagerAry.Length; i++)
            {
                if (this.spObjectManagerAry[i].Count > 0)
                {
                    return this.spObjectManagerAry[i].PopNextWaiter();
                }
            }

            return null;
        }
        #endregion

        #region GetWaitersByPriority
        public T[] GetWaitersByPriority()
        {
            if (this.Count == 0)
            {
                return null;
            }

            T[] resultAry = new T[this.Count];
            int startIndex = 0;
            for (int i = 0; i < this.spObjectManagerAry.Length; i++)
            {
                if (this.spObjectManagerAry[i].Count > 0)
                {
                    T[] temp = this.spObjectManagerAry[i].GetWaitersByPriority();
                    for (int index = 0; index < temp.Length; index++)
                    {
                        resultAry[startIndex++] = temp[index];
                    }
                }
            }

            return resultAry;
        } 
        #endregion

        #region RemoveWaiter
        public void RemoveWaiter(T waiter)
        {
            if ((waiter.PriorityLevel < 0) || (waiter.PriorityLevel >= this.priorityLevelCount))
            {
                return;
            }

            this.spObjectManagerAry[waiter.PriorityLevel].RemoveWaiter(waiter);
        } 
        #endregion

        #region Clear
        public void Clear()
        {
            if (this.spObjectManagerAry == null)
            {
                return;
            }

            for (int i = 0; i < this.spObjectManagerAry.Length; i++)
            {
                this.spObjectManagerAry[i].Clear();
            }
        }

        public void Clear(int priorityLevel)
        {
            if ((this.spObjectManagerAry == null) || (priorityLevel < 0) || (priorityLevel >= this.priorityLevelCount))
            {
                return ;
            }

            this.spObjectManagerAry[priorityLevel].Clear();
        }
        
        #endregion

        #region Contains
        public bool Contains(T waiter)
        {
            if ((waiter.PriorityLevel < 0) || (waiter.PriorityLevel >= this.priorityLevelCount))
            {
                return false;
            }

            return this.spObjectManagerAry[waiter.PriorityLevel].Contains(waiter);
        } 
        #endregion
    }
}
