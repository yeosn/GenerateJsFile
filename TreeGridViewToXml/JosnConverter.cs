using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.IO;

using TimeLineDataEditor;

namespace TreeGridViewToXml
{
    public class JosnConverter
    {
        private List<string> _tokenPathList;
        private string _jsonText = null;

        public JosnConverter()
        {
            Initial();
        }

        private void Initial()
        {
            _tokenPathList = new List<string>();
        }

        private void GetNullTextChildren(string jsonText)
        {
            //获取json中text为空的节点
            JObject jo = JObject.Parse(jsonText);
            JToken startToken = jo["root"];
            GetNullTextChildren(startToken);
        }

        private void GetNullTextChildren(JToken startToken, string startNode = "children")
        {
            //JObject jo = JObject.Parse(jsonText);
            //JToken jo = JToken.Parse(jsonText);
            //JToken jts = jo[startNode]["children"];
            JArray jts = (JArray)startToken[startNode];//["children"];
            //string[] s = jts.Select(c => (string)c).Where().ToArray();
            if (jts != null)
            {
                foreach (JToken jt in jts)
                {
                    JToken jToken = jt["data"];
                    if (jToken["text"] != null && jToken["text"].ToString().Equals(""))
                    {
                        _tokenPathList.Add(jToken.Parent.Parent.Path);
                    }
                    //string jText = jt.ToString();
                    //string start = "children";
                    GetNullTextChildren(jt);
                }
            }

            //jo["root"]["children"][0]["children"][1].Remove();
        }

        /// <summary>
        /// 删除jsonText中指定path的节点
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public string RemoveNode(string jsonText, List<string> jTokenPathList)
        {
            JObject jo = JObject.Parse(jsonText);

            for (int i = jTokenPathList.Count - 1; i >= 0; i--)
            {
                string path = jTokenPathList[i];
                JToken jToken = jo.SelectToken(path);

                jToken.Remove();
            }
            //foreach (string path in this._tokenPathList)
            //{                
            //    jo.SelectToken(path).Remove();
            //}

            if (jo != null)
            {
                return jo.ToString();
            }
            else
            {
                return null;
            }
        }

        private string XmlToJson(string xmlFileName)
        {
            //xml转换成json
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFileName);
            XmlNodeConverter converter = new XmlNodeConverter();
            string jsonText = JsonConvert.SerializeXmlNode(doc.GetElementsByTagName("root")[0], Newtonsoft.Json.Formatting.Indented);
            this._jsonText = jsonText;
            return jsonText;
        }

        /// <summary>
        /// 这个方法目前没有用到
        /// </summary>
        /// <param name="fileName"></param>
        private void Save(string fileName)
        {
            //生成km文件
            StreamWriter sw = new StreamWriter(@"H:\bb.km");
            sw.Write(_jsonText);
            sw.Close();
        }

        //public void XmlToJson(string xmlFileName, string jsonFileName)
        //{
        //    //xml转换成json
        //    XmlToJson(xmlFileName);

        //    //获取json中text为空的节点
        //    GetNullTextChildren();

        //    //删除json中text为空的节点
        //    this._jsonText = RemoveNode(this._jsonText);

        //    //生成km文件
        //    Save(jsonFileName);
        //}

        public void XmlToJson(string xmlFileName, string jsonFileName)
        {
            //xml转换成json
            string jsonText = XmlToJson(xmlFileName);

            //获取json中text为空的节点
            GetNullTextChildren(jsonText);

            //删除json中text为空的节点
            this._jsonText = RemoveNode(jsonText, this._tokenPathList);

            SaveToJsFile(jsonFileName, this._jsonText);
        }

        private void SaveToJsFile(string jsFileName, string jsonText)
        {
            TimeLineDataJsCode timeLineDataJsCode = new TimeLineDataJsCode();
            TimeLineData data = new TimeLineData();
            string jText = data.GetDataCodeString(jsonText);
            jText = timeLineDataJsCode.WrapJsHead(jText);
            timeLineDataJsCode.WriteToJsFile(jsFileName, jText);
        }
    }
}
