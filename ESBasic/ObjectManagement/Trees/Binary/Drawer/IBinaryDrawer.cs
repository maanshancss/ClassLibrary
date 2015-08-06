using System;
using System.Drawing ;

namespace ESBasic.ObjectManagement.Trees.Binary
{
	/// <summary>
	/// IBinaryDrawer �������豸�ϻ��ƶ�������
	/// ���ߣ���ΰ sky.zhuwei@163.com 
	/// 2005.05.11
	/// </summary>
    public interface IBinaryTreeDrawer<TVal> where TVal : IComparable
	{
		void Initialize(DrawerParas paras) ;
		void ResetGraphic(Graphics g) ;     //���豸�����仯��ߴ�ı�ʱ�������豸���
        void DrawBinaryTree(IBinaryTree<TVal> tree, int offsetLeft, int offsetHigh); //offset���ڹ���
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
		public int Radius = 10 ; //�ڵ�Բ�İ뾶
		public Pen PenNode = new Pen(Color.Black ,1) ;
		public Pen PenLine = new Pen(Color.Black ,1) ;
		public SolidBrush BrushNode = new SolidBrush(Color.Pink);		
		public Font  FontText =  new Font("Arial", 9);
		public SolidBrush BrushText = new SolidBrush(Color.Black);
	}
}
