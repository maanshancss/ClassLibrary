using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ESBasic.Collections;
using ESBasic.Widget.Internals;
using System.IO;
using System.Runtime.InteropServices;

namespace ESBasic.Widget
{
    /// <summary>
    /// AgileRichTextBox 支持图片和动画的RichTextBox。
    /// </summary>
    public class AgileRichTextBox : RichTextBox
    {
        #region RichEditOle
        private RichEditOle richEditOle;
        private RichEditOle RichEditOle
        {
            get
            {
                if (richEditOle == null)
                {
                    if (base.IsHandleCreated)
                    {
                        richEditOle = new RichEditOle(this);
                    }
                }

                return richEditOle;
            }
        } 
        #endregion     

        #region Initialize
        private IImagePathGetter imagePathGetter = new DefaultImagePathGetter();
        public void Initialize(IImagePathGetter getter)
        {
            this.imagePathGetter = getter;
        } 
        #endregion                         

        #region InsertImage
        /// <summary>
        /// InsertImage 在position位置处，插入标志为imageID图片。
        /// </summary>      
        /// <param name="position">插入的位置</param>
        /// <param name="imageID">图片的标志，必须大于0</param>
        public void InsertImage(uint imageID, int position )
        {
            if (imageID <= 0)
            {
                throw new Exception("imageID must greater than 0."); 
            }
            
            GifBox gif = new GifBox();
            gif.BackColor = base.BackColor;
            gif.Image = Image.FromFile(this.imagePathGetter.GetPath(imageID));
            this.RichEditOle.InsertControl(gif, position, imageID);
        }

        /// <summary>
        /// InsertImage 在position位置处，插入图片。
        /// </summary>   
        /// <param name="image">要插入的图片</param>
        /// <param name="position">插入的位置</param>       
        public void InsertImage(Image image, int position)
        {  
            GifBox gif = new GifBox();
            gif.BackColor = base.BackColor;
            gif.Image = image;
            this.RichEditOle.InsertControl(gif,position , 0);
        }
        #endregion

        #region AppendRtf
        public void AppendRtf(string _rtf)
        {
            base.Select(this.TextLength, 0);
            base.SelectedRtf = _rtf;
            base.Update();
            base.Select(this.Rtf.Length, 0);
            
        } 
        #endregion

        #region GetAllImage
        /// <summary>
        /// GetAllImage 获取Box中每一张由IImagePathGetter管理的图片的位置和标志
        /// </summary>        
        /// <param name="containsForeignObject">内容中是否包含不是由IImagePathGetter管理的图片对象</param>
        /// <returns>key为位置，val为图片的ID</returns>
        public SortedArray<int, uint> GetAllImage(out bool containsForeignObject)
        {
            containsForeignObject = false;
            
            SortedArray<int, uint> array = new SortedArray<int, uint>();
            List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].dwUser != 0)
                {
                    array.Add(list[i].posistion, list[i].dwUser);
                }
                else
                {  
                    containsForeignObject = true;
                }
            }
           
            return array;
        }
        #endregion

        #region AppendRichText
        /// <summary>
        /// AppendRichText 在现有内容后面追加富文本。
        /// </summary>
        /// <param name="textContent">文本内容</param>
        /// <param name="imagePosition_imageID">图片在textContent中的位置以及图片的ID</param>
        public void AppendRichText(string textContent, SortedArray<int, uint> imagePosition_imageID ,Font font ,Color color)
        {
            int count = this.Text.Length; //.TextLength
            if (imagePosition_imageID != null)
            {               
                string temp = textContent;
                List<int> posList = imagePosition_imageID.GetAllKeyList();
                for (int i = posList.Count - 1; i >= 0; i--)
                {
                    temp = temp.Remove(posList[i], 1);
                }
                this.AppendText(temp);

                for (int i = 0; i < posList.Count; i++)
                {
                    int position = posList[i];
                    this.InsertImage(imagePosition_imageID[position], count + position);
                }
            }
            else
            {                
                this.AppendText(textContent);
            }

            this.Select(count, textContent.Length);
            if (color != null)
            {
                this.SelectionColor = color;
            }
            if (font != null)
            {
                this.SelectionFont = font;
            }
        }

        public void AppendRichText(string textContent, SortedArray<int, uint> imagePosition_imageID, Dictionary<int, Image> foreignObjectAry, Font font, Color color)
        {
            int count = this.Text.Length; //.TextLength
            if (imagePosition_imageID != null)
            {
                string pureText = textContent;
                //去掉表情和图片的占位符
                List<int> posList = imagePosition_imageID.GetAllKeyList();
               // List<int> foreignPosList = foreignObjectAry.Keys();

                List<int> tempList = new List<int>();
                tempList.AddRange(posList);
                foreach (int key in foreignObjectAry.Keys)
                {
                    tempList.Add(key);
                }
               
                tempList.Sort();

                for (int i = tempList.Count - 1; i >= 0; i--)
                {
                    pureText = pureText.Remove(tempList[i], 1);
                }
                this.AppendText(pureText);
                //插入表情
                for (int i = 0; i < tempList.Count; i++)
                {
                    int position = tempList[i];
                    if (posList.Contains(position))
                    {
                        this.InsertImage(imagePosition_imageID[position], count + position);
                    }
                    else
                    {
                        this.InsertImage(foreignObjectAry[position], count + position);
                    }
                }
            }
            else
            {
                this.AppendText(textContent);
            }

            this.Select(count, textContent.Length);
            if (color != null)
            {
                this.SelectionColor = color;
            }
            if (font != null)
            {
                this.SelectionFont = font;
            }
        } 
        #endregion       

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    } 

}
