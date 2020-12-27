using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialgoueSystem
{
    public class DialogueNode
    {
        // Common Data
        public int nodeId;
        public string text;
        public List<OptionNode> options;

        public DialogueNode()
        {
            // Options
            options = new List<OptionNode>();
        }

        public void addDialogue(string text, int id)
        {
            this.text = text;
            this.nodeId = id;
        }

        public void addOption(string text, int dest)
        {
            OptionNode op = null;

            op = new OptionNode(text, dest);
            this.options.Add(op);
        }

        public void addOption(OptionNode op)
        {
            this.options.Add(op);
        }
    }
}
