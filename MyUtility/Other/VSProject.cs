

using System;
using System.IO;
using System.Text;
using System.Xml;
namespace MyUtility.Other
{
    public class VSProject
    {
        public void AddClass(string filename, string classname)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            string name = document.DocumentElement.FirstChild.Name;
            if (name != null)
            {
                if (!(name == "CSHARP"))
                {
                    if (!(name == "PropertyGroup"))
                    {
                        return;
                    }
                }
                else
                {
                    this.AddClass2003(filename, classname);
                    return;
                }
                this.AddClass2005(filename, classname);
            }
        }

        public void AddClass2003(string filename, string classname)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlElement element in document.DocumentElement.ChildNodes)
            {
                foreach (XmlElement element2 in element)
                {
                    if (element2.Name == "Files")
                    {
                        foreach (XmlElement element3 in element2)
                        {
                            if (element3.Name == "Include")
                            {
                                XmlElement newChild = document.CreateElement("File", document.DocumentElement.NamespaceURI);
                                newChild.SetAttribute("RelPath", classname);
                                newChild.SetAttribute("SubType", "Code");
                                newChild.SetAttribute("BuildAction", "Compile");
                                element3.AppendChild(newChild);
                                break;
                            }
                        }
                        continue;
                    }
                }
            }
            document.Save(filename);
        }

        public void AddClass2005(string filename, string classname)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlElement element in document.DocumentElement.ChildNodes)
            {
                if (element.Name == "ItemGroup")
                {
                    string innerText = element.ChildNodes[0].InnerText;
                    if (element.ChildNodes[0].Name == "Compile")
                    {
                        XmlElement newChild = document.CreateElement("Compile", document.DocumentElement.NamespaceURI);
                        newChild.SetAttribute("Include", classname);
                        element.AppendChild(newChild);
                        break;
                    }
                }
            }
            document.Save(filename);
        }

        public void AddClass2005Aspx(string filename, string aspxname)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            foreach (XmlElement element in document.DocumentElement.ChildNodes)
            {
                if (element.Name == "ItemGroup")
                {
                    string innerText = element.ChildNodes[0].InnerText;
                    if (element.ChildNodes[0].Name == "Compile")
                    {
                        XmlElement newChild = document.CreateElement("Compile", document.DocumentElement.NamespaceURI);
                        newChild.SetAttribute("Include", aspxname);
                        element.AppendChild(newChild);
                        break;
                    }
                }
            }
            document.Save(filename);
        }

        public void AddMethodToClass(string ClassFile, string strContent)
        {
            if (File.Exists(ClassFile))
            {
                string str = File.ReadAllText(ClassFile, Encoding.Default);
                if (str.IndexOf(" class ") > 0)
                {
                    int num = str.LastIndexOf("}");
                    int num2 = str.Substring(0, num - 1).LastIndexOf("}");
                    string str4 = str.Substring(0, num2 - 1) + "\r\n" + strContent + "\r\n}\r\n}";
                    StreamWriter writer = new StreamWriter(ClassFile, false, Encoding.Default);
                    writer.Write(str4);
                    writer.Flush();
                    writer.Close();
                }
            }
        }
    }
}
