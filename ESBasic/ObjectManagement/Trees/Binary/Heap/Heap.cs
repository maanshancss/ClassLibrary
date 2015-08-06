using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Threading.Synchronize;

namespace ESBasic.ObjectManagement.Trees.Binary
{
    /// <summary>
    /// Heap IHeap接口的参考实现，默认为最大堆。该实现是线程安全的。
    /// </summary>
    [Serializable]
    public class Heap<TVal> : IHeap<TVal> where TVal : System.IComparable
    {        
        private List<Node<TVal>> allNode = new List<Node<TVal>>();

        #region SmartRWLocker
        [NonSerialized]
        private SmartRWLocker _smartRWLocker = null;
        /// <summary>
        /// SmartRWLocker 为支持反序列化后_smartRWLocker不为null而设计。
        /// </summary>
        private SmartRWLocker SmartRWLocker
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

        #region HeapType
        private HeapType heapType = HeapType.Max;
        public HeapType HeapType
        {
            get { return heapType; }
        } 
        #endregion
        #region Count
        private int count = 0;
        public int Count
        {
            get
            {
                return this.count;
            }
        } 
        #endregion
        #region Root
        private Node<TVal> root;
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
                return decimal.ToInt32(decimal.Ceiling((decimal)Math.Log(this.count + 1, 2)));
            }
        } 
        #endregion

        public Heap() { }
        public Heap(HeapType type)
        {
            this.heapType = type;
        }

        #region Insert
        public void Insert(TVal val)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                if (this.Contains(val)) return;
                Node<TVal> newNode = new Node<TVal>(val, null);

                if (this.root == null)
                {
                    this.root = newNode;
                }
                else
                {
                    int indx = 0;
                    if (this.allNode.Count % 2 == 1)
                    {
                        indx = (this.allNode.Count - 1) / 2;
                        this.allNode[indx].LeftChild = newNode;
                        newNode.Parent = this.allNode[indx];
                    }
                    else
                    {
                        indx = (this.allNode.Count - 2) / 2;
                        this.allNode[indx].RightChild = newNode;
                        newNode.Parent = this.allNode[indx];
                    }
                    this.SwapFromLeafToRoot(newNode);
                }
                this.count++;
                this.allNode.Add(newNode);
            }
        }

       

        #endregion

        public TVal Pop()
        {
            if (this.root == null)
            {
                throw new Exception("The Root Is Null");
            }
            TVal rootValue = this.root.TheValue;
            this.Remove(rootValue);
            return rootValue;
        }      

        #region Contains
        public bool Contains(TVal val)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Read))            
            {
                return this.Contain(val, this.root);
            }
        } 
        #endregion

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

        public void Remove(TVal val)
        {
            using (this.SmartRWLocker.Lock(AccessMode.Write))
            {
                Node<TVal> node = this.Get(val);
                if (node == null) return;
                if (this.allNode.Count == 1)
                {
                    this.root = null;
                    return;
                }
                Node<TVal> theLastNode = this.allNode[this.allNode.Count - 1];
                if (node.TheValue.CompareTo(theLastNode.TheValue) != 0)
                {
                    node.TheValue = theLastNode.TheValue;
                }

                if (this.allNode.Count % 2 == 0)
                {
                    this.allNode[(this.allNode.Count - 2) / 2].LeftChild = null;
                }
                else
                {
                    this.allNode[(this.allNode.Count - 3) / 2].RightChild = null;
                }
                this.allNode.RemoveAt(this.allNode.Count - 1);
                this.count--;
                this.SwapFromRootToLeaf(node);
            }

        }

        #region private
        
        #region SwapFromLeafToRoot
        private void SwapFromLeafToRoot(Node<TVal> node)
        {
            Node<TVal> currentNode = node;
            if (this.heapType == HeapType.Max)
            {
                while (currentNode.Parent != null && currentNode.Parent.TheValue.CompareTo(currentNode.TheValue) < 0)
                {
                    this.SwapValueOfTwoNode(currentNode, currentNode.Parent);
                    currentNode = currentNode.Parent;
                }
            }
            else
            {
                while (currentNode.Parent != null && currentNode.Parent.TheValue.CompareTo(currentNode.TheValue) > 0)
                {
                    this.SwapValueOfTwoNode(currentNode, currentNode.Parent);
                    currentNode = currentNode.Parent;
                }
            }
        } 
        #endregion
        #region SwapValueOfTwoNode
        private void SwapValueOfTwoNode(Node<TVal> node1, Node<TVal> node2)
        {
            TVal temp = node1.TheValue;
            node1.TheValue = node2.TheValue;
            node2.TheValue = temp;
        } 
        #endregion
        #region Contain
        private bool Contain(TVal val, Node<TVal> node)
        {
            if (node == null) return false;           
            if (node.TheValue.CompareTo(val) == 0)
            {
                return true;
            }
            if ((this.heapType == HeapType.Max && node.TheValue.CompareTo(val) < 0) || (this.heapType == HeapType.Min && node.TheValue.CompareTo(val) > 0))
            {
                return false;
            }
            return this.Contain(val, node.LeftChild) || this.Contain(val, node.RightChild);
            
        } 
        #endregion
        #region SwapFromRootToLeaf
        private void SwapFromRootToLeaf(Node<TVal> root)
        {
            Node<TVal> currentNode = root;
            while (!(currentNode.LeftChild == null && currentNode.RightChild == null))
            {
                if (currentNode.RightChild == null)
                {
                    if ((this.heapType == HeapType.Max && currentNode.TheValue.CompareTo(currentNode.LeftChild.TheValue) < 0) ||
                        (this.heapType == HeapType.Min && currentNode.TheValue.CompareTo(currentNode.LeftChild.TheValue) > 0))
                    {
                        this.SwapValueOfTwoNode(currentNode, currentNode.LeftChild);
                        currentNode = currentNode.LeftChild;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if ((this.heapType == HeapType.Max && currentNode.LeftChild.TheValue.CompareTo(currentNode.RightChild.TheValue) < 0 ) ||
                        (this.heapType == HeapType.Min && currentNode.LeftChild.TheValue.CompareTo(currentNode.RightChild.TheValue) > 0))
                    {
                        if ((this.heapType == HeapType.Max && currentNode.TheValue.CompareTo(currentNode.RightChild.TheValue) < 0) ||
                            (this.heapType == HeapType.Min && currentNode.TheValue.CompareTo(currentNode.RightChild.TheValue) > 0))
                        {
                            this.SwapValueOfTwoNode(currentNode, currentNode.RightChild);
                            currentNode = currentNode.RightChild;
                        }
                        else
                        {
                            break;
                        }
                        
                    }
                    else
                    {
                        if ((this.heapType == HeapType.Max && currentNode.TheValue.CompareTo(currentNode.LeftChild.TheValue) < 0) ||
                            (this.heapType == HeapType.Min && currentNode.TheValue.CompareTo(currentNode.LeftChild.TheValue) > 0))
                        {
                            this.SwapValueOfTwoNode(currentNode, currentNode.LeftChild);
                            currentNode = currentNode.LeftChild;
                        }
                        else
                        {
                            break;
                        }
                    }

                }
            }

        } 
        #endregion
        #region GetTheLastNode
        private Node<TVal> GetTheLastNode(Node<TVal> rootNode)
        {
            if (rootNode == null) return null;

            Node<TVal> lastNode = null;
            Queue<Node<TVal>> queue = new Queue<Node<TVal>>();
            queue.Enqueue(rootNode);
            while (queue.Count > 0)
            {
                lastNode = queue.Dequeue();
                if (lastNode.LeftChild != null)
                {
                    queue.Enqueue(lastNode.LeftChild);
                }
                if (lastNode.RightChild != null)
                {
                    queue.Enqueue(lastNode.RightChild);
                }
            }
            return lastNode;
        } 
        #endregion
        #endregion
        
    }   
}
