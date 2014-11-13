using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EZUI
{
    /// <summary>
    /// Handles Focusing events
    /// </summary>
    public delegate void FocusHandler();

    /// <summary>
    /// Handles any Widget activity
    /// </summary>
    /// <param name="W">The calling Widget</param>
    public delegate void WidgetActivityHandler(Widget W);

    /// <summary>
    /// Handles clicking on the Widget
    /// </summary>
    /// <param name="W">The calling Widget</param>
    public delegate void WidgetClickHandler(Widget W);

    /// <summary>
    /// A window/ dialog box for the Scene
    /// </summary>
    public class Widget : DrawableGameComponent
    {
        /*
         * WHAT I CAN DO:
         * →Define a list of GUIcons
         * →Define and draw a Background and Background Color
         * →Define and draw the GUIcons
         */

        /**/
        #region Variables
        #region Public
        #region Primitives
        public int WindowWidth, WindowHeight;

        /// <summary>
        /// Denotes whether of not the Widget is in focus
        /// </summary>
        public bool InFocus
        {
            get
            {
                return in_focus;
            }
            set
            {
                bool temp = in_focus;
                in_focus = value;
                if (in_focus && !temp)
                    OnFocus();
                else if (!in_focus && temp)
                    OnLostFocus();
            }
        }

        /// <summary>
        /// Denotes whether or not the Widget is open
        /// </summary>
        public bool IsActive
        {
            get
            {
                return is_active;
            }
            set
            {
                bool temp = is_active;
                is_active = value;
                if (is_active && !temp)
                    OnActivate();
                else if (!is_active && temp)
                    OnDeactivate();
            }
        }

        /// <summary>
        /// The meta name of the Widget
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// What the Widget's Title bar will say
        /// </summary>
        public string HeaderText
        {
            get;
            private set;
        }

        /// <summary>
        /// Denotes whether or not this Widget will open
        /// other Widgets that force focus until completion
        /// </summary>
        public bool ForceFocus
        {
            get;
            private set;
        }
        #endregion

        #region Objects

        /// <summary>
        /// The back ground color for the widget
        /// </summary>
        public Color BackGroundColor;

        /// <summary>
        /// The tint for the font
        /// </summary>
        public Color MaskingTint;

        /// <summary>
        /// The GUIcon used for the background
        /// </summary>
        public GUIcon Background;

        /// <summary>
        /// The GUIcon used for the background image of the header of the Widget
        /// </summary>
        public GUIcon HeaderBackground;

        /// <summary>
        /// Offset position for the Widget and its GUIcons collectively
        /// </summary>
        public Vector2 Position
        {
            get;
            private set;
        }

        /// <summary>
        /// The color of the Header
        /// </summary>
        public Color HeaderColor;

        /// <summary>
        /// The tint of the title bar's text
        /// </summary>
        public Color HeaderTextColor;

        #endregion

        #region Delegates/Events
        /// <summary>
        /// For when the Widget gains focus/gets clicked on
        /// </summary>
        public event FocusHandler GainFocus;

        /// <summary>
        /// For the when the Widget loses focus
        /// </summary>
        public event FocusHandler LoseFocus;

        /// <summary>
        /// For when the Widget pops up
        /// </summary>
        public event WidgetActivityHandler Activated;

        /// <summary>
        /// For when the Widget
        /// </summary>
        public event WidgetActivityHandler Deactivated;

        /// <summary>
        /// Handles when the Widget has been Left clicked
        /// </summary>
        public event WidgetClickHandler LClicked;

        /// <summary>
        /// Handles when the Widget has been right clicked
        /// </summary>
        public event WidgetClickHandler RClicked;

        #endregion
        #endregion


        #region Private

        #region Primitives
        private bool in_focus, is_active, headerGrabbed = false;

        internal bool isLClicked
        {
            get;
            set;
        }
        internal bool isRClicked
        {
            get;
            set;
        }
        int headerHeight;
        #endregion

        #region Objects
        Game game;
        private List<GUIcon> GUICs;
        Viewport view;
        /*
         * Will be used to determine how much room is given to 
         * clicks for the widget. It will represent the area 
         * surrounding all of the GUIC's within the widget Any
         * click within this box will only affect this widget.
         */

        internal Rectangle HeaderHitbounds;

        /// <summary>
        /// Hitbounds for the Widget
        /// </summary>
        internal Rectangle Hitbounds;

        private Vector2 BackgroundSize;
        private Vector2 mouseHeaderDistance;
        Color[] bgColorData;
        Texture2D BGRect;

        SpriteFont headerFont;

        SpriteBatch spriteBatch;
        #endregion

        #region Delegates/Events
        #endregion

        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Sets up an interface Scene
        /// </summary>
        /// <param name="game">The game in which this will be</param>
        /// <param name="name">The name of the Widget</param>
        internal Widget(Game game, string name)
            : base(game)
        {
            this.game = game;
            GUICs = new List<GUIcon>();
            BackGroundColor = Color.CornflowerBlue;
            MaskingTint = new Color(255, 255, 255, 0);
            Name = name;
            isLClicked = isRClicked = false;
        }

        /// <summary>
        /// Sets up an interface Scene
        /// </summary>
        /// <param name="game">The game in which this will be</param>
        /// <param name="name">The name of the Widget</param>
        /// <param name="bgColor">The color of the background to be drawn
        /// <param name="position">The position of the main body of the Widget</param>
        /// <param name="BGSize">The startup size of the Background for the Widget </param>
        /// if no picture is necessary(smallest is 5 X 11)</param>
        /// <param name="headerHeight">The height of the draggable header of the Widget (can't be smaller than 10)</param>
        /// <param name="headerColor">The color of the title bar</param>
        /// <param name="forceFocus">To designate this widget holding focus until deactivation</param>
        public Widget(Game game, string name, Color bgColor, Vector2 position,
            Vector2 BGSize, int headerHeight, Color headerColor, bool forceFocus)
            : this(game, name)
        {
            BackGroundColor = bgColor;
            BackgroundSize = BGSize;
            if (BGSize.Y < 10) BGSize.Y = 11;
            if (BGSize.X <= 0) BGSize.X = 5;
            Position = position;
            this.headerHeight = headerHeight >= 10 ? headerHeight : 10;

            Hitbounds = new Rectangle((int)Position.X, (int)Position.Y + headerHeight, (int)BGSize.X, (int)BGSize.Y - headerHeight);
            HeaderHitbounds = new Rectangle((int)Position.X, (int)Position.Y, (int)BGSize.X, headerHeight);

            ForceFocus = forceFocus;

            bgColorData = new Color[(int)BGSize.X * (int)BGSize.Y];
            for (int index = 0; index < bgColorData.Length; index++)
                bgColorData[index] = bgColor;

            for (int index = 0; index < BGSize.X * headerHeight; index++)
                bgColorData[index] = headerColor;
        }

        /// <summary>
        /// Sets up an interface Scene
        /// </summary>
        /// <param name="game">The game in which this will be</param>
        /// <param name="name">The name of the Widget</param>
        /// <param name="position">The position of the main body of the Widget</param>
        /// <param name="background">Sets a user-define background for the main body of the Widget</param>
        /// <param name="headerBackgroud">Sets a user-defined background for the header of the Widget</param>
        public Widget(Game game, string name, Vector2 position, GUIcon background, GUIcon headerBackgroud)
            : this(game, name)
        {
            Position = position;
            Background = background;
            HeaderBackground = headerBackgroud;
            if (Background.SrcBounds == Rectangle.Empty)
            {
                Hitbounds = new Rectangle((int)Position.X, (int)Position.Y, background.Image.Width, background.Image.Height);
            }
            else
            {
                Hitbounds = new Rectangle((int)Position.X, (int)Position.Y, Background.SrcBounds.Width, Background.SrcBounds.Height);
            }

            if (HeaderBackground.SrcBounds == Rectangle.Empty)
            {
                HeaderHitbounds = new Rectangle((int)Position.X, (int)Position.Y,
                    HeaderBackground.Image.Width, HeaderBackground.Image.Height);
            }
            else
            {
                HeaderHitbounds = new Rectangle((int)Position.X, (int)Position.Y,
                    HeaderBackground.SrcBounds.Width, HeaderBackground.SrcBounds.Height);
            }
        }

        /// <summary>
        /// Sets up an interface Scene
        /// </summary>
        /// <param name="game">The game in which this will be</param>
        /// <param name="name">The name of the Widget</param>
        /// <param name="position">The position of the main body of the Widget</param>
        /// <param name="background">Sets a user-defined background for the entire Widget</param>
        /// <param name="headerHeight">The height of the draggable header of the Widget (can't be smaller than 10)</param>
        public Widget(Game game, string name, Vector2 position, GUIcon background, int headerHeight)
            : this(game, name)
        {
            Position = position;
            Background = background;
            this.headerHeight = headerHeight >= 10 ? headerHeight : 10;
            Hitbounds = new Rectangle((int)position.X, (int)position.Y + headerHeight,
                Background.Image.Width, background.Image.Height - headerHeight);

            HeaderHitbounds = new Rectangle((int)Position.X, (int)Position.Y, Background.Image.Width, headerHeight);
        }
        #endregion

        #region Methods

        #region Event-Based
        private void OnActivate()
        {
            if (Activated != null)
                Activated(this);
            InFocus = true;
            foreach (GUIcon GUIC in GUICs)
            {
                GUIC.IsActive = true;
            }
        }

        private void OnDeactivate()
        {
            if (Deactivated != null)
                Deactivated(this);
            InFocus = false;

            foreach (GUIcon GUIC in GUICs)
            {
                GUIC.IsActive = false;
            }
        }

        private void OnLostFocus()
        {
            if (LoseFocus != null)
                LoseFocus();
        }

        private void OnFocus()
        {
            if (GainFocus != null)
                GainFocus();
        }
        #endregion

        #region Regular

        /// <summary>
        /// Adds a GUIcon to the Widget
        /// </summary>
        /// <param name="GUIC">The GUIcon to be added</param>
        public void AddGUIcon(GUIcon GUIC)
        {
            GUICs.Add(GUIC);
            CalculateBoundSize();
            GUIC.Initialize();
        }

        /// <summary>
        /// Finds a GUIcon by name
        /// </summary>
        /// <param name="name">The name of GUIcon being sought</param>
        /// <returns>Returns the GUIcon if found. Else returns null</returns>
        public GUIcon FindGUIcon(string name)
        {
            return GUICs.FirstOrDefault(X => X.Name == name);
        }

        /// <summary>
        /// Defines the details of the header for this Widget
        /// </summary>
        /// <param name="headerFont">The SpriteFont used for the header</param>
        /// <param name="text">What the header will say</param>
        /// <param name="textColor">The color of the header's text</param>
        public void DefineHeader(SpriteFont headerFont, string text, Color textColor)
        {
            this.headerFont = headerFont;
            HeaderText = text;
            HeaderTextColor = textColor;
        }

        private void CalculateBoundSize()
        {
            /*
             * In order to be able to calculate the bound size
             * of the rectangle, you'll need to know each GUIC's
             * button Texture2D and position, and their rotation 
             * origin location. Then adjust size for each of the 
             * widgets: check to see if they exceed the bounds of 
             * the already established widget in any way.
             */
            int LeastX = 0, LeastY = 0, MaxHeight = 0, MaxWidth = 0;
            foreach (GUIcon g in GUICs)
            {
                if (g.HitBounds.X < LeastX)
                    LeastX = g.HitBounds.X;
                if (g.HitBounds.X + g.HitBounds.Width > MaxWidth)
                    MaxWidth = g.HitBounds.X + g.HitBounds.Width;
                if (g.HitBounds.Y < LeastY)
                    LeastY = g.HitBounds.Y;
                if (g.HitBounds.Y + g.HitBounds.Height > MaxHeight)
                    MaxHeight = g.HitBounds.Y + g.HitBounds.Height;
            }
        }
        #endregion

        #region Internal Methods

        internal void CheckForPointerCollision(MouseState MS, bool L, bool click)
        {
            Point MousePoint = new Point(MS.X, MS.Y);
            if (Hitbounds.Contains(MousePoint) || HeaderHitbounds.Contains(MousePoint))
            {
                if (L)
                {
                    isLClicked = true;
                    if (click && LClicked != null)
                        LClicked(this);
                }
                else
                {
                    isRClicked = true;
                    if (click && RClicked != null)
                        RClicked(this);
                }
            }
        }

        internal void CheckForGUIClick(MouseState MS, bool L)
        {
            foreach (GUIcon GUIC in GUICs)
            {
                if (L)
                    GUIC.CheckForLClick(MS);
                else
                    GUIC.CheckForRClick(MS);
            }
        }

        internal void CheckForHeaderGrab(MouseState MS)
        {
            if (HeaderHitbounds.Contains(new Point(MS.X, MS.Y)))
            {
                headerGrabbed = true;
                mouseHeaderDistance = new Vector2(MS.X - Position.X, MS.Y - Position.Y);
            }
        }

        internal void CheckForHeaderRelease(MouseState MS)
        {
            if (headerGrabbed)
            {
                headerGrabbed = false;
                mouseHeaderDistance = Vector2.Zero;
            }
        }
        #endregion

        #region GameComponent Methods
        /// <summary>
        /// Sets up the Widget
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            BGRect = new Texture2D(GraphicsDevice, Hitbounds.Width, Hitbounds.Height + headerHeight);
            BGRect.SetData(bgColorData);

            foreach (GUIcon GUIC in GUICs)
            {
                GUIC.Initialize();
            }
        }

        internal void UpdateWidget(MouseState MS)
        {

            if (headerGrabbed)
            {
                Vector2 temp = new Vector2(MS.X, MS.Y);
                Position = temp - mouseHeaderDistance;
                Hitbounds.X = (int)Position.X;
                Hitbounds.Y = (int)Position.Y + headerHeight;
                HeaderHitbounds.X = (int)Position.X;
                HeaderHitbounds.Y = (int)Position.Y;
            }

            foreach (GUIcon GUIC in GUICs)
            {
                GUIC.UpdateGUIcon(MS);
            }
        }



        /// <summary>
        /// Draws the Widget
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //Draw the Background
            if (Background != null)
            {
                spriteBatch.Draw(Background.Image, Position, Background.Tint);
            }
            else
            {
                spriteBatch.Draw(BGRect, Position, Color.White);
            }

            if (headerFont != null)
            {
                spriteBatch.DrawString(headerFont, HeaderText, Position, HeaderTextColor);
            }
            spriteBatch.End();

            //Draw the intermediates
            foreach (GUIcon GUIC in GUICs)
            {
                GUIC.DrawGUIcon(Position, gameTime);
            }

            //Draw the overlay
            //spriteBatch.Draw(,Vector2.Zero,MaskingTint);
        }
        #endregion
        #endregion
    }
}
