using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.ObjectManagement.Trees.Multiple
{
    /// <summary>
    /// MNode IMultiTree中的节点类型
    /// </summary>
    [Serializable]
    public class MNode<TVal> where TVal : IMTreeVal
    {
        #region Ctor
        public MNode() { }
        public MNode(TVal val ,MNode<TVal> _parent)
        {
            this.theValue = val;
            this.parent = _parent;
            
        }
        #endregion   
  
        #region Parent
        private MNode<TVal> parent;
        public MNode<TVal> Parent
        {            
            get { return parent; }
        } 
        #endregion

        #region TheValue
        private TVal theValue = default(TVal);
        public TVal TheValue
        {
            get { return theValue; }
            set { theValue = value; }
        }
        #endregion

        #region Children
        private IList<MNode<TVal>> children = new List<MNode<TVal>>();
        public IList<MNode<TVal>> Children
        {
            get { return children; }
            set { children = value; }
        }
        #endregion

        #region AddChild
        public MNode<TVal> AddChild(TVal child)
        {
            MNode<TVal> childNode = new MNode<TVal>(child ,this);
            this.children.Add(childNode);

            return childNode;
        }
        #endregion

        public override string ToString()
        {
            return this.theValue.ToString();
        }
    }
}
