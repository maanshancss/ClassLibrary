using System;
using System.IO ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;

namespace ESBasic.Helpers
{
	/// <summary>
	/// SerializeHelper ���ڼ����л��ͷ����л����� ��    
	/// ���ߣ���ΰ sky.zhuwei@163.com 
	/// 2004.05.12
	/// </summary>
	public static class SerializeHelper
	{
        #region BinaryFormatter
        #region SerializeObject
        public static byte[] SerializeObject(object obj) //obj ����������
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();//���������,mem_stream�Ļ�������С�ǿɱ��

            formatter.Serialize(memoryStream, obj);

            byte[] buff = memoryStream.ToArray();
            memoryStream.Close();

            return buff;
        }

        public static void SerializeObject(object obj, ref byte[] buff, int offset) //obj ����������
        {
            byte[] rude_buff = SerializeHelper.SerializeObject(obj);
            for (int i = 0; i < rude_buff.Length; i++)
            {
                buff[offset + i] = rude_buff[i];
            }
        }
        #endregion

        #region DeserializeBytes
        public static object DeserializeBytes(byte[] buff, int index, int count)
        {
            IFormatter formatter = new BinaryFormatter();

            MemoryStream stream = new MemoryStream(buff, index, count);
            object obj = formatter.Deserialize(stream);
            stream.Close();

            return obj;
        }
        #endregion 
        #endregion

        #region SoapFormatter
        #region SerializeObjectToString
        /// <summary>
        /// SerializeObjectToString ���������л�ΪSOAP XML ��ʽ��
        /// ���Ҫ������ת��Ϊ����xml��ʽ����ʹ��ESBasic.Persistence.SimpleXmlConverter�ࡣ
        /// </summary>        
        public static string SerializeObjectToString(object obj)
        {
            IFormatter formatter = new SoapFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, obj);
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            string res = reader.ReadToEnd();
            stream.Close();

            return res;
        }
        #endregion

        #region DeserializeString
        public static object DeserializeString(string str)
        {
            byte[] buff = System.Text.Encoding.Default.GetBytes(str);
            IFormatter formatter = new SoapFormatter();
            MemoryStream stream = new MemoryStream(buff, 0, buff.Length);
            object obj = formatter.Deserialize(stream);
            stream.Close();

            return obj;
        }
        #endregion		 
        #endregion

        #region XmlSerializer
        #region XmlObject
        public static string XmlObject(object obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            xmlSerializer.Serialize(stream, obj);
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            string res = reader.ReadToEnd();
            stream.Close();

            return res;
        }
        #endregion

        #region ObjectXml
        public static T ObjectXml<T>(string str)
        {
            return (T)SerializeHelper.ObjectXml(str, typeof(T));
        }

        public static object ObjectXml(string str, Type targetType)
        {
            byte[] buff = System.Text.Encoding.Default.GetBytes(str);
            XmlSerializer xmlSerializer = new XmlSerializer(targetType);
            MemoryStream stream = new MemoryStream(buff, 0, buff.Length);
            object obj = xmlSerializer.Deserialize(stream);
            stream.Close();

            return obj;
        }
        #endregion		 
        #endregion

        #region SaveToFile
        /// <summary>
        /// SaveToFile ������Ķ��������л��󱣴浽�ļ���
        /// </summary>       
        public static void SaveToFile(object obj, string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate);            
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);

            stream.Flush();
            stream.Close();           
        } 
        #endregion

        #region ReadFromFile
        /// <summary>
        /// ReadFromFile ���ļ���ȡ�����Ʒ����л�Ϊ����
        /// </summary> 
        public static object ReadFromFile(string filePath)
        {
            byte[] buff = FileHelper.ReadFileReturnBytes(filePath);
            return SerializeHelper.DeserializeBytes(buff, 0, buff.Length);
        } 
        #endregion
	}	
}
