using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic
{
    /// <summary>
    /// Demisemiquaver 三十二进制。字符编码中不包括字符"0"/"I"/"O"/"U"
    /// </summary>
    public static class Demisemiquaver
    {
        private static IDictionary<long, string> CodeDictionary = new Dictionary<long, string>();

        #region Static Ctor
        static Demisemiquaver()
        {
            int increase = 0;
            int codeOfA = (int)'A';
            for (int i = 0; i < 32; i++)
            {
                if (i == 0)
                {
                    Demisemiquaver.CodeDictionary.Add(i, "Z");
                }
                else if (i < 10)
                {
                    Demisemiquaver.CodeDictionary.Add(i, i.ToString());
                }
                else
                {
                    int delt = i - 10;
                    char ch = (char)(codeOfA + delt);

                    char temp = (char)(ch + increase);

                    if (temp == 'I' || temp == 'O' || temp == 'U')
                    {
                        ++increase;
                    }

                    temp = (char)(ch + increase);
                    Demisemiquaver.CodeDictionary.Add(i, temp.ToString());
                }
            }
        }
        #endregion

        #region Convert
        /// <summary>
        /// Convert 将一个数转换为32进制的字符串。
        /// </summary>        
        public static string Convert(long num)
        {
            List<string> result = new List<string>();
            StringBuilder sb = new StringBuilder("");

            while (num > 0)
            {
                sb.Insert(0, Demisemiquaver.CodeDictionary[num % 32]);
                num = num >> 5;
            }

            return sb.ToString();
        } 
        #endregion
    }
}
