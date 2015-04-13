using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHounds
{
    public class Image2D
    {
        public Texture2D image;
        public Rectangle sourceBounds;
        public SpriteEffects spriteEf;
        public Vector2 center;
        /// <summary>
        /// Denotes what drawtype is available:
        /// 1 is for basic (T2D, V2,Clr),
        /// 2 is for sourceBounds (T2D,V2,Rect,C),
        ///  and 3 is for the full draw (T2D, V2, Rect, C,float,V2,float,SprEff,int)
        /// </summary>
        public int drawType { get; private set; }
        public Image2D()
            : base()//Default declaration
        {
            sourceBounds = Rectangle.Empty;
            spriteEf = SpriteEffects.None;
            center = Vector2.Zero;
        }

        public Image2D(Texture2D image)
            : this()
        {
            this.image = image;
            drawType = 1;
        }

        public Image2D(Texture2D image, Rectangle source_bounds)
            : this(image)
        {
            sourceBounds = source_bounds;
            drawType = 2;
        }

        public Image2D(Texture2D image, Rectangle source_bounds, Vector2 center, SpriteEffects sprite_effects)
            : this()
        {
            this.image = image;
            this.center = center;
            sourceBounds = source_bounds;
            spriteEf = sprite_effects;
            drawType = 3;
        }
    }
}
