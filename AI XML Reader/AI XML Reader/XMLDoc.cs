using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AI_XML_Reader
{
    /// <summary>
    /// A class that holds the XML file's file path information
    /// </summary>
    public class XMLDoc
    {
        /// <summary>
        /// The collection of lines to print on the console window
        /// </summary>
        List<string> WriteLines;

        /// <summary>
        /// The full path location of the file
        /// </summary>
        string FullPath;

        /// <summary>
        /// The root node
        /// </summary>
        public Node Root
        {
            get;
            private set;
        }

        /// <summary>
        /// Holds all of the nodes for this file
        /// </summary>
        List<Node> Nodes;

        /// <summary>
        /// The file itself
        /// </summary>
        string file;

        StreamWriter sw;

        public XMLDoc(string path)
        {
            FullPath = path;
            StreamReader sr = new StreamReader(FullPath);
            sw = new StreamWriter("Output.txt");
            file = sr.ReadToEnd();
            Nodes = new List<Node>();
            ParseDocument();
            PrintStreamSubtree(Root);
            sw.Close();
            sr.Close();
        }

        /// <summary>
        /// Takes a XML document and parses it into an XML tree.
        /// The purpose of this method is to parse the entire
        /// document by running through it once while grabbing
        /// and separating the information as it should. It
        /// should only take one run-through to get the entire
        /// tree.
        /// </summary>
        public void ParseDocument()
        {
            ///These two lists, Nodes (in-class) and Closures, work in parallel with a common
            ///index.

            ///This one keeps track of whether or not a
            ///specific node is closed. TRUE is open, FALSE is
            ///closed
            List<bool> Closures = new List<bool>();

            ///These are used for when ascertaining
            ///node-attribute information. They mark the
            ///beginning and end of the starter tag.
            int Mark = 0;

            ///For the node name
            string CurText = "";

            ///Records the current node's attribute name and value
            ///over time and assigns it to the appropriate string
            ///within the node.
            string AttrName = "";
            string AttrValue = "";

            int str_index = -1;

            //run through the entire file, character by character
            while (++str_index != file.Length)
            {
                if (file[str_index] == '<')//if you found a tag symbol...
                {
                    //...then check to see if this isn't a closing tag
                    if (file[++str_index] != '/')
                    {
                        //...then you're inside the tag

                        ///The extra loop is the same as the attribute
                        ///Name/Value finder below, but inspired by
                        ///paranoia. It gets the name of the tags
                        while (CurText == "")
                        {
                            while (file[str_index] != ' ' && file[str_index] != '>')
                            {
                                CurText += file[str_index++];
                            }
                        }
                        if (Nodes.Count == 0)//If there aren't any nodes yet...
                        {
                            //then this is the new node

                            Nodes.Add(new Node(CurText, true));
                            Root = Nodes[Nodes.Count - 1];
                        }
                        else
                            Nodes.Add(new Node(CurText));//It's just a regular old node

                        CurText = "";//Cleanup

                        ///Add a new closure tag, signifying that
                        ///the current tag is open. This stays in-
                        ///sync with the Nodes list at all times.
                        Closures.Add(true);

                        ///For each node in the list check to see if the others
                        ///are open or closed. When the most recent open Node
                        ///is found, this new node is that node's child. Then
                        ///search within its children make them all siblings of
                        ///each other.
                        for (int index = Closures.Count - 2; index >= 0; index--)
                        {
                            if (Closures[index])
                            {
                                //You've found the parent
                                Nodes[index].AddChild(Nodes[Nodes.Count - 1]);
                                Nodes[Nodes.Count - 1].Parent = Nodes[index];
                                foreach (Node child in Nodes[index].Children)
                                {
                                    //Make them siblings
                                    if (child != Nodes[Nodes.Count - 1])
                                    {
                                        child.AddSibling(Nodes[Nodes.Count - 1]);
                                        Nodes[Nodes.Count - 1].AddSibling(child);
                                    }
                                }
                                ///After that, there's nothing else to look for
                                break;
                            }
                        }
                    }
                    else
                    {
                        ///This is a closing tag to a separate open tag.
                        ///Find the most recent open node and close it.
                        for (int i = Closures.Count - 1; i >= 0; i--)
                        {
                            if (Closures[i])
                            {
                                Closures[i] = false;
                                break;
                            }
                        }
                        //After that you're done with this tag
                        continue;
                    }
                }
                else if (file[str_index] == '/')//if the close symbol was found
                {
                    ///If you've made it here, then this is a
                    ///closing tag without a separate opening tag.
                    ///All you do is close the most recent node.
                    Closures[Closures.Count - 1] = false;
                }
                else if (file[str_index] == '=')
                {
                    ///You've found an attribute. Double back to find
                    ///out the name, then move forward to find out
                    ///the value
                    ///

                    //This is Mark.
                    /*...hi*/
                    Mark = str_index;
                    //Mark's going to handle this part.

                    ///Just to make sure there were no spaces
                    ///before the '=' symbol
                    while (AttrName == "")
                    {
                        while (file[--Mark] != ' ')
                        {
                            AttrName += file[Mark];
                        }
                    }
                    //You put it in backwards; flip it.
                    char[] temp = AttrName.ToCharArray();
                    Array.Reverse(temp);
                    AttrName = new string(temp);

                    //Then add the attribute name
                    Nodes[Nodes.Count - 1].Attributes.Add(AttrName, "");

                    //Set the index back to the '\"' sign
                    str_index++;

                    //Then look for the first quote
                    while (file[str_index++] != '\"') { }
                    ///At this point str_index should equal the
                    ///character after the first '\"' symbol

                    //If there is at least one character between the two quotes...
                    if (str_index - 1 != str_index)
                    {
                        while (file[str_index] != '\"')
                        {
                            AttrValue += file[str_index++];
                        }
                        //Assign the value to the appropriate attribute
                        Nodes[Nodes.Count - 1].Attributes[AttrName] = AttrValue;
                    }
                    AttrName = "";
                    AttrValue = "";
                }
            }
            #region Old Code
            /*
            for (int curnode = Nodes.Count - 1; curnode >= 0; curnode--)
            {
                for (int index = Nodes.Count - 2; index >= 0; index--)
                {
                    //Check to see if they're cousins.
                    Node CurNode = Nodes[curnode].Parent;
                    if (curnode == index)
                    {
                        if (index != 0)
                        {
                            index--;
                        }
                        else
                            continue;
                    }
                    Node Ancestor = Nodes[index].Parent;
                    ///At this point the two current Nodes
                    ///in check are the parents. If they
                    ///are siblings, then CurNode and
                    ///Ancestor will be equal, else
                    ///they'll be either siblings or
                    ///cousins of each other.
                    if (Ancestor == CurNode ||
                        (Ancestor == Nodes[0] || CurNode == Nodes[0]) ||
                        (Ancestor == null || CurNode == null) ||
                        (Ancestor.Parent == Nodes[0] || CurNode.Parent == Nodes[0]))
                        ///Then they have the same parent, or only 
                        ///one of their parents are the root, and
                        ///cannot be cousins
                        continue;
                    //but if they're siblings or cousins(which would be found out at this point)...
                    else if (Ancestor.Siblings.Contains(CurNode) || CurNode.Cousins.Contains(Ancestor))
                    {
                        ///Then add both the current node in check and
                        ///its siblings to the most recent node's
                        ///cousin list. Some duplication occurs right now
                        if (!Nodes[curnode].Cousins.Contains(Nodes[index]))
                        {
                            Ancestor = Nodes[curnode].Parent;
                            CurNode = Nodes[index].Parent;
                            foreach (Node child in Ancestor.Children)
                            {
                                child.Cousins.Add(Nodes[index]);
                                child.Cousins.AddRange(Nodes[index].Siblings);
                            }
                            foreach (Node child in CurNode.Children)
                            {
                                child.Cousins.Add(Nodes[curnode]);
                                child.Cousins.AddRange(Nodes[curnode].Siblings);
                            }
                        }
                        ///As a shortcut, once I've found the cousins, I can set the
                        ///curnode and index equal to the IndexOf() their parents,
                        ///and continue from there, since everything on this current
                        ///level has been associated to each other.
                    }
                }
            }
            */
            #endregion
        }

        private void PrintStreamSubtree(Node root, int tabID = 0)
        {
            for (int i = 0; i < tabID; i++)
            {
                sw.Write("\t");
            }
            sw.Write("Name: {0}; ", root.Name);
            for (int i = 0; i < root.Attributes.Keys.Count; i++)
            {
                sw.Write("{0}: {1}{2}",
                    root.Attributes.Keys.ToList()[i],
                    root.Attributes.Values.ToList()[i],
                    (i == root.Attributes.Keys.Count - 1 ? "" : "; "));
            }
            sw.WriteLine();
            if (root.HasChildren)
            {
                tabID++;
                foreach (Node no in root.Children)
                {
                    PrintStreamSubtree(no, tabID);
                }
            }
        }

        public void PrintSubtree()
        {
            PrintSubtree(Root);
        }

        private void PrintSubtree(Node root, int tabID = 0)
        {
            for (int i = 0; i < tabID; i++)
            {
                Console.Write("\t");
            }
            Console.Write("Name: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(root.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" | ");
            for (int i = 0; i < root.Attributes.Keys.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(root.Attributes.Keys.ToList()[i]);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(": ");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(root.Attributes.Values.ToList()[i]);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("{0}", (i == root.Attributes.Keys.Count - 1 ? "" : "; "));
            }
            Console.WriteLine();
            if (root.HasChildren)
            {
                tabID++;
                foreach (Node no in root.Children)
                {
                    PrintSubtree(no, tabID);
                }
            }
        }

        #region I didn't use these. The idea I had for them won't work.
        public void WriteSubtree()
        {
            WriteSubtree(Root);
        }

        private void WriteSubtree(Node root, int tabID = 0)
        {
            WriteLines.Add("");
            for (int i = 0; i < tabID; i++)
            {
                WriteLines[WriteLines.Count - 1] += "\t";
            }
            Console.Write("Name: {0}; ", root.Name);
            for (int i = 0; i < root.Attributes.Keys.Count; i++)
            {
                WriteLines[WriteLines.Count - 1] +=
                    root.Attributes.Keys.ToList()[i] +
                    ": " +
                    root.Attributes.Values.ToList()[i] +
                    (i == root.Attributes.Keys.Count - 1 ? "" : "; ");
            }
            if (root.HasChildren)
            {
                tabID++;
                foreach (Node no in root.Children)
                {
                    WriteSubtree(no, tabID);
                }
            }
        }
        #endregion

        /// <summary>
        /// Searches for a possible reponse given a behavior using
        /// Breadth-First Searching
        /// </summary>
        /// <param name="Head">The root at which to start the search</param>
        /// <param name="behavior">The behavior for which to be looking</param>
        /// <returns>Currently, a list of nodes that have the responses
        /// corresponding to the behavior given</returns>
        public List<Node> BreadthFirstSearch(Node Head, string behavior)
        {
            List<Node> Queue = new List<Node>();
            List<Node> result = new List<Node>();

            ///This increments over time, so that it, at worst,
            ///adds only one level of nodes containing the
            ///useful one. It adds the Nodes and checks them
            ///at the same time. It was intended to not
            ///add/check more Nodes than I needed to.

            Queue.Add(Head);
            for (int i = 0; i < Queue.Count; i++)
            {
                if (Queue[i].Attributes.Count != 0)
                {
                    if (Queue[i].Attributes.ContainsKey("behavior"))
                    {
                        if (Queue[i].Attributes["behavior"] == behavior)
                        {
                            result = BreadthFirstCollection(Queue[i]);
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("You don't know how to spell \"behavior\".");
                        return null;
                    }
                }
                if (Queue[i].HasChildren)
                {
                    Queue.AddRange(Queue[i].Children);
                }
            }
            return result;
        }

        private List<Node> BreadthFirstCollection(Node Head)
        {
            List<Node> results = new List<Node>();
            List<Node> Queue = new List<Node>();

            Queue.Add(Head);

            for (int i = 0; i < Queue.Count; i++)
            {
                if (Queue[i].Attributes.Count != 0)
                {
                    if (Queue[i].Attributes.ContainsKey("response"))
                    {
                        if (Queue[i].Attributes["response"] != "")
                        {
                            results.Add(Queue[i]);
                        }
                        else if (Queue[i].HasChildren)
                        {
                            Queue.AddRange(Queue[i].Children);
                        }
                    }
                    else
                    {
                        Console.WriteLine("You don't know how to spell \"response\".");
                        return null;
                    }
                }
            }
            return results;
        }


        public List<Node> DepthFirstSearch(Node Head, string behavior)
        {
            List<Node> result = new List<Node>();

            if (Head.Attributes.Count != 0)
            {
                if (Head.Attributes.ContainsKey("behavior"))
                {
                    if (Head.Attributes["behavior"] == behavior)
                    {
                        result.AddRange(DepthFirstCollection(Head));
                        return result;
                    }
                }
            }
            if (Head.HasChildren)
            {
                foreach (Node child in Head.Children)
                {
                    result.AddRange(DepthFirstSearch(child, behavior));
                }
                return result;
            }
            return result;
        }

        private List<Node> DepthFirstCollection(Node Head)
        {
            List<Node> Result = new List<Node>();
            if (Head.Attributes.Count != 0)
            {
                if (Head.Attributes.ContainsKey("response"))
                {
                    if (Head.Attributes["response"] != "")
                    {
                        Result.Add(Head);
                    }
                }
            }
            if (Head.HasChildren)
            {
                foreach (Node child in Head.Children)
                {
                    Result.AddRange(DepthFirstCollection(child));
                }
            }
            return Result;
        }

    }
}
