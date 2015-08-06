using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using ESBasic.Helpers;

namespace ESBasic.Serialization
{
    /// <summary>
    /// SimpleXmlConverter 将对象object转换为XML。支持N层对象嵌入，并且支持IList<>接口。
    /// </summary>
    public static class SimpleXmlConverter
    {
        #region XmlSerializeObject
        public static string XmlSerializeObject(object target)
        {
            string typeName = TypeHelper.GetClassSimpleName(target.GetType());
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement objNode = xmlDoc.CreateElement(typeName);

            foreach (PropertyInfo pro in target.GetType().GetProperties())
            {
                if ((!pro.CanRead))
                {
                    continue;
                }

                SimpleXmlConverter.DoXmlSerializeObject(objNode, pro, pro.GetValue(target, null));
            }

            return objNode.OuterXml;
        }

        #region DoXmlSerializeObject
        private static void DoXmlSerializeObject(XmlNode curNode, PropertyInfo pro, object val)
        {
            bool isGenericList = pro.PropertyType.IsGenericType && (pro.PropertyType.GetGenericTypeDefinition() == typeof(IList<>));
            if (isGenericList)
            {
                string listName = pro.Name;
                XmlNode listNode = curNode.OwnerDocument.CreateElement(listName);
                curNode.AppendChild(listNode);

                Type listElementType = pro.PropertyType.GetGenericArguments()[0];
                if (TypeHelper.IsSimpleType(listElementType))
                {
                    foreach (object element in (IEnumerable)val)
                    {
                        XmlHelper.SetPropertyValue(listNode, "value", element.ToString(), XmlPropertyPosition.ChildNode, false);
                    }
                }
                else
                {
                    foreach (object element in (IEnumerable)val)
                    {
                        string newObjName = TypeHelper.GetClassSimpleName(listElementType);
                        XmlNode newObjNode = curNode.OwnerDocument.CreateElement(newObjName);
                        listNode.AppendChild(newObjNode);

                        PropertyInfo[] childPros = listElementType.GetProperties();
                        foreach (PropertyInfo childPro in childPros)
                        {
                            if ((!childPro.CanRead))
                            {
                                continue;
                            }

                            object childVal = childPro.GetValue(element, null);
                            SimpleXmlConverter.DoXmlSerializeObject(newObjNode, childPro, childVal);
                        }
                    }
                }
            }
            else if (TypeHelper.IsSimpleType(pro.PropertyType))
            {
                string valStr = (val == null ? "" : val.ToString());
                XmlHelper.SetPropertyValue(curNode, pro.Name, valStr);
            }
            else
            {
                string newObjName = TypeHelper.GetClassSimpleName(pro.PropertyType);
                XmlNode newObjNode = curNode.OwnerDocument.CreateElement(newObjName);
                curNode.AppendChild(newObjNode);

                PropertyInfo[] childPros = pro.PropertyType.GetProperties();
                foreach (PropertyInfo childPro in childPros)
                {
                    if ((!childPro.CanRead))
                    {
                        continue;
                    }

                    SimpleXmlConverter.DoXmlSerializeObject(newObjNode, childPro, childPro.GetValue(val, null));
                }
            }

        }
        #endregion
        #endregion
    }
}

#region Example
/*
<SHHistory>
  <GameRoundID>29001</GameRoundID>
  <PlayerCount>3</PlayerCount>
  <His_SHPlayers>cat1_01,ca02,cat2_01</His_SHPlayers>
  <His_CardAction>
    <Cards>N2,F4,L1</Cards>
    <SeatNO>0</SeatNO>
    <SbAmount>-80,-180,247</SbAmount>    
  </His_CardAction>
  <BookList>
    <Book>
      <Name>ABCCC</Name>
      <Price>29.30</Price>
    </Book>
    <Book>
      <Name>Avfre</Name>
      <Price>45.70</Price>
    </Book>
  </BookList>
</SHHistory>
 */
#endregion
