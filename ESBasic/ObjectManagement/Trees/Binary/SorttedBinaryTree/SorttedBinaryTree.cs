using System;
using System.Collections.Generic ;

namespace ESBasic.ObjectManagement.Trees.Binary
{
	/// <summary>
	/// SorttedBinaryTree ISorttedBinaryTree的默认实现。Remove方法由CQ实现。
	/// 作者：朱伟 sky.zhuwei@163.com 
	/// 2004.12.26
	/// </summary>
    public class SorttedBinaryTree<TVal> : ISorttedBinaryTree<TVal> where TVal : IComparable
	{
		protected Node<TVal> root = null ;

        #region property
        public int Depth
        {
            get
            {
                return this.GetDepth();
            }
        }

        #region GetDepth
        private int GetDepth()
        {
            IList<Node<TVal>> list = this.GetAllNodesAscend(true);
            if (list == null)
            {
                return 0;
            }

            IList<int> depthList = new List<int>();
            foreach (Node<TVal> node in list)
            {
                if (node.IsLeaf)
                {
                    depthList.Add(this.ComputeDepth(node));
                }
            }

            int depth = depthList[0];
            foreach (int dep in depthList)
            {
                if (dep > depth)
                {
                    depth = dep;
                }
            }

            return depth;
        }

        private int ComputeDepth(Node<TVal> leaf)
        {
            int count = 1;
            Node<TVal> curNode = leaf;

            while (curNode.Parent != null)
            {
                ++count;
                curNode = curNode.Parent;
            }

            return count;
        }
        #endregion		      

        public Node<TVal> Root
        {
            get
            {
                return this.root;
            }
        }  

        public int Count
        {
            get
            {
                if (this.root == null)
                {
                    return 0;
                }

                int count = 1;
                this.CountAllNodes(this.root, ref count);

                return count;
            }
        }

        #region CountAllNodes
        private void CountAllNodes(Node<TVal> childTreeRoot, ref int count)
        {
            if (childTreeRoot == null)
            {
                return;
            }

            ++count;

            this.CountAllNodes(childTreeRoot.LeftChild, ref count);
            this.CountAllNodes(childTreeRoot.RightChild, ref count);
        }
        #endregion

        #endregion

		#region Method
        #region Insert
        //如果树中有一个节点的值等于TheValue的值，则TheValue将被忽略
		public virtual void Insert(TVal theValue)
		{
			if(this.root == null)
			{
                this.root = new Node<TVal>(theValue ,null);
				return ;
			}

            Node<TVal> tempRoot = this.root;
            while (true)
            {
                int compareResult = theValue.CompareTo(tempRoot.TheValue);
                if (compareResult == 0)
                {
                    return ;
                }

                if (compareResult > 0)
                {
                    if (tempRoot.RightChild == null)
                    {
                        tempRoot.RightChild = new Node<TVal>(theValue ,tempRoot);
                        return;
                    }

                    tempRoot = tempRoot.RightChild;
                }
                else
                {
                    if (tempRoot.LeftChild == null)
                    {
                        tempRoot.LeftChild = new Node<TVal>(theValue ,tempRoot);
                        return;
                    }

                    tempRoot = tempRoot.LeftChild;
                }
            }           
		}
		#endregion

        #region Remove
        public virtual void Remove(TVal theValue)
        {
            this.Remove(theValue, ref this.root);
        }  

        private void Remove(TVal theValue, ref Node<TVal> node)
        {
            if (node == null) return;
            if (node.TheValue.CompareTo(theValue) == 0)
            {
                if (node.LeftChild != null && node.RightChild != null)
                {
                    Node<TVal> rightMinNode = this.GetMinNode(node.RightChild);
                    if (rightMinNode.Parent.TheValue.CompareTo(theValue) == 0)
                    {
                        Node<TVal> parent = node.Parent;
                        rightMinNode.LeftChild = node.LeftChild;
                        node = rightMinNode;
                        node.Parent = parent;
                    }
                    else
                    {
                        rightMinNode.Parent.LeftChild = rightMinNode.RightChild;
                        if (rightMinNode.RightChild != null)
                        {
                            rightMinNode.RightChild.Parent = rightMinNode.Parent;
                        }
                        node.TheValue = rightMinNode.TheValue;

                    }
                }
                else if (node.LeftChild != null)
                {
                    Node<TVal> parent = node.Parent;
                    node = node.LeftChild;
                    node.Parent = parent;
                }
                else if (node.RightChild != null)
                {
                    Node<TVal> parent = node.Parent;
                    node = node.RightChild;
                    node.Parent = parent;
                }
                else
                {
                    node = null;
                }
            }
            else
            {
                if (theValue.CompareTo(node.TheValue) < 0)
                {
                    Node<TVal> leftNode = node.LeftChild;
                    this.Remove(theValue, ref leftNode);
                    node.LeftChild = leftNode;
                }
                else
                {
                    Node<TVal> rightNode = node.RightChild;
                    this.Remove(theValue, ref rightNode);
                    node.RightChild = rightNode;
                }
            }
        }

		     
		#endregion

