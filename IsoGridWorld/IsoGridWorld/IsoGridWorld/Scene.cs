using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsoGridWorld
{
    public delegate void MethodInvoker();

    public class Scene : DrawableGameComponent
    {
        #region Variables
        #region Public
        public System.Drawing.Color ClearColor;

        public string Text;

        public Text ScreenFont;

        public Color BGColor;

        public Vector2 TextPosition;

        //public bool DrawList;

        public bool IsActive;

        public bool DrawImages;
        public bool DrawText;

        public List<Image> Images;

        public List<Text> Texts;
        #endregion
        #region Private

        private struct KeyStat
        {
            public Keys Key;
            public bool SinglePress;
            public Action<int> Method;
            public int MethodParam;
            public KeyStat(Keys key, bool singlePress, Action<int> method, int methodParam)
            {
                Key = key;
                SinglePress = singlePress;
                Method = method;
                MethodParam = methodParam;
            }
        }



        private List<KeyStat> KeyStats;

        private Game game;

        //private KeyboardState CurkeyState;

        private KeyboardState PrevkeyState;

        private SpriteBatch spriteBatch;
        #endregion
        #endregion
        public Scene(Game game)
            : base(game)
        {
            this.game = game;
            PrevkeyState = Keyboard.GetState();
            Images = new List<Image>();
            Texts = new List<Text>();
            KeyStats = new List<KeyStat>();
            IsActive = false;
            TextPosition = Vector2.Zero;
            BGColor = Color.White;
        }

        public Scene(Game game, Color bgColor)
            : this(game)
        {
            DrawImages = true;
            DrawText = true;
            BGColor = bgColor;
        }

        public Scene(Game game, Text screenFont, Color bgColor, Vector2 textPosition)
            : this(game, bgColor)
        {
            ScreenFont = screenFont;
            TextPosition = textPosition;
        }


        public void AddText(SpriteFont font, Vector2 position, Color textColor, string words = "")
        {
            Texts.Add(new Text(font, position, textColor, words));
        }

        public void AddImage(Image image)
        {
            Images.Add(image);
        }

        //TODO: get this working
        public void AddKey(Keys key, bool SinglePress, Action<int> invokedMethod, int methodParam)
        {
            KeyStats.Add(new KeyStat(key, SinglePress, invokedMethod, methodParam));
        }

        public override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            /*
            CurkeyState = Keyboard.GetState();
            foreach (KeyStat KS in KeyStats)
            {
                if (KS.SinglePress)
                {
                    if (CurkeyState.IsKeyDown(KS.Key) && PrevkeyState.IsKeyUp(KS.Key))
                    {
                        KS.Method.Invoke(KS.MethodParam);
                    }
                }
                else
                {
                    if (CurkeyState.IsKeyDown(KS.Key))
                    {
                        KS.Method.Invoke(KS.MethodParam);
                    }
                }
            }
            PrevkeyState = CurkeyState;
            */
        }
        /*
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (IsActive)
            {
                //GraphicsDevice.Clear(BGColor);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
               SamplerState.PointClamp, null, null);

                //Draw images
                if (DrawImages)
                {
                    foreach (Image Img in Images)
                    {
                        if (Img.IntricateDrawing)
                        {
                            spriteBatch.Draw(Img.Avatar, Img.Position, Img.ImageColor);
                        }
                        else
                        {
                            spriteBatch.Draw(Img.Avatar, Img.Position, Img.SourceBounds, Img.ImageColor, (float)Img.Rotation, Img.Center, Img.Scale, Img.SPE, 0);
                        }
                    }
                }
                //Draw Font
                if (DrawText)
                {
                    foreach (Text T in Texts)
                    {
                        spriteBatch.DrawString(T.Font, T.Words, T.Position, T.FontColor);
                    }
                }
                spriteBatch.End();
            }
        }
        */
    }
}
