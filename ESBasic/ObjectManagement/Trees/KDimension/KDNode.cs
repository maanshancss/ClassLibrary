using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Trees.KDimension
{
    public class KDNode<T> where T : IKDTreeVal, IEquatable<T>
    {
        #region Ctor
        public KDNode(T val, int _depth)
        {
            this.theValue = val;
            this.depth = _depth;
        } 
        #endregion

        #region TheValue
        private T theValue;
        public T TheValue
        {
            get { return theValue; }
            set { theValue = value; }
        }
        #endregion

        #region Left
        private KDNode<T> left = null;
        internal KDNode<T> Left
        {
            get { return left; }
            set { left = value; }
        }
        #endregion

        #region Right
        private KDNode<T> right = null;
        internal KDNode<T> Right
        {
            get { return right; }
            set { right = value; }
        }
        #endregion

        #region Depth
        private int depth;
        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }
        #endregion       

        #region ToString
        public override string ToString()
        {
            return this.theValue.ToString();
        } 
        #endregion
    }

    public interface IKDTreeVal
    {
        IComparable this[int index] { get; }
    }   
}
