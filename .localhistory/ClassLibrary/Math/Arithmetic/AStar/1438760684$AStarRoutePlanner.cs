using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ESBasic.Geometry;

namespace ESBasic.Arithmetic.AStar
{
    /// <summary>
    /// AStarRoutePlanner A*路径规划。每个单元格Cell的位置用Point表示
    /// F = G + H 。
    /// G = 从起点A，沿着产生的路径，移动到网格上指定方格的移动耗费。
    /// H = 从网格上那个方格移动到终点B的预估移动耗费。使用曼哈顿方法，它计算从当前格到目的格之间水平和垂直的方格的数量总和，忽略对角线方向。
    /// zhuweisky 2008.10.18
    /// </summary>
    public class AStarRoutePlanner
    {
        private int lineCount = 10;   //反映地图高度，对应Y坐标
        private int columnCount = 10; //反映地图宽度，对应X坐标
        private ICostGetter costGetter = new SimpleCostGetter();
        private bool[][] obstacles = null; //障碍物位置，维度：Column * Line         

        #region Ctor
        public AStarRoutePlanner() :this(10 ,10 ,new SimpleCostGetter())
        {           
        }
        public AStarRoutePlanner(int _lineCount, int _columnCount, ICostGetter _costGetter)
        {
            this.lineCount = _lineCount;
            this.columnCount = _columnCount;
            this.costGetter = _costGetter;

            this.InitializeObstacles();
        }

        /// <summary>
        /// InitializeObstacles 将所有位置均标记为无障碍物。
        /// </summary>
        private void InitializeObstacles()
        {
            this.obstacles = new bool[this.columnCount][];
            for (int i = 0; i < this.columnCount; i++)
            {
                this.obstacles[i] = new bool[this.lineCount];
            }

            for (int i = 0; i < this.columnCount; i++)
            {
                for (int j = 0; j < this.lineCount; j++)
                {
                    this.obstacles[i][j] = false;
                }
            }
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize 在路径规划之前先设置障碍物位置。
        /// </summary>        
        public void Initialize(IList<Point> obstaclePoints)
        {
            if (obstacles != null)
            {
                foreach (Point pt in obstaclePoints)
                {
                    this.obstacles[pt.X][pt.Y] = true;
                }
            }
        } 
        #endregion

        #region Plan
        public IList<Point> Plan(Point start, Point destination)
        {
            Rectangle map = new Rectangle(0, 0, this.columnCount, this.lineCount);
            if ((!map.Contains(start)) || (!map.Contains(destination)))
            {
                throw new Exception("StartPoint or Destination not in the current map!");
            }

            RoutePlanData routePlanData = new RoutePlanData(map, destination);

            AStarNode startNode = new AStarNode(start, null, 0, 0);
            routePlanData.OpenedList.Add(startNode);

            AStarNode currenNode = startNode;

            //从起始节点开始进行递归调用
            return DoPlan(routePlanData, currenNode);
        } 
        #endregion

        #region DoPlan
        private IList<Point> DoPlan(RoutePlanData routePlanData, AStarNode currenNode)
        {
            IList<CompassDirections> allCompassDirections = CompassDirectionsHelper.GetAllCompassDirections();
            foreach (CompassDirections direction in allCompassDirections)
            {
                Point nextCell = GeometryHelper.GetAdjacentPoint(currenNode.Location, direction);
                if (!routePlanData.CellMap.Contains(nextCell)) //相邻点已经在地图之外
                {
                    continue;
                }

                if (this.obstacles[nextCell.X][nextCell.Y]) //下一个Cell为障碍物
                {
                    continue;
                }

                int costG = this.costGetter.GetCost(currenNode.Location, direction);
                int costH = Math.Abs(nextCell.X - routePlanData.Destination.X) + Math.Abs(nextCell.Y - routePlanData.Destination.Y);
                if (costH == 0) //costH为0，表示相邻点就是目的点，规划完成，构造结果路径
                {
                    IList<Point> route = new List<Point>();
                    route.Add(routePlanData.Destination);
                    route.Insert(0, currenNode.Location);
                    AStarNode tempNode = currenNode;
                    while (tempNode.PreviousNode != null)
                    {
                        route.Insert(0, tempNode.PreviousNode.Location);
                        tempNode = tempNode.PreviousNode;
                    }

                    return route;
                }

                AStarNode existNode = this.GetNodeOnLocation(nextCell, routePlanData);
                if (existNode != null)
                {
                    if (existNode.CostG > costG)
                    {
                        //如果新的路径代价更小，则更新该位置上的节点的原始路径
                        existNode.ResetPreviousNode(currenNode, costG);
                    }
                }
                else
                {
                    AStarNode newNode = new AStarNode(nextCell, currenNode, costG, costH);
                    routePlanData.OpenedList.Add(newNode);
                }
            }

            //将已遍历过的节点从开放列表转移到关闭列表
            routePlanData.OpenedList.Remove(currenNode);
            routePlanData.ClosedList.Add(currenNode);

            AStarNode minCostNode = this.GetMinCostNode(routePlanData.OpenedList);
            if (minCostNode == null) //表明从起点到终点之间没有任何通路。
            {
                return null;
            }

            //对开放列表中的下一个代价最小的节点作递归调用
            return this.DoPlan(routePlanData, minCostNode);
        } 
        #endregion

        #region GetNodeOnLocation
        /// <summary>
        /// GetNodeOnLocation 目标位置location是否已存在于开放列表或关闭列表中
        /// </summary>       
        private AStarNode GetNodeOnLocation(Point location, RoutePlanData routePlanData)
        {
            foreach (AStarNode temp in routePlanData.OpenedList)
            {
                if (temp.Location == location)
                {
                    return temp;
                }
            }

            foreach (AStarNode temp in routePlanData.ClosedList)
            {
                if (temp.Location == location)
                {
                    return temp;
                }
            }

            return null;
        } 
        #endregion

        #region GetMinCostNode
        /// <summary>
        /// GetMinCostNode 从开放列表中获取代价F最小的节点，以启动下一次递归
        /// </summary>      
        private AStarNode GetMinCostNode(IList<AStarNode> openedList)
        {
            if (openedList.Count == 0)
            {
                return null;
            }

            AStarNode target = openedList[0];
            foreach (AStarNode temp in openedList)
            {
                if (temp.CostF < target.CostF)
                {
                    target = temp;
                }
            }

            return target;
        } 
        #endregion
        
    }
}
