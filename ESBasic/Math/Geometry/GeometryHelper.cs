using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Geometry
{
    public static class GeometryHelper
    {
        #region GetAdjacentPoint
        /// <summary>
        /// GetAdjacentPoint 获取某个方向上的相邻点
        /// </summary>       
        public static Point GetAdjacentPoint(Point current, CompassDirections direction)
        {
            switch (direction)
            {
                case CompassDirections.North:
                    {
                        return new Point(current.X, current.Y - 1);
                    }
                case CompassDirections.South:
                    {
                        return new Point(current.X, current.Y + 1);
                    }
                case CompassDirections.East:
                    {
                        return new Point(current.X + 1, current.Y);
                    }
                case CompassDirections.West:
                    {
                        return new Point(current.X - 1, current.Y);
                    }
                case CompassDirections.NorthEast:
                    {
                        return new Point(current.X + 1, current.Y - 1);
                    }
                case CompassDirections.NorthWest:
                    {
                        return new Point(current.X - 1, current.Y - 1);
                    }
                case CompassDirections.SouthEast:
                    {
                        return new Point(current.X + 1, current.Y + 1);
                    }
                case CompassDirections.SouthWest:
                    {
                        return new Point(current.X - 1, current.Y + 1);
                    }
                default:
                    {
                        return current;
                    }
            }
        }
        #endregion     

        #region GetDistanceSquare
        /// <summary>
        /// GetDistanceSquare 获取两个点之间距离的平方。
        /// </summary>      
        public static int GetDistanceSquare(Point pt1, Point pt2)
        {
            int deltX = pt1.X - pt2.X;
            int deltY = pt1.Y - pt2.Y;

            return deltX * deltX + deltY * deltY;
        }
        #endregion

        #region GetDirectionBetween
        /// <summary>
        /// GetDirectionBetween 获取从起点到终点的方向
        /// </summary>       
        public static CompassDirections GetDirectionBetween(Point origin, Point dest, int torrence)
        {
            int deltX = dest.X - origin.X;
            int deltY = dest.Y - origin.Y;

            if (deltX > torrence && deltY < -torrence)
            {
                return CompassDirections.NorthEast;
            }

            if (deltX > torrence && deltY > torrence)
            {
                return CompassDirections.SouthEast;
            }

            if (deltX < -torrence && deltY < -torrence)
            {
                return CompassDirections.NorthWest;
            }

            if (deltX < -torrence && deltY > torrence)
            {
                return CompassDirections.SouthWest;
            }

            if (Math.Abs(deltX) < torrence && deltY < -torrence)
            {
                return CompassDirections.North;
            }

            if (Math.Abs(deltX) < torrence && deltY > torrence)
            {
                return CompassDirections.South;
            }

            if (deltX > torrence && Math.Abs(deltY) < torrence)
            {
                return CompassDirections.East;
            }

            if (deltX < -torrence && Math.Abs(deltY) < torrence)
            {
                return CompassDirections.West;
            }

            return CompassDirections.NotSet;
        }
        #endregion        

        public const float ToleranceForFloat = 0.00001f;

        #region IsInRegion
        /// <summary>
        /// IsInRegion 判断某个数是否在指定区域内
        /// </summary>		
        public static bool IsInRegion(float target, float f1, float f2)
        {
            if ((target > f1) && (target > f2))
            {
                return false;
            }

            if ((target < f1) && (target < f2))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region IsTriangle
        /// <summary>
        /// IsTriangle 判断集合中的头三个点Point是否可以构成一个三角形
        /// </summary>		
        public static bool IsTriangle(IList<Point> ptList)
        {
            //如果有两个点相同
            Point pt0 = (Point)ptList[0];
            Point pt1 = (Point)ptList[1];
            Point pt2 = (Point)ptList[2];

            if (pt0.Equals(pt1) || pt0.Equals(pt2) || pt1.Equals(pt2))
            {
                return false;
            }

            float length_01 = (float)Math.Sqrt((pt0.X - pt1.X) * (pt0.X - pt1.X) + (pt0.Y - pt1.Y) * (pt0.Y - pt1.Y));
            float length_02 = (float)Math.Sqrt((pt0.X - pt2.X) * (pt0.X - pt2.X) + (pt0.Y - pt2.Y) * (pt0.Y - pt2.Y));
            float length_12 = (float)Math.Sqrt((pt2.X - pt1.X) * (pt2.X - pt1.X) + (pt2.Y - pt1.Y) * (pt2.Y - pt1.Y));

            bool result0 = (length_01 + length_02 <= length_12);
            bool result1 = (length_01 + length_12 <= length_02);
            bool result2 = (length_02 + length_12 <= length_01);

            if (result0 || result1 || result2)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region ComputeLineLength
        public static float ComputeLineLength(Point pt1, Point pt2)
        {
            return (float)Math.Sqrt((pt2.X - pt1.X) * (pt2.X - pt1.X) + (pt2.Y - pt1.Y) * (pt2.Y - pt1.Y));
        }
        #endregion

        #region CreateRectangle
        public static Rectangle CreateRectangle(Point pt1, Point pt2)
        {
            int minX = pt1.X;
            int maxX = pt1.X;
            if (pt2.X < minX)
            {
                minX = pt2.X;
            }
            else
            {
                maxX = pt2.X;
            }

            int minY = pt1.Y;
            int maxY = pt1.Y;
            if (pt2.Y < minY)
            {
                minY = pt2.Y;
            }
            else
            {
                maxY = pt2.Y;
            }

            Point minPt = new Point(minX, minY);
            Size size = new Size(maxX - minX, maxY - minY);

            Rectangle rect = new Rectangle(minPt, size);
            int area = rect.Height * rect.Width;

            return rect;
        }
        #endregion

        #region CreateSquare
        public static Rectangle CreateSquare(Point fixedPt, Point pt2)
        {
            int minX = fixedPt.X;
            int maxX = fixedPt.X;
            if (pt2.X < minX)
            {
                minX = pt2.X;
            }
            else
            {
                maxX = pt2.X;
            }

            int minY = fixedPt.Y;
            int maxY = fixedPt.Y;
            if (pt2.Y < minY)
            {
                minY = pt2.Y;
            }
            else
            {
                maxY = pt2.Y;
            }
            
            Size size = new Size(maxX - minX, maxY - minY);
           
            int len = size.Width > size.Height ? size.Height : size.Width;

            int xCoef = pt2.X > fixedPt.X ? 1 : -1;
            int yCoef = pt2.Y > fixedPt.Y ? 1 : -1;          
            
            
            return CreateRectangle(fixedPt , new Point(fixedPt.X + xCoef*len ,fixedPt.Y + yCoef*len));
        }
        #endregion

        #region ComputeBounds
        public static Rectangle ComputeBounds(Point[] points)
        {
            int minX = points[0].X;
            int minY = points[0].Y;
            int maxX = points[0].X;
            int maxY = points[0].Y;

            foreach (Point pt in points)
            {
                if (pt.X < minX)
                {
                    minX = pt.X;
                }
                if (pt.X > maxX)
                {
                    maxX = pt.X;
                }

                if (pt.Y < minY)
                {
                    minY = pt.Y;
                }
                if (pt.Y > maxY)
                {
                    maxY = pt.Y;
                }
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        } 
        #endregion

        #region Multiple
        public static Point MultiplePoint(Point origin, float coef)
        {
            return new Point((int)(origin.X * coef), (int)(origin.Y * coef));
        }

        public static Size MultipleSize(Size origin, float coef)
        {
            return new Size((int)(origin.Width * coef), (int)(origin.Height * coef));
        }

        public static Rectangle MultipleRectangle(Rectangle origin, float coef)
        {
            return new Rectangle(MultiplePoint(origin.Location, coef), MultipleSize(origin.Size, coef));
        } 
        #endregion
    }
}
