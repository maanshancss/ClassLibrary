using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Serialization
{
    /// <summary>
    /// FieldNotNullAttribute ������Щ������Ϊnull��Filed������������ʱͨ��FieldChecker����ЩField���м�顣
    /// ֻ���������͵�Field��Ч��
    /// zhuweisky 2007.03.13
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class FieldNotNullAttribute :Attribute
    {
    }

    /// <summary>
    /// FieldChecker ��FieldNotNull��ǩ���м��
    /// </summary>
    public static class FieldChecker
    {
        /// <summary>
        /// CheckFiledNotNull ���targets������class��־[FieldNotNullAttribute]�Ķ���ı�־Ϊ[FieldNotNullAttribute]�ĳ�Ա��ֵ��Ϊnull��
        /// </summary>       
        /// <returns>Key��object ��Vlaue��ֵΪnull��Filed���б�</returns>
        public static IDictionary<object, IList<string>> CheckFiledNotNull(IList<object> targets)
        {
            IDictionary<object, IList<string>> dic = new Dictionary<object, IList<string>>();

            foreach (object target in targets)
            {
                Type targetType = target.GetType() ;
                if (targetType.GetCustomAttributes(typeof(FieldNotNullAttribute), false).Length < 1)
                {
                    continue;
                }

                foreach (FieldInfo field in targetType.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (field.GetCustomAttributes(typeof(FieldNotNullAttribute), false).Length < 1)
                    {
                        continue;
                    }                    

                    if ((! field.FieldType.IsValueType) &&(field.GetValue(target) == null))
                    {
                        if (! dic.ContainsKey(target))
                        {
                            dic.Add(target, new List<string>());
                        }

                        IList<string> fieldList = dic[target];
                        fieldList.Add(field.Name);
                    }
                }
            }

            return dic;
        }
    }
}
