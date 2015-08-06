using System;
using System.Drawing ;

namespace ESBasic.ObjectManagement.Trees.Binary
{
	/// <summary>
	/// BinaryDrawer IBinaryDrawer 的默认实现。
	/// 作者：朱伟 sky.zhuwei@163.com 
	/// 2005.05.11
	/// </summary>
    public class BinaryTreeDrawer<TVal> : IBinaryTreeDrawer<TVal> where TVal : IComparable
	{
		private	DrawerParas drawParas = null ;
        private IBinaryTree<TVal> curTree = null;
		private int offsetX = 0 ;
		private int offsetY = 0 ;

		public BinaryTreeDrawer()
		{			
		}

		#region IBinaryDrawer 成员

		public void Initialize(DrawerParas paras)
		{
			this.drawParas = paras ;
		}

		public void ResetGraphic(Graphics g) 
		{
			if(this.drawParas != null)
			{
				this.drawParas.Graphic = g ;
			}
		}

		public Size GetCanvasSize(int binaryTreeDepth ,int radius)
		{
			int width = radius * (int)(Math.Pow(2 ,binaryTreeDepth-1)) ;
			int heigh = radius * binaryTreeDepth ;

			return new Size(width ,heigh);
		}

		public void Zoom(double coeff)
		{
			this.drawParas.Radius = (int)(this.drawParas.Radius * coeff );
			if(this.curTree != null)
			{
				this.DrawBinaryTree(this.curTree ,this.offsetX ,this.offsetY) ;
			}
		}

        public void DrawBinaryTree(IBinaryTree<TVal> tree, int offsetLeft, int offsetHigh) 
		{
			if((this.drawParas == null) || (tree == null) ||(tree.Count == 0))
			{
				return ;
			}

			this.curTree = tree ;
			this.offsetX = offsetLeft ;
			this.offsetY = offsetHigh ;

			try
			{				
				this.drawParas.Graphic.Clear(this.drawParas.GraphicBackColor) ;				

				Point[][] position = this.GetNodePosition(tree.Depth) ;

				Node<TVal> root = tree.Root ;

				this.DrawTree(root ,0 ,0 ,this.drawParas.Graphic ,position ,offsetLeft ,offsetHigh) ;						
			}
			catch(Exception ee)
			{
				ee =ee ;
			}
		}

		public int Radius
		{
			get
			{
				if(this.drawParas == null)
				{
					return 0 ;
				}

				return this.drawParas.Radius ;
			}
		}

		#endregion

		#region private
		#region GetNodePosition
		private Point[][] GetNodePosition(int depth)
		{			
			Point[][] position = new Point[depth][] ;

			for(int i=0 ;i<depth ;i++)
			{
				position[i] = new Point[(int)Math.Pow(2 ,i)] ;
			}

			//初始化最下一层
			for(int j=0 ;j<Math.Pow(2 ,depth-1) ;j++ )
			{
				position[depth-1][j].X =  2*j ;//(int)(-( 2*(Math.Pow(2 ,depth-1)) -1 )/2) + 2*j ;
				position[depth-1][j].Y = 2 * (depth) ;
			}


			//初始化其它层
			if(depth >=2)
			{			
				for(int i=depth-2 ;i>=0 ;i--)
				{
					for(int j=0 ;j<Math.Pow(2 ,i) ;j++ )
					{
						position[i][j].X = (position[i+1][2*j].X + position[i+1][2*j + 1].X)/2 ;
						position[i][j].Y = 2 * (i+1) ;
					}
				}
			}

			return position ;
		}
		#endregion

		#region DrawTree
		private void DrawTree(Node<TVal> root ,int rowIndex ,int colIndex ,Graphics g ,Point[][] position ,int offsetLeft ,int offsetHigh)
		{			
			if(root == null)
			{
				return ;
			}		
			
			int radius = this.drawParas.Radius ;

			int x = position[rowIndex][colIndex].X*radius  - offsetLeft + radius ;
			int y = position[rowIndex][colIndex].Y*radius  - offsetHigh + radius ;

			
			g.FillEllipse(this.drawParas.BrushNode ,x ,y ,radius*2 ,radius*2) ;
			g.DrawEllipse(this.drawParas.PenNode ,x ,y ,radius*2 ,radius*2) ;

            g.DrawString(root.TheValue.ToString(), this.drawParas.FontText, this.drawParas.BrushText, x + radius / 4, y + radius / 4);

			//递归
			int x2 = 0 ;
			int y2 = 0 ;
			if(root.LeftChild != null)
			{
				int col = 2*colIndex  ;
				x2 = position[rowIndex+1][col].X*radius  - offsetLeft + radius ;
				y2 = position[rowIndex+1][col].Y*radius  - offsetHigh + radius ;
				g.DrawLine(this.drawParas.PenNode ,x + radius/2 ,y + radius/2 ,x2 + radius/2 ,y2 + radius/2) ;
				this.DrawTree(root.LeftChild ,rowIndex+1 ,col ,g, position ,offsetLeft ,offsetHigh) ;
			}

			if(root.RightChild != null)
			{
				int col = 2*colIndex +1 ;
				x2 = position[rowIndex+1][col].X*radius  - offsetLeft + radius ;
				y2 = position[rowIndex+1][col].Y*radius  - offsetHigh + radius ;
				g.DrawLine(this.drawParas.PenNode ,x + radius/2 ,y + radius/2 ,x2 + radius/2 ,y2 + radius/2) ;
				this.DrawTree(root.RightChild ,rowIndex+1 ,col ,g, position ,offsetLeft ,offsetHigh) ;
			}
		}

		
		#endregion

		#endregion
	}
}
