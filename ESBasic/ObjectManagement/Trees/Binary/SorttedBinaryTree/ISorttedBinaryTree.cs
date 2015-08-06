using System;
using System.Collections.Generic ;

namespace ESBasic.ObjectManagement.Trees.Binary
{
	/// <summary>
	/// IBinaryTree �������ӿ� ��
	/// ���ߣ���ΰ sky.zhuwei@163.com 
	/// </summary>
    public interface ISorttedBinaryTree<TVal> : IBinaryTree<TVal> where TVal : IComparable
	{
		void Insert(TVal val) ;//���������һ���ڵ��ֵ����val��ֵ����val��������
		void Remove(TVal val) ;       
		bool Contains(TVal var) ;

        Node<TVal> Get(TVal var);
        Node<TVal> GetMaxNode(Node<TVal> childTree);
        Node<TVal> GetMinNode(Node<TVal> childTree);
        IList<Node<TVal>> GetAllNodesAscend(bool ascend);

		//����������
		IList<Node<TVal>> Traverse(TraverseMode mode) ; 
	}		

	public enum TraverseMode
	{
		PreOrder ,MidOrder ,PostOrder
	}
}
