using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EZUI
{

    /*
     * WHAT I CAN DO:
     * →Create GUIcons
     * →Define their image, Hitbounds, Tint and position through the constructor
     * →Assign a method to three delegates based on click, MouseEnter and MouseExit
     * and check for each to happen
     * 
     */

    /*
     * WHAT I CAN'T DO:
     * →Dynamically animate GUIcons
     * →Dynamically animate SpriteFonts
     *      →This may be done using PicAnimator
     */

    #region Delegates
    /// <summary>
    /// This will occur once this GUIcon has been on clicked
    /// </summary>
    public delegate void MouseClickHandler(GUIcon GUIC);

    /// <summary>
    /// Handles any time the Mouse cursor enters a GUIcon
    /// </summary>
    /// <param name="GUIC">The calling GUIcon</param>
    public delegate void MouseEnterHandler(GUIcon GUIC);

    /// <summary>
    /// Handles any time the Mouse exits the GUIcon
    /// </summary>
    /// <param name="GUIC">The calling GUIcon</param>
    public delegate void MouseExitHandler(GUIcon GUIC);

    /// <summary>
    /// Handles any activation or deactivation of the GUIcon
    /// </summary>
    /// <param name="GUIC">The calling GUIcon</param>
    public delegate void GUIconActivationHandler(GUIcon GUIC);
    #endregion

    /// <summary>
    /// The most basic element of a Widget or Scene
    /// </summary>
    public class GUIcon : DrawableGameComponent
    {
        #region Variables
        #region public

        #region Primitives
        /// <summary>
        /// Used to determine the rotation origin of the image
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Used to determine if the GUIcon is active
        /// </summary>
        public bool IsActive
        {
            get
            {
                return b_is_active;
            }
            set
            {
                b_is_active = value;
                bool temp = b_is_active;

                if (b_is_active && !temp)
                {
                    OnDeactivate();
                }
                else if (!b_is_active && temp)
                {
                    OnActivate();
                }
            }
        }


        /// <summary>
        /// Used to determine how the GUIcon should be drawn 
        /// when considering the Draw method down below (1-3)
        /// </summary>
        public int DrawType
        {
            get;
            private set;
        }

        /// <summary>
        /// The meta-name of the GUIcon
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// What the text of the GUIcon reads
        /// </summary>
        public string FontText;

        /// <summary>
        /// Denotes whether or not the cursor is hovering over the GUIcon
        /// </summary>
        public virtual bool IsEntered
        {
            get
            {
                return b_is_entered;
            }
            private set
            {
                b_is_entered = value;
                if (b_is_entered)
                    OnMouseEnter();
                else
                    OnMouseExit();
            }
        }

        /// <summary>
        /// Denotes whether or not the GUIcon has been clicked
        /// </summary>
        public bool IsLClicked
        {
            get
            {
                return b_is_lclicked;
            }
            private set
            {
                b_is_lclicked = value;
                if (b_is_lclicked)
                    OnMouseLClick();
            }
        }

        /// <summary>
        /// Denotes whether or not the GUIcon has been clicked
        /// </summary>
        public bool IsRClicked
        {
            get
            {
                return b_is_rclicked;
            }
            private set
            {
                b_is_rclicked = value;
                if (b_is_rclicked)
                    OnMouseRClick();
            }
        }
        #endregion

        #region Objects
        /// <summary>
        /// The collision bounds for the GUIcon in terms of click or whatever
        /// </summary>
        public Rectangle HitBounds;

        /// <summary>
        /// The area of space referencing the sampled image
        /// </summary>
        public Rectangle SrcBounds;

        /// <summary>
        /// The image itself
        /// </summary>
        public Texture2D Image;


        //public Anima Animation;

        /// <summary>
        /// The center orientation of the image
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// The position of the GUIcon
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The velocity of the GUIcon
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// The acceleration of the GUIcon
        /// </summary>
        public Vector2 Acceleration;

        /// <summary>
        /// Used to designate the GUIcon's font's position
        /// </summary>
        public Vector2 FontPosition;

        /// <summary>
        /// Used to determine the size of which to display the scale of the image
        /// </summary>
        public Vector2 Scale;

        /// <summary>
        /// The tint of the image
        /// </summary>
        public Color Tint;

        /// <summary>
        /// The Basic Background color
        /// </summary>
        public Color BGColor;

        /// <summary>
        /// The color for the font
        /// </summary>
        public Color FontColor;

        /// <summary>
        /// The SpriteFont used for the GUIcon
        /// </summary>
        public SpriteFont Font;

        /// <summary>
        /// The effect of the image for the GUIcon
        /// </summary>
        public SpriteEffects spriteEffects;
        #endregion

        #region Delegates/Events
        /// <summary>
        /// The event for when the GUIcon is left clicked
        /// </summary>
        public event MouseClickHandler MouseLClickEvent;

        /// <summary>
        /// The event for when the GUIcon is right clicked
        /// </summary>
        public event MouseClickHandler MouseRClickEvent;

        /// <summary>
        /// The event for whenever the mouse enters the GUIcon's collision bounds
        /// </summary>
        public event MouseEnterHandler MouseEnterEvent;

        /// <summary>
        /// The event for whenever the mouse exits the GUIcon's collision bounds
        /// </summary>
        public event MouseExitHandler MouseExitEvent;

        /// <summary>
        /// Sends an event when the GUIcon is activated
        /// </summary>
        public event GUIconActivationHandler Activated;

        /// <summary>
        /// Sends and event when the GUIcon is deactivated
        /// </summary>
        public event GUIconActivationHandler Deactivated;
        #endregion
        #endregion

        #region private

        #region Primitives
        private int i_image_width, i_image_height, i_half_width, i_half_height;

        private const int BASIC = 0, COMMON = 1, ANIMATE = 2, ROTATE = 3;

        private bool b_is_lclicked, b_is_rclicked, b_is_entered, b_is_active;
        #endregion

        #region Objects
        internal Texture2D tempBG;

        private Color[] colordata;
        private SpriteBatch spriteBatch;


        #endregion

        #region Delegates/Events
        #endregion
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a default clickable icon for the screen to display
        /// </summary>
        /// <param name="game">The Game in which this GUIcon will be used</param>
        /// <param name="name">The name of the GUIcon</param>
        internal GUIcon(Game game, string name)
            : base(game)
        {
            Name = name;
            Position = Vector2.Zero;
            Scale = new Vector2(1.0f, 1.0f);
            Tint = Color.White;
            Rotation = 0.0f;
            SrcBounds = Rectangle.Empty;
            HitBounds = Rectangle.Empty;
            IsActive = IsEntered = IsLClicked = false;
            spriteEffects = SpriteEffects.None;
        }

        /// <summary>
        /// Creates a clickable icon for the screen to display.
        /// This creates a basic background for the GUIcon
        /// </summary>
        /// <param name="game">The Game in which this GUIcon will be used</param>
        /// <param name="name">The name of the GUIcon</param>
        ///<param name="Bounds">The hitboudns for the button</param>
        /// <param name="position">Where to display the GUIcon. In this constructor,
        /// the hitbounds are automatically calculated by the image</param>
        /// <param name="BackgroundColor">The tint of the image</param>
        /// <param name="tint">The tint of the background</param>
        public GUIcon(Game game, string name, Vector2 Bounds, Vector2 position, Color BackgroundColor, Color tint)
            : this(game, name)
        {
            HitBounds = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)Bounds.X,
                (int)Bounds.Y);
            colordata = new Color[HitBounds.Width * HitBounds.Height];
            for (int i = 0; i < colordata.Length; i++)
            {
                colordata[i] = BackgroundColor;
            }
            i_half_width = HitBounds.Width / 2;
            i_half_height = HitBounds.Height / 2;
            Center = new Vector2(i_half_width, i_half_height);
            Position = position;
            BGColor = BackgroundColor;
            Tint = tint;
            DrawType = BASIC;
        }

        /// <summary>
        /// Creates a clickable icon for the screen to display
        /// </summary>
        /// <param name="game">The Game in which this GUIcon will be used</param>
        /// <param name="name">The name of the GUIcon</param>
        /// <param name="image">The Texture2D used for image</param>
        /// <param name="position">Where to display the GUIcon. In this constructor,
        /// the hitbounds are automatically calculated by the image</param>
        /// <param name="tint">The tint of the image</param>
        public GUIcon(Game game, string name, Texture2D image, Vector2 position, Color tint)
            : this(game, name)
        {
            Image = image;
            i_image_width = image.Width;
            i_image_height = image.Height;
            i_half_width = i_image_width / 2;
            i_half_height = i_image_height / 2;
            Position = position;
            HitBounds = new Rectangle((int)Position.X, (int)Position.Y, i_image_width, i_image_height);
            Tint = tint;
            Center = new Vector2(i_half_width, i_half_height);
            DrawType = COMMON;
        }

        /// <summary>
        /// Creates a clickable icon for the screen to display
        /// </summary>
        /// <param name="game">The Game in which this GUIcon will be used</param>
        /// <param name="name">The name of the GUIcon</param>
        /// <param name="image">The Texture2D used for image</param>
        /// <param name="position">Where to display the GUIcon</param>
        /// <param name="tint">The tint of the image</param>
        /// <param name="hitBounds">The hitbox for the image</param>
        public GUIcon(Game game, string name, Texture2D image, Vector2 position, Color tint, Rectangle hitBounds)
            : this(game, name)
        {
            Image = image;
            Position = position;
            HitBounds = hitBounds;
            Tint = tint;
            Center = new Vector2(image.Width / 2, image.Height / 2);
            DrawType = COMMON;
        }
        #endregion

        #region Methods
        #region Regular
        /// <summary>
        /// Inherently determines the drawtype based on the method version used
        /// </summary>
        /// <param name="SourceBounds">Standard</param>
        public void DefineDrawType(Rectangle SourceBounds)
        {
            SrcBounds = SourceBounds;
            DrawType = ANIMATE;
        }

        /// <summary>
        /// Inherently determines the drawtype based on the method version used
        /// </summary>
        /// <param name="SourceBounds">Standard</param>
        /// <param name="RotationOrigin">Standard</param>
        /// <param name="Scale">Standard</param>
        /// <param name="spriteEffects">Standard</param>
        public void DefineDrawType(Rectangle SourceBounds, Vector2 RotationOrigin, Vector2 Scale, SpriteEffects spriteEffects)
        {
            SrcBounds = SourceBounds;
            Center = RotationOrigin;
            this.Scale = Scale;
            this.spriteEffects = spriteEffects;
            DrawType = ROTATE;
        }

        /// <summary>
        /// Inherently determines the drawtype based on the method version used
        /// </summary>
        /// <param name="SourceBounds">Standard</param>
        /// <param name="Scale">Standard</param>
        /// <param name="spriteEffects">Standard</param>
        public void DefineDrawType(Rectangle SourceBounds, Vector2 Scale, SpriteEffects spriteEffects)
        {
            SrcBounds = SourceBounds;
            this.Scale = Scale;
            this.spriteEffects = spriteEffects;
            DrawType = ROTATE;
        }

        /// <summary>
        /// Define the parameters for a SpriteFont to draw
        /// </summary>
        /// <param name="font">Standard</param>
        /// <param name="fontColor">Standard</param>
        /// <param name="fontPosition">Standard</param>
        /// <param name="text">What the SpriteFont Object will read</param>
        public void DefineFont(SpriteFont font, Color fontColor, Vector2 fontPosition, string text)
        {
            Font = font;
            FontText = text;
            FontColor = fontColor;
            FontPosition = fontPosition;
        }

        /// <summary>
        /// Used to set a GUIcon equal to a Piece
        /// </summary>
        /// <param name="p"></param>
        public void Equals(GUIcon p)
        {
            HitBounds = p.HitBounds;
            Image = p.Image;
            Center = p.Center;
            Position = p.Position;
            Velocity = p.Velocity;
            Acceleration = p.Acceleration;
            Scale = p.Scale;
            Rotation = p.Rotation;
            Tint = p.Tint;
            IsActive = p.IsActive;
            FontText = p.FontText;
            Font = p.Font;
        }

        #endregion

        /// <summary>
        /// Sets up the GUIcon
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tempBG = new Texture2D(GraphicsDevice, HitBounds.Width, HitBounds.Height);
            tempBG.SetData(colordata);
        }

        internal void DrawGUIcon(GameTime gameTime)
        {
            spriteBatch.Begin();
            switch (DrawType)
            {
                case BASIC:
                    spriteBatch.Draw(tempBG, Position, Tint);
                    break;
                case COMMON:
                    //Draw the image
                    spriteBatch.Draw(Image, Position, Tint);
                    break;
                case ANIMATE:
                    //Draw the image
                    spriteBatch.Draw(Image, Position, SrcBounds, Tint);
                    break;
                case ROTATE:
                    //Draw the image
                    spriteBatch.Draw(Image, Position, SrcBounds, Tint,
                        Rotation, Center, Scale, spriteEffects, 0);
                    /*If I find that layerDepth is very important, the I'll include it as a definable parameter*/
                    break;
            }
            //Draw the Text
            if (Font != null)
                spriteBatch.DrawString(Font, FontText, FontPosition + Position, FontColor);
            spriteBatch.End();
        }

        internal void DrawGUIcon(Vector2 Offset, GameTime gameTime)
        {

            HitBounds.X = (int)(Position.X + Offset.X);
            HitBounds.Y = (int)(Position.Y + Offset.Y);
            spriteBatch.Begin();
            switch (DrawType)
            {
                case BASIC:
                    spriteBatch.Draw(tempBG, Position + Offset, Tint);
                    break;
                case COMMON:
                    //Draw the image
                    spriteBatch.Draw(Image, Position + Offset, Tint);
                    break;
                case ANIMATE:
                    //Draw the image                
                    spriteBatch.Draw(Image, Position + Offset, SrcBounds, Tint);
                    break;
                case ROTATE:
                    //Draw the image                
                    spriteBatch.Draw(Image, Position + Offset, SrcBounds, Tint,
                        Rotation, Center, Scale, spriteEffects, 0);
                    /*If I find that layerDepth is very important, the I'll include it as a definable parameter*/
                    break;
            }
            //Draw the Text
            if (Font != null)
                spriteBatch.DrawString(Font, FontText, FontPosition + Position, FontColor);
            spriteBatch.End();
        }

        /*
         * public GUIcon(string filename)
         * : this()
         * {
         *     Filename = filename;
         * }
         * 
         * public GUIcon(string filename, Rectangle ButtonBounds)
         *     : this()
         * {
         *     Filename = filename;
         *     HitBounds = ButtonBounds;
         * }
         * 
         * public GUIcon(string filename, Rectangle ButtonBounds, Vector2 position)
         *     : this(filename, ButtonBounds)
         * {
         *     Position = position;
         * }
         */

        internal void UpdateGUIcon(MouseState MS)
        {
            CheckForMouseEnterOrExit(MS);
        }

        /// <summary>
        /// Checks to see if the mouse has entered or exited this 
        /// particular GUIcon
        /// </summary>
        /// <param name="MS">State of the mouse when the click occurred</param>
        internal void CheckForMouseEnterOrExit(MouseState MS)
        {
            Point MousePoint = new Point(MS.X, MS.Y);
            if (HitBounds.Contains(MousePoint))
            {
                IsEntered = true;
            }
            else
                IsEntered = false;
        }

        #region Event-Based

        /// <summary>
        /// Throws the GUIconActivation event
        /// </summary>
        private void OnActivate()
        {
            if (Activated != null)
                Activated(this);
        }

        /// <summary>
        /// Throws the GUIconDeactivation event
        /// </summary>
        private void OnDeactivate()
        {
            if (Deactivated != null)
                Deactivated(this);
        }

        /// <summary>
        /// Throws the MethodLauncher event
        /// </summary>
        private void OnMouseLClick()
        {
            if (MouseLClickEvent != null)
                MouseLClickEvent(this);
        }

        /// <summary>
        /// Throws the MethodLauncher event
        /// </summary>
        private void OnMouseRClick()
        {
            if (MouseRClickEvent != null)
                MouseRClickEvent(this);
        }

        /// <summary>
        /// Throws the MouseEnter Event
        /// </summary>
        private void OnMouseEnter()
        {
            if (MouseEnterEvent != null)
                MouseEnterEvent(this);
        }

        /// <summary>
        /// Throwst the MouseExit event
        /// </summary>
        private void OnMouseExit()
        {
            if (MouseExitEvent != null)
                MouseExitEvent(this);
        }


        /// <summary>
        /// Checks to see if the click has occurred on this 
        /// particular GUIcon
        /// </summary>
        /// <param name="MS">State of the mouse when the click occurred</param>
        internal void CheckForLClick(MouseState MS)
        {
            Point MousePoint = new Point(MS.X, MS.Y);
            if (HitBounds.Contains(MousePoint))
            {
                IsLClicked = true;
            }
        }

        /// <summary>
        /// Checks to see if the click has occurred on this 
        /// particular GUIcon
        /// </summary>
        /// <param name="MS">State of the mouse when the click occurred</param>
        internal void CheckForRClick(MouseState MS)
        {
            Point MousePoint = new Point(MS.X, MS.Y);
            if (HitBounds.Contains(MousePoint))
            {
                IsLClicked = true;
            }
        }
        #endregion
        #endregion
    }
}