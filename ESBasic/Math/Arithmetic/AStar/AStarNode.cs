using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ESBasic.Arithmetic.AStar
{
    /// <summary>
    /// AStarNode 用于保存规划到当前节点时的各个Cost值以及父节点。
    /// zhuweisky 2008.10.18
    /// </summary>
    public class AStarNode
    {
        #region Ctor
        public AStarNode(Point loc, AStarNode previous, int _costG, int _costH)
        {
            this.location = loc;
            this.previousNode = previous;
            this.costG = _costG;
            this.costH = _costH;
        }
        #endregion

        #region Location
        private Point location = new Point(0, 0);
        /// <summary>
        /// Location 节点所在的位置，其X值代表ColumnIndex，Y值代表LineIndex
        /// </summary>
        public Point Location
        {
            get { return location; }
        } 
        #endregion       

        #region PreviousNode
        private AStarNode previousNode = null;
        /// <summary>
        /// PreviousNode 父节点，即是由哪个节点导航到当前节点的。
        /// </summary>
        public AStarNode PreviousNode
        {
            get { return previousNode; }
        }
        #endregion

        #region CostF
        /// <summary>
        /// CostF 从起点导航经过本节点然后再到目的节点的估算总代价。
        /// </summary>
        public int CostF
        {
            get
            {
                return this.costG + this.costH;
            }
        }
        #endregion

        #region CostG
        private int costG = 0;
        /// <summary>
        /// CostG 从起点导航到本节点的代价。
        /// </summary>
        public int CostG
        {
            get { return costG; }
        }
        #endregion

        #region CostH
        private int costH = 0;
        /// <summary>
        /// CostH 使用启发式方法估算的从本节点到目的节点的代价。
        /// </summary>
        public int CostH
        {
            get { return costH; }
        }
        #endregion

        #region ResetPreviousNode
        /// <summary>
        /// ResetPreviousNode 当从起点到达本节点有更优的路径时，调用该方法采用更优的路径。
        /// </summary>        
        public void ResetPreviousNode(AStarNode previous, int _costG)
        {
            this.previousNode = previous;
            this.costG = _costG;         
        }
        #endregion

        public override string ToString()
        {
            return this.location.ToString();
        }
    }
}
