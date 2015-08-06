using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using ESBasic.Helpers;
using System.IO;

namespace ESBasic.Serialization
{
    /// <summary>
    /// AgileConfiguration �����ļ�������object֮���ӳ�䡣
    /// ʵ�ʵ����������ڵ�����ֻҪ�̳�AgileConfiguration������ӵ����XML�����ļ�֮����Զ����л��ͷ����л���������
    /// zhuweisky 2007.02.28
    /// </summary>
    public abstract class AgileConfiguration
    {        
        /// <summary>
        /// Load ��XML����ת��ΪObject
        /// </summary>        
        public static AgileConfiguration Load(string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                return null;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(configFilePath);

            return (AgileConfiguration)SpringFox.ObjectXml(doc.ChildNodes[0].OuterXml);
        }

        /// <summary>
        /// Save �����ñ��浽xml�ļ�
        /// </summary>   
        public void Save(string configFilePath)
        {
            string xml = SpringFox.XmlObject(this);
            FileHelper.GenerateFile(configFilePath, xml);
        }
    }
}
  