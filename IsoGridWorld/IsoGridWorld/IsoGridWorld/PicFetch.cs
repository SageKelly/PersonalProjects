using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IsoGridWorld
{
    class PicFetch
    {
        public string Directory
        {
            get;
            private set;
        }

        public FileStream Fetcher;

        public PicFetch() { }

        public PicFetch(string path, FileMode mode)
        {
            Directory = path;
            Fetcher = new FileStream(path, mode);
        }
    }
}
