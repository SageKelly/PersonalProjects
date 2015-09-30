using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletManager
{
    /// <summary>
    /// Used to keep track of whether or not assets for
    /// a particular Bullet or Player has been previously loaded
    /// </summary>
    public class FileLoadManager
    {
        public string Filename;
        public bool isBullet { get; private set; }
        public bool isLoaded;

        public FileLoadManager(string filename, bool is_bullet = false)
        {
            Filename = filename;
            isBullet = is_bullet;
            isLoaded = false;
        }
    }
}
