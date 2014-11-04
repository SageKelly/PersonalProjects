using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PicAnimator
{
    /// <summary>
    /// A collection of animations, the animation of which cannot be concatenated with another Mover object
    /// </summary>
    public class Mover : DataContainer
    {
        #region Public Vars
        /// <summary>
        /// Dictates the state of this Mover object
        /// </summary>
        public PlayState PState
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
        /// A collection of Animation objects
        /// </summary>
        public List<Animation> Animations
        {
            get;
            private set;
        }

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
            Animations = new List<Animation>();
        }

        /// <summary>
        /// Allows for a combination of Animations to be played
        /// one after the other.
        /// </summary>
        /// <param name="loop_for">Determines how many subsequent times the data will loop.
        /// "-1" is infinite, "0" is none.</param>
        public Mover(int loop_for)
            : this()
        {
            LoopFor = loop_for;
            if (LoopFor <= INF_CONST)
                IsInfinite = true;
            else
                IsInfinite = false;
            PState = PlayState.Inactive;

            Animations = new List<Animation>();
        }

        /// <summary>
        /// Allows for a combination of Animations to be played
        /// one after the other.
        /// </summary>
        /// <param name="data">The list of animas for the animation</param>
        /// <param name="loop_for">Determines how many subsequent times the data will loop.
        /// "-1" is infinite, "0" is none.</param>
        public Mover(List<Animation> data, int loop_for)
            : this(loop_for)
        {
            Animations = data;
        }

        /// <summary>
        /// Allows for a combination of Animations to be played
        /// one after the other.
        /// </summary>
        /// <param name="Animas">The list of Animas to add</param>
        /// <param name="mover_loop_for">Determins how many subsequent times the mover data wil loop.
        /// "-1" is infinite, "0" is none.</param>
        /// <param name="anim_loop_for">The same mover_loop_foor, but for the list of anima</param>
        public Mover(List<Anima> Animas, int mover_loop_for, int anim_loop_for)
            : this(mover_loop_for)
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
        /// <param name="Frames">The list of frames to add</param>
        /// <param name="mover_loop_for">Determines how many subsequent times the mover data wil loop.
        /// "-1" is infinite, "0" is none.</param>
        /// <param name="anim_loop_for">The same mover_loop_foor, but for the automatically-generated animation</param>
        /// <param name="anima_loop_for">The same mover_loop_foor, but for the automatically-generated anima</param>
        public Mover(List<Frame> Frames, int mover_loop_for, int anim_loop_for, int anima_loop_for)
            : this(mover_loop_for)
        {
            if (anima_loop_for == -1)
            {
                if (anim_loop_for == -1)
                {
                    throw new LoopingException("No infinite sub-loops allowed");
                }
                if (mover_loop_for == -1)
                {
                    throw new LoopingException("No infinite sub-loops allowed");
                }
            }

            this.AddData(new Animation(anim_loop_for));
            Animations[Animations.Count - 1].AddData(new Anima(Frames, anima_loop_for));
        }

        /// <summary>
        /// Accepts a base frame to automatically generate a Mover
        /// </summary>
        /// <param name="baseFrame">The base frame to use to add the rest</param>
        /// <param name="mover_loop_for">Determines how many subsequent times the mover data wil loop.
        /// "-1" is infinite, "0" is none.</param>
        /// <param name="anim_loop_for">The same as mover_loop_for, but for the automatically-generated animation</param>
        /// <param name="anima_loop_for">The same as mover_loop_for, but for the automatically-genereated anima</param>
        /// <param name="ExtraFrames">The amount of subsequent frames after the intial frame</param>
        /// <param name="XSourceDelta">The amount of change in the source image file's X axis in relation to the
        /// initial frame</param>
        /// <param name="YSourceDelta">The amount of change in the source image file's Y axis in relation to the
        /// initial frame</param>
        public Mover(Frame baseFrame, int mover_loop_for, int anim_loop_for,
            int anima_loop_for, int ExtraFrames, int XSourceDelta, int YSourceDelta)
            : this(mover_loop_for)
        {
            if (anima_loop_for == -1)
            {
                if (anim_loop_for == -1)
                {
                    throw new LoopingException("No infinite sub-loops allowed");
                }
                if (mover_loop_for == -1)
                {
                    throw new LoopingException("No infinite sub-loops allowed");
                }
            }
            this.AddData(new Animation(anim_loop_for));
            Anima temp = new Anima(anima_loop_for);
            temp.AddFrames(baseFrame, ExtraFrames, XSourceDelta, YSourceDelta);
            Animations[Animations.Count - 1].AddData(temp);
        }

        /// <summary>
        /// Adds new Animation objects to the list of the animas
        /// </summary>
        /// <param name="data">The Animation object to be added</param>
        public void AddData(Animation data)
        {
            foreach (Animation anim in Animations)
            {
                if (anim.IsInfinite)
                {
                    throw new LoopingException();
                }
            }
            Animations.Add(data);
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


    }
}