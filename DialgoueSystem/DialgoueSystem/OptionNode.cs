﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialgoueSystem
{
    public class OptionNode
    {
        public string text;
        public string itemReq;
        public int itemReqAmount;

        public int destId;

        public OptionNode() { }

        public OptionNode(string text, int dest)
        {
            this.text = text;
            this.destId = dest;
        }
    }
}
