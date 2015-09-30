using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BulletManager
{
    public static class ImageLoader
    {
        /// <summary>
        /// Keeps track of the Player-type assets
        /// </summary>
        static Dictionary<Type, FileLoadManager> PlayerFilenames;
        /// <summary>
        /// Keeps track of the Bullet-type assets
        /// </summary>
        static Dictionary<Type, FileLoadManager> BulletFilenames;
        static bool isLoaded;

        private const string CHARACTER_PATH = @"Assets\Characters\";
        private const string BULLET_PATH = @"Assets\Bullets\";

        public static Texture2D LoadPlayerAssets(Game game, Type name)
        {
            if (!isLoaded)
            {
                SetupDictionaries(game);
            }
            if (!PlayerFilenames[name].isLoaded)
            {
                PlayerFilenames[name].isLoaded = true;
                return Texture2D.FromStream(game.GraphicsDevice, new FileStream(PlayerFilenames[name].Filename, FileMode.Open));
            }
            else return null;
        }

        public static Texture2D LoadBulletAssets(Game game, Type name)
        {
            if (!isLoaded)
                SetupDictionaries(game);
            if (!BulletFilenames[name].isLoaded)
            {
                return Texture2D.FromStream(game.GraphicsDevice, new FileStream(BulletFilenames[name].Filename, FileMode.Open));
            }
            else return null;
        }

        public static Texture2D LoadBulletEffectsAssets(Game game)
        {
            return null;
        }

        private static void SetupDictionaries(Game game)
        {
            PlayerFilenames = new Dictionary<Type, FileLoadManager>();
            BulletFilenames = new Dictionary<Type, FileLoadManager>();

            //populate Players
            PlayerFilenames.Add(typeof(LilWaynnabe),
                new FileLoadManager(CHARACTER_PATH + "Lil Waynnabe.png"));
            PlayerFilenames.Add(typeof(MirrorMirror),
                new FileLoadManager(CHARACTER_PATH + "Mirror-Mirror.png"));

            //populate Bullets
            BulletFilenames.Add(typeof(WaterBullet), new FileLoadManager(BULLET_PATH + "Water Bullet.png"));
        }
    }
}
