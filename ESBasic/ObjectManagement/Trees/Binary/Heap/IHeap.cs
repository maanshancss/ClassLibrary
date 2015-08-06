using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Trees.Binary
{
    /// <summary>
    /// IHeap 最大、最小堆。CQ，2008.11.26
    /// </summary>   
    public interface IHeap<TVal> : IBinaryTree<TVal> where TVal : System.IComparable
    {
        HeapType HeapType { get; }

        void Insert(TVal val);

        /// <summary>
        /// Pop 弹出根节点的值，并删除根节点。如果树为空，则
        /// </summary>      
        TVal Pop();        
    }

    public enum HeapType
    {
        /// <summary>
        /// 最大堆
        /// </summary>
        Max,
        /// <summary>
        /// 最小堆
        /// </summary>
        Min
    }
}