		#region Contains
        public bool Contains(TVal var)
		{			
			return (this.Get(var) != null) ;
		}
		#endregion      		

		#region GetAllNodesAscend 
		//非深拷贝 ，外部不得改变得到的各个元素的值
        public IList<Node<TVal>> GetAllNodesAscend(bool ascend)
		{
            IList<Node<TVal>> list = new List<Node<TVal>>();
			this.DoGetAllNodes(this.root ,ascend ,ref list ,TraverseMode.MidOrder) ;

			return list ;
		}

        private void DoGetAllNodes(Node<TVal> childTreeRoot, bool ascend, ref IList<Node<TVal>> list, TraverseMode mode)
		{	
			if(childTreeRoot == null)
			{
				return ;
			}

			//中序遍历
			if(mode == TraverseMode.MidOrder)
			{
				if(ascend)
				{					
					this.DoGetAllNodes(childTreeRoot.LeftChild ,ascend ,ref list ,mode) ;
					list.Add(childTreeRoot) ;
					this.DoGetAllNodes(childTreeRoot.RightChild ,ascend ,ref list ,mode) ;
				}
				else
				{
					this.DoGetAllNodes(childTreeRoot.RightChild ,ascend ,ref list ,mode) ;
					list.Add(childTreeRoot) ;
					this.DoGetAllNodes(childTreeRoot.LeftChild ,ascend ,ref list ,mode) ;
				}
			}
			else if(mode == TraverseMode.PreOrder)
			{
				list.Add(childTreeRoot) ;
				this.DoGetAllNodes(childTreeRoot.LeftChild ,ascend ,ref list ,mode) ;				
				this.DoGetAllNodes(childTreeRoot.RightChild ,ascend ,ref list ,mode) ;
			}
			else
			{
				this.DoGetAllNodes(childTreeRoot.LeftChild ,ascend ,ref list ,mode) ;				
				this.DoGetAllNodes(childTreeRoot.RightChild ,ascend ,ref list ,mode) ;
				list.Add(childTreeRoot) ;
			}
		}
		#endregion

		#region Traverse
        public IList<Node<TVal>> Traverse(TraverseMode mode)
		{
            IList<Node<TVal>> list = new List<Node<TVal>>();
			switch(mode)
			{
				case TraverseMode.MidOrder :
				{
					this.DoGetAllNodes(this.root ,true ,ref list ,TraverseMode.MidOrder) ;
					return list ;
				}			
				case TraverseMode.PreOrder :
				{
					this.DoGetAllNodes(this.root ,true ,ref list ,TraverseMode.PreOrder) ;
					return list ;
				}
				case TraverseMode.PostOrder :
				{
					this.DoGetAllNodes(this.root ,true ,ref list ,TraverseMode.PostOrder) ;
					return list ;
				}
				default:
				{
					throw new Exception("Error TraverseMode !") ;
				}
			}
		}	
		#endregion

        #region Get
        public Node<TVal> Get(TVal theValue)
        {
            if (this.root == null)
            {
                return null;
            }

            if (theValue.CompareTo(this.root.TheValue) == 0)
            {
                return this.root;
            }

            Node<TVal> tempRoot = this.root;
            while (tempRoot != null)
            {
                int compareResult = theValue.CompareTo(tempRoot.TheValue);
                if (compareResult == 0)
                {
                    return tempRoot;
                }
                if (compareResult > 0)
                {
                    tempRoot = tempRoot.RightChild;
                }
                else
                {
                    tempRoot = tempRoot.LeftChild;
                }
            }

            return null;

        }
        #endregion			

        #region GetMaxNode ,GetMinNode
        public Node<TVal> GetMaxNode(Node<TVal> childTree)
        {
            Node<TVal> temp = childTree;
            if (childTree.RightChild != null)
            {
                temp = childTree.RightChild;
            }

            return temp;
        }

        public Node<TVal> GetMinNode(Node<TVal> childTree)
        {
            Node<TVal> temp = childTree;
            if (childTree.LeftChild != null)
            {
                temp = childTree.LeftChild;
            }

            return temp;
        }
		#endregion

        #endregion

    }   
}


