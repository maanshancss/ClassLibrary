using System;
using System.Drawing ;

namespace ESBasic.ObjectManagement.Trees.Binary
{
	/// <summary>
	/// IBinaryDrawer 用于在设备上绘制二叉树。
	/// 作者：朱伟 sky.zhuwei@163.com 
	/// 2005.05.11
	/// </summary>
    public interface IBinaryTreeDrawer<TVal> where TVal : IComparable
	{
		void Initialize(DrawerParas paras) ;
		void ResetGraphic(Graphics g) ;     //在设备发生变化或尺寸改变时需重设设备句柄
        void DrawBinaryTree(IBinaryTree<TVal> tree, int offsetLeft, int offsetHigh); //offset用于滚动
		void Zoom(double coeff) ;
		Size GetCanvasSize(int binaryTreeDepth ,int radius) ;

		int Radius{ get ;}
	}
	

	public class DrawerParas
	{
        public DrawerParas() { }
        public DrawerParas(Graphics _graphic)
        {
            this.Graphic = _graphic;
        }

		public Graphics Graphic = null;
		public Color GraphicBackColor = Color.Gainsboro;
		public int Radius = 10 ; //节点圆的半径
		public Pen PenNode = new Pen(Color.Black ,1) ;
		public Pen PenLine = new Pen(Color.Black ,1) ;
		public SolidBrush BrushNode = new SolidBrush(Color.Pink);		
		public Font  FontText =  new Font("Arial", 9);
		public SolidBrush BrushText = new SolidBrush(Color.Black);
	}
}
