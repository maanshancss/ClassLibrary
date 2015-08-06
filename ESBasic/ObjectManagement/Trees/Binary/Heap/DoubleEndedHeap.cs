using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.ObjectManagement.Trees.Binary;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement.Trees.Binary
{
    /// <summary>
    /// DoulbeEndedHeap 双端堆，跟结点为空，左子树为最小堆，右子树为最大堆
    /// </summary>
    /// <typeparam name="TVal"></typeparam>
    [Serializable]
    public class DoulbeEndedHeap<TVal> : CompleteBinaryTree<TVal> where TVal : System.IComparable
    { 

        #region Insert
        /// <summary>
        /// 把新的结点插入到完全二叉树的最后一个结点，如果该结点在最大堆，判断最小堆所对应的结点值是否比该结点的值大，如果是，则交换数据，重新调整左右子树。
        /// 如果不是，则调整右子树。
        /// 如果该结点在左子树，判断最大堆对应的结点值是否比该值小，如果是，则交换数据，重新调整左右子树。如果不是，则调整左子树。
        /// </summary>        
        public override void Insert(TVal val)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                if (this.root == null)
                {
                    base.Insert(default(TVal));
                    base.Insert(val);
                }
                else
                {
                    base.Insert(val);
                    this.VerifyDoubleEndedHeap(this.allNode.Count - 1);
                }                
            }
        }
        #endregion
        #region RemoveMin
        /// <summary>
        /// 删除最小值，最小值为最小堆的根结点        
        /// </summary>        
        public void RemoveMin()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                base.Remove(1);
                if (this.count > 1)
                {
                    int lastIndx = this.SwapFromRootToLeaf(this.allNode[1], true);
                    this.VerifyDoubleEndedHeap(lastIndx);
                }
                else
                {
                    base.Remove(0);
                }
            }
        } 
        #endregion
        #region RemoveMax
        /// <summary>
        /// 删除最大值，最大值为最大堆的根结点
        /// </summary>        
        public void RemoveMax()
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                if (this.allNode.Count > 2)
                {
                    base.Remove(2);
                    if (this.allNode.Count > 2)
                    {
                        int lastIndx = this.SwapFromRootToLeaf(this.allNode[2], false);
                        this.VerifyDoubleEndedHeap(lastIndx);
                    }
                }
                else if (this.allNode.Count == 2)
                {
                    this.RemoveMin();
                }
            }
        } 
        #endregion


        #region private
        #region VerifyDoubleEndedHeap
        private void VerifyDoubleEndedHeap(int nodeIndx)
        {
            bool isInRightTree = this.IsInRightTree(nodeIndx);
            int oppositeNodeIndx = this.GetOppositeNodeIndx(nodeIndx, isInRightTree);
            if (oppositeNodeIndx > 0)
            {
                if (isInRightTree)
                {
                    if (this.allNode[nodeIndx].TheValue.CompareTo(this.allNode[oppositeNodeIndx].TheValue) < 0)
                    {
                        this.SwapValueOfTwoNode(this.allNode[nodeIndx], this.allNode[oppositeNodeIndx]);
                        //调整最小堆
                        this.SwapFromLeafToRoot(this.allNode[oppositeNodeIndx], true);
                    }
                    else
                    {
                        if (this.allNode[nodeIndx].LeftChild == null && this.allNode[oppositeNodeIndx].LeftChild != null)
                        {
                            oppositeNodeIndx = oppositeNodeIndx * 2 + 1;
                            if (oppositeNodeIndx + 1 < this.allNode.Count && this.allNode[oppositeNodeIndx].TheValue.CompareTo(this.allNode[oppositeNodeIndx + 1].TheValue) < 0)
                            {
                                oppositeNodeIndx++;
                            }
                            if (this.allNode[oppositeNodeIndx].TheValue.CompareTo(this.allNode[nodeIndx].TheValue) > 0)
                            {
                                this.SwapValueOfTwoNode(this.allNode[nodeIndx], this.allNode[oppositeNodeIndx]);
                                //调整最小堆
                                this.SwapFromLeafToRoot(this.allNode[oppositeNodeIndx], true);
                            }
                        }
                    }
                    //调整最大堆
                    this.SwapFromLeafToRoot(this.allNode[nodeIndx], false);

                }
                else
                {
                    if (this.allNode[nodeIndx].TheValue.CompareTo(this.allNode[oppositeNodeIndx].TheValue) > 0)
                    {
                        this.SwapValueOfTwoNode(this.allNode[nodeIndx], this.allNode[oppositeNodeIndx]);
                        //调整最大堆
                        this.SwapFromLeafToRoot(this.allNode[oppositeNodeIndx], false);
                    }
                    //调整最小堆
                    this.SwapFromLeafToRoot(this.allNode[nodeIndx], true);
                }
            }
        }
        #endregion
        #region SwapFromRootToLeaf
        private int SwapFromRootToLeaf(Node<TVal> rootNode, bool isMinHeap)
        {
            TVal rootVal = rootNode.TheValue;
            Node<TVal> currentNode = rootNode;
            if (isMinHeap)
            {
                while (!(currentNode.LeftChild == null && currentNode.RightChild == null))
                {
                    Node<TVal> minNode = currentNode.LeftChild;
                    if (currentNode.RightChild != null && currentNode.RightChild.TheValue.CompareTo(currentNode.LeftChild.TheValue) < 0)
                    {
                        minNode = currentNode.RightChild;
                    }
                    if (rootVal.CompareTo(minNode.TheValue) < 0)
                    {
                        break;
                    }
                    else
                    {
                        currentNode.TheValue = minNode.TheValue;
                        currentNode = minNode;
                    }
                }
            }
            else
            {
                while (!(currentNode.LeftChild == null && currentNode.RightChild == null))
                {
                    Node<TVal> maxNode = currentNode.LeftChild;
                    if (currentNode.RightChild != null && currentNode.RightChild.TheValue.CompareTo(currentNode.LeftChild.TheValue) > 0)
                    {
                        maxNode = currentNode.RightChild;
                    }
                    if (rootVal.CompareTo(maxNode.TheValue) > 0)
                    {
                        break;
                    }
                    else
                    {
                        currentNode.TheValue = maxNode.TheValue;
                        currentNode = maxNode;
                    }
                }
            }
            currentNode.TheValue = rootVal;
            return this.GetIndxOfNode(currentNode.TheValue);

        }
        #endregion
        #region SwapFromLeafToRoot
        private void SwapFromLeafToRoot(Node<TVal> node, bool isMinHeap)
        {
            Node<TVal> currentNode = node;
            if (!isMinHeap)
            {
                while (currentNode.Parent != this.root && currentNode.Parent.TheValue.CompareTo(currentNode.TheValue) < 0)
                {
                    this.SwapValueOfTwoNode(currentNode, currentNode.Parent);
                    currentNode = currentNode.Parent;
                }
            }
            else
            {
                while (currentNode.Parent != this.root && currentNode.Parent.TheValue.CompareTo(currentNode.TheValue) > 0)
                {
                    this.SwapValueOfTwoNode(currentNode, currentNode.Parent);
                    currentNode = currentNode.Parent;
                }
            }
        }
        #endregion 
        #endregion

    }
}
