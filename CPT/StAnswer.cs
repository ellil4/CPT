using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPT
{
    public enum ITEM_TYPE
    {
        NON_TARGET, TARGET
    }

    public class StAnswer
    {
        public ITEM_TYPE Type;
        public List<long> PressTime;

        public StAnswer()
        {
            PressTime = new List<long>();
        }
    }
}
