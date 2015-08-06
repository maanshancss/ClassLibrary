using System;
using System.Drawing ; 
using System.Collections ;
using System.Collections.Generic;

namespace ESBasic.Geometry
{
	/// <summary>
	/// Triangle 几何三角形。
	/// </summary>
	public class Triangle
	{
        private IList<Point> vertextList = null;
        private List<float> lengthList = null;
		private float myArea = 0 ;

		#region ctor
        public Triangle(IList<Point> ptList)
		{
			this.vertextList = ptList ;
			this.FillLengthList() ;
		}

		public Triangle(Point pt0 ,Point pt1 ,Point pt2)
		{
            List<Point> ptList = new List<Point>();
			ptList.Add(pt0) ;
			ptList.Add(pt1) ;
			ptList.Add(pt2) ;		

			this.vertextList = ptList ;
			this.FillLengthList() ;
		}

		private void FillLengthList()
		{
			Point pt0 = (Point)this.vertextList[0] ;
			Point pt1 = (Point)this.vertextList[1] ;
			Point pt2 = (Point)this.vertextList[2] ;		

			float length_01 = (float)Math.Sqrt((pt0.X - pt1.X)*(pt0.X - pt1.X) + (pt0.Y - pt1.Y)*(pt0.Y - pt1.Y)) ;
			float length_02 = (float)Math.Sqrt((pt0.X - pt2.X)*(pt0.X - pt2.X) + (pt0.Y - pt2.Y)*(pt0.Y - pt2.Y)) ;
			float length_12 = (float)Math.Sqrt((pt2.X - pt1.X)*(pt2.X - pt1.X) + (pt2.Y - pt1.Y)*(pt2.Y - pt1.Y)) ;

            this.lengthList = new List<float>();
			this.lengthList.Add(length_12) ;
			this.lengthList.Add(length_02) ;
			this.lengthList.Add(length_01) ;
		}

		#endregion

		#region Area ,GetEdgeLength
		/// <summary>
		/// Area 三角形的面积
		/// </summary>		
		public float Area
		{
			get
			{
				if(this.myArea == 0)
				{
					this.myArea = this.GetArea() ;
				}

				return this.myArea ;
			}
		}		

		private float GetArea()
		{
			float len0 = (float)this.lengthList[0] ;
			float len1 = (float)this.lengthList[1] ;
			float len2 = (float)this.lengthList[2] ;

			float p = (len0 + len1 + len2) * 0.5f ;

			return (float)Math.Sqrt(p * (p-len0) * (p-len1) * (p-len2)) ;
		}


		public float GetEdgeLength(int index)//0<= index <=2
		{
			if((index <0) ||(index >2))
			{
				return 0 ;
			}

			return (float)this.lengthList[index] ;
		}
		#endregion

		#region Contains
		/// <summary>
		/// Contains 判断某点是否在三角形内部
		/// </summary>		
		public bool Contains(Point pt)
		{
            Triangle tr1 = new Triangle(pt, this.vertextList[0], this.vertextList[1]);
            Triangle tr2 = new Triangle(pt, this.vertextList[0], this.vertextList[2]);
            Triangle tr3 = new Triangle(pt, this.vertextList[1], this.vertextList[2]);

            return this.Area > (tr1.Area + tr2.Area + tr3.Area - 1);
			
		}
		#endregion
	}
}
