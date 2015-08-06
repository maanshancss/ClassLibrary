using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace ESBasic.Helpers
{
	/// <summary>
    /// XmlHelper 用于简化XML文件和XmlNode的操作，仅仅支持两级节点。可能配合使用的ESBasic.Persistence命名空间下的类。
    /// zhuweisky 2006.06
	/// </summary>
	public static class XmlHelper
	{
		#region GetPropertyValue 
		public static string GetPropertyValue(XmlNode obj, string proName)
		{
			if(obj.Attributes[proName] != null)
			{
				return obj.Attributes[proName].Value ;
			}

         	foreach (XmlNode propNode in obj.ChildNodes)
			{
				if (propNode.Name == proName)
				{
					return propNode.InnerText;
				}
			}

			return null;
		}
		#endregion 

		#region SetPropertyValue
		public static void SetPropertyValue(XmlNode objNode, string proName ,string val )
		{	
			XmlHelper.SetPropertyValue(objNode ,proName ,val ,XmlPropertyPosition.ChildNode) ;
		}

        public static void SetPropertyValue(XmlNode objNode, string proName, string val, XmlPropertyPosition pos)
        {
            XmlHelper.SetPropertyValue(objNode, proName, val, pos, true);
        }

        #region SetPropertyValue
        public static void SetPropertyValue(XmlNode objNode, string proName, string val, XmlPropertyPosition pos, bool overrideExist)
        {
            if (pos == XmlPropertyPosition.ChildNode)
            {
                if (overrideExist)
                {
                    foreach (XmlNode childNode in objNode.ChildNodes)
                    {
                        if (childNode.Name == proName)
                        {
                            childNode.InnerText = val;
                            return;
                        }
                    }
                }

                XmlNode newChildNode = objNode.OwnerDocument.CreateElement(proName);
                newChildNode.InnerText = val;
                objNode.AppendChild(newChildNode);
            }
            else
            {
                if (overrideExist)
                {
                    foreach (XmlAttribute attr in objNode.Attributes)
                    {
                        if (attr.Name == proName)
                        {
                            attr.Value = val;
                            return;
                        }
                    }
                }

                XmlAttribute newAttr = objNode.OwnerDocument.CreateAttribute(proName);
                objNode.Attributes.Append(newAttr);
                newAttr.Value = val;
            }
        } 
        #endregion
		#endregion

        #region FillObjectNode
        /// <summary>
        /// FillObjectNode 使用obj各个属性的名字和值为objNode添加子节点
        /// </summary>        
        public static void FillObjectNode(XmlNode objNode, object obj)
        {
            Type t = obj.GetType();

            foreach (PropertyInfo info in t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default))
            {
                string proValue = "";
                object val = info.GetValue(obj, null);
                if (val != null)
                {
                    proValue = val.ToString();
                }

                XmlHelper.SetPropertyValue(objNode, info.Name, proValue, XmlPropertyPosition.ChildNode);
            }
        } 
        #endregion

        #region ConfigObject
        /// <summary>
        /// ConfigObject 使用XmlNode的Attributes信息和Child Node信息来设置target的同名属性
        /// </summary>       
        public static void ConfigObject(XmlNode objNode, object target)
        {
            foreach (XmlAttribute attr in objNode.Attributes)
            {
                ReflectionHelper.SetProperty(target, attr.Name, attr.Value);
            }

            foreach (XmlNode childNode in objNode.ChildNodes)
            {
                ReflectionHelper.SetProperty(target, childNode.Name, childNode.InnerText);
            }
        }
        #endregion

		#region GetChildNode
        /// <summary>
        /// GetChildNode 获取满足指定名称的第一个Child Node
        /// </summary> 
		public static XmlNode GetChildNode(XmlNode obj, string childNodeName)
		{
			foreach (XmlNode propNode in obj.ChildNodes)
			{
				if (propNode.Name == childNodeName)
				{
					return propNode;
				}
			}

			return null;
		}
		#endregion

		#region GetChildNodes
        /// <summary>
        /// GetChildNodes 获取指定名称的Child Node的列表
        /// </summary>       
		public static IList<XmlNode> GetChildNodes(XmlNode obj, string childNodeName) //List中为XmlNode
		{
			IList<XmlNode> list = new List<XmlNode>() ;
			foreach (XmlNode propNode in obj.ChildNodes)
			{
				if (propNode.Name == childNodeName)
				{
					list.Add(propNode);
				}
			}
            
			return list;
		}
		#endregion		

        #region ParseXmlNodeString ,GetXmlNodeString
        /// <summary>
        /// ParseXmlNodeString 将OutXml字符串解析为XmlNode
        /// </summary>       
        public static XmlNode ParseXmlNodeString(string outXml)
        {
            XmlDocument xmlDoc2 = new XmlDocument();
            xmlDoc2.LoadXml(outXml);
            return xmlDoc2.ChildNodes[0];
        }

        /// <summary>
        /// GetXmlNodeString 获取Node的OuterXml字符串
        /// </summary>       
        public static string GetXmlNodeString(XmlNode node)
        {
            return node.OuterXml;
        } 
        #endregion       
	}

	public enum XmlPropertyPosition
	{
		Attribute ,ChildNode
	}
}
