using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLineDataEditor
{
    class EditorConstant
    {
        public const string KEY_DATE = "date";
        public const string KEY_INTRO = "intro";
        public const string KEY_MEDIA = "media";
        public const string KEY_LIKE = "like";
        public const string KEY_COMMENT = "comment";

        public const string DATA_HEADER = "var data = [];";

        public const string DATA_KEY_VALUE_CODE_PRE_PADDING = "    "; //4个空格
        public const string DATA_KEY_VALUE_SEPARATOR = ":";
        public const string DATA_KEY_VALUE_PAIR_SEPARATOR = ",";
        public const string DATA_CODE_FORE_PART = "data.push({";
        public const string DATA_CODE_END_PART = "})";

        public const string DATA_CODE_FORE_PART2 = "data.push(";
        public const string DATA_CODE_END_PART2 = ")";
    }
}
