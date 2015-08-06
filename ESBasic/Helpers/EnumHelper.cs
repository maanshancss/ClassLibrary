using System;
using System.Reflection;
using System.Collections.Generic;

namespace ESBasic.Helpers
{
	/// <summary>
	/// EnumHelper ��ժҪ˵����
	/// </summary>
	public static class EnumHelper
    {
        #region ConvertEnumToList
        /// <summary>
        /// ConvertEnumToFieldDescriptionList ��Enum������ö��ֵ�ŵ�IList�У��԰󶨵���ComoboBox�ȿؼ�
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
        /// ConvertEnumToFieldTextList ��ȡEnum������Field���ı���ʾ��
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
        /// ParseEnumValue ��ConvertEnumToList���ʹ�ã���ComoboBox�ȿؼ���ѡ�е�stringת��Ϊö��ֵ
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
