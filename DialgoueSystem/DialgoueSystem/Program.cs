using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Xml;


namespace DialgoueSystem
{
    public class Program
    {
        // Globals
        public static StreamReader sr;
        public static string line;

        public static void Main(string[] args)
        {
            Dialogue dia = load_dialogue("sample.txt");
            runDialogue(dia);
        }

        public static Dialogue load_dialogue(string name)
        {
            sr = new StreamReader(name);

            Dialogue dia = new Dialogue();

            while(sr.Peek() >= 0)
            {
                line = sr.ReadLine();

                if (line == null || line == "" || line[0] == '\r' || line[0] == '#')
                    continue;

                DialogueNode dialogue = parseDialogueNode();
                dia.addNode(dialogue);
                
            }

            return dia;

        }

        static void runDialogue(Dialogue dia)
        {
            int node = 0;

            while(node != -1)
            {
                node = runNodes(dia.nodes[node]);
            }

            Console.WriteLine();

        }

        static int runNodes(DialogueNode node)
        {
            int next_node = -1;

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(node.text);

            Console.ForegroundColor = ConsoleColor.White;
            for(int i = 0; i < node.options.Count; i++)
            {
                Console.WriteLine(i+1 + ": " + node.options[i].text);
            }

            Console.Write("Enter your choice: ");
            char key = Console.ReadKey().KeyChar;

            next_node = node.options[int.Parse(key.ToString()) -1].destId;

            return next_node;
        }

        // Function used to Build your DialogueNode
        public static DialogueNode parseDialogueNode()
        {
            bool hasStatements = true;
            bool flag = false;
            string data;

            DialogueNode node = new DialogueNode();

            while (hasStatements)
            {
                if (!flag)
                {
                    data = line;
                    flag = true;
                }
                else
                {
                    line = sr.ReadLine();
                    data = line;
                }

                string prefix = ExtractUpToDelimeter(data, ':');

                // Parse Dialogue Node data
                if (prefix.ToLower() == "nodeid")
                {
                    node.nodeId = parseId(data, ':');
                }
                // Dialogue Text
                else if (prefix.ToLower() == "text")
                {
                    node.text = parseText(data, ':');
                }
                // Parse Option Data
                else if (prefix.ToLower() == "options")
                {
                    OptionNode op;

                    // Number of options
                    int optionCount = parseId(data, ':');

                    for (int i = 0; i < optionCount; i++)
                    {

                        op = new OptionNode();

                        data = line;
                        line = sr.ReadLine();
                        prefix = ExtractUpToDelimeter(data, ':');

                        // Keep looping until you come across destId field
                        while (prefix.ToLower() != "destid")
                        {
                            // Text
                            if (prefix.ToLower() == "text")
                            {
                                op.text = parseText(data, ':');
                            }
                            else
                            {
                                Console.WriteLine("Error in options");
                            }

                            data = line;
                            line = sr.ReadLine();
                            prefix = ExtractUpToDelimeter(data, ':');
                        }


                        int destId = parseId(data, ':');

                        op.destId = destId;

                        node.addOption(op);

                    }

                    // End of Dialogue Node block
                    hasStatements = false;
                }
                else
                {
                    // Perfect place to throw an error and end program.
                    Console.WriteLine("Not a field");
                }
            }

            return node;
        }

        // Function that extracts text up to a specified delimeter
        public static string ExtractUpToDelimeter(string statement, char delimeter)
        {
            // String to hold the data we actually want.
            StringBuilder buffer = new StringBuilder();

            // String that holds the modified raw text
            StringBuilder temp = new StringBuilder();

            // Holds the actual raw text
            string data = statement;

            // We use this to jump to to the delimeter in our .Substring function
            int index;

            // Jump to relevant delimeter
            index = data.IndexOf(delimeter);

            // Store the modified raw text in data
            data = data.Substring(0, index);

            // Append it to string builder so that we can manipulate it
            temp.Append(data);

            // Only append to relevant characters to our buffer
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == '\n' || temp[i] == '\r' || temp[i] == ':' || temp[i] == ' ')
                    continue;

                buffer.Append(temp[i]);
            }

            return buffer.ToString();
        }

        // Function to extract text from file (uses statement)
        public static string parseText(string statement, char delimeter)
        {
            // String to hold the data we actually want.
            StringBuilder buffer = new StringBuilder();

            // String that holds the modified raw text
            StringBuilder temp = new StringBuilder();

            // Holds the actual raw text
            string data;

            // We use this to jump to to the delimeter in our .Substring function
            int index;

            // Grab line from text file
            data = statement;

            // Jump to relevant delimeter
            index = data.IndexOf(delimeter);

            // Store the modified raw text in data
            data = data.Substring(index + 1);

            // Append it to string builder so that we can manipulate it
            temp.Append(data);

            // Only append to relevant characters to our buffer
            for (int i = (temp[0] == ' ') ? 1 : 0; i < temp.Length; i++)
            {
                if (temp[i] == '\n' || temp[i] == '\r' || temp[i] == ':')
                    continue;

                buffer.Append(temp[i]);
            }

            return buffer.ToString();
        }


        // Function to extract ids from text file (uses statement)
        public static int parseId(string statement, char delimeter)
        {
            // String to hold the data we actually want.
            StringBuilder buffer = new StringBuilder();

            // String that holds the modified raw text
            StringBuilder temp = new StringBuilder();

            // Holds the actual raw text
            string data;

            // We use this to jump to to the delimeter in our .Substring function
            int index;

            // Grab line from text file
            data = statement;

            // Jump to relevant delimeter
            index = data.IndexOf(delimeter);

            // Store the modified raw to text in data
            data = data.Substring(index);

            // Append it to string builder so that we can manipulate it
            temp.Append(data);

            //Debug.Log("Parsing ID, line: " + temp.ToString());

            // Only append to our buffer if it is a number
            for (int i = 0; i < temp.Length; i++)
            {
                if (Char.IsDigit(temp[i]) || temp[i] == '-')
                {
                    buffer.Append(temp[i]);
                }
            }

            return Int32.Parse(buffer.ToString());
        }
    }
}
