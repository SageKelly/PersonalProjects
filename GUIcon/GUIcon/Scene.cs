using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EZUI
{
    /// <summary>
    /// Handles any of the Scene's activities
    /// </summary>
    /// <param name="S">The calling Scene</param>
    public delegate void ActivityHandler(Scene S);

    /// <summary>
    /// Represents a game state
    /// </summary>
    public class Scene : DrawableGameComponent
    {
        #region Variables
        #region PUBLIC

        #region Primitives
        /// <summary>
        /// The meta-name of the Scene
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Denotes whether or not the Scene is active
        /// </summary>
        public bool IsActive
        {
            get { return b_is_active; }
            set
            {
                bool temp = b_is_active;
                b_is_active = value;
                if (b_is_active && !temp)
                    OnActivate();
                else if (!b_is_active && temp)
                    OnDeactivate();
            }
        }


        #endregion

        #region Objects

        /// <summary>
        /// The background color for the Scene
        /// </summary>
        public Color BGColor;
        #endregion

        #region Delegates/Events
        /// <summary>
        /// An event that triggers when the Scene is turned on
        /// </summary>
        public event ActivityHandler Activate;

        /// <summary>
        /// An event that triggers when the Scene is turned off
        /// </summary>
        public event ActivityHandler Deactivate;
        #endregion

        #endregion

        #region PRIVATE


        #region Primitives
        private bool b_is_active;
        #endregion

        #region Objects
        private List<Widget> Widgets;
        private List<Widget> ActiveWidgets;
        private List<Widget> Clickable;
        private Widget InFocusWidget;

        /// <summary>
        /// The list of GUIcons
        /// </summary>
        private List<GUIcon> GUICs;

        private Game game;
        #endregion

        #region Delegates/Events
        internal ClickEventHandler LClickEvent, RClickEvent;
        #endregion
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// A set of Widgets for a particular game state
        /// </summary>
        /// <param name="game">The game in which this scene will be</param>
        /// <param name="name">The name of the Scene</param>
        /// <param name="BackgroundColor">The color of the background for the scene</param>
        public Scene(Game game, string name, Color BackgroundColor)
            : base(game)
        {
            this.game = game;
            Name = name;
            Widgets = new List<Widget>();
            ActiveWidgets = new List<Widget>();
            Clickable = new List<Widget>();
            GUICs = new List<GUIcon>();
            BGColor = BackgroundColor;
            //graphics = new GraphicsDeviceManager(game);
        }

        /// <summary>
        /// A set of Widgets for a particular game state
        /// </summary>
        /// <param name="game">The game in which this scene will be</param>
        /// <param name="name">The name of the Scene</param>
        /// <param name="BackgroundColor">The color of the background for the scene</param>
        /// <param name="widgets">Predefined Widgets to be inserted into this Scene</param>
        public Scene(Game game, string name, Color BackgroundColor, List<Widget> widgets)
            : this(game, name, BackgroundColor)
        {
            this.Widgets = widgets;
            foreach (Widget w in Widgets)
            {
                w.Activated += new WidgetActivityHandler(AddToActiveList);
                w.Deactivated += new WidgetActivityHandler(RemoveFromActiveList);
            }
        }
        #endregion

        #region Methods
        #region Event-Based
        private void OnActivate()
        {
            if (Activate != null)
                Activate(this);
            foreach (GUIcon GUIC in GUICs)
            {
                GUIC.IsActive = true;
            }
        }

        private void OnDeactivate()
        {
            if (Deactivate != null)
                Deactivate(this);
            foreach (Widget w in Widgets)
            {
                w.IsActive = false;
            }
            foreach (GUIcon GUIC in GUICs)
            {
                GUIC.IsActive = false;
            }
        }

        internal bool CheckForFocusSwitch(MouseState MS, bool L, bool click)
        {
            //Holds the focus switch value
            bool temp = false;
            //Checks for focus switching
            for (int i = 0; i < Clickable.Count; i++)
            {
                Widget W = Clickable[i];

                W.CheckForPointerCollision(MS, L, click);

                if (W == InFocusWidget)
                    continue;//Since this Widget is in focus nothing happened
                else if (W != InFocusWidget)
                {
                    if (!InFocusWidget.isLClicked || !InFocusWidget.isRClicked)//i.e. this widget wasn't clicked THROUGH the InFocusWidget
                    {
                        //Then switch focus to this one and move it to the top of the lists
                        InFocusWidget.InFocus = false;
                        InFocusWidget = W;
                        InFocusWidget.InFocus = true;

                        ActiveWidgets.Remove(W);
                        ActiveWidgets.Insert(0, W);

                        Clickable.Remove(W);
                        Clickable.Insert(0, W);
                        temp = true;
                        break;
                    }
                }
            }

            for (int i = 0; i < Clickable.Count; i++)
            {
                if (Clickable[i] != InFocusWidget)
                {
                    if (L)
                        Clickable[i].isLClicked = false;
                    else
                        Clickable[i].isRClicked = false;
                }
            }
            return temp;
            /*At the end of this method, one of three cases may exist:
             * 1. A focus-switch occurred
             * 2. No focus-switch occurred and no widgets were clicked
             * 3. No focus-switch occurred and only the InFocusWidget was clicked
            */
        }

        internal void OnLClickEvent(MouseState MS)
        {
            if (!CheckForFocusSwitch(MS, true, true) && InFocusWidget != null)
            {
                //Since a focus switch didn't happen you can check for button clicks
                InFocusWidget.CheckForGUIClick(MS, true);
            }
            else if (InFocusWidget == null || !InFocusWidget.isLClicked && LClickEvent != null)
            {
                LClickEvent(MS);
                if (InFocusWidget != null)
                    InFocusWidget.isLClicked = false;
                /*This is sensitive to cases 2 and 3, for if either happened
                 * this should never happen, or else a GUIcon that belongs to
                 * the Scene may be clicked through a Widget.
                 */
            }
        }

        internal void OnRClickEvent(MouseState MS)
        {

            if (!CheckForFocusSwitch(MS, false, true) && InFocusWidget != null)
            {
                InFocusWidget.CheckForGUIClick(MS, false);
            }
            else if (InFocusWidget == null || !InFocusWidget.isRClicked && RClickEvent != null)
            {
                RClickEvent(MS);
                if (InFocusWidget != null)
                    InFocusWidget.isRClicked = false;
            }
        }

        internal void OnLHoldEvent(MouseState MS)
        {
            CheckForFocusSwitch(MS, true, false);

            if (InFocusWidget != null)
                InFocusWidget.CheckForHeaderGrab(MS);
        }

        internal void OnRHoldEvent(MouseState MS)
        {
            CheckForFocusSwitch(MS, false, false);

            if (InFocusWidget != null)
                InFocusWidget.CheckForHeaderGrab(MS);
        }

        internal void OnLReleaseEvent(MouseState MS)
        {
            if (InFocusWidget != null)
                InFocusWidget.CheckForHeaderRelease(MS);
        }

        internal void OnRReleaseEvent(MouseState MS)
        {
            if (InFocusWidget != null)
                InFocusWidget.CheckForHeaderRelease(MS);
        }

        /// <summary>
        /// Adds a Widget to the Active and Clickable Widget lists
        /// </summary>
        /// <param name="W">The alleged Widget</param>
        private void AddToActiveList(Widget W)
        {
            ActiveWidgets.Insert(0, W);

            if (W.ForceFocus)
                Clickable.Clear();

            Clickable.Insert(0, W);
            if (InFocusWidget != null)
                InFocusWidget.InFocus = false;
            InFocusWidget = W;
            InFocusWidget.InFocus = true;
        }

        /// <summary>
        /// Removes a Widget from the Active and Clickable Widget lists
        /// </summary>
        /// <param name="W">The alleged Widget</param>
        private void RemoveFromActiveList(Widget W)
        {
            W.InFocus = false;
            ActiveWidgets.Remove(W);
            Clickable.Remove(W);
            if (W.ForceFocus)
            {
                //Remove everything, then add back the ones that should be clickable now...
                Clickable.Clear();
                for (int i = 0; i < ActiveWidgets.Count; i++)
                {
                    Clickable.Add(ActiveWidgets[i]);
                    //...up to the next forcefocus Widget
                    if (ActiveWidgets[i].ForceFocus)
                        break;
                }
            }
            if (ActiveWidgets.Count != 0)
            {
                InFocusWidget = ActiveWidgets[0];
                InFocusWidget.InFocus = true;
            }
            else
            {
                InFocusWidget = null;
            }
        }
        #endregion

        #region Regular
        /// <summary>
        /// Finds a Widget by name
        /// </summary>
        /// <param name="name">The name of Widget being sought</param>
        /// <returns>Returns the Widget if found. Else returns null</returns>
        public Widget FindWidget(string name)
        {
            return Widgets.FirstOrDefault(X => X.Name == name);
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
        /// Adds a Widget to the Scene
        /// </summary>
        /// <param name="W">The Widget to be added</param>
        public void AddWidget(Widget W)
        {
            Widgets.Add(W);
            W.Initialize();
            W.Activated += new WidgetActivityHandler(AddToActiveList);
            W.Deactivated += new WidgetActivityHandler(RemoveFromActiveList);
        }

        /// <summary>
        /// Adds a GUIcon to the Scene. Is active by default
        /// </summary>
        /// <param name="GUIC">The GUIcon to be added</param>
        public void AddGUIcon(GUIcon GUIC)
        {
            GUICs.Add(GUIC);
            GUIC.Initialize();
            GUIC.IsActive = true;
            LClickEvent += new ClickEventHandler(GUIC.CheckForLClick);
            RClickEvent += new ClickEventHandler(GUIC.CheckForRClick);
        }

        #endregion

        #region GameComponent Methods

        /// <summary>
        /// Updates the Scene
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="MS">Mouse State</param>
        internal void UpdateScene(GameTime gameTime, MouseState MS)
        {
            base.Update(gameTime);
            if (ActiveWidgets.Count > 0)
                foreach (Widget w in ActiveWidgets)
                {
                    w.UpdateWidget(MS);
                }
            else
                foreach (GUIcon GUIC in GUICs)
                {
                    GUIC.UpdateGUIcon(MS);
                }
        }


        /// <summary>
        /// Draws the Scene
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        internal void DrawScene(GameTime gameTime)
        {
            foreach (GUIcon GUIC in GUICs)
            {
                GUIC.DrawGUIcon(gameTime);
            }

            for (int index = ActiveWidgets.Count - 1; index >= 0; index--)
            {
                ActiveWidgets[index].Draw(gameTime);
            }
        }
        #endregion
        #endregion
    }
}
