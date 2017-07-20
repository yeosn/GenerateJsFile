using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLineDataEditor
{
    class EditorUtility
    {
        static public string AddSingleQuotes(string str)
        {
            str = str == null ? "" : str;
            return "'" + str + "'";
        }

        /// <summary>
        /// 去掉字符串两端的单引号
        /// </summary>
        /// <param name="str">要去掉单引号的字符串</param>
        /// <returns>如果字符串确实被添加了单引号，则返回去掉单引号之后的字符串；否则返回原字符串</returns>
        static public string DepriveSingleQuotes(string str)
        {
            str = str.Trim();
            if (str.StartsWith("'") && str.EndsWith("'"))
            {
                if (str.Length >= 3)
                {
                    str = str.Substring(1, str.Length - 2);
                }
                else
                {
                    str = "";
                }
            }
            return str;
        }

        /// <summary>
        /// 去掉字符串末尾的逗号
        /// </summary>
        /// <param name="str">要去掉末尾逗号的字符串</param>
        /// <returns>如果字符串确实以逗号结尾，则返回去掉末尾逗号之后的字符串；否则返回原字符串</returns>
        static public string DepriveEndingComma(string str)
        {
            if(str.EndsWith(","))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }
    }
}
