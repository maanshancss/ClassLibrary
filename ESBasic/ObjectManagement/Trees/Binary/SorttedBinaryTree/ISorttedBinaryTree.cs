using System;
using System.Collections.Generic ;

namespace ESBasic.ObjectManagement.Trees.Binary
{
	/// <summary>
	/// IBinaryTree 二叉树接口 。
	/// 作者：朱伟 sky.zhuwei@163.com 
	/// </summary>
    public interface ISorttedBinaryTree<TVal> : IBinaryTree<TVal> where TVal : IComparable
	{
		void Insert(TVal val) ;//如果树中有一个节点的值等于val的值，则val将被忽略
		void Remove(TVal val) ;       
		bool Contains(TVal var) ;

        Node<TVal> Get(TVal var);
        Node<TVal> GetMaxNode(Node<TVal> childTree);
        Node<TVal> GetMinNode(Node<TVal> childTree);
        IList<Node<TVal>> GetAllNodesAscend(bool ascend);

		//遍历二叉树
		IList<Node<TVal>> Traverse(TraverseMode mode) ; 
	}		

	public enum TraverseMode
	{
		PreOrder ,MidOrder ,PostOrder
	}
}
