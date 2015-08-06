using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ESBasic
{
    /// <summary>
    /// ByteConverter 实现基础数据类型与byte[]之间的相互转换。另包括char, bool, DateTime, Enum，不支持string、数组等。另外注意：  
    /// (1)使用一个byte来表示bool。
    /// (2)使用long来表示DateTime。
    /// (3)使用double来表示decimal。
    /// (4)使用int来表示枚举类型。    
    /// </summary>
    public static class ByteConverter
    {
        #region SupportType
        public static bool SupportType(Type destDataType)
        {
            if ((destDataType == typeof(int)) || (destDataType == typeof(uint)) || (destDataType == typeof(double))
              || (destDataType == typeof(short)) || (destDataType == typeof(ushort)) || (destDataType == typeof(decimal))
              || (destDataType == typeof(long)) || (destDataType == typeof(ulong)) || (destDataType == typeof(float))
              || (destDataType == typeof(byte)) || (destDataType == typeof(sbyte)) || destDataType == typeof(char)
              || destDataType == typeof(bool) || destDataType == typeof(DateTime) || destDataType.IsEnum)
            {
                return true;
            }

            return false;
        } 
        #endregion

        #region ToBytes
        public static byte[] ToBytes(Type type, object obj)
        {
            if (type == typeof(int))
            {
                return BitConverter.GetBytes((int)obj);
            }
            if (type == typeof(uint))
            {
                return BitConverter.GetBytes((uint)obj);
            }
            if (type == typeof(double))
            {
                return BitConverter.GetBytes((double)obj);
            }
            if (type == typeof(short))
            {
                return BitConverter.GetBytes((short)obj);
            }
            if (type == typeof(ushort))
            {
                return BitConverter.GetBytes((ushort)obj);
            }
            if (type == typeof(decimal))
            {
                return BitConverter.GetBytes(decimal.ToDouble((decimal)obj));
            }
            if (type == typeof(long))
            {
                return BitConverter.GetBytes((long)obj);
            }
            if (type == typeof(ulong))
            {
                return BitConverter.GetBytes((ulong)obj);
            }
            if (type == typeof(float))
            {
                return BitConverter.GetBytes((float)obj);
            }
            if (type == typeof(byte) || type == typeof(sbyte))
            {
                return new byte[] { (byte)obj };
            }
            if (type == typeof(char))
            {
                return BitConverter.GetBytes((char)obj);
            }
            if (type == typeof(bool))
            {
                byte temp = 0;
                if ((bool)obj)
                {
                    temp = 1;
                }
                return new byte[] { temp };
               
            }
            if (type == typeof(DateTime))
            {
                DateTime dt = (DateTime)obj;
                return BitConverter.GetBytes(dt.ToBinary());
            }
            if (type.IsEnum)
            {
                return BitConverter.GetBytes((int)obj);
            }                    

            throw new Exception(string.Format("Not Support the Type {0} !", type));
        }

        public static byte[] ToBytes<T>(T t) where T : struct
        {
            return ByteConverter.ToBytes(typeof(T), t);
        } 
        #endregion

        #region Parse
        public static T Parse<T>(byte[] buff, ref int offset) where T : struct
        {
            return (T)ByteConverter.Parse(typeof(T), buff, ref offset);
        }

        public static object Parse(Type type, byte[] buff, ref int offset)
        {
            if (type == typeof(int))
            {
                int temp = BitConverter.ToInt32(buff, offset);
                offset += 4;
                return temp;
            }
            if (type == typeof(uint))
            {
                uint temp = BitConverter.ToUInt32(buff, offset);
                offset += 4;
                return temp;
            }
            if (type == typeof(double))
            {
                double temp = BitConverter.ToDouble(buff, offset);
                offset += 8;
                return temp;
            }
            if (type == typeof(short))
            {
                short temp = BitConverter.ToInt16(buff, offset);
                offset += 2;
                return temp;
            }
            if (type == typeof(ushort))
            {
                ushort temp = BitConverter.ToUInt16(buff, offset);
                offset += 2;
                return temp;
            }
            if (type == typeof(decimal))
            {
                decimal temp = (decimal)BitConverter.ToDouble(buff, offset);
                offset += 8;
                return temp;
            }
            if (type == typeof(long))
            {
                long temp = BitConverter.ToInt64(buff, offset);
                offset += 8;
                return temp;
            }
            if (type == typeof(ulong))
            {
                ulong temp = BitConverter.ToUInt64(buff, offset);
                offset += 8;
                return temp;
            }
            if (type == typeof(float))
            {
                float temp = BitConverter.ToSingle(buff, offset);
                offset += 4;
                return temp;
            }
            if (type == typeof(byte))
            {
                byte temp = buff[offset];
                offset += 1;
                return temp;
            }
            if (type == typeof(sbyte))
            {
                sbyte temp = (sbyte)buff[offset];
                offset += 1;
                return temp;
            }
            if (type == typeof(char))
            {
                char temp = BitConverter.ToChar(buff, offset);
                offset += 2;
                return temp;
            }
            if (type == typeof(bool))
            {
                byte temp = buff[offset];
                offset += 1;
                return temp == 1;
            }
            if (type == typeof(DateTime))
            {
                long temp = BitConverter.ToInt64(buff, offset);
                offset += 8;
                return DateTime.FromBinary(temp);
            }
            if (type.IsEnum)
            {
                int temp = BitConverter.ToInt32(buff, offset);
                offset += 4;
                return temp;
            }          

            throw new Exception(string.Format("Not Support the Type {0} !", type));
        } 
        #endregion
    }
}
