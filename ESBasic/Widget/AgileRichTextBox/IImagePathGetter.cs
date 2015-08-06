using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Widget
{
    /// <summary>
    /// IImagePathGetter 可以根据图片的ID获取图片的路径。为了适应AgileRichTextBox，确保imageID > 0。
    /// </summary>
    public interface IImagePathGetter
    {
        string GetPath(uint imageID);
    }

    public class DefaultImagePathGetter : IImagePathGetter
    {
        private string folderPath = null;
        private string imageExtendName = ".gif";

        #region Ctor
        public DefaultImagePathGetter()
        {
        }

        public DefaultImagePathGetter(string _folderPath, string _imageExtendName)
        {
            this.folderPath = _folderPath;
            this.imageExtendName = _imageExtendName ?? ".gif";
        }
        #endregion

        #region IImagePathComputer 成员

        public string GetPath(uint imageID)
        {
            if (this.folderPath == null || this.folderPath == "")
            {
                return string.Format("{0}{1}", imageID, this.imageExtendName);
            }
            return string.Format("{0}\\{1}{2}", this.folderPath, imageID, this.imageExtendName);
        }

        #endregion
    }
}
