using System;
using System.Collections.Generic;

namespace ESBasic.DataStructure
{
    /// <summary>
    /// IPriorityManager 依据时间来管理所有waiter的优先级。线程安全。
    /// zhuweisky 2007.04.16 
    /// </summary>
    public interface IPriorityManager
    {
        void AddWaiter(int waiterID);

        int Count { get; }

        /// <summary>
        /// GetNextWaiter 返回等待时间最长的waiter。
        /// 注意，返回时并不会从等待列表中删除waiter。如果要删除某个等待者，请调用RemoveWaiter。
        /// </summary>       
        int? GetNextWaiter();

        int[] GetWaitersByPriority();

        void RemoveWaiter(int waiterID);

        void Clear();

        bool Contains(int waiterID);

        IList<int> GetWaiterList();
    }
}
