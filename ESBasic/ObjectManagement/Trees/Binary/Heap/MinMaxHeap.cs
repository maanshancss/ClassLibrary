using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Trees.Binary;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement.Trees.Binary
{
    /// <summary>
    /// MinMaxHeap 最小最大堆，由最小层、最大层、最小层、最大层交替出现，根结点（第一层）为最小层，根结点也是最小结点，所有最小层，小的值在上层。
    /// 第二层为最大层，其中较大的一个子结点为最大值。所有最大层也是大的值在上层。
    /// </summary>
    [Serializable]
    public class MinMaxHeap<TVal> : CompleteBinaryTree<TVal> where TVal : System.IComparable
    { 
        #region Insert
        /// <summary>
        /// 把新的结点插入到完全二叉树的最后一个结点，然后保证满足最小最大堆的条件，调整堆。
        /// </summary>
        /// <param name="val"></param>
        public override void Insert(TVal val)
        { 
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                base.Insert(val);
                if (this.count > 1)
                {
                    this.AdjustHeapFromDownToUp(this.allNode[this.allNode.Count - 1]);
                }
            }
        }
        #endregion

        #region RemoveMin
        /// <summary>
        /// 把最后一个结点的值赋给跟结点，然后调整堆，保证堆是最小最大堆
        /// </summary>        
        public void RemoveMin()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                if (this.root == null) return;
                base.Remove(0);
                if (this.count > 1)
                {
                    this.VerifyRootSmallThanChildren(this.root);
                    //保证所有最小层，小的值在上层
                    this.VerifyMinLayerRightFromUpToDown(this.root);
                }
            }

        } 
        #endregion

        #region RemoveMax
        /// <summary>
        /// 把最后一个结点的值赋给跟结点中较大的子结点，然后删除最后的一个结点，保证堆是最大最小堆
        /// </summary>        
        public void RemoveMax()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                if (this.root == null) return;
                if (this.allNode.Count == 1)
                {
                    base.Remove(0);
                }
                else if (this.allNode.Count == 2)
                {
                    base.Remove(1);
                }
                else
                {
                    Node<TVal> maxNode;
                    if (this.root.LeftChild.TheValue.CompareTo(this.root.RightChild.TheValue) >= 0)
                    {
                        base.Remove(1);
                        maxNode = this.root.LeftChild;
                    }
                    else
                    {
                        base.Remove(2);
                        maxNode = this.root.RightChild;
                    }

                    this.VerifyRootBiggerThanChildren(maxNode);
                    //保证所有最大层，大的值在上层。
                    this.VerifyMaxLayerRightFromUpToDown(maxNode);
                }
            }
        } 
        #endregion

        #region private
        #region AdjustHeapFromDownToUp
        private void AdjustHeapFromDownToUp(Node<TVal> newNode)
        {
            bool isMaxLayer = this.GetLayerOfNode(this.count-1) % 2 == 0 ? true : false;
            if (isMaxLayer)
            {
                if (newNode.TheValue.CompareTo(newNode.Parent.TheValue) < 0)
                {
                    //与父结点交换数据
                    this.SwapValueOfTwoNode(newNode, newNode.Parent);
                    //判断新的父结点和上级所有最小层的值，保证小的值在上层
                    this.VerifyMinLayerRightFromDownToUp(newNode.Parent);
                }
                else
                {
                    //判断上级所有最大层，保证大的值在上层
                    this.VerifyMaxLayerRightFromDownToUp(newNode);
                }
            }
            else
            {
                if (newNode.TheValue.CompareTo(newNode.Parent.TheValue) > 0)
                {
                    //与父结点交换数据
                    this.SwapValueOfTwoNode(newNode, newNode.Parent);
                    //判断新的父结点和上级所有最大层的值，保证大的值在上层
                    this.VerifyMaxLayerRightFromDownToUp(newNode.Parent);
                }
                else
                {
                    //判断上级所有最小层，保证小的值在上层
                    this.VerifyMinLayerRightFromDownToUp(newNode);
                }

            }
        }
        #endregion

        #region VerifyMinLayerRightFromDownToUp
        private void VerifyMinLayerRightFromDownToUp(Node<TVal> node)
        {
            while (node.Parent != null && node.Parent.Parent != null && node.Parent.Parent.TheValue.CompareTo(node.TheValue) > 0)
            {
                this.SwapValueOfTwoNode(node, node.Parent.Parent);
                node = node.Parent.Parent;
            }
        }
        #endregion

        #region VerifyMaxLayerRightFromDownToUp
        private void VerifyMaxLayerRightFromDownToUp(Node<TVal> node)
        {
            while (node.Parent != null && node.Parent.Parent != null && node.Parent.Parent.TheValue.CompareTo(node.TheValue) < 0)
            {
                this.SwapValueOfTwoNode(node, node.Parent.Parent);
                node = node.Parent.Parent;
            }
        }
        #endregion

        #region VerifyMinLayerRightFromUpToDown
        private void VerifyMinLayerRightFromUpToDown(Node<TVal> node)
        {
            while (node.LeftChild != null && node.LeftChild.LeftChild != null)
            {
                Node<TVal> minNode = node;

                if (minNode.TheValue.CompareTo(node.LeftChild.LeftChild.TheValue) > 0)
                {
                    minNode = node.LeftChild.LeftChild;
                }
                if (node.LeftChild.RightChild != null && minNode.TheValue.CompareTo(node.LeftChild.RightChild.TheValue) > 0)
                {
                    minNode = node.LeftChild.RightChild;
                }

                if (node.RightChild != null)
                {
                    if (node.RightChild.LeftChild != null)
                    {
                        if (minNode.TheValue.CompareTo(node.RightChild.LeftChild.TheValue) > 0)
                        {
                            minNode = node.RightChild.LeftChild;
                        }
                        if (node.RightChild.RightChild != null && minNode.TheValue.CompareTo(node.RightChild.RightChild.TheValue) > 0)
                        {
                            minNode = node.RightChild.RightChild;
                        }
                    }
                }
                if (minNode.TheValue.CompareTo(node.TheValue) != 0)
                {
                    this.SwapValueOfTwoNode(node, minNode);
                    node = minNode;
                }
                else
                {
                    break;
                }

            }

        }
        #endregion

        #region VerifyMaxLayerRightFromUpToDown
        private void VerifyMaxLayerRightFromUpToDown(Node<TVal> node)
        {
            while (node.LeftChild != null && node.LeftChild.LeftChild != null)
            {
                Node<TVal> maxNode = node;

                if (maxNode.TheValue.CompareTo(node.LeftChild.LeftChild.TheValue) < 0)
                {
                    maxNode = node.LeftChild.LeftChild;
                }
                if (node.LeftChild.RightChild != null && maxNode.TheValue.CompareTo(node.LeftChild.RightChild.TheValue) < 0)
                {
                    maxNode = node.LeftChild.RightChild;
                }

                if (node.RightChild != null)
                {
                    if (node.RightChild.LeftChild != null)
                    {
                        if (maxNode.TheValue.CompareTo(node.RightChild.LeftChild.TheValue) < 0)
                        {
                            maxNode = node.RightChild.LeftChild;
                        }
                        if (node.RightChild.RightChild != null && maxNode.TheValue.CompareTo(node.RightChild.RightChild.TheValue) < 0)
                        {
                            maxNode = node.RightChild.RightChild;
                        }
                    }
                }
                if (maxNode.TheValue.CompareTo(node.TheValue) != 0)
                {
                    this.SwapValueOfTwoNode(node, maxNode);
                    node = maxNode;
                }
                else
                {
                    break;
                }
            }
        }
        #endregion 

        #region VerifyRootSmallThanChildren
        /// <summary>
        /// 确保根结点比子结点小
        /// </summary>
        private void VerifyRootSmallThanChildren(Node<TVal> rootNode)
        {
            Node<TVal> minChild = rootNode;
            if (rootNode.LeftChild != null)
            {
                if (rootNode.LeftChild.TheValue.CompareTo(minChild.TheValue) < 0)
                {
                    minChild = rootNode.LeftChild;
                }
                if (rootNode.RightChild != null && rootNode.RightChild.TheValue.CompareTo(minChild.TheValue) < 0)
                {
                    minChild = rootNode.RightChild;
                }
            }
            if (minChild.TheValue.CompareTo(rootNode.TheValue) != 0)
            {
                this.SwapValueOfTwoNode(minChild, rootNode);
            }
        }
        #endregion

        #region VerifyRootBiggerThanChildren
        /// <summary>
        ///确保根结点比子结点大
        /// </summary>
        private void VerifyRootBiggerThanChildren(Node<TVal> rootNode)
        {
            Node<TVal> maxChild = rootNode;
            if (rootNode.LeftChild != null)
            {
                if (rootNode.LeftChild.TheValue.CompareTo(maxChild.TheValue) > 0)
                {
                    maxChild = rootNode.LeftChild;
                }
                if (rootNode.RightChild != null && rootNode.RightChild.TheValue.CompareTo(maxChild.TheValue) > 0)
                {
                    maxChild = rootNode.RightChild;
                }
            }
            if (maxChild.TheValue.CompareTo(rootNode.TheValue) != 0)
            {
                this.SwapValueOfTwoNode(rootNode, maxChild);
            }
        }
        #endregion
        #endregion       
    }
}

