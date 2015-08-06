using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ESBasic.Widget
{
    /// <summary>
    /// CaptureScreenForm 用于获取截屏区域。
    /// </summary>
    public partial class CaptureScreenForm : Form
    {
        private Point? start = null;
        private Point? end = null;
        private Point? tempEnd = null ;
        private Pen pen = new Pen(Color.LightSkyBlue,2); 
        private SolidBrush solidBrush = new SolidBrush(Color.FromArgb(90, Color.FromArgb(229, 243, 251)));
        private string theTip;
        private Image backImage;


        public CaptureScreenForm() :this(null)
        { }

        public CaptureScreenForm(string tip)
        {
            
            InitializeComponent();        
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true); //
            this.theTip = tip;                  
            this.BackgroundImage = this.GetDesktopImage();

            this.ShowTip();
        }

        private Image GetDesktopImage()
        {
            Rectangle rect = Screen.GetBounds(this);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Location, new Point(0, 0), rect.Size);           
            return bmp;
        }

        private void ShowTip()
        {
            if (!string.IsNullOrEmpty(this.theTip))
            {
                this.toolTip1.SetToolTip(this, this.theTip);
            }
        }
       
        private void CaptureScreenForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                return;
            }
            else
            {
                this.start = e.Location;               
            }
        }

        private void CaptureScreenForm_MouseMove(object sender, MouseEventArgs e)
        {
           if (this.start == null)
           {
               this.ShowTip();
               return;
           }

           this.tempEnd = e.Location;
           this.Refresh();          
        }

        private void CaptureScreenForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            this.end = e.Location;
            if (this.CaptureRegion.Width < 8 || this.CaptureRegion.Height < 8)
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            this.DialogResult = DialogResult.OK;
        }       

        private void CaptureScreenForm_Paint(object sender, PaintEventArgs e)
        {
            if (this.start == null || this.tempEnd == null)
            {
                return;
            }
            e.Graphics.DrawRectangle(this.pen, this.TempCaptureRegion);
            e.Graphics.FillRectangle(this.solidBrush, this.TempCaptureRegion);
        }

        #region TempCaptureRegion
        private Rectangle TempCaptureRegion
        {
            get
            {
                int x = this.start.Value.X;
                int y = this.start.Value.Y;

                if (x > this.tempEnd.Value.X)
                {
                    x = this.tempEnd.Value.X;
                }

                if (y > this.tempEnd.Value.Y)
                {
                    y = this.tempEnd.Value.Y;
                }

                return new Rectangle(new Point(x, y), new Size(Math.Abs(this.start.Value.X - this.tempEnd.Value.X), Math.Abs(this.start.Value.Y - this.tempEnd.Value.Y)));
            }
        } 
        #endregion

        #region CaptureRegion
        public Rectangle CaptureRegion
        {
            get
            {
                int x = this.start.Value.X;
                int y = this.start.Value.Y;

                if (x > this.end.Value.X)
                {
                    x = this.end.Value.X;
                }

                if (y > this.end.Value.Y)
                {
                    y = this.end.Value.Y;
                }

                return new Rectangle(new Point(x, y), new Size(Math.Abs(this.start.Value.X - this.end.Value.X), Math.Abs(this.start.Value.Y - this.end.Value.Y)));
            }
        }
        #endregion

        private void CaptureScreenForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }    
       
    }
}
