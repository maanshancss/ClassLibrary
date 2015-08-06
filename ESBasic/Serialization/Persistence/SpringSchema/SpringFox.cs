using System;
using System.Reflection;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ESBasic.Helpers;

namespace ESBasic.Serialization
{
    /// <summary>
    /// SpringFox 用于将object 与 xml字符串相互转换。XML大纲遵循Spring的object配置大纲。
    /// (1)只转换object的简单类型的属性。
    /// (2)除了支持IList<>，将忽略其它集合和数组类型的属性。
    /// (3)可以转换内嵌的自定义简单类型，但不支持循环对象引用。
    /// 用途之一：SpringFox可以在配置文件Xml与配置对象Object之间进行双向映射，再结合PropertyGrid控件可以很方便的实现自动化配置管理
    /// zhuweisky 2007.02.27
    /// </summary>
    public static class SpringFox
    {        
        #region XmlObject 
        /// <summary>
        /// XmlObject 将object序列化为xml字符串
        /// </summary>   
        public static string XmlObject(object obj)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement objNode = xmlDoc.CreateElement("object");
            SpringFox.SetPropertyValue(objNode, "type", TypeHelper.GetTypeRegularName(obj.GetType()), XmlPropertyPosition.Attribute);
            SpringFox.DoXmlObject(obj, xmlDoc, objNode );

            return objNode.OuterXml;
        }

