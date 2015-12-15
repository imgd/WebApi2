using System;
using System.Text;
using System.Xml;
using System.IO;

namespace WebApi2.Demo.Common
{

    /// <summary>
    /// xml操作类
    /// </summary>
    public class XmlHelper
    {
        private string strFile;
        public XmlDocument doc;

        public XmlHelper(string xmlFile)
        {
            strFile = xmlFile;
        }
        //加载Xml
        public bool LoadXmlSuccess()
        {
            if (File.Exists(strFile))
            {
                doc = new XmlDocument();
                doc.Load(strFile);
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// read
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadNoteValue(string key)
        {
            if (LoadXmlSuccess())
            {
                XmlNodeList nodeList = doc.GetElementsByTagName(key);
                if (nodeList.Count == 0)
                    return "NotFound";

                else
                {
                    XmlNode mNode = nodeList[0];
                    return mNode.InnerText;
                }
            }
            else
                return "FileNotFound";
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="nkey"></param>
        /// <param name="mValue"></param>
        public void AddNoteValue(string nKey, string mValue)
        {
            string keyval = ReadNoteValue(nKey);
            if (keyval != "NotFound" || keyval != "FileNotFound")
            {
                XmlNodeList nodeList = doc.GetElementsByTagName("RegRoot");
                XmlNode mNode = nodeList[0];
                XmlElement nElement = doc.CreateElement(nKey);
                nElement.InnerText = mValue;
                mNode.AppendChild(nElement);
                SaveXml();
            }
        }
        /// <summary>
        /// update
        /// </summary>
        /// <param name="nkey"></param>
        /// <param name="mValue"></param>
        public void UpdateNoteValue(string nKey, string mValue)
        {

            string keyval = ReadNoteValue(nKey);
            if (keyval != "NotFound" || keyval != "FileNotFound")
            {
                XmlNodeList nodeList = doc.GetElementsByTagName(nKey);
                XmlNode mNode = nodeList[0];
                mNode.InnerText = mValue;
                SaveXml();
            }

        }
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="nKey"></param>
        public void DeleteNoteValue(string nKey)
        {
            string keyval = ReadNoteValue(nKey);
            if (keyval != "NotFound" || keyval != "FileNotFound")
            {
                XmlNodeList mParentnodeList = doc.GetElementsByTagName("RegRoot");
                XmlNode mParentNode = mParentnodeList[0];
                XmlNodeList nodeList = doc.GetElementsByTagName(nKey);
                XmlNode node = nodeList[0];
                mParentNode.RemoveChild(node);
                SaveXml();
            }
        }
        /// <summary>
        /// 根据节点属性查找节点  修改节点的属性
        /// </summary>
        /// <param name="nodexpa">属性节点名称</param>
        /// <param name="setAttributeName">设置属性名称</param>
        /// <param name="setAttributeValue">设置属性值</param>
        /// <returns></returns>
        public int UpdateNodeAttribute(string nodexpa, string setAttributeName, string setAttributeValue)
        {
            if (LoadXmlSuccess())
            {
                XmlElement element = (XmlElement)doc.SelectSingleNode(nodexpa);
                element.SetAttribute(setAttributeName, setAttributeValue);
                try
                {
                    SaveXml();
                }
                catch(Exception ex) 
                { 
                    return -1; 
                }
                return 1;
            }
            else
                return 0;          
        }
        //写内容到Xml
        private void SaveXml()
        {
            //保存XML
            XmlTextWriter writer = new XmlTextWriter(strFile, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            doc.Save(writer);
            writer.Close();
        }
    }
}
