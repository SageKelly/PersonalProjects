using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace IsoGridWorld
{
    /// <summary>
    /// Designed only for this project, it gets the list of Tile Objects and returns them
    /// </summary>
    public static class ImageGetter
    {
        /// <summary>
        /// Used the load the XML template file
        /// </summary>
        static XmlDocument XMLFile;
        static XmlNode ProjectNode;
        static List<XmlNode> isopieces, isoanims;
        static List<PicFetch> FilePicGetters;
        static List<XmlNode> files;
        /// <summary>
        /// List of all the sprites in the game
        /// </summary>
        static List<Image> spritelist;

        /// <summary>
        /// The universal footprint for each isopiece
        /// </summary>
        static Vector2 footprint;

        /// <summary>
        /// List of all reserved words within the XML file
        /// </summary>
        #region reserved words
        static string project = "project";//Check
        static string name = "name";//isopiece: Check| 
        static string footprint_width = "footprint_width";//Check
        static string file = "file";//Check
        static string directory = "directory";//Check
        static string isopiece = "isopiece";//Check
        static string id = "id";//Check
        static string srcx = "srcx";//Check
        static string srcy = "srcy";//Check
        static string srcheight = "srcheight";//Check
        static string isoanim = "isoanim";
        /*
        static string framecount = "framecount";
        static string startx = "startx";
        static string starty = "starty";
        static string start_height = "start_height";
        static string fps = "fps";
        */
        #endregion

        /// <summary>
        /// Loads the sprites from the given XML file path
        /// </summary>
        /// <param name="path">The path the of the XML file</param>
        /// <param name="game">The game in which it will be used</param>
        public static List<Image> LoadXMLFile(string path, Game game)
        {
            XMLFile = new XmlDocument();
            try
            {
                XMLFile.Load(path);
            }
            catch (XmlException)
            {
                Console.WriteLine("Something is wrong with your XML file. We assume you have too many root nodes in your file. Fix that, then come back.");
            }
            FilePicGetters = new List<PicFetch>();
            spritelist = new List<Image>();
            RunParser(game);
            return spritelist;
        }

        /// <summary>
        /// Parses the XML file
        /// </summary>
        /// <param name="game">The game in which it will run</param>
        /// <returns>returns the list of Tiles</returns>
        public static void RunParser(Game game)
        {
            #region NewReader
            string fp_width = "";

            if (XMLFile.HasChildNodes)
                ProjectNode = FindAllNodes(XMLFile, project)[0];//project

            ///Look for the footprint width
            if (AttributeExists(ProjectNode, footprint_width))
            {
                fp_width = ProjectNode.Attributes[footprint_width].Value;
            }
            else throw new XmlException("Footprint width does not exist.");

            if (!IsANumber(fp_width))
                throw new XmlException(fp_width + " is not a number.");
            else if ((int.Parse(fp_width)) % 2 != 0)//If it isn't even
                throw new XmlException(fp_width + " is not even.");
            //The height is always half the width
            else footprint = new Vector2(int.Parse(fp_width), int.Parse(fp_width) / 2 + 1);

            //Find all the files
            if (ProjectNode.HasChildNodes)
                files = FindAllNodes(ProjectNode, file);

            #region Old Way
            foreach (XmlNode f in files)
            {

                PicFetch CurFile;
                //Look for directory
                if (AttributeExists(f, directory))
                {
                    CurFile = new PicFetch(f.Attributes[directory].Value, FileMode.Open);
                    FilePicGetters.Add(CurFile);
                }
                else
                {
                    throw new XmlException("directory doesn't exist.");
                }

                Texture2D texture = Texture2D.FromStream(game.GraphicsDevice, CurFile.Fetcher);
                isopieces = FindAllNodes(f, isopiece);
                isoanims = FindAllNodes(f, isoanim);

                foreach (XmlNode iso in isopieces)
                {
                    ///The main elements of an isopiece
                    string str_name, str_id, str_srcx, str_srcy, str_srcheight;
                    int i_id, i_srcx, i_srcy, i_srcheight;

                    //The center of the footprint
                    int cposx, cposy;


                    //Look for name
                    if (AttributeExists(iso, name))
                        str_name = iso.Attributes[name].Value;
                    else throw new XmlException(name + " missing from file");


                    //Look for id
                    if (AttributeExists(iso, id))
                    {
                        str_id = iso.Attributes[id].Value;
                        if (IsANumber(str_id))
                            i_id = int.Parse(str_id);
                        else throw new XmlException(str_id + " is not a number");
                    }
                    else throw new XmlException(id + " missing from file");

                    //Look for srcx and srcy
                    if (AttributeExists(iso, srcx))
                    {
                        str_srcx = iso.Attributes[srcx].Value;
                        if (IsANumber(str_srcx))
                            i_srcx = int.Parse(str_srcx);
                        else throw new XmlException(str_srcx + " is not a number");
                    }
                    else throw new XmlException(srcx + " missing from file");

                    if (AttributeExists(iso, srcy))
                    {
                        str_srcy = iso.Attributes[srcy].Value;
                        if (IsANumber(str_srcy))
                            i_srcy = int.Parse(str_srcy);
                        else throw new XmlException(str_srcy + " is not a number");
                    }
                    else throw new XmlException(srcy + " missing from file");


                    ///Look for the srcheight of the sprite. This is not
                    ///necessarily the height of the footprint, but
                    ///could be taller. Therefore, the center of the
                    ///footprint must be calculated based on the height
                    ///that is given here. This must be an even number.
                    if (AttributeExists(iso, srcheight))
                    {
                        str_srcheight = iso.Attributes[srcheight].Value;
                        if (!IsANumber(str_srcheight))
                            throw new XmlException(str_srcheight + " is not a number.");
                        else i_srcheight = int.Parse(str_srcheight);
                    }
                    else throw new XmlException(srcheight + " missing from file");

                    //Set its center
                    cposx = (int)footprint.X / 2;

                    if (i_srcheight != footprint.Y)
                    {
                        cposy = (int)(i_srcheight - (footprint.Y / 2));
                    }
                    else cposy = (int)footprint.Y / 2;

                    //Add the Tile to the list
                    spritelist.Add(new Image(str_name, i_id, texture, Vector2.Zero,
                           new Rectangle(i_srcx, i_srcy, (int)footprint.X, i_srcheight), Color.White, 0,
                           new Vector2(cposx, cposy), 1, SpriteEffects.None));
                }
                CurFile.Fetcher.Close();
            }
            #endregion /OldWay
            #endregion \NewReader
        }

        public static Vector2 GetFootPrint(string path)
        {
            Vector2 result = Vector2.Zero;
            string fp_width;

            XMLFile.Load(path);
            XmlNode head = XMLFile.FirstChild;

            ProjectNode = FindAllNodes(XMLFile, project)[0];
            ///Look for the footprint width
            if (AttributeExists(ProjectNode, footprint_width))
            {
                fp_width = ProjectNode.Attributes[footprint_width].Value;
            }
            else throw new XmlException("Footprint width does not exist.");

            if (!IsANumber(fp_width))
                throw new XmlException(fp_width + " is not a number.");
            else if ((int.Parse(fp_width)) % 2 != 0)//If it isn't even
                throw new XmlException(fp_width + " is not even.");
            //The height is always half the width
            else result = new Vector2(int.Parse(fp_width), int.Parse(fp_width) / 2 + 1);
            return result;
        }


        /// <summary>
        /// Determines if a given string is a number
        /// </summary>
        /// <param name="input">the given input</param>
        /// <returns>returns true if all characters are numbers.</returns>
        public static bool IsANumber(string input)
        {
            for (int index = 0; index < input.Length; index++)
            {
                for (int num = 0; num < 10; num++)
                {
                    //If you reached the last number and there's still no match...
                    if (input[index] != num.ToString()[0] && num == 9)
                        return false;//you know the rest
                    else if (input[index] == num.ToString()[0])
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// Finds all of the appropriate node within a given node. 
        /// </summary>
        /// <param name="root">root within to search</param>
        /// <param name="s_node_name">name of the pursued node</param>
        /// <returns>Returns 'null' if nothing is found.</returns>
        private static List<XmlNode> FindAllNodes(XmlNode root, string s_node_name)
        {
            List<XmlNode> temp = new List<XmlNode>();
            if (root.HasChildNodes)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    if (root.ChildNodes[i].Name == s_node_name)
                        temp.Add(root.ChildNodes[i]);
                }
            }
            if (temp.Count != 0)
                return temp;
            else
                return null;
        }

        /// <summary>
        /// Searches for the existence of a certain attribute inside a give XmlNode
        /// </summary>
        /// <param name="head">The node in which to look for the attribute</param>
        /// <param name="name">The attribute's name</param>
        /// <returns>Returns true if the attribute name exists</returns>
        public static bool AttributeExists(XmlNode head, string name)
        {
            foreach (XmlAttribute a in head.Attributes)
            {
                if (a.Name == name)
                    return true;
            }
            return false;
        }

        private static List<XmlNode> FindElement(XmlNode head, string name, int steps)
        {
            List<XmlNode> temp = new List<XmlNode>();
            for (int i = 0; i < XMLFile.ChildNodes.Count; i++)
            {
                if (XMLFile.ChildNodes[i].Name == name)
                {
                    temp.Add(XMLFile.ChildNodes[i]);
                }
            }
            return temp;
        }
    }
}
