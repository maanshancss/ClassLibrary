using System;
using System.Collections.Generic ;

namespace ESBasic.ObjectManagement.Trees.Multiple
{
	/// <summary>
    /// IMultiTree 多叉树基础接口。该接口的实现必须是线程安全的。
	/// zhuweisky 2005.07.28
	/// </summary>
    public interface IMultiTree<TVal> where TVal : IMTreeVal
	{
        /// <summary>
        /// Initialize 使用已经存在的某个树（或子树）来构造一个新树。
        /// </summary>      
        void Initialize(MNode<TVal> _rootNode);


        /// <summary>
        /// Initialize 通过各个节点的内部值，重新构造多叉树的层级关系。
        /// </summary>
        void Initialize(TVal rootVal, IList<TVal> members);       

        /// <summary>
        /// Root 返回多叉树的根节点。
        /// </summary>
        MNode<TVal> Root { get;}	
	
        /// <summary>
        /// Count 多叉树的当前节点总数。
        /// </summary>
		int   Count {get ;}

        /// <summary>
        /// GetNodesOnDepthIndex 获取某一深度的所有节点。Root的深度索引为0
        /// </summary>        
        IList<MNode<TVal>> GetNodesOnDepthIndex(int depthIndex);

        /// <summary>
        /// GetNodesOnDepthIndex 获取所属idPath体系下并且深度为depthIndex的所有节点。Root的深度索引为0
        /// </summary>        
        IList<MNode<TVal>> GetNodesOnDepthIndex(string idPath, char separator ,int depthIndex);

        MNode<TVal> GetNodeByID(string valID);

        /// <summary>
        /// GetNodeByPath 根据节点的ID的路径快速搜索到对应的节点，如idPath--0.1.1.0.2.5。比GetNodeByID效率要高。
        /// </summary>        
        MNode<TVal> GetNodeByPath(string idPath, char separator);

        /// <summary>
        /// GetFamilyByPath 获取路径上的所有节点（从根到叶的顺序，即返回列表的第一个元素为根）。如果路径上的某个节点值在树中不存在，则直接返回null。
        /// </summary>        
        List<MNode<TVal>> GetFamilyByPath(string idPath, char separator);

        /// <summary>
        /// GetOffsprings 获取某个节点的所有子孙节点。
        /// </summary>       
        IList<MNode<TVal>> GetOffsprings(string valID);
        
        /// <summary>
        /// GetLeaves 获取某个路径下的所有叶子节点。如果Path已经是叶子节点，则返回包含自己的列表。
        /// </summary>     
        IList<TVal> GetLeaves(string path, char separator);

        /// <summary>
        /// ActionOnEachNode 从root开始对每个节点进行一次action。
        /// </summary>       
        void ActionOnEachNode(CbGeneric<MNode<TVal>> action);
	}
}

