using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using ESBasic.Helpers;
using System.IO;

namespace ESBasic.Serialization
{
    /// <summary>
    /// AgileConfiguration 用于文件配置与object之间的映射。
    /// 实际的配置项所在的类型只要继承AgileConfiguration，即可拥有与XML配置文件之间的自动序列化和反序列化的能力。
    /// zhuweisky 2007.02.28
    /// </summary>
    public abstract class AgileConfiguration
    {        
        /// <summary>
        /// Load 将XML配置转换为Object
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
        /// Save 将配置保存到xml文件
        /// </summary>   
        public void Save(string configFilePath)
        {
            string xml = SpringFox.XmlObject(this);
            FileHelper.GenerateFile(configFilePath, xml);
        }
    }
}
  