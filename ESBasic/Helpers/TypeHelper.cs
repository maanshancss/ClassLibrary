using System;
using System.Collections.Generic;
using System.Text;

namespace ESBasic.Helpers
{
    public static class TypeHelper
    {
        #region IsSimpleType
        /// <summary>
        /// IsSimpleType �Ƿ�Ϊ�����ͣ���ֵ���ַ����ַ��������ڡ�������ö�١�Type
        /// </summary>      
        public static bool IsSimpleType(Type t)
        {
            if (TypeHelper.IsNumbericType(t))
            {
                return true;
            }

            if (t == typeof(char))
            {
                return true;
            }            

            if (t == typeof(string))
            {
                return true;
            }
            

            if (t == typeof(bool))
            {
                return true;
            }
            

            if (t == typeof(DateTime))
            {
                return true;
            }
            
            if (t == typeof(Type))
            {
                return true;
            }           

            if (t.IsEnum)
            {
                return true;
            }

            return false;
        } 
        #endregion

        #region IsNumbericType �Ƿ�Ϊ��ֵ����
        public static bool IsNumbericType(Type destDataType)
        {
            if ((destDataType == typeof(int)) || (destDataType == typeof(uint)) || (destDataType == typeof(double))
                || (destDataType == typeof(short)) || (destDataType == typeof(ushort)) || (destDataType == typeof(decimal))
                || (destDataType == typeof(long)) || (destDataType == typeof(ulong)) || (destDataType == typeof(float))
                || (destDataType == typeof(byte)) || (destDataType == typeof(sbyte)))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region IsIntegerCompatibleType �Ƿ�Ϊ������������
        public static bool IsIntegerCompatibleType(Type destDataType)
        {
            if ((destDataType == typeof(int)) || (destDataType == typeof(uint)) || (destDataType == typeof(short)) || (destDataType == typeof(ushort)) 
                || (destDataType == typeof(long)) || (destDataType == typeof(ulong)) || (destDataType == typeof(byte)) || (destDataType == typeof(sbyte)))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region GetClassSimpleName
        /// <summary>
        /// GetClassSimpleName ��ȡclass���������ƣ��� Person
        /// </summary>      
        public static string GetClassSimpleName(Type t)
        {
            string[] parts = t.ToString().Split('.');
            return parts[parts.Length - 1].ToString();
        } 
        #endregion

        #region IsFixLength
        public static bool IsFixLength(Type destDataType)
        {
            if (TypeHelper.IsNumbericType(destDataType))
            {
                return true;
            }

            if (destDataType == typeof(byte[]))
            {
                return true;
            }

            if ((destDataType == typeof(DateTime)) || (destDataType == typeof(bool)))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region ChangeType
        /// <summary>
        /// ChangeType ��System.Convert.ChangeType��������ǿ��֧��(0,1)��bool��ת�����ַ���->ö�١�int->ö�١��ַ���->Type
        /// </summary>       
        public static object ChangeType(Type targetType, object val)
        {
            #region null
            if (val == null)
            {
                return null;
            } 
            #endregion

            if (targetType.IsAssignableFrom(val.GetType()))
            {
                return val;
            }

            #region Same Type
            if (targetType == val.GetType())
            {
                return val;
            } 
            #endregion

            #region bool 1,0
            if (targetType == typeof(bool))
            {
                if (val.ToString() == "0")
                {
                    return false;
                }

                if (val.ToString() == "1")
                {
                    return true;
                }               
            } 
            #endregion           

            #region Enum
            if (targetType.IsEnum)
            {
                int intVal = 0;
                bool suc = int.TryParse(val.ToString() ,out intVal);
                if (!suc)
                {
                    return Enum.Parse(targetType, val.ToString());
                }
                else
                {
                    return val;
                }
            }
            #endregion

            #region Type
            if (targetType == typeof(Type))
            {
                return ReflectionHelper.GetType(val.ToString());
            }
            #endregion          

            if (targetType == typeof(IComparable))
            {
                return val;
            }

            //��double��ֵ����ֵ�͵�DataRow���ֶ��ǿ��Եģ�����ͨ�����丳ֵ��object�ķ�double��������ֵ���͵����ԣ�ȴ����        
            return System.Convert.ChangeType(val, targetType);          
            
        }            
        #endregion

        #region GetDefaultValue
        public static object GetDefaultValue(Type destType)
        {
            if (TypeHelper.IsNumbericType(destType))
            {
                return 0;
            }

            if (destType == typeof(string))
            {
                return "";
            }

            if (destType == typeof(bool))
            {
                return false;
            }

            if (destType == typeof(DateTime))
            {
                return DateTime.Now;
            }

            if (destType == typeof(Guid))
            {
                return System.Guid.NewGuid();
            }

            if (destType == typeof(TimeSpan))
            {
                return System.TimeSpan.Zero;
            }

            return null;
        } 
        #endregion

        #region GetDefaultValueString
        public static string GetDefaultValueString(Type destType)
        {
            if (TypeHelper.IsNumbericType(destType))
            {
                return "0";
            }

            if (destType == typeof(string))
            {
                return "\"\"";
            }

            if (destType == typeof(bool))
            {
                return "false";
            }

            if (destType == typeof(DateTime))
            {
                return "DateTime.Now";
            }

            if (destType == typeof(Guid))
            {
                return "System.Guid.NewGuid()";
            }

            if (destType == typeof(TimeSpan))
            {
                return "System.TimeSpan.Zero";
            }


            return "null";
        }
        #endregion

        #region GetTypeRegularName
        /// <summary>
        /// GetTypeRegularName ��ȡ���͵���ȫ���ƣ���"ESBasic.Filters.SourceFilter,ESBasic"
        /// </summary>      
        public static string GetTypeRegularName(Type destType)
        {
            string assName = destType.Assembly.FullName.Split(',')[0];

            return string.Format("{0},{1}", destType.ToString(), assName);

        }

        public static string GetTypeRegularNameOf(object obj)
        {
            Type destType = obj.GetType();
            return TypeHelper.GetTypeRegularName(destType);
        } 
        #endregion

        #region GetTypeByRegularName
        /// <summary>
        /// GetTypeByFullString ͨ�����͵���ȫ���ƻ�ȡ���ͣ�regularName��"ESBasic.Filters.SourceFilter,ESBasic"
        /// </summary>       
        public static Type GetTypeByRegularName(string regularName)
        {
            return ReflectionHelper.GetType(regularName);
        } 
        #endregion                   
    }
}
