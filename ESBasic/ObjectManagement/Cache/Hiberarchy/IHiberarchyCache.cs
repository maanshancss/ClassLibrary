using System;
using System.Collections.Generic;
using ESBasic.ObjectManagement.Trees.Multiple;

namespace ESBasic.ObjectManagement.Cache
{
    /// <summary>
    /// IHiberarchyCache 增强的、寻址快速的层级结构缓存。该接口的实现必须是线程安全的。
    /// 内部的Tree和Dictionary的数据始终是同步的。
    /// </summary>   
    public interface IHiberarchyCache<TVal> where TVal : IHiberarchyVal
    {        
        /// <summary>
        /// RootID 设置根节点的ID。
        /// </summary>
        string RootID { set; }

        /// <summary>
        /// SequenceCodeSplitter 节点路径（序列号）的分割符。
        /// </summary>
        char SequenceCodeSplitter { get; set; }

        IObjectRetriever<string ,TVal> ObjectRetriever { set; }
        
        int Count { get; }

        void Initialize();
        
        /// <summary>
        /// Get 如果目标对象在缓存中不存在，则通过ObjectRetriever去提取。
        /// </summary>           
        TVal Get(string id);

        /// <summary>
        /// HaveContained 缓存中是否一经包含了目标对象。
        /// </summary>       
        bool HaveContained(string id);
        
        /// <summary>
        /// GetAllKeyListCopy 获取所有ID的列表的拷贝。
        /// </summary>         
        IList<string> GetAllKeyListCopy();

        /// <summary>
        /// GetAllValListCopy 获取所有的节点值列表的拷贝。
        /// </summary>       
        IList<TVal> GetAllValListCopy();
        
        /// <summary>
        /// GetChildrenOf 获取parentID的所有孩子节点的节点值列表。
        /// </summary>     
        IList<TVal> GetChildrenOf(string parentID);

        /// <summary>
        /// GetChildrenCount 获取parentID直接下级的个数。
        /// </summary>        
        int GetChildrenCount(string parentID);

        /// <summary>
        /// CreateHiberarchyTree 返回表示层级信息的最单纯的数据结构。
        /// 注意：返回的Tree实际上与内部的AgileMultiTree是引用的根节点是同一个节点。
        /// </summary>        
        MultiTree<TVal> CreateHiberarchyTree();

        /// <summary>
        /// GetNodesOnDepthIndex 获取某一深度的所有节点。Root的深度索引为0
        /// </summary> 
        IList<TVal> GetNodesOnDepthIndex(int depthIndex);

        /// <summary>
        /// GetNodesOnDepthIndex 获取所属parentID体系下并且深度为depthIndex的所有节点。Root的深度索引为0
        /// </summary>        
        IList<TVal> GetNodesOnDepthIndex(string parentID, int depthIndex);        
    }
}
