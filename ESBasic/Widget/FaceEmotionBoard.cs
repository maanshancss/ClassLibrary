using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ESBasic.Widget
{
    /// <summary>
    /// FaceEmotionBoard 表情面板。
    /// </summary>
    public partial class FaceEmotionBoard : UserControl
    {
        private Rectangle validRegion;        
        private IList<Image> imageList = new List<Image>();  
        /// <summary>
        /// EmotionClicked 某个表情图片被点击。参数为被点击图片的索引。
        /// </summary>
        public event CbSimpleInt EmotionClicked;

        #region FaceEmotionBoard
        public FaceEmotionBoard()
        {
            InitializeComponent();
            this.EmotionClicked += delegate { };
        } 
        #endregion

        #region Property
        #region ImageLength
        private int imageLength = 24;
        public int ImageLength
        {
            get { return imageLength; }
            set { imageLength = value; }
        }
        #endregion

        #region CountPerLine
        private int countPerLine = 15;
        public int CountPerLine
        {
            get { return countPerLine; }
            set { countPerLine = value; }
        }
        #endregion

        #region Span
        private int span = 4;
        public int Span
        {
            get { return span; }
            set { span = value; }
        }
        #endregion 
        #endregion

        #region Initialize
        public void Initialize(IList<Image> _imageList)
        {
            this.imageList = _imageList;
            int countPerCol = this.imageList.Count / this.countPerLine;
            countPerCol += (this.imageList.Count % this.countPerLine == 0) ? 0 : 1;
            this.validRegion = new Rectangle(new Point(0, 0), new Size(this.countPerLine * (this.span + this.imageLength), countPerCol * (this.span + this.imageLength)));
        } 
        #endregion

        #region OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int countPerCol = this.imageList.Count / this.countPerLine;
            countPerCol += (this.imageList.Count % this.countPerLine == 0) ? 0 : 1;

            Pen pen = new Pen(Color.LightSkyBlue);
            for (int i = 0; i <= this.countPerLine; i++)
            {
                e.Graphics.DrawLine(pen, new Point(i * (this.imageLength + this.span), 0), new Point(i * (this.imageLength + this.span), countPerCol * (this.imageLength + this.span)));
            }

            for (int i = 0; i <= countPerCol; i++)
            {
                e.Graphics.DrawLine(pen, new Point(0, i * (this.imageLength + this.span)), new Point((this.imageLength + this.span) * this.countPerLine, i * (this.imageLength + this.span)));
            }

            for (int i = 0; i < this.imageList.Count; i++)
            {
                int y = i / this.countPerLine;
                int x = i % this.countPerLine;

                e.Graphics.DrawImage(this.imageList[i], new Point(x * (this.imageLength + this.span) + this.span, y * (this.imageLength + this.span) + this.span));
            }
        } 
        #endregion

        #region FaceEmotionBoard_MouseClick
        private void FaceEmotionBoard_MouseClick(object sender, MouseEventArgs e)
        {
            int index = this.GetEmotionIndex(e.Location);
            if (index >= 0)
            {
                this.EmotionClicked(index);
            }
        }

        private int GetEmotionIndex(Point pt)
        {
            if (!this.validRegion.Contains(pt))
            {
                return -1;
            }

            int col = (pt.X - this.span) / (this.imageLength + this.span);
            int line = (pt.Y - this.span) / (this.imageLength + this.span);
            return line * this.countPerLine + col;
        } 
        #endregion

        private void FaceEmotionBoard_MouseHover(object sender, EventArgs e)
        {            
            //Point pt = this.PointToClient(Cursor.Position);
            //int index = this.GetEmotionIndex(pt);
            //if (index >= 0)
            //{
            //    this.toolTip1.Show("Good", this, new Point(pt.X,pt.Y+10),2000);
            //}
        }


    }
}
