using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.DataStructure
{
    public class PriorityManager : IPriorityManager
    {
        private LinkedList<int> waiterIDList = new LinkedList<int>();
        private object listLock = new object();

        #region AddWaiter
        public void AddWaiter(int waiterID)
        {
            if (this.waiterIDList.Contains(waiterID))
            {
                return;
            }

            lock (listLock)
            {
                this.waiterIDList.AddLast(waiterID);
            }
        } 
        #endregion

        #region Contains
        public bool Contains(int waiterID)
        {
            return this.waiterIDList.Contains(waiterID);
        } 
        #endregion

        #region RemoveWaiter
        public void RemoveWaiter(int waiterID)
        {
            if (!this.waiterIDList.Contains(waiterID))
            {
                return;
            }

            lock (listLock)
            {
                this.waiterIDList.Remove(waiterID);
            }
        } 
        #endregion        

        #region GetNextWaiter
        public int? GetNextWaiter()
        {
            lock (this.listLock)
            {
                if (this.waiterIDList.Count == 0)
                {
                    return null;
                }

                return this.waiterIDList.First.Value;    
            }
        } 
        #endregion

        #region GetWaitersByPriority
        public int[] GetWaitersByPriority()
        {
            lock (this.listLock)
            {
                int[] waiters = new int[this.Count];
                if (waiters.Length == 0)
                {
                    return waiters;
                }

                LinkedListNode<int> firstNode = this.waiterIDList.First;
                waiters[0] = firstNode.Value;

                LinkedListNode<int> temp = firstNode;
                for (int i = 1; i < waiters.Length; i++)
                {
                    temp = temp.Next;
                    waiters[i] = temp.Value;
                }

                return waiters;
            }
        } 
        #endregion

        #region GetWaiterList
        public IList<int> GetWaiterList()
        {
            lock (this.waiterIDList)
            {
                return ESBasic.Collections.CollectionConverter.CopyAllToList<int>(this.waiterIDList);               
            }
        } 
        #endregion

        #region Clear
        public void Clear()
        {
            lock (this.waiterIDList)
            {
                this.waiterIDList.Clear();
            }
        } 
        #endregion

        #region Count
        public int Count
        {
            get
            {
                return this.waiterIDList.Count;
            }
        } 
        #endregion       
    }
}
