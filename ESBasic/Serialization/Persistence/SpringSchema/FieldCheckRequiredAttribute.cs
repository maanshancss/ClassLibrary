using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Serialization
{
    /// <summary>
    /// FieldNotNullAttribute 修饰那些不允许为null的Filed，可以在运行时通过FieldChecker对这些Field进行检查。
    /// 只对引用类型的Field有效。
    /// zhuweisky 2007.03.13
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class FieldNotNullAttribute :Attribute
    {
    }

    /// <summary>
    /// FieldChecker 对FieldNotNull标签进行检查
    /// </summary>
    public static class FieldChecker
    {
        /// <summary>
        /// CheckFiledNotNull 检查targets中所有class标志[FieldNotNullAttribute]的对象的标志为[FieldNotNullAttribute]的成员的值不为null。
        /// </summary>       
        /// <returns>Key：object ；Vlaue：值为null的Filed的列表</returns>
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
