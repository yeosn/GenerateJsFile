using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TreeGridViewToXml
{
    /// <summary>
    /// MindNode对应的是脑图中的节点，而非XML文件中的节点
    /// 建立MindNode的目的是为了更方便地表示脑图节点间的层次关系
    /// 同时也可以通过自身的方法将其转换为对应的XML节点，并存储于成员变量DataElement。
    /// </summary>
    public class MindNode
    {
        public MindNode ParentNode { get; set; }

        private string _id;
        private string _created;
        private string _text;

        public XElement DataElement { get; set; }
        private XElement _idElement;
        private XElement _createdElement;
        private XElement _textElement;   
        
        private List<MindNode> _children;

        private List<string> _keyList;
        private Dictionary<string, string> _keyValueMap;

        public MindNode(string id, string created, string text)
        {
            Initial();
            
            _id = id;
            this.AddOrUpdate("id", id);

            _created = created;
            this.AddOrUpdate("created", created);

            _text = text;
            this.AddOrUpdate("text", text);
        }

        private void Initial()
        {
            _children = new List<MindNode>();

            _keyList = new List<string>();
            _keyValueMap = new Dictionary<string, string>();
        }

        public void AddOrUpdate(string key, string value)
        {
            if (!_keyValueMap.ContainsKey(key))
            {
                _keyValueMap.Add(key, value);
                _keyList.Add(key);
            }
            else
            {
                _keyValueMap[key] = value;
            }
        }

        /// <summary>
        /// 添加一个子节点（脑图节点）
        /// </summary>
        /// <param name="id">节点id</param>
        /// <param name="created">节点创建时间</param>
        /// <param name="text">节点名称</param>
        public void AddChild(string id, string created, string text)
        {
            MindNode childNode = new MindNode(id, created, text);
            //把子节点的父节点设置为当前节点，以表示其层次关系，以便自顶向下生成XML节点
            childNode.ParentNode = this;
            //把子节点添加到子节点列表中，以便在生成XML文档时能通过父节点找到其下所有子节点
            _children.Add(childNode);
        }

        /// <summary>
        /// 添加一个子节点（脑图节点）
        /// </summary>
        /// <param name="childNode">要添加的子节点</param>
        public void AddChild(MindNode childNode)
        {
            childNode.ParentNode = this;
            _children.Add(childNode);
        }

        private XElement GenerateXmlNode(string nodeName)
        {
            XElement node = new XElement(nodeName);
            return node;
        }

        private XElement GenerateXmlNode(string nodeName, string nodeValue)
        {
            XElement node = new XElement(nodeName);
            node.Value = nodeValue;
            return node;
        }

        /// <summary>
        /// 生成XML文件的<data>节点
        /// </summary>
        public void GenerateDataNode()
        {
            //创建data标签
            DataElement = this.GenerateXmlNode("data");

            foreach(string key in this._keyList)
            {
                XElement xEle = this.GenerateXmlNode(key, this._keyValueMap[key]);
                DataElement.Add(xEle);
            }

            //_idElement = this.GenerateXmlNode("id", this._id);
            //_createdElement = this.GenerateXmlNode("created", this._created);
            //_textElement = this.GenerateXmlNode("text", this._text);

            //DataElement.Add(_idElement, _createdElement, _textElement);

            //通过递归的方式把子节点的data标签也创建出来
            foreach (MindNode mindNode in this._children)
            {
                mindNode.GenerateDataNode();
            }
        }

        /// <summary>
        /// 生成XML文件的<children>节点
        /// </summary>
        public void GenerateChildNode()
        {           
            //针对当前节点下的所有子节点，各创建一个<children>标签
            //再把子节点的<data>添加到这个标签下，
            //然后把这个<children>标签添加到当前<data>节点的兄弟节点
            foreach (MindNode mindNode in this._children)
            {
                //创建children标签
                XElement xChildren = new XElement("children");
                xChildren.Add(mindNode.DataElement);
                this.DataElement.Parent.Add(xChildren);

                //通过递归的方式把所有节点的子节点标签都创建出来
                mindNode.GenerateChildNode();
            }
        }
    }
}
