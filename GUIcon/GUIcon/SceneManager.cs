using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace EZUI
{
    /// <summary>
    /// Handles any clicks that occur
    /// </summary>
    /// <param name="MS">The State of the Mouse</param>
    public delegate void ClickEventHandler(MouseState MS);

    /// <summary>
    /// Handles when the mouse is holding down a button
    /// </summary>
    /// <param name="MS"></param>
    public delegate void HoldEventHandler(MouseState MS);

    /// <summary>
    /// Handles when the mouse has been released from a hold event
    /// </summary>
    /// <param name="MS"></param>
    public delegate void ReleaseEventHandler(MouseState MS);

    /// <summary>
    /// Keeps track of all the scenes within the game
    /// </summary>
    public class SceneManager : DrawableGameComponent
    {
        #region Variable Regions For Copying
        #region Variables
        #region public

        #region Primitives
        #endregion

        #region Objects
        #endregion

        #region Delegates/Events
        #endregion
        #endregion

        #region private

        #region Primitives
        #endregion

        #region Objects
        #endregion

        #region Delegates/Events
        #endregion
        #endregion
        #endregion
        #endregion

        #region Variables

        #region public

        #region Primitives
        /// <summary>
        /// Denotes whether or not mouse events are being raised by the SceneManager
        /// </summary>
        public bool TrackingMouse = true;



        #endregion

        #region Objects

        #endregion

        #region Delegates/Events
        #endregion
        #endregion

        #region private

        #region Primitives
        private bool potentialLHold = false, potentialRHold = false;
        /// <summary>
        ///1
        ///</summary>
        private const int DIST_TOL = 2;
        #endregion

        #region Objects
        private SpriteBatch spriteBatch;
        private Scene ActiveScene, DefaultActiveScene;
        MouseState mState, prevMState, secMState;

        private Dictionary<string, Scene> Scenes;
        private Game main_game;
        private Vector2 firstPosition, secondPosition;
        private bool HoldMove = false;

        const int lClickColor = 0,
            rClickColor = 1,
            lHoldColor = 2,
            rHoldColor = 3;

        Color[] colors;

        Texture2D circle;
        SpriteFont debugFont;

        /// <summary>
        /// Toggles debug utilities on and off
        /// </summary>
        bool isDebug = false;

        #endregion

        #region Delegates/Events

        private event ClickEventHandler LClickEvent, RClickEvent;
        private event HoldEventHandler LHoldEvent, RHoldEvent;
        private event ReleaseEventHandler LReleaseEvent, RReleaseEvent;
        #endregion
        #endregion
        #endregion

        /// <summary>
        /// A class that keeps a collection of scenes and maintains the order to their drawing.
        /// </summary>
        /// <param name="game">The game in which the class is being used</param>
        /// <param name="DefaultScene">The starting scene</param>
        public SceneManager(Game game, Scene DefaultScene)
            : base(game)
        {
            main_game = game;
            Scenes = new Dictionary<string, Scene>();
            Scenes.Add(DefaultScene.Name, DefaultScene);

            DefaultActiveScene = DefaultScene;
            ActiveScene = DefaultActiveScene;

            this.LClickEvent += new ClickEventHandler(DefaultScene.OnLClickEvent);
            this.RClickEvent += new ClickEventHandler(DefaultScene.OnRClickEvent);
            LHoldEvent += new HoldEventHandler(DefaultScene.OnLHoldEvent);
            RHoldEvent += new HoldEventHandler(DefaultScene.OnRHoldEvent);
            LReleaseEvent += new ReleaseEventHandler(DefaultScene.OnLReleaseEvent);
            RReleaseEvent += new ReleaseEventHandler(DefaultScene.OnRReleaseEvent);

            main_game.Components.Add(this);
            game.IsMouseVisible = true;

            colors = new Color[4];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.White;
                colors[i].A = 0;
            }
        }

        /// <summary>
        /// Adds scenes to the collection of scenes
        /// </summary>
        /// <param name="SceneObject">The Scene Object</param>
        public void AddScene(Scene SceneObject)
        {
            //Check for duplicate names
            Scene temp = FindScene(SceneObject.Name);
            if (temp != null)
            {
                Scenes.Add(SceneObject.Name, SceneObject);
                SceneObject.Initialize();
                LClickEvent += new ClickEventHandler(SceneObject.OnLClickEvent);
                RClickEvent += new ClickEventHandler(SceneObject.OnRClickEvent);
                LHoldEvent += new HoldEventHandler(SceneObject.OnLHoldEvent);
                RHoldEvent += new HoldEventHandler(SceneObject.OnRHoldEvent);
                LReleaseEvent += new ReleaseEventHandler(SceneObject.OnLReleaseEvent);
                RReleaseEvent += new ReleaseEventHandler(SceneObject.OnRReleaseEvent);
            }
        }

        /// <summary>
        /// Finds a Scene Object
        /// </summary>
        /// <param name="name">The name of the Scene</param>
        /// <returns>Return the Scene if found. Else, it returns null</returns>
        public Scene FindScene(string name)
        {
            if (Scenes.Keys.Contains(name))
                return Scenes[name];
            else return null;
        }

        /// <summary>
        /// Sets up the SceneManager to display debug information
        /// </summary>
        /// <param name="debugSprite">A necessary sprite</param>
        /// <param name="font">a necessary SpriteFont</param>
        public void SetupDebug(Texture2D debugSprite, SpriteFont font)
        {
            circle = debugSprite;
            debugFont = font;
            isDebug = true;
        }

        /// <summary>
        /// Turns off debug mode
        /// </summary>
        public void TurnOffDebug()
        {
            isDebug = false;
        }

        /// <summary>
        /// Sets up the SceneManager
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        /// <summary>
        /// Updates SceneManager: THIS DOES NOT NEED TO BE CALLED
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (TrackingMouse)
            {
                mState = Mouse.GetState();

                //TODO: Find out how to change the cursor
                //Clicks
                if ((potentialLHold || potentialRHold) && !HoldMove)
                {
                    secondPosition = new Vector2(mState.X, mState.Y);
                    if (Vector2.Distance(firstPosition, secondPosition) > DIST_TOL)
                    {
                        if (potentialLHold)
                        {
                            LHoldEvent(mState);
                            colors[lHoldColor] = Color.White;
                            colors[lHoldColor].A = 255;
                        }
                        else
                        {
                            RHoldEvent(mState);
                            colors[rHoldColor] = Color.White;
                            colors[rHoldColor].A = 255;
                        }
                        HoldMove = true;
                    }
                }
                if (!HoldMove)
                {
                    if ((mState.LeftButton == ButtonState.Released && prevMState.LeftButton == ButtonState.Pressed) && LClickEvent != null)
                    {
                        LClickEvent(mState);

                        colors[lClickColor].A = 255;
                    }
                    if ((mState.RightButton == ButtonState.Released && prevMState.RightButton == ButtonState.Pressed) && RClickEvent != null)
                    {
                        RClickEvent(mState);

                        colors[rClickColor].A = 255;
                    }
                }

                //Hold
                if ((mState.LeftButton == ButtonState.Pressed &&
                    prevMState.LeftButton == ButtonState.Pressed &&
                    secMState.LeftButton == ButtonState.Released) && LHoldEvent != null)
                {
                    potentialLHold = true;
                    firstPosition = new Vector2(mState.X, mState.Y);
                    colors[lHoldColor] = Color.Yellow;
                    colors[lHoldColor].A = 255;
                }

                if ((mState.RightButton == ButtonState.Pressed &&
                    prevMState.RightButton == ButtonState.Pressed &&
                    secMState.RightButton == ButtonState.Released) && RHoldEvent != null)
                {
                    potentialRHold = true;
                    firstPosition = new Vector2(mState.X, mState.Y);
                    colors[rHoldColor] = Color.Yellow;
                    colors[rHoldColor].A = 255;
                }

                //Release
                if (potentialLHold && HoldMove &&
                    (mState.LeftButton == ButtonState.Released &&
                    prevMState.LeftButton == ButtonState.Released) && LReleaseEvent != null)
                {
                    potentialLHold = false;
                    HoldMove = false;
                    LReleaseEvent(mState);
                }

                if (potentialRHold && HoldMove &&
                    (mState.RightButton == ButtonState.Released &&
                    prevMState.RightButton == ButtonState.Released) && RReleaseEvent != null)
                {
                    potentialRHold = false;
                    HoldMove = false;
                    RReleaseEvent(mState);
                }

                secMState = prevMState;
                prevMState = mState;
            }
            //TODO: Add the Header Dragging  capability

            if (ActiveScene == null)
                ActiveScene = DefaultActiveScene;

            if (ActiveScene.IsActive)
                ActiveScene.UpdateScene(gameTime, mState);
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i].A > 0)
                {
                    if (colors[i].A - 4 >= 0)
                        colors[i].A -= 4;
                    else if (colors[i].A - 4 < 0)
                    {
                        colors[i].A -= colors[i].A;
                    }
                }
            }
        }

        /// <summary>
        /// Draws the active Scene
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (ActiveScene != null && ActiveScene.IsActive)
                GraphicsDevice.Clear(ActiveScene.BGColor);
            else
                GraphicsDevice.Clear(Color.Black);



            spriteBatch.Begin();
            if (isDebug && circle != null)
            {
                for (int i = 0; i < colors.Length; i++)
                {
                    spriteBatch.Draw(circle, new Vector2(0, i * 34), colors[i]);
                }
                if (debugFont != null)
                {
                    spriteBatch.DrawString(debugFont, mState.LeftButton.ToString(), new Vector2(0, 4 * 34 + 10), Color.Black);
                    spriteBatch.DrawString(debugFont, prevMState.LeftButton.ToString(), new Vector2(0, 4 * 34 + 25), Color.Black);
                    spriteBatch.DrawString(debugFont, secMState.LeftButton.ToString(), new Vector2(0, 4 * 34 + 40), Color.Black);
                    spriteBatch.DrawString(debugFont, firstPosition.ToString(), new Vector2(0, 4 * 34 + 55), Color.Black);
                    spriteBatch.DrawString(debugFont, secondPosition.ToString(), new Vector2(0, 4 * 34 + 70), Color.Black);
                }
            }
            /*
            if (ActiveScene == null)
                ActiveScene = DefaultActiveScene;
            */
            if (ActiveScene.IsActive)
                ActiveScene.DrawScene(gameTime);

            spriteBatch.End();

        }
    }
}
