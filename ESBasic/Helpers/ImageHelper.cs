using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace ESBasic.Helpers
{
    public static class ImageHelper
    {
        private static float[][] ColorMatrix = null;

        static ImageHelper()
        {
            ImageHelper.ColorMatrix = new float[][] {   
                new   float[]   {0.299f,   0.299f,   0.299f,   0,   0},  
                new   float[]   {0.587f,   0.587f,   0.587f,   0,   0},   
                new   float[]   {0.114f,   0.114f,   0.114f,   0,   0},    
                new   float[]   {0,   0,   0,   1,   0},          
                new   float[]   {0,   0,   0,   0,   1}};
        }

        /// <summary>
        /// 将图像转化为灰度图像。
        /// </summary>      
        public static Bitmap ConvertToGrey(Image origin)
        {
            Bitmap newBitmap = new Bitmap(origin);
            Graphics g = Graphics.FromImage(newBitmap);
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cm = new ColorMatrix(ImageHelper.ColorMatrix);
            ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            g.DrawImage(newBitmap, new Rectangle(0, 0, newBitmap.Width, newBitmap.Height), 0, 0, newBitmap.Width, newBitmap.Height, GraphicsUnit.Pixel, ia);
            g.Dispose();

            return newBitmap;
        }

        /// <summary>
        /// 从字节数组中加载图片。
        /// </summary>        
        public static Image Convert(byte[] buff)
        {
            MemoryStream ms = new MemoryStream(buff);
            Image img = System.Drawing.Image.FromStream(ms);
            ms.Close();
            return img;
        }

        public static Image ConvertToJPG(Image img)
        {         
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            Image img2 = System.Drawing.Image.FromStream(ms);
            ms.Close();
            return img2; 
        }

        /// <summary>
        /// 深度复制图片。
        /// </summary>      
        public static Image CopyImageDeeply(Image img)
        {
            Bitmap bmp2 = new Bitmap(img.Width, img.Height, img.PixelFormat);
            Graphics g = Graphics.FromImage(bmp2);
            g.DrawImage(img, 0, 0, img.Width, img.Height);   //不能为 g.DrawImage(img, new Point(0, 0)); 否则因为dpi的问题，可能只绘制部分图像
            g.Dispose();

            return bmp2;
        }

        /// <summary>
        /// 将图像使用JPEG格式存储。
        /// </summary>        
        public static byte[] Convert(Image img)
        {
            //需要将图片先复制一份
            Image bmp2 = CopyImageDeeply(img);        

            MemoryStream ms = new MemoryStream();
            bmp2.Save(ms, ImageFormat.Jpeg);            
            byte[] buff = ms.ToArray();
            ms.Close();

            bmp2.Dispose();//释放bmp文件资源
            return buff;
        }

        public static void Save(Image img, string path ,ImageFormat format)
        {
            if (img == null || path == null)
            {
                return;
            }

            //需要将图片先复制一份
            Image bmp2 = CopyImageDeeply(img); 
            bmp2.Save(path, format);
        }

        public static bool IsGif(Image img)
        {
            Guid[] guids = img.FrameDimensionsList;
            FrameDimension fd = new FrameDimension(guids[0]);
            return img.GetFrameCount(fd) > 1;
        }

        public static Icon ConvertToIcon(Image img ,int iconLength)
        {
            using (Bitmap bm = new Bitmap(img ,new Size(iconLength,iconLength)))
            {
                return Icon.FromHandle(bm.GetHicon());
            }
        }

        public static Bitmap ConstructRGB24Bitmap(byte[] coreData, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData bitmapData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb); //不包括头的数据
            Marshal.Copy(coreData, 0, bitmapData.Scan0, coreData.Length);//(bitmapData.Scan0, data, 0, data.Length);
            bm.UnlockBits(bitmapData);
            return bm;
        }

        public static byte[] GetRGB24CoreData(Bitmap bm)
        {
            byte[] imageBuff = new byte[bm.Width * bm.Height * 3];
            BitmapData data = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb); //不包括头的数据
            Marshal.Copy(data.Scan0, imageBuff, 0, imageBuff.Length);
            bm.UnlockBits(data);
            return imageBuff;
        }

        /// <summary>
        /// 截取RGB24图像的一部分（从左上角开始）。
        /// </summary>
        /// <param name="origin">原始位图的核心数据</param>
        /// <param name="originSize">位图大小</param>
        /// <param name="newSize">要截取的大小</param>
        /// <returns>被截的部分图像的RGB24数据</returns>
        public static byte[] ReviseRGB24Data(byte[] origin, Size originSize, Size newSize)
        {
            Bitmap oldBm = ConstructRGB24Bitmap(origin, originSize.Width, originSize.Height);

            Bitmap newBitmap = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            g.DrawImage(oldBm, 0, 0, new RectangleF(0, 0, newSize.Width, newSize.Height), GraphicsUnit.Pixel);   //不能为 g.DrawImage(img, new Point(0, 0)); 否则因为dpi的问题，可能只绘制部分图像
            g.Dispose();

            byte[] imageBuff = ImageHelper.GetRGB24CoreData(newBitmap);
            return imageBuff;
        }
    }
}
