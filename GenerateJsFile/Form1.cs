using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TreeGridViewToXml;
using TimeLineDataEditor;

namespace GenerateJsFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //生成xml文件
            CreateXmlText();

            //转换成km
            //把 H:\aa.xml 转换成 H:\aa.js
            JosnConverter jConverter = new JosnConverter();
            jConverter.XmlToJson(@"H:\aa.xml", @"H:\aa.js");
        }

        private string CreateXmlText()
        {
            //创建根节点
            TgvXml tgvXml = new TgvXml();
            MindNode root = new MindNode("b2e32ca53b43", "1499742764", "For KM");
            //这一句非常重要，把root节点设置为根节点
            tgvXml.SetRootMindNode(root);

            MindNode A = new MindNode("b2e32ca53b43", "1499742764", "A");
            A.AddOrUpdate("expandState", "collapse");
            A.AddOrUpdate("priority", "1");
            root.AddChild(A);
            MindNode A1 = new MindNode("b2e32ca53b43", "1499742764", "A.1");
            A.AddChild(A1);
            MindNode A11 = new MindNode("b2e32ca53b43", "1499742764", "A.1.1");
            A1.AddChild(A11);
            MindNode A12 = new MindNode("b2e32ca53b43", "1499742764", "A.1.2");
            A1.AddChild(A12);
            MindNode A13 = new MindNode("b2e32ca53b43", "1499742764", "A.1.3");
            A1.AddChild(A13);
            MindNode A2 = new MindNode("b2e32ca53b43", "1499742764", "");
            A.AddChild(A2);

            MindNode B = new MindNode("b2e32ca53b43", "1499742764", "B");
            root.AddChild(B);
            MindNode B1 = new MindNode("b2e32ca53b43", "1499742764", "B.1");
            B.AddChild(B1);
            MindNode B11 = new MindNode("b2e32ca53b43", "1499742764", "B.1.1");
            B1.AddChild(B11);
            MindNode B12 = new MindNode("b2e32ca53b43", "1499742764", "");
            B1.AddChild(B12);
            MindNode B2 = new MindNode("b2e32ca53b43", "1499742764", "B.2");
            B.AddChild(B2);
            MindNode B21 = new MindNode("b2e32ca53b43", "1499742764", "B.2.1");
            B2.AddChild(B21);
            MindNode B22 = new MindNode("b2e32ca53b43", "1499742764", "B.2.2");
            B2.AddChild(B22);
            MindNode B3 = new MindNode("b2e32ca53b43", "1499742764", "B.3");
            B.AddChild(B3);

            MindNode C = new MindNode("b2e32ca53b43", "1499742764", "C");
            root.AddChild(C);

            MindNode D = new MindNode("b2e32ca53b43", "1499742764", "D");
            root.AddChild(D);

            //string xmlText = tgvXml.GenerateXml();
            tgvXml.Save(@"H:\aa.xml");

            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimeLineDataJsCode timeLineDataJsCode = new TimeLineDataJsCode();
            //读取已有的js文件
            timeLineDataJsCode.ReadFromJsFile(@"H:\GitHub\TimeLineDataEditor\TimeLineDataEditor\data.js");

            //在js代码中查找intro中含有“草长莺飞”字符串的data.push语句，并逐条删除
            List<TimeLineData> dataList = timeLineDataJsCode.Find("草长莺飞");
            foreach (TimeLineData data in dataList)
            {
                timeLineDataJsCode.Remove(data);
            }

            //创建一个新的data.push语句
            TimeLineData tlData = new TimeLineData();
            tlData.AddOrUpdate("date", "2017-07=20");
            tlData.AddOrUpdate("intro", "创建一个新的date.push");
            tlData.AddOrUpdate("NewNode", "原本不存在的键");
            timeLineDataJsCode.Add(tlData);

            //保存成新的js文件
            timeLineDataJsCode.WriteToJsFile(@"H:\data.JS");
        }
    }
}
