using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
//Last edited: April 6, 2015
namespace PicAnimator
{
    /// <summary>
    /// A collection of animations, the animation of which cannot be concatenated with another Mover object
    /// </summary>
    public class Mover : DataContainer<Animation>
    {
        #region Public Vars
        /// <summary>
        /// Dictates the state of this Mover object
        /// </summary>
        internal PlayState PState
        {
            get
            {
                return pstate;
            }
            set
            {
                pstate = value;
                if (pstate == PlayState.Pause)
                    OnPause();
                else if (pstate == PlayState.Play)
                    OnPlay();
                else if (pstate == PlayState.Stop)
                    OnStop();
            }
        }

        /// <summary>
        /// The name of the Mover
        /// </summary>
        public string name { get; private set; }

        /// <summary>
        /// Used for registering to the animation beginning its playback
        /// </summary>
        public event AnimatorEventHandler PlayingEvent;

        /// <summary>
        /// Used to register to the animation pausing
        /// </summary>
        public event AnimatorEventHandler PausingEvent;

        /// <summary>
        /// Used to register to the animation stopping
        /// </summary>
        public event AnimatorEventHandler StoppingEvent;

        #endregion
        #region Private Vars
        private PlayState pstate;
        #endregion
        private Mover()
        {
            hasLooped = false;
            InnerItems = new List<Animation>();
        }

        /// <summary>
        /// Allows for a combination of InnerItems to be played
        /// one after the other.
        /// </summary>
        /// <param name="name">The name of the Mover</param>
        /// <param name="loop_for">Determines how many subsequent times the data will loop.
        /// "-1" is infinite, "0" is none.</param>
        public Mover(string name,int loop_for )
            : this()
        {
            this.name = name;
            LoopFor = loop_for;
            if (LoopFor <= INF_CONST)
                IsInfinite = true;
            else
                IsInfinite = false;
            PState = PlayState.Inactive;
        }

        /// <summary>
        /// Allows for a combination of InnerItems to be played
        /// one after the other.
        /// </summary>
        /// <param name="name">The name of the Mover</param>
        /// <param name="data">The list of animas for the animation</param>
        /// <param name="loop_for">Determines how many subsequent times the data will loop.
        /// "-1" is infinite, "0" is none.</param>
        public Mover(string name, List<Animation> data, int loop_for)
            : this(name, loop_for)
        {
            InnerItems = data;
        }

        /// <summary>
        /// Allows for a combination of InnerItems to be played
        /// one after the other.
        /// </summary>
        /// <param name="name">The name of the Mover</param>
        /// <param name="Animas">The list of Animas to add</param>
        /// <param name="mover_loop_for">Determins how many subsequent times the Mover data wil loop.
        /// "-1" is infinite, "0" is none.</param>
        /// <param name="anim_loop_for">The same mover_loop_foor, but for the list of anima</param>
        public Mover(string name, List<Anima> Animas, int mover_loop_for, int anim_loop_for)
            : this(name,mover_loop_for)
        {
            if (mover_loop_for == -1)
            {
                if (anim_loop_for == -1)
                    throw new LoopingException("No infinite sub-loops allowed");
            }

            this.AddData(new Animation(Animas, anim_loop_for));
        }

        /// <summary>
        /// Take a list of frames and turns them into a Mover object
        /// </summary>
        /// <param name="name">The name of the Mover</param>
        /// <param name="Frames">The list of frames to add</param>
        /// <param name="mover_loop_for">Determines how many subsequent times the Mover data wil loop.
        /// "-1" is infinite, "0" is none.</param>
        /// <param name="anim_loop_for">The same mover_loop_foor, but for the automatically-generated animation</param>
        /// <param name="anima_loop_for">The same mover_loop_foor, but for the automatically-generated anima</param>
        public Mover(string name, List<Frame> Frames, int mover_loop_for, int anim_loop_for, int anima_loop_for)
            : this(name, mover_loop_for)
        {
            if (mover_loop_for == -1)
            {
                if (anim_loop_for == -1)
                {
                    throw new LoopingException("No infinite sub-loops allowed");
                }
                if (anima_loop_for == -1)
                {
                    throw new LoopingException("No infinite sub-loops allowed");
                }
            }

            this.AddData(new Animation(anim_loop_for));
            InnerItems[InnerItems.Count - 1].AddData(new Anima(Frames, anima_loop_for));
        }

        /// <summary>
        /// Accepts a base frame to automatically generate a Mover
        /// </summary>
        /// <param name="name">The name of the Mover</param>
        /// <param name="baseFrame">The base frame to use to add the rest</param>
        /// <param name="mover_loop_for">Determines how many subsequent times the Mover data wil loop.
        /// "-1" is infinite, "0" is none.</param>
        /// <param name="anim_loop_for">The same as mover_loop_for, but for the automatically-generated animation</param>
        /// <param name="anima_loop_for">The same as mover_loop_for, but for the automatically-genereated anima</param>
        /// <param name="ExtraFrames">The amount of subsequent frames after the intial frame</param>
        /// <param name="XSourceDelta">The amount of change in the source image file's X axis in relation to the
        /// initial frame</param>
        /// <param name="YSourceDelta">The amount of change in the source image file's Y axis in relation to the
        /// initial frame</param>
        public Mover(string name, Frame baseFrame, int mover_loop_for, int anim_loop_for,
            int anima_loop_for, int ExtraFrames, int XSourceDelta, int YSourceDelta)
            : this(name, mover_loop_for)
        {
            if (mover_loop_for == -1)
            {
                if (anim_loop_for == -1)
                {
                    throw new LoopingException("No infinite sub-loops allowed");
                }
                if (anima_loop_for == -1)
                {
                    throw new LoopingException("No infinite sub-loops allowed");
                }
            }
            this.AddData(new Animation(anim_loop_for));
            InnerItems[InnerItems.Count - 1].AddData(new Anima(baseFrame,anima_loop_for,ExtraFrames,XSourceDelta,YSourceDelta));
        }

        /// <summary>
        /// Adds new Animation objects to the list of the animas
        /// </summary>
        /// <param name="data">The Animation object to be added</param>
        public void AddData(Animation data)
        {
            foreach (Animation anim in InnerItems)
            {
                if (anim.IsInfinite)
                {
                    throw new LoopingException();
                }
            }
            InnerItems.Add(data);
        }

        private void OnPause()
        {
            if (PausingEvent != null)
                PausingEvent();
        }

        private void OnPlay()
        {
            if (PlayingEvent != null)
                PlayingEvent();
        }

        private void OnStop()
        {
            if (StoppingEvent != null)
                StoppingEvent();
        }

        /// <summary>
        /// Runs the Activated event delegate
        /// </summary>
        protected override void OnActivate()
        {
            base.OnActivate();
            foreach(Animation an in InnerItems)
            {
                an.IsActive = true;
            }
        }

        /// <summary>
        /// Runs the Deactivated event delegate
        /// </summary>
        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            foreach (Animation an in InnerItems)
            {
                an.IsActive = false;
            }
        }
    }
}