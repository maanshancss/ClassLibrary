using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESBasic.Collections;

namespace ESBasic.Widget
{
    /// <summary>
    /// ThumbnailListBox 用于管理并显示所有的缩略图控件实例。
    /// </summary>
    public partial class ThumbnailListBox : UserControl
    {
        public event CbGeneric<int> ThumbnailClicked; //参数为index

        public ThumbnailListBox()
        {
            InitializeComponent();
            this.ThumbnailClicked += delegate { };
        }

        #region ThumbnailCoef
        private float thumbnailCoef = 0.1f;
        public float ThumbnailCoef
        {
            get { return thumbnailCoef; }
            set { thumbnailCoef = value; }
        } 
        #endregion

        #region Load
        public void Load(IList<ThumbnailData> images)
        {
            this.flowLayoutPanel1.Controls.Clear();
            foreach (ThumbnailData data in images)
            {
                Thumbnail thumbnail = new Thumbnail();
                thumbnail.Initialize(data.Name, data.Image, this.thumbnailCoef);
                thumbnail.ThumbnailClicked += new CbGeneric<string>(thumbnail_ThumbnailClicked);
                this.flowLayoutPanel1.Controls.Add(thumbnail);
            }
        }

        void thumbnail_ThumbnailClicked(string imageName)
        {
            int index = -1;
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                Thumbnail thumbnail = (Thumbnail)this.flowLayoutPanel1.Controls[i];
                if (thumbnail.ImageName == imageName)
                {
                    thumbnail.Selected = true;
                    index = i;
                }
                else
                {
                    thumbnail.Selected = false;
                }
            }

            this.ThumbnailClicked(index);
        }
        #endregion

        #region RemoveThumbnail
        public void RemoveThumbnail(int index)
        {
            this.flowLayoutPanel1.Controls.RemoveAt(index);
        }         

        public void RemoveThumbnail(string imageName)
        {
            Thumbnail target = null;
            foreach (Thumbnail obj in this.flowLayoutPanel1.Controls)
            {
                if (obj.ImageName == imageName)
                {
                    target = obj;
                    break;
                }
            }

            if (target != null)
            {
                this.flowLayoutPanel1.Controls.Remove(target);
            }
        }
        #endregion

        #region SelectThumbnail
        public void SelectThumbnail(int index)
        {           
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                Thumbnail thumbnail = (Thumbnail)this.flowLayoutPanel1.Controls[i];
                if (i == index)
                {
                    thumbnail.Selected = true;                    
                }
                else
                {
                    thumbnail.Selected = false;
                }
            }            
        }

        public void SelectThumbnail(string imageName)
        {            
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                Thumbnail thumbnail = (Thumbnail)this.flowLayoutPanel1.Controls[i];
                if (thumbnail.ImageName == imageName)
                {
                    thumbnail.Selected = true;
                }
                else
                {
                    thumbnail.Selected = false;
                }
            }
        } 
        #endregion

        #region AppendThumbnail
        public void AppendThumbnail(ThumbnailData data)
        {
            Thumbnail thumbnail = new Thumbnail();
            thumbnail.Initialize(data.Name, data.Image, this.thumbnailCoef);
            thumbnail.ThumbnailClicked += new CbGeneric<string>(thumbnail_ThumbnailClicked);
            this.flowLayoutPanel1.Controls.Add(thumbnail);
        } 
        #endregion

        #region UpdateThumbnail
        public void UpdateThumbnail(int index, Image image)
        {
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {                
                if (i == index)
                {
                    Thumbnail thumbnail = (Thumbnail)this.flowLayoutPanel1.Controls[i];
                    thumbnail.UpdateImage(image, this.thumbnailCoef);
                    break;
                }
            }
        }

        public void UpdateThumbnail(string imageName, Image image)
        {
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                Thumbnail thumbnail = (Thumbnail)this.flowLayoutPanel1.Controls[i];
                if (thumbnail.ImageName == imageName)
                {
                    thumbnail.UpdateImage(image, this.thumbnailCoef);
                    break;
                }               
            }
        } 
        #endregion        
    }

    public class ThumbnailData
    {
        #region Ctor
        public ThumbnailData() { }
        public ThumbnailData(string _name, Image _image)
        {
            this.name = _name;
            this.image = _image;
        } 
        #endregion

        #region Image
        private Image image;
        public Image Image
        {
            get { return image; }
            set { image = value; }
        }        
        #endregion

        #region Name
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        } 
        #endregion
    }
}
