using System;
using System.Drawing;
using ESBasic.Geometry;

namespace ESBasic.Geometry
{
	/// <summary>
	/// LineSegment �߶εĶ����ʾ��
	/// </summary>
	public class LineSegment
	{
		private Point pt1 ;
		private Point pt2 ;

		public LineSegment(Point p1 ,Point p2)
		{			
			this.pt1 = p1 ;
			this.pt2 = p2 ;
		}

		#region property
		public Point FirstPoint
		{
			get
			{
				return this.pt1 ;
			}
		}

		
		public Point SecondPoint
		{
			get
			{
				return this.pt2 ;
			}
		}

        public int DeltX
		{
			get
			{
				return (this.pt2.X - this.pt1.X) ;
			}
		}

        public int DeltY
		{
			get
			{
				return (this.pt2.Y - this.pt1.Y) ;
			}
		}
		#endregion

		#region Contains
		/// <summary>
		/// Ŀ����Ƿ����߶��� (y2-y1)(x-x1) = (y-y1)(x2-x1)
		/// </summary>		
		public bool Contains(Point pt)
		{
			if((pt.Equals(this.pt1)) ||(pt.Equals(this.pt2)))
			{
				return true ;
			}

			float left  = (this.pt2.Y - this.pt1.Y) * (pt.X       - this.pt1.X) ;
			float right = (pt.Y       - this.pt1.Y) * (this.pt2.X - this.pt1.X) ;

			if(Math.Abs(left - right) < GeometryHelper.ToleranceForFloat)
			{
				return false ;
			}

			if(! GeometryHelper.IsInRegion(pt.X ,this.pt1.X ,this.pt2.X))
			{
				return false ;
			}

			if(! GeometryHelper.IsInRegion(pt.Y ,this.pt1.Y ,this.pt2.Y))
			{
				return false ;
			}

			return true ;
		}
		#endregion

		#region GetIntersectWithVerAxis ,GetIntersectWithHorAxis
		/// <summary>
		/// GetIntersectWithVerAxis ������Y��Ľ��㣬û�н����򷵻�float.MaxValue
		/// </summary>		
		public float GetIntersectWithVerAxis()
		{
			if(this.pt1.Y == this.pt2.Y)
			{
				return float.MaxValue ;
			}

			int temp = this.DeltY*(-1)*this.pt1.X ;
            int intersect = (temp / this.DeltX + this.pt1.Y);

			if(this.Contains(new Point(0 ,intersect)) )
			{
				return intersect ;
			}

			return float.MaxValue ;
		}

		/// <summary>
		/// GetIntersectWithVerAxis ������X��Ľ��㣬û�н����򷵻�float.MaxValue
		/// </summary>	
		public float GetIntersectWithHorAxis()
		{
			if(this.pt1.X == this.pt2.X)
			{
				return float.MaxValue ;
			}

            int temp = this.DeltX * (-1) * this.pt1.Y;
            int intersect = (temp / this.DeltY + this.pt1.X);

			if(this.Contains(new Point(intersect ,0)))
			{
				return intersect ;
			}

			return float.MaxValue ;
		}
		#endregion

		#region Draw
		//��������
		public void Draw(Graphics g ,Pen pen)
		{
			g.DrawLine(pen ,this.pt1 ,this.pt2) ;			
		}

		//���ߣ�����ʾ����
		public void Draw(Graphics g ,Pen pen ,SolidBrush brushNode ,Font  fontText)
		{
			g.DrawLine(pen ,this.pt1 ,this.pt2) ;
			string ss1 = string.Format("({0},{1})" ,this.pt1.X ,this.pt1.Y) ;
			string ss2 = string.Format("({0},{1})" ,this.pt2.X ,this.pt2.Y) ;

			//SolidBrush brushNode = new SolidBrush(Color.Blue);		
			//Font  fontText =  new Font("Arial", 9);
			g.DrawString(ss1 ,fontText ,brushNode ,this.pt1) ;
			g.DrawString(ss2 ,fontText ,brushNode ,this.pt2) ;
		}
		#endregion
	}

    
}
