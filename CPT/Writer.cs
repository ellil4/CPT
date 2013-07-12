using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CPT
{
    class Writer
    {
        public Writer(String path, List<StAnswer> src)
        {
            StreamWriter sw = new StreamWriter(path, true, Encoding.GetEncoding("gb2312"));
            for(int i = 0; i < src.Count; i++)
            {
                String content = i.ToString() + "\t" + src[i].Type.ToString() + "\t";
                for(int j = 0; j < src[i].PressTime.Count; j++)
                {
                    content += src[i].PressTime[j].ToString() + "\t";
                }

                sw.WriteLine(content);
            }

            sw.Close();
        }
    }
}
