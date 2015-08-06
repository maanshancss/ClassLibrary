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
    /// Thumbnail 用于显示缩略图的控件。
    /// </summary>
    public partial class Thumbnail : UserControl
    {
        public event CbGeneric<string> ThumbnailClicked;
        
        public Thumbnail()
        {
            InitializeComponent();
            this.ThumbnailClicked += delegate { };
        }

        #region ImageName
        private string imageName = "";
        /// <summary>
        /// ImageName 缩略图的名称，将被显示在缩略图的下方。
        /// </summary>
        public string ImageName
        {
            get { return imageName; }
        } 
        #endregion

        #region Selected
        public bool Selected
        {
            get
            {
                return this.BackColor == Color.WhiteSmoke;
            }
            set
            {
                if (this.Selected == value)
                {
                    return;
                }

                this.BackColor = value ? Color.WhiteSmoke : Color.Transparent;
                this.BorderStyle = value ? BorderStyle.FixedSingle : BorderStyle.None;

                if (value)
                {
                    this.ThumbnailClicked(this.label1.Text);
                }
            }
        } 
        #endregion

        private bool ThumbnailCallBack()//GDI+委托
        {
            return false;
        }

        public void UpdateImage(Image image, float thumbnailCoef)
        {
            int deltHeight = this.Height - this.pictureBox1.Height;
            int deltWidth = this.Width - this.pictureBox1.Width;

            Size thumbnailSize = new Size((int)(image.Width * thumbnailCoef), (int)(image.Height * thumbnailCoef));
            this.Size = new Size(thumbnailSize.Width + deltWidth, thumbnailSize.Height + deltHeight);                       
            
            System.Drawing.Image.GetThumbnailImageAbort myCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallBack);

            Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, myCallBack, IntPtr.Zero);//生成缩略图
            this.pictureBox1.Image = thumbnail;   
        }

        public void Initialize(string name, Image image, float thumbnailCoef)
        {
            this.imageName = name;
            this.label1.Text = name;
            this.UpdateImage(image, thumbnailCoef);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Selected = true;            
        }
    }
}
