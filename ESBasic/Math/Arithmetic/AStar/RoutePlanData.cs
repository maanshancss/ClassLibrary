using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ESBasic.Arithmetic.AStar
{
    /// <summary>
    /// RoutePlanData 用于封装一次路径规划过程中的规划信息。
    /// </summary>
    public class RoutePlanData
    {
        #region CellMap
        private Rectangle cellMap;
        /// <summary>
        /// CellMap 地图的矩形大小。经过单元格标准处理。
        /// </summary>
        public Rectangle CellMap
        {
            get { return cellMap; }
        } 
        #endregion

        #region ClosedList
        private IList<AStarNode> closedList = new List<AStarNode>();
        /// <summary>
        /// ClosedList 关闭列表，即存放已经遍历处理过的节点。
        /// </summary>
        public IList<AStarNode> ClosedList
        {
            get { return closedList; }
        } 
        #endregion

        #region OpenedList
        private IList<AStarNode> openedList = new List<AStarNode>();
        /// <summary>
        /// OpenedList 开放列表，即存放已经开发但是还未处理的节点。
        /// </summary>
        public IList<AStarNode> OpenedList
        {
            get { return openedList; }
        } 
        #endregion

        #region Destination
        private Point destination;
        /// <summary>
        /// Destination 目的节点的位置。
        /// </summary>
        public Point Destination
        {
            get { return destination; }           
        } 
        #endregion

        #region Ctor
        public RoutePlanData(Rectangle map, Point _destination)
        {
            this.cellMap = map;
            this.destination = _destination;
        } 
        #endregion
    }
}
