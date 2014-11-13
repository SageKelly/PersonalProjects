using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace EZUI
{
    /// <summary>
    /// A class that holds both Texture2Ds and user-defined GUIcons
    /// </summary>
    internal class PicLibrary
    {
        Dictionary<string, Texture2D> Pics;
        Dictionary<string, GUIcon> GUIcons;

        /// <summary>
        /// Keeps track of all the images within the SceneManager: DOES NOT WORK; DO NOT USE
        /// </summary>
        public PicLibrary()
        {
            Pics = new Dictionary<string, Texture2D>();
            GUIcons = new Dictionary<string, GUIcon>();
        }

        #region Methods
        /// <summary>
        /// Adds a image to a list with a name attached
        /// </summary>
        /// <param name="name">The name of the image</param>
        /// <param name="pic">The image</param>
        public void AddPic(string name, Texture2D pic)
        {
            Pics.Add(name, pic);
        }

        /// <summary>
        /// Adds a GUIcon to a list with a name attached
        /// </summary>
        /// <param name="name">The name of the GUIcon</param>
        /// <param name="GUIC">The GUIcon</param>
        public void AddGUIcon(string name, GUIcon GUIC)
        {
            GUIcons.Add(name,GUIC);
        }

        /// <summary>
        /// Returns the requested image by name
        /// </summary>
        /// <param name="name">The name of the image</param>
        /// <returns>The image</returns>
        public Texture2D GetPic(string name)
        {
            return Pics[name];
        }

        /// <summary>
        /// Returns the GUIcon by name
        /// </summary>
        /// <param name="name">The name of the GUIcon</param>
        /// <returns>The GUIcon</returns>
        public GUIcon GetGUIcon(string name)
        {
            return GUIcons[name];
        }
        #endregion
    }
}
