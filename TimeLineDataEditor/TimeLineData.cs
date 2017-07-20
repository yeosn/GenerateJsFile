using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLineDataEditor
{
    public class TimeLineData
    {
        private Dictionary<string, string> _dataMap;    //用于存放datapush中的键值对
        private List<string> _dataList; //仅用于存储_dataMap中的Key，是为了方便遍历_dataMap

        public TimeLineData()
        {
            _dataMap = new Dictionary<string, string>();
            _dataList = new List<string>();

            Initial();
        }
        
        public void Initial()
        {
            this.Clear();
            this.AddOrUpdate(EditorConstant.KEY_DATE, "");
            this.AddOrUpdate(EditorConstant.KEY_INTRO, "");
            this.AddOrUpdate(EditorConstant.KEY_MEDIA, "");
            this.AddOrUpdate(EditorConstant.KEY_LIKE, "");
            this.AddOrUpdate(EditorConstant.KEY_COMMENT, "");
        }
        /// <summary>
        /// 添加或更新键值对
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddOrUpdate(string key, string value)
        {            
            if(!_dataMap.ContainsKey(key))
            {
                _dataMap.Add(key, value);
                _dataList.Add(key);
            }
            else
            {
                _dataMap[key] = value;
            }
        }

        public void Clear()
        {
            if (_dataMap != null && _dataList != null)
            {
                _dataMap.Clear();
                _dataList.Clear();
            }
        }

        public void Remove(string key)
        {
            if (_dataMap.ContainsKey(key))
            {
                _dataMap.Remove(key);
                _dataList.Remove(key);
            }
        }

        public void Reset(string key)
        {
            if (_dataMap.ContainsKey(key))
            {
                _dataMap[key] = null;
            }
        }

        /// <summary>
        /// 获取单个的键值对字符串，首位无填充
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetKeyValueCodeString(string key, string value)
        {
            string code = null;
            if(key.Equals(EditorConstant.KEY_LIKE) || key.Equals(EditorConstant.KEY_COMMENT))
            {
                code = EditorUtility.AddSingleQuotes(key) + " " + EditorConstant.DATA_KEY_VALUE_SEPARATOR + " " + value;
            }
            else
            {
                code = EditorUtility.AddSingleQuotes(key) + " " + EditorConstant.DATA_KEY_VALUE_SEPARATOR + " " + EditorUtility.AddSingleQuotes(value);
            }            
            return code;
        }

        /// <summary>
        /// 获取单个的键值对字符串，前面用4个空格填充
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetWrappedKeyValueCodeString(string key, string value)
        {
            string code = GetKeyValueCodeString(key, value);
            code = EditorConstant.DATA_KEY_VALUE_CODE_PRE_PADDING + code;
            return code;
        }

        /// <summary>
        /// 获取data.push语句中的全部键值对语句
        /// </summary>
        /// <returns></returns>
        private string GetDataContentString()
        {
            string code = null;
            string strKey = null;
            string strValue = null;
            string wrappedKeyValueString = null;

            for(int i = 0; i < _dataList.Count; i++)
            {
                strKey = _dataList[i].Trim();
                strValue = _dataMap[strKey].Trim();
                wrappedKeyValueString = GetWrappedKeyValueCodeString(strKey, strValue);
                if(i < _dataList.Count - 1)
                {
                    wrappedKeyValueString = wrappedKeyValueString + EditorConstant.DATA_KEY_VALUE_PAIR_SEPARATOR + System.Environment.NewLine;
                }
                else
                {
                    wrappedKeyValueString = wrappedKeyValueString + System.Environment.NewLine;
                }
                code = code + wrappedKeyValueString;
            }
            return code;
        }

        /// <summary>
        /// 获取一条完整的data.push语句
        /// </summary>
        /// <returns></returns>
        public string GetDataCodeString()
        {
            string code = EditorConstant.DATA_CODE_FORE_PART + System.Environment.NewLine;
            code = code + GetDataContentString() + EditorConstant.DATA_CODE_END_PART;            
            return code;            
        }

        /// <summary>
        /// 获取一条完整的data.push语句
        /// </summary>
        /// <returns></returns>
        public string GetDataCodeString(string dataString)
        {
            string code = EditorConstant.DATA_CODE_FORE_PART2 + System.Environment.NewLine;
            code = code + dataString + EditorConstant.DATA_CODE_END_PART2;
            return code;
        }

        /// <summary>
        /// 通过解析codeLine，把其中包含的键值对提取出来
        /// </summary>
        /// <param name="codeLine"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void GetKeyAndValue(string codeLine, out string key, out string value)
        {
            codeLine = EditorUtility.DepriveEndingComma(codeLine);

            int index = codeLine.IndexOf(":");
            key = codeLine.Substring(0, index);
            value = codeLine.Substring(index+1, codeLine.Length-index-1);

            key = EditorUtility.DepriveSingleQuotes(key.Trim());
            value = EditorUtility.DepriveSingleQuotes(value.Trim());
        }

        /// <summary>
        /// 通过解析codeLine，把其中包含的键值对提取出来并存入到容器中
        /// </summary>
        /// <param name="codeLine"></param>
        /// <returns>提取成功返回true，否则false</returns>
        public bool AddKeyAndValue(string codeLine)
        {
            try
            {
                string key, value;
                this.GetKeyAndValue(codeLine, out key, out value);
                this.AddOrUpdate(key.ToLower(), value);
                return true;
            }
            catch
            {
                return false;
            }            
        }

        /// <summary>
        /// 判断intro中是否包含传入的字符串
        /// </summary>
        /// <param name="intro"></param>
        /// <returns></returns>
        public bool Find(string intro)
        {
            string s = this._dataMap[EditorConstant.KEY_INTRO].Trim();
            if ((int)s.IndexOf(intro) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
