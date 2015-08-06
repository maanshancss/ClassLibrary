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
                throw new ArgumentNullException(String.Format("参数{0}不能为null", name), name);
            }

            if (t is ValueType)
            {
                throw new ArgumentException(String.Format("参数{0}不能使用默认值", name), name);
            }
        }

        public static void NotDefault<T>(T t)
        {
            if (t == null)
            {
                throw new ArgumentNullException("参数不能为null");
            }

            if (t is ValueType)
            {
                throw new ArgumentException("参数{0}不能使用默认值");
            }
        } 
        #endregion
          
    }
}
