using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


namespace ESBasic.Helpers
{
    public static class PublicHelper
    {
        #region CompressBitmapToJpg
        /// <summary>
        /// CompressBitmapToJpg 将位图压缩为JPG格式
        /// </summary>       
        public static byte[] CompressBitmapToJpg(System.Drawing.Bitmap bm)
        {
            MemoryStream memStream = new MemoryStream();
            bm.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] bResult = memStream.ToArray();
            memStream.Close();

            return bResult;
        }
        //byte[] bImage = this.CompressBitmapToJpg(bm) ;
        //this.pictureBox1.Image = Image.FromStream(new MemoryStream(bImage)) ;

        public static Image CompressBitmapToJpg2(System.Drawing.Bitmap bm)
        {
            MemoryStream memStream = new MemoryStream();
            bm.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            Image img = Image.FromStream(memStream);
            memStream.Close();

            return img;
        }
        #endregion

        #region CopyData
        /// <summary>
        /// CopyData 拷贝二进制数据
        /// </summary>      
        public static void CopyData(byte[] source, byte[] dest, int destOffset)
        {
            Buffer.BlockCopy(source, 0, dest, destOffset, source.Length);           
        }
        #endregion

        #region GetSizeString
        public static string GetSizeString(ulong size)
        {
            return GetSizeString(size, 2);
        }
        /// <summary>
        /// GetSizeString 将文件大小表示为简洁的形式。
        /// </summary>        
        public static string GetSizeString(ulong size, byte numOfLittle)
        {
            string sizeInG = (size / (1024.0 * 1024.0 * 1024.0)).ToString();
            if (double.Parse(sizeInG) > 1)
            {
                int pos = sizeInG.IndexOf('.');
                if (pos < 0)
                {
                    return sizeInG + "G";
                }
                else
                {
                    int len = pos + numOfLittle + 1;
                    if (numOfLittle == 0)
                    {
                        len = pos;
                    }
                    if (sizeInG.Length < len)
                    {
                        len = sizeInG.Length;
                    }
                    return sizeInG.Substring(0, len) + "G";
                }
            }

            string sizeInM = (size / (1024.0 * 1024.0)).ToString();
            if (double.Parse(sizeInM) > 1)
            {
                int pos = sizeInM.IndexOf('.');
                if (pos < 0)
                {
                    return sizeInM + "M";
                }
                else
                {
                    int len = pos + numOfLittle + 1;
                    if (numOfLittle == 0)
                    {
                        len = pos;
                    }
                    if (sizeInM.Length < len)
                    {
                        len = sizeInM.Length;
                    }
                    return sizeInM.Substring(0, len) + "M";
                }
            }
            else
            {
                string sizeInK = (size / (1024.0)).ToString();
                int pos = sizeInK.IndexOf('.');

                if (pos < 0)
                {
                    return sizeInK + "K";
                }
                else
                {
                    int len = pos + numOfLittle + 1;
                    if (numOfLittle == 0)
                    {
                        len = pos;
                    }
                    if (sizeInK.Length < len)
                    {
                        len = sizeInK.Length;
                    }
                    return sizeInK.Substring(0, len) + "K";
                }
            }
        } 

        #endregion
    }
}
