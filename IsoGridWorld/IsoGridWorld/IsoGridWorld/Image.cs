using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace IsoGridWorld
{
    public class Image
    {
        public string Name;

        public int ID;

        public Texture2D Avatar;

        public Vector2 Position;

        public Color ImageColor;

        public Vector2 Center;

        public bool IntricateDrawing;

        public int Scale;

        public Rectangle SourceBounds;

        public double Rotation;

        public SpriteEffects SPE;

        public Image()
        {
            Position = Vector2.Zero;
            ImageColor = Color.White;
            Center = Vector2.Zero;
            IntricateDrawing = false;
            Scale = 1;
            SourceBounds = Rectangle.Empty;
            SPE = SpriteEffects.None;
        }

        public Image(string name, int id, Texture2D avatar, Vector2 position, Color textureColor)
            : this()
        {
            Name = name;
            ID = id;
            Avatar = avatar;
            Position = position;
            ImageColor = textureColor;
        }

        public Image(string name, int id, Texture2D avatar, Vector2 position, Rectangle srcBounds, Color textureColor)
            : this(name, id, avatar, position, textureColor)
        {
            SourceBounds = srcBounds;
        }

        public Image(string name, int id, Texture2D avatar, Vector2 position, Rectangle sourceBounds,
            Color textureColor, double rotation, Vector2 center, int scale, SpriteEffects spe)
            : this(name, id, avatar, position, textureColor)
        {
            SourceBounds = sourceBounds;
            Rotation = rotation;
            Center = center;
            Scale = scale;
            SPE = spe;
        }
    }
}