        #region DoXmlObject
        /// <summary>
        /// DoXmlObject 根据obj的各个属性创建对应的Node作为curNode的子节点
        /// </summary>      
        private static void DoXmlObject(object obj, XmlDocument xmlDoc, XmlNode curNode)
        {
            try
            {
                if (TypeHelper.IsSimpleType(obj.GetType()))
                {
                    return;
                }

                #region logic
                foreach (PropertyInfo pro in obj.GetType().GetProperties())
                {
                    if ((!pro.CanRead))
                    {
                        continue;
                    }

                    object[] nonXmlAttrs = pro.GetCustomAttributes(typeof(NonXmlAttribute), true);
                    if ((nonXmlAttrs != null) && (nonXmlAttrs.Length > 0))
                    {
                        continue;
                    }

                    object proVal = pro.GetValue(obj, null);
                    if (proVal == null)
                    {
                        continue;
                    }

                    if (TypeHelper.IsSimpleType(pro.PropertyType))
                    {
                        SpringFox.SetPropertyValue(curNode, pro.Name, proVal.ToString(), XmlPropertyPosition.ChildNode);
                    }
                    else
                    {
                        bool isGenericList = pro.PropertyType.IsGenericType && (pro.PropertyType.GetGenericTypeDefinition() == typeof(IList<>));

                        if (isGenericList)
                        {
                            #region IList<>
                            XmlElement proNode = xmlDoc.CreateElement("property");
                            SpringFox.SetPropertyValue(proNode, "name", pro.Name, XmlPropertyPosition.Attribute);
                            curNode.AppendChild(proNode);

                            Type listElementType = pro.PropertyType.GetGenericArguments()[0];

                            XmlElement listNode = xmlDoc.CreateElement("list");
                            SpringFox.SetPropertyValue(listNode, "element-type", TypeHelper.GetTypeRegularName(listElementType), XmlPropertyPosition.Attribute);
                            proNode.AppendChild(listNode);

                            if (TypeHelper.IsSimpleType(listElementType))
                            {
                                foreach (object element in (IEnumerable)proVal)
                                {
                                    XmlHelper.SetPropertyValue(listNode, "value", element.ToString(), XmlPropertyPosition.ChildNode, false);
                                }
                            }
                            else
                            {
                                foreach (object element in (IEnumerable)proVal)
                                {
                                    XmlElement elementNode = xmlDoc.CreateElement("object");
                                    if (listElementType != element.GetType())
                                    {
                                        XmlHelper.SetPropertyValue(elementNode, "type", TypeHelper.GetTypeRegularName(element.GetType()), XmlPropertyPosition.Attribute);
                                    }
                                    listNode.AppendChild(elementNode);
                                    SpringFox.DoXmlObject(element, xmlDoc, elementNode);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region 非集合、非数组
                            bool isCollection = typeof(IEnumerable).IsAssignableFrom(pro.PropertyType);
                            bool isAry = pro.PropertyType.IsSubclassOf(typeof(Array));

                            if ((!isCollection) && (!isAry))
                            {
                                XmlElement proNode = xmlDoc.CreateElement("property");
                                SpringFox.SetPropertyValue(proNode, "name", pro.Name, XmlPropertyPosition.Attribute);
                                curNode.AppendChild(proNode);

                                XmlElement subProNode = xmlDoc.CreateElement("object");
                                SpringFox.SetPropertyValue(subProNode, "type", TypeHelper.GetTypeRegularName(proVal.GetType()), XmlPropertyPosition.Attribute);
                                proNode.AppendChild(subProNode);

                                SpringFox.DoXmlObject(proVal, xmlDoc, subProNode);
                            }
                            #endregion
                        }
                    }

                }
                #endregion
            }
            catch (Exception ee)
            {
                string msg = string.Format("XmlObject {0} Error !", obj.GetType().ToString());
                throw new Exception(msg, ee);
            }
        }
        #endregion
        #endregion

        #region ObjectXml
        /// <summary>
        /// ObjectXml 将xml字符串转换为object
        /// </summary>       
        public static object ObjectXml(string xml)
        {            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode objNode = xmlDoc.ChildNodes[0];
            Type objType = ReflectionHelper.GetType(XmlHelper.GetPropertyValue(objNode, "type"));
            object obj = Activator.CreateInstance(objType);

            SpringFox.ConfigObject(objNode, ref obj);

            return obj;
        } 
        #endregion

        #region private
        #region SetPropertyValue
        private static void SetPropertyValue(XmlNode objNode, string proName, string val, XmlPropertyPosition pos)
        {
            if (pos == XmlPropertyPosition.ChildNode)
            {
                foreach (XmlNode childNode in objNode.ChildNodes)
                {
                    if (childNode.Attributes["name"].Value == proName)
                    {
                        childNode.Attributes["value"].Value = val;
                        return;
                    }
                }

                XmlNode newChildNode = objNode.OwnerDocument.CreateElement("property");
                XmlAttribute proNameAttr = objNode.OwnerDocument.CreateAttribute("name");
                proNameAttr.Value = proName;
                newChildNode.Attributes.Append(proNameAttr);

                XmlAttribute proValueAttr = objNode.OwnerDocument.CreateAttribute("value");
                proValueAttr.Value = val;
                newChildNode.Attributes.Append(proValueAttr);

                objNode.AppendChild(newChildNode);
            }
            else
            {
                foreach (XmlAttribute attr in objNode.Attributes)
                {
                    if (attr.Name == proName)
                    {
                        attr.Value = val;
                        return;
                    }
                }

                XmlAttribute newAttr = objNode.OwnerDocument.CreateAttribute(proName);
                objNode.Attributes.Append(newAttr);
                newAttr.Value = val;
            }
        }
        #endregion

        #region ConfigObject
        /// <summary>
        /// ConfigObject 使用objNode的各个子节点配置target的对应的属性
        /// </summary>        
        private static void ConfigObject(XmlNode objNode, ref object target)
        {
            foreach (XmlAttribute attr in objNode.Attributes)
            {
                ReflectionHelper.SetProperty(target, attr.Name, attr.Value);
            }

            foreach (XmlNode childNode in objNode.ChildNodes)
            {
                if (childNode.Attributes["value"] != null)
                {
                    ReflectionHelper.SetProperty(target, childNode.Attributes["name"].Value, childNode.Attributes["value"].Value);
                }
                else
                {
                    XmlNode childProNode = childNode.ChildNodes[0];
                    if (childProNode.Name == "object")
                    {
                        Type proType = ReflectionHelper.GetType(childProNode.Attributes["type"].Value);
                        object proObj = Activator.CreateInstance(proType);                        
                        SpringFox.ConfigObject(childProNode, ref proObj);
                        ReflectionHelper.SetProperty(target, childNode.Attributes["name"].Value, proObj);
                    }
                    else if (childProNode.Name == "list")
                    {                       
                        Type listElementType = ReflectionHelper.GetType(childProNode.Attributes["element-type"].Value);
                        Type closedGenericListType = typeof(List<>).MakeGenericType(listElementType);
                        object list = Activator.CreateInstance(closedGenericListType);
                        //ISimpleList simpleList = (ISimpleList)SpringFox.DynamicProxyCreator.CreateDynamicProxy<ISimpleList>(list);

                        #region Add object into list
                        if (TypeHelper.IsSimpleType(listElementType))
                        {
                            foreach (XmlNode elementNode in childProNode.ChildNodes)
                            {
                                object element = TypeHelper.ChangeType(listElementType, elementNode.InnerText);
                                closedGenericListType.GetMethod("Add").Invoke(list, new object[] { element });
                                //simpleList.Add(element);
                            }
                        }
                        else
                        {
                            foreach (XmlNode elementNode in childProNode.ChildNodes)
                            {
                                Type curElementType = listElementType;
                                if (elementNode.Attributes["type"] != null)
                                {
                                    curElementType = ReflectionHelper.GetType(elementNode.Attributes["type"].Value);
                                }
                                object element = Activator.CreateInstance(curElementType);
                                SpringFox.ConfigObject(elementNode, ref element);
                                closedGenericListType.GetMethod("Add").Invoke(list, new object[] { element });
                                //simpleList.Add(element);
                            }
                        }
                        
                        #endregion

                        ReflectionHelper.SetProperty(target, childNode.Attributes["name"].Value, list);
                    }
                }
            }
        }
        #endregion 
        #endregion
    }

    /// <summary>
    /// NotXmlAttribute 如果某个Property标记为NotXmlAttribute，则将不会被序列化到xml中
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]    
    public class NonXmlAttribute : Attribute
    {
    }

    /// <summary>
    /// ISimpleList 用于创建动态代理，将运行时的IList<>接口转换为静态的ISimpleList接口
    /// </summary>
    public interface ISimpleList
    {
        void Add(object element);
    }
}

#region Sample
/*
<object type="XmlFoxTest.Grade,XmlFoxTest">
  <property name="Name" value="GradeOne" />
  <property name="StudentList">
    <list element-type="XmlFoxTest.Student,XmlFoxTest">      
      <object>
        <property name="Name" value="sky" />
        <property name="BookList">
          <list element-type="System.Int32,mscorlib">
            <value>4</value>
            <value>6</value>
          </list>
        </property>
      </object>
    </list>
  </property>
</object>
*/
#endregion
