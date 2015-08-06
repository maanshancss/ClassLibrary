using System;
using System.Reflection;
using System.Collections.Generic;

namespace ESBasic.Helpers
{
	/// <summary>
	/// EnumHelper 的摘要说明。
	/// </summary>
	public static class EnumHelper
    {
        #region ConvertEnumToList
        /// <summary>
        /// ConvertEnumToFieldDescriptionList 将Enum的所有枚举值放到IList中，以绑定到如ComoboBox等控件
		/// </summary>		
        public static IList<string> ConvertEnumToFieldDescriptionList(Type enumType)
		{
            IList<string> resultList = new List<string>();

            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo fi in fields)
            {                
                object[] attrs = fi.GetCustomAttributes(typeof(EnumDescription), false);
                if((attrs != null) && (attrs.Length>0))
                {
                    EnumDescription des = (EnumDescription)attrs[0];
                    resultList.Add(des.Description);
                }
                else
                {
                    if (fi.Name != "value__")
                    {
                        resultList.Add(fi.Name);
                    }
                }
            }

			return resultList ;
		}
		#endregion		

        #region ConvertEnumToFieldTextList
        /// <summary>
        /// ConvertEnumToFieldTextList 获取Enum的所有Field的文本表示。
        /// </summary>       
        public static IList<string> ConvertEnumToFieldTextList(Type enumType)
        {
            IList<string> resultList = new List<string>();

            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo fi in fields)
            {
                if (fi.Name != "value__")
                {
                    resultList.Add(fi.Name);
                }
            }

            return resultList;
        } 
        #endregion

        #region ParseEnumValue
        /// <summary>
        /// ParseEnumValue 与ConvertEnumToList结合使用，将ComoboBox等控件中选中的string转换为枚举值
        /// </summary>       
        public static object ParseEnumValue(Type enumType, string filedValOrDesc)
        {
            if ((enumType == null) || (filedValOrDesc == null))
            {
                return null;
            }

            FieldInfo[] fields = enumType.GetFields();
            foreach (FieldInfo fi in fields)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(EnumDescription), false);
                if ((attrs != null) && (attrs.Length > 0))
                {
                    EnumDescription des = (EnumDescription)attrs[0];
                    if (filedValOrDesc == des.Description)
                    {
                        return Enum.Parse(enumType, fi.Name);
                    }
                }                
            }

            return Enum.Parse(enumType, filedValOrDesc);
        }        
        #endregion 
	}
}
