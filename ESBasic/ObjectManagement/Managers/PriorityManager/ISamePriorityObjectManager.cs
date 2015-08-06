using System;
using System.Collections.Generic;

namespace ESBasic.ObjectManagement.Managers
{
    /// <summary>
    /// ISamePriorityObjectManager 同一优先级对象管理器，用于管理同一优先级的所有对象，这些对象将按照先来后到的顺序确定其优先顺序（“第二优先级”）。
    /// 该接口的实现必须保证是线程安全的。
    /// zhuweisky 2007.04.16 /2008.08.13修订
    /// </summary>
    /// <typeparam name="T">要被管理的对象的类型</typeparam>
    public interface ISamePriorityObjectManager<T>
    {
        event CbGeneric<T> WaiterDiscarded; 

        /// <summary>
        /// Capacity 先级对象管理器的最大容量。当容器中的个数达到此容量时，若再向容器中添加waiter，则将根据ActionTypeOnAddOverflow属性的值来采取对应的动作。
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// DetectSpanInMSecsOnWait 当容器中的个数达到此容量时，若ActionTypeOnAddOverflow属性值为Wait，则相邻的两次检测时间间隔，单位毫秒。
        /// </summary>
        int DetectSpanInMSecsOnWait { get; set; }

        ActionTypeOnAddOverflow ActionTypeOnAddOverflow { get; }

        /// <summary>
        /// AddWaiter 添加一个等待者。如果等待者在管理器中已经存在，则直接返回。
        /// </summary>       
        void AddWaiter(T waiter);

        /// <summary>
        /// 当前管理器中等待者的数量。
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 是否已经满了。
        /// </summary>
        bool Full { get; }

        /// <summary>
        /// GetNextWaiter 返回等待时间最长的waiter。
        /// 注意，返回时并不会从等待列表中删除waiter。如果要删除某个等待者，请调用RemoveWaiter。
        /// </summary>       
        T GetNextWaiter();

        /// <summary>
        /// PopNextWaiter 弹出等待时间最长的waiter。注意，此方法将会从等待列表中删除waiter。  
        /// </summary>        
        T PopNextWaiter();

        /// <summary>
        /// GetWaitersByPriority 按照等待者加入的先后顺序返回等待者数组，数组中index越小的等待者其等待时间越长，其优先级也越高。
        /// </summary>       
        T[] GetWaitersByPriority();

        /// <summary>
        /// RemoveWaiter 从管理器中移除指定的等待者。
        /// </summary>        
        void RemoveWaiter(T waiter);

        /// <summary>
        /// Clear 清空管理器中的所有等待者。
        /// </summary>
        void Clear();

        /// <summary>
        /// Contains 管理器中是否存在指定的等待者。
        /// </summary>       
        bool Contains(T waiter);
    }

    /// <summary>
    /// ActionTypeOnAddOverflow 当ISamePriorityObjectManager的Count达到Capacity时，再向容器中添加waiter时所采取的措施。
    /// </summary>
    public enum ActionTypeOnAddOverflow
    {
        /// <summary>
        /// Wait 等待一直到Count小于Capacity时，再加入。default默认值。
        /// </summary>
        Wait = 0,
        /// <summary>
        /// DiscardOldest 丢弃容器中最早的waiter。
        /// </summary>
        DiscardOldest,
        /// <summary>
        /// DiscardLatest 丢弃容器中最新的waiter。
        /// </summary>
        DiscardLatest,
        /// <summary>
        /// DiscardCurrent 丢弃当前要添加的waiter。
        /// </summary>
        DiscardCurrent
    }

    [Serializable]
    public class PriorityManagerPara
    {
        #region Ctor
        public PriorityManagerPara() { }
        public PriorityManagerPara(int _capacity, ActionTypeOnAddOverflow actionType, int _detectSpanInMSecsOnWait)
        {
            this.capacity = _capacity;
            this.actionTypeOnAddOverflow = actionType;
            this.detectSpanInMSecsOnWait = _detectSpanInMSecsOnWait;
        }
        public PriorityManagerPara(int _capacity, ActionTypeOnAddOverflow actionType)
        {
            this.capacity = _capacity;
            this.actionTypeOnAddOverflow = actionType;           
        }
        #endregion

        #region Capacity
        private int capacity = int.MaxValue;
        public int Capacity
        {
            get { return capacity; }
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
    }
}
