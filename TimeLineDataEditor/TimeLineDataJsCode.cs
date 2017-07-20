using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TimeLineDataEditor
{
    public class TimeLineDataJsCode
    {
        private const string _header = EditorConstant.DATA_HEADER;
        private List<TimeLineData> _timeLineDataList;

        public TimeLineDataJsCode()
        {
            _timeLineDataList = new List<TimeLineData>();
        }

        public void Add(TimeLineData data)
        {
            if (!_timeLineDataList.Contains(data))
            {
                _timeLineDataList.Add(data);
            }            
        }

        public void Remove(TimeLineData data)
        {
            if (_timeLineDataList.Contains(data))
            {
                _timeLineDataList.Remove(data);
            }
        }

        public void Clear()
        {
            if (_timeLineDataList != null)
            {
                _timeLineDataList.Clear();
            }
        }

        /// <summary>
        /// 获取整个js文件中全部的data.push语句连接成的字符串
        /// </summary>
        /// <returns></returns>
        private string GetDataPushCodeString()
        {
            string code = null;
            string str = null;
            for(int i = 0; i < _timeLineDataList.Count; i++)
            {
                str = _timeLineDataList[i].GetDataCodeString();
                code = code + System.Environment.NewLine + System.Environment.NewLine + str;
            }
            return code;
        }

        /// <summary>
        /// 获取完整的js文件字符串
        /// </summary>
        /// <returns></returns>
        private string GetCodeString()
        {
            string code = null;
            string dataPushString = null;
            dataPushString = GetDataPushCodeString();
            code = _header + dataPushString;
            return code;
        }

        public void WriteToJsFile(string fileFullName)
        {
            string code = GetCodeString();

            FileStream fs = new FileStream(fileFullName, FileMode.OpenOrCreate);
            fs.SetLength(0);
            //获得字节数组
            byte[] data = System.Text.Encoding.UTF8.GetBytes(code);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }

        public void WriteToJsFile(string fileFullName, string content)
        {
            FileStream fs = new FileStream(fileFullName, FileMode.OpenOrCreate);
            fs.SetLength(0);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(content);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// 解析Js代码，把代码按一定规则拆分并存储到容器中
        /// </summary>
        private void Parse(StreamReader sr)
        {
            string line = null;
            while((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Equals(EditorConstant.DATA_CODE_FORE_PART))
                {
                    //开始解析data.push代码段
                    TimeLineData tld = new TimeLineData();
                    ParseDataPush(sr, tld);
                }
            }
        }

        /// <summary>
        /// 专门解析data.push代码段
        /// </summary>
        /// <param name="sr">StreamReader的当前行应该是data.push代码段中第一个属性的前一行，也就是"data.push({"所在的行</param>
        /// <param name="timeLineData">用于存放解析出的键值对的TimeLineData对象</param>
        private int ParseDataPush(StreamReader sr, TimeLineData timeLineData)
        {
            /*  判断当前行是不是一条注释语句。
                如果是注释语句就不做处理，直接进入到下一行；

                如果不是注释语句，那就简单地认为是有效的键值对语句，
                从而提取出Key和Value，存储到TimeLineData对象中，
                并加入到链表_timeLineDataList中；

                如果是结束符号"})"，则退出*/
            int keyValueCount = 0;  //已经获取到的键值对的个数
            string line = null;
            while ((line = sr.ReadLine().Trim()) != EditorConstant.DATA_CODE_END_PART)
            {
                if (!IsComment(line))
                {
                    //提取该行的Key和Value并存储到TimeLineData对象中
                    timeLineData.AddKeyAndValue(line);

                    keyValueCount++;
                }
            }
            this._timeLineDataList.Add(timeLineData);
            return keyValueCount;
        }

        /// <summary>
        /// 判断一条语句是不是有效的注释语句
        /// </summary>
        /// <param name="line">要判断的语句</param>
        /// <returns>是注释语句则返回true，否则返回false</returns>
        private bool IsComment(string line)
        {
            //去掉首尾空格后截取前两位字符。如果是以"//"开头，则是注释语句，否则不是。
            line = line.Trim();
            return line.StartsWith("//");
        }

        public void ReadFromJsFile(string fileFullName)
        {
            //读取Js文件
            StreamReader sr = new StreamReader(fileFullName, Encoding.UTF8);

            //解析Js代码，把代码按一定规则拆分并存储到容器中
            this.Parse(sr);

            sr.Close();
        }

        public List<TimeLineData> Find(string intro)
        {
            List < TimeLineData > dataList= new List<TimeLineData>();
            if (this._timeLineDataList.Count > 0)
            {
                foreach (TimeLineData data in this._timeLineDataList)
                {
                    if(data.Find(intro))
                    {
                        dataList.Add(data);
                    }
                }
            }
            return dataList;
        }

        /// <summary>
        /// 在传入的字符串的前面加上var data = [];
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string WrapJsHead(string code)
        {
            string str = EditorConstant.DATA_HEADER + System.Environment.NewLine + System.Environment.NewLine + code;
            return str;
        }
    }
}
