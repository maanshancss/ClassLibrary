using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Helpers
{
    public  static class ValidationHelper
    {
        #region NotDefault
        public static void NotDefault<T>(T t, string name)
        {
            if (t == null)
            {
                throw new ArgumentNullException(String.Format("����{0}����Ϊnull", name), name);
            }

            if (t is ValueType)
            {
                throw new ArgumentException(String.Format("����{0}����ʹ��Ĭ��ֵ", name), name);
            }
        }

        public static void NotDefault<T>(T t)
        {
            if (t == null)
            {
                throw new ArgumentNullException("��������Ϊnull");
            }

            if (t is ValueType)
            {
                throw new ArgumentException("����{0}����ʹ��Ĭ��ֵ");
            }
        } 
        #endregion
          
    }
}
