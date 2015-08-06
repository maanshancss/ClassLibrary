using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Trees.Binary;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement.Trees.Binary
{
    public class CompleteBinaryTree<TVal> : IBinaryTree<TVal> where TVal : System.IComparable
    {
        protected List<Node<TVal>> allNode = new List<Node<TVal>>();

        #region SmartRWLocker
        [NonSerialized]
        protected SmartRWLocker _smartRWLocker = null;
        /// <summary>
        /// SmartRWLocker 为支持反序列化后_smartRWLocker不为null而设计。
        /// </summary>
        protected SmartRWLocker SmartRWLocker
        {
            get
            {
                if (this._smartRWLocker == null)
                {
                    this._smartRWLocker = new SmartRWLocker();
                }

                return this._smartRWLocker;
            }
        }
        #endregion

        #region Property
        #region Count
        protected int count = 0;
        public int Count
        {
            get
            {
                return this.count;
            }
        }
        #endregion

        #region Root
        protected Node<TVal> root;
        public Node<TVal> Root
        {
            get { return this.root; }
        }
        #endregion

        #region Depth
        public int Depth
        {
            get
            {
                return (int)Math.Log(this.count, 2) + 1;
            }
        }
        #endregion 
        #endregion

        #region Method
        #region Insert
        public virtual void Insert(TVal val)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                Node<TVal> newNode = new Node<TVal>(val, null);

                if (this.root == null)
                {
                    this.root = newNode;
                }
                else
                {
                    int fatherIndx = this.GetFatherIndx(this.allNode.Count);
                    if (this.allNode.Count % 2 == 1)
                    {
                        this.allNode[fatherIndx].LeftChild = newNode;
                        newNode.Parent = this.allNode[fatherIndx];
                    }
                    else
                    {
                        this.allNode[fatherIndx].RightChild = newNode;
                        newNode.Parent = this.allNode[fatherIndx];
                    }
                }
                this.count++;
                this.allNode.Add(newNode);
            }
        }
        #endregion

        #region Remove
        /// <summary>
        /// 把最后一个结点值赋给要删除的结点，然后删除最后一个结点
        /// </summary>
        /// <param name="val"></param>
        public virtual void Remove(TVal val)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                int index = this.GetIndxOfNode(val);
                this.Remove(index);
            }
        }
        public virtual void Remove(int nodeIndex)
        {
            if (nodeIndex < 0) return;
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                this.SwapValueOfTwoNode(this.allNode[nodeIndex], this.allNode[this.allNode.Count - 1]);
                this.RemoveLastNode();
            }
        }
        #endregion

        #region Get
        public Node<TVal> Get(TVal val)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))
            {
                foreach (Node<TVal> node in this.allNode)
                {
                    if (node.TheValue.CompareTo(val) == 0)
                    {
                        return node;
                    }
                }
                return null;
            }
        }
        #endregion

        #region GetFatherIndx
        /// <summary>
        /// 获取某结点的父结点在list中的index
        /// </summary>        
        public int GetFatherIndx(int nodeIndx)
        {
            if (nodeIndx == 0) return -1;
            if (nodeIndx % 2 == 1)
            {
                return (nodeIndx - 1) / 2;
            }
            else
            {
                return (nodeIndx - 2) / 2;
            }
        }
        #endregion

        #region IsInRightTree
        /// <summary>
        /// 判断某结点是否在右子树中
        /// </summary>
        /// <param name="nodeIndx">判断的结点index，必须大于0</param>        
        public bool IsInRightTree(int nodeIndex)
        {
            int layerCount = (int)Math.Log(nodeIndex + 1, 2) + 1;
            if (nodeIndex >= Math.Pow(2, layerCount - 1) - 1 + Math.Pow(2, layerCount - 2))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region GetOppositeNodeIndx
        /// <summary>
        /// 获取某结点的对称结点的索引
        /// </summary>

        public int GetOppositeNodeIndx(int nodeIndx, bool isInRightTree)
        {
            int layerCount = (int)Math.Log(nodeIndx + 1, 2) + 1;
            if (isInRightTree)
            {
                return nodeIndx - (int)Math.Pow(2, layerCount - 2);
            }
            else
            {
                int rightNodeIndx = nodeIndx + (int)Math.Pow(2, layerCount - 2);
                if (rightNodeIndx > this.allNode.Count - 1)
                {
                    return this.GetFatherIndx(rightNodeIndx);
                }
                return rightNodeIndx;
            }
        }
        #endregion

        #region SwapValueOfTwoNode
        public void SwapValueOfTwoNode(Node<TVal> node1, Node<TVal> node2)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                TVal temp = node1.TheValue;
                node1.TheValue = node2.TheValue;
                node2.TheValue = temp;
            }
        }
        #endregion

        #region GetLayerOfNode
        /// <summary>
        /// 根据索引得到该结点所在的层，根结点为第一层。
        /// </summary>

        public int GetLayerOfNode(int nodeIndex)
        {
            return (int)Math.Log(nodeIndex + 1, 2) + 1;
        }
        #endregion

        #region GetIndxOfNode
        /// <summary>
        /// 根据结点的值，获取该结点的索引，如果没有找到，返回-1
        /// </summary>        
        public int GetIndxOfNode(TVal val)
        {
            for (int i = 0; i < this.allNode.Count; i++)
            {
                if (this.allNode[i].TheValue.CompareTo(val) == 0)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        #region RemoveLastNode
        /// <summary>
        /// 删除最后一个结点
        /// </summary>
        private void RemoveLastNode()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                if (this.count == 0)
                {
                    return;
                }
                else if (this.count == 1)
                {
                    this.root = null;
                }
                else
                {
                    int fatherIndx = this.GetFatherIndx(this.allNode.Count - 1);
                    if (this.allNode.Count % 2 == 1)
                    {
                        this.allNode[fatherIndx].RightChild = null;
                    }
                    else
                    {
                        this.allNode[fatherIndx].LeftChild = null;
                    }
                }
                this.count--;
                this.allNode.RemoveAt(this.allNode.Count - 1);
            }
        }
        #endregion 
        #endregion
    }
}
