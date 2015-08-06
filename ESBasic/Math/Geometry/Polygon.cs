using System;
using System.Collections;
using System.Drawing ;

namespace ESBasic.Geometry
{
	/// <summary>
	/// Polygon 多边形的对象表示。
	/// zhuweisky 2005.09.06
	/// </summary>
	public class Polygon
	{
		private Point[] theVertex ;

		public Polygon(Point[] vertex)
		{
			this.theVertex = vertex ;

			if((this.theVertex == null) || (this.theVertex.Length < 3))
			{
				throw new ArgumentException("The vertex count is not right !") ;
			}
		}

		public Polygon(ArrayList ptList)
		{
			if((ptList == null) || (ptList.Count < 3))			
			{
				throw new ArgumentException("The vertex count is not right !") ;
			}

			this.theVertex = new Point[ptList.Count] ;
			ptList.CopyTo(this.theVertex ) ;			
		}

		#region GetVertex ,GetLineSegment
		/// <summary>
		/// GetVertex 获取指定索引的顶点
		/// </summary>	
		public Point GetVertex(int index)
		{
			if((index >= this.theVertex.Length) ||(index < 0))
			{
				throw new IndexOutOfRangeException("The index is out of range !") ;
			}

			return this.theVertex[index] ;
		}

		/// <summary>
		/// GetEdge 获取指定索引的边
		/// </summary>		
		public LineSegment GetEdge(int index)
		{
			if((index >= this.theVertex.Length) ||(index < 0))
			{
				throw new IndexOutOfRangeException("The index is out of range !") ;
			}

			if(index == (this.theVertex.Length-1))
			{
				return new LineSegment(this.theVertex[index] ,this.theVertex[0]) ;				
			}

			return new LineSegment(this.theVertex[index] ,this.theVertex[index+1]) ;			
		}
		#endregion

		#region Contains ,IsOnEdge
		/// <summary>
		/// Contains 指定点是否位于多边形内部
		/// </summary>		
		public bool Contains(Point target)
		{
			if(this.IsOnEdge(target))
			{
				return true ;
			}
			
			Polygon transPolygon = this.Offset(target.X ,target.Y) ;
			ArrayList intersecXPoslist = new ArrayList() ;
			ArrayList intersecYPoslist = new ArrayList() ;
			ArrayList intersecXNeglist = new ArrayList() ;
			ArrayList intersecYNeglist = new ArrayList() ;

			for(int i=0 ;i<this.theVertex.Length ;i++)
			{
				LineSegment seg = transPolygon.GetEdge(i) ;
				float interX = seg.GetIntersectWithHorAxis() ;
				float interY = seg.GetIntersectWithVerAxis() ;

				if(interX != float.MaxValue)
				{
					if(interX > 0)
					{
						intersecXPoslist.Add(interX) ;
					}
					else
					{
						intersecXNeglist.Add(interX) ;
					}
				}

				if(interY != float.MaxValue)
				{
					if(interY > 0)
					{
						intersecYPoslist.Add(interY) ;
					}
					else
					{
						intersecYNeglist.Add(interY) ;
					}
				}
			}

			if((intersecXPoslist.Count %2 == 0) ||(intersecYPoslist.Count %2 == 0) 
			 ||(intersecXNeglist.Count %2 == 0) ||(intersecYNeglist.Count %2 == 0))
			{
				return false ;
			}

			return true ;
		}		

		public bool IsOnEdge(Point target)
		{
			for(int i=0 ;i<this.theVertex.Length ;i++)
			{
				LineSegment seg = this.GetEdge(i) ;
				if(seg.Contains(target))
				{
					return true ;
				}
			}

			return false ;
		}
		#endregion

		#region Offset
        public Polygon Offset(int offsetX, int offsetY)
		{
			Point[] result = new Point[this.theVertex.Length] ;
 
			for(int i=0 ;i<result.Length ;i++)
			{
				result[i] = new Point(this.theVertex[i].X-offsetX ,this.theVertex[i].Y-offsetY) ;
			}

			return new Polygon(result) ;
		}
		#endregion

		#region Draw
		public void Draw(Graphics g ,Pen pen)
		{
			for(int i=0 ;i<this.theVertex.Length ;i++)
			{
				LineSegment seg = this.GetEdge(i) ;
				seg.Draw(g ,pen) ;
			}
		}

		//画线，并显示坐标
		public void Draw(Graphics g ,Pen pen ,SolidBrush brushNode ,Font  fontText)
		{
			for(int i=0 ;i<this.theVertex.Length ;i++)
			{
				LineSegment seg = this.GetEdge(i) ;
				seg.Draw(g ,pen ,brushNode ,fontText) ;
			}
		}
		#endregion
	}
}
