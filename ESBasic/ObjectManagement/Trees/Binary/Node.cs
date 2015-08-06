using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Trees.Binary
{
    [Serializable]
    public class Node<TVal> where TVal : IComparable
    {
        #region Ctor
        public Node() { }
        public Node(TVal _theValue ,Node<TVal> _parent)
        {
            this.theValue = _theValue;
            this.parent = _parent;           
        } 
        #endregion      

        #region BalanceFactor
        private int balanceFactor = 0;
        public int BalanceFactor
        {
            get { return balanceFactor; }
            set { balanceFactor = value; }
        } 
        #endregion

        #region TheValue
        private TVal theValue;
        public TVal TheValue
        {
            get { return theValue; }
            set { theValue = value; }
        }
        #endregion

        #region Parent
        private Node<TVal> parent;
        public Node<TVal> Parent
        {
            get { return parent; }
            set { parent = value; }
        } 
        #endregion

        #region LeftChild
        private Node<TVal> leftChild;
        public Node<TVal> LeftChild
        {
            get { return leftChild; }
            set { leftChild = value; }
        }
        #endregion

        #region RightChild
        private Node<TVal> rightChild;
        public Node<TVal> RightChild
        {
            get { return rightChild; }
            set { rightChild = value; }
        }
        #endregion

        #region IsLeaf
        public bool IsLeaf
        {
            get
            {
                return (this.leftChild == null) && (this.rightChild == null);
            }
        } 
        #endregion

        public override string ToString()
        {
            return this.theValue.ToString();
        }
    }
}
