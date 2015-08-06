using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ESBasic.Security
{
    /// <summary>
    /// ValidationCodeImageCreator ���������������֤��ͼƬ
    /// </summary>
    public static class ValidationCodeImageCreator
    {
        public static Bitmap Generate(int codeLength, out string validationCode)
        {
            validationCode = ValidationCodeImageCreator.GenCode(codeLength);
            return ValidationCodeImageCreator.GenImg(validationCode);
        }

        private static string GenCode(int num)
        {
            //string[] source ={"2","3","4","5","6","7","8","9","2","3","4","5","6","7","8","9","2","3","4","5","6","7","8","9",
            //                  "A","B","C","D","E","F","G","H","J","K","L","M","N","P","Q","R","S","T","U","V","W","X","Y","Z"};

            string[] source ={"1", "2", "3", "4", "5", "6", "7", "8", "9" };
                             
            string code = "";
            Random rd = new Random();
            for (int i = 0; i < num; i++)
            {
                code += source[rd.Next(0, source.Length)];
            }
            return code;
        }

        private static Bitmap GenImg(string code)
        {
            return ValidationCodeImageCreator.GenImg(code, Color.DimGray, Color.White);//Color.DimGray ,Gainsboro
        }

        private static Bitmap GenImg(string code, Color foreColor, Color backColor)
        {
            return ValidationCodeImageCreator.GenImg(code, foreColor, backColor, new Font("Courier New", 18, FontStyle.Bold));
        }

        //����ͼƬ
        private static Bitmap GenImg(string code ,Color foreColor ,Color backColor ,Font font)
        {
            int width = code.Length * 18;

            Bitmap myPalette = new Bitmap(width, 28);//����һ������
            Graphics gh = Graphics.FromImage(myPalette);//�ڻ����϶����ͼ��ʵ��
            gh.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rc = new Rectangle(0, 0, width, 28);//����һ������

            gh.FillRectangle(new SolidBrush(backColor), rc);//������             
            gh.DrawString(code,  font, new SolidBrush(foreColor), rc);//�ھ����ڻ����ַ���         
            gh.Dispose();

            return myPalette;
        }
    }
}
