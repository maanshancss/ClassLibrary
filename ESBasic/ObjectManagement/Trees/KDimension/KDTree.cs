using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ESBasic.ObjectManagement.Trees.Binary;
using ESBasic.Arithmetic;

namespace ESBasic.ObjectManagement.Trees.KDimension
{
    /// <summary>
    /// KDTree K维二叉树。
    /// dudu 2009.10.13
    /// </summary>    
    public class KDTree<T> where T : IKDTreeVal, IEquatable<T>
    {
        private KDNode<T> root = null;
        private int dimension = 2;

        #region Ctor
        public KDTree(int _dimension)
        {
            this.dimension = _dimension;
        } 
        #endregion

        #region Insert
        public KDNode<T> Insert(T val)
        {
            if (this.root == null)
            {
                this.root = new KDNode<T>(val, 0);
                return this.root;
            }
            else
            {
                if (this.root.TheValue.Equals(val))
                {
                    return this.root;
                }
                if (this.root.TheValue[0].CompareTo(val[0]) >= 0)
                {
                    return this.Insert(val, this.root.Left, this.root, true);
                }
                else
                {
                    return this.Insert(val, this.root.Right, this.root, false);
                }
            }
        }
        #region private Insert
        private KDNode<T> Insert(T val, KDNode<T> insertNode, KDNode<T> parentNode, bool isLeft)
        {
            if (insertNode == null)
            {
                if (isLeft)
                {
                    parentNode.Left = new KDNode<T>(val, parentNode.Depth + 1);
                    return parentNode.Left;
                }
                else
                {
                    parentNode.Right = new KDNode<T>(val, parentNode.Depth + 1);
                    return parentNode.Right;
                }
            }
            else
            {
                if (insertNode.TheValue.Equals(val))
                {
                    return insertNode;
                }
                int index = insertNode.Depth % this.dimension;
                if (insertNode.TheValue[index].CompareTo(val[index]) >= 0)
                {
                    return this.Insert(val, insertNode.Left, insertNode, true);
                }
                else
                {
                    return this.Insert(val, insertNode.Right, insertNode, false);
                }
            }
        }
        #endregion
        #endregion

        #region Get
        public KDNode<T> Get(T val)
        {
            return this.search(val, this.root);
        }       

        #region  private search
        private KDNode<T> search(T val, KDNode<T> node)
        {
            if (node == null) return null;

            if (node.TheValue.Equals(val))
            {
                return node;
            }
            else
            {
                int index = node.Depth % this.dimension;
                if (node.TheValue[index].CompareTo(val[index]) <= 0)
                {
                    return this.search(val, node.Right);
                }
                else
                {
                    return this.search(val, node.Left);
                }
            }
        }
        #endregion
        #endregion

        #region GetBetween
        public List<T> GetBetween(T min, bool[] minClosedAry, T max, bool[] maxClosedAry)
        {
            List<T> result = new List<T>();
            this.SearchBetween(min, max, minClosedAry, maxClosedAry, this.root, ref result);
            return result;
        }
        

        #region SearchBetween
        private void SearchBetween(T min, T max, bool[] minClosedAry, bool[] maxClosedAry, KDNode<T> node, ref List<T> result)
        {
            if (node == null)
            {
                return;
            }
            
            if (this.IsInRange(node.TheValue, min, max, minClosedAry, maxClosedAry))
            {
                result.Add(node.TheValue);
            }
            int index = node.Depth % this.dimension;
            if (node.TheValue[index].CompareTo(min[index]) < 0)
            {
                this.SearchBetween(min, max, minClosedAry, maxClosedAry, node.Right, ref result);
            }
            else if (node.TheValue[index].CompareTo(min[index]) >= 0 && node.TheValue[index].CompareTo(max[index]) < 0)
            {
                this.SearchBetween(min, max, minClosedAry, maxClosedAry, node.Left, ref result);
                this.SearchBetween(min, max, minClosedAry, maxClosedAry, node.Right, ref result);
            }
            else
            {
                this.SearchBetween(min, max, minClosedAry, maxClosedAry, node.Left, ref result);
            }
        }
        #endregion

        #region IsInRange
        private bool IsInRange(T val, T min, T max, bool[] minAry, bool[] maxAry)
        {
            for (int i = 0; i < this.dimension; i++)
            {
                if ((minAry[i] && val[i].CompareTo(min[i]) < 0) || (!minAry[i] && val[i].CompareTo(min[i]) <= 0)
                    || (maxAry[i] && val[i].CompareTo(max[i]) > 0) || (!maxAry[i] && val[i].CompareTo(max[i]) >= 0))
                {
                    return false;
                }
            }
            return true;
        } 
        #endregion

        #endregion

        #region GetBetween重载 2010.3.9
        public List<T> GetBetween(KDSearchScope[] searchAry)
        {
            List<T> result = new List<T>();
            if (searchAry.Length != this.dimension)
            {
                return result;
            }

            this.SearchBetween(searchAry, this.root, ref result);
            return result;
        } 
        #endregion

        #region SearchBetween
        private void SearchBetween(KDSearchScope[] searchAry, KDNode<T> node, ref List<T> result)
        {
            if (node == null)
            {
                return;
            }
            if (this.IsInRange(node.TheValue, searchAry))
            {
                result.Add(node.TheValue);
            }
            int index = node.Depth % this.dimension;
            if (searchAry[index].KDSearchType == KDSearchType.Default)
            {
                if (node.TheValue[index].CompareTo(searchAry[index].MinValue) < 0)
                {
                    this.SearchBetween(searchAry, node.Right, ref result);
                }
                else if (node.TheValue[index].CompareTo(searchAry[index].MinValue) >= 0 && node.TheValue[index].CompareTo(searchAry[index].MaxValue) < 0)
                {
                    this.SearchBetween(searchAry, node.Left, ref result);
                    this.SearchBetween(searchAry, node.Right, ref result);
                }
                else
                {
                    this.SearchBetween(searchAry, node.Left, ref result);
                }
            }
            else
            {
                this.SearchBetween(searchAry, node.Left, ref result);
                this.SearchBetween(searchAry, node.Right, ref result);
            }

        } 
        #endregion

        #region IsInRange
        private bool IsInRange(T val, KDSearchScope[] searchAry)
        {
            for (int i = 0; i < this.dimension; i++)
            {
                if (searchAry[i].KDSearchType == KDSearchType.Like)
                {
                    return FuzzyMatchHelper.IsLike(searchAry[i].MatchString, val[i].ToString());                    
                }
                else if (searchAry[i].KDSearchType == KDSearchType.Like)
                {
                    return !FuzzyMatchHelper.IsLike(searchAry[i].MatchString, val[i].ToString());
                }
                else
                {
                    if ((searchAry[i].MinClosed && val[i].CompareTo(searchAry[i].MinValue) < 0) 
                        || (!searchAry[i].MinClosed && val[i].CompareTo(searchAry[i].MinValue) <= 0)
                        || (searchAry[i].MaxClosed && val[i].CompareTo(searchAry[i].MaxValue) > 0)
                        || (!searchAry[i].MaxClosed && val[i].CompareTo(searchAry[i].MaxValue) >= 0))
                    {
                        return false;
                    }
                }                
            }
            return true;
        }
        #endregion

        
        
    }

   


}
