using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TreeGridViewToXml
{
    public class TgvXml
    {
        private XDocument document;
        private MindNode rootMindNode;

        public TgvXml()
        {
            Initial();
        }

        private void Initial()
        {
            //设置编码格式
            XDeclaration declaration = new XDeclaration("1.0", "UTF-8", null);
            document = new XDocument(declaration);

            //创建一个根节点，节点名称为root
            XElement root = new XElement("root");
            document.Add(root);
        }

        /// <summary>
        /// 设置一个根节点
        /// </summary>
        /// <param name="mindNode">将此节点设置为根节点</param>
        public void SetRootMindNode(MindNode mindNode)
        {
            this.rootMindNode = mindNode;
        }
        
        /// <summary>
        /// 再内存中生成所有的Xml节点内容，返回xml文件内容的字符串
        /// </summary>        
        public string GenerateXml()
        {
            //先把所有的MindNode都生成<data>节点
            //再把根MindNode节点对应的XElement设置为XML的根节点
            //然后递归把每一个MindNode节点的子节点生成XElement
            rootMindNode.GenerateDataNode();
            document.Root.Add(rootMindNode.DataElement);
            rootMindNode.GenerateChildNode();
            return document.ToString();
        }

        /// <summary>
        /// 保存为xml文件
        /// </summary>
        /// <param name="fileName">要保存的xml文件的路径</param> 
        public void Save(string fileName)
        {
            GenerateXml();
            document.Save(fileName);      
        }
    }
}
