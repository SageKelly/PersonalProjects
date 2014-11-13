using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoGridWorld
{
    public class Text
    {
        public SpriteFont Font;

        public string Words;

        public Vector2 Position;

        public Color FontColor;

        public Text()
        {
            Position = Vector2.Zero;
            FontColor = Color.White;
        }

        public Text(SpriteFont font, Vector2 position, Color fontColor, string words = "")
        {
            Font = font;
            Position = position;
            FontColor = fontColor;
            Words = words;
        }
    }
}
