using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using PicAnimator;
//Last edited: April 6, 2015
namespace PicAnimator
{
    /// <summary>
    /// Holds a collection of Anima data
    /// </summary>
    public class Animation : DataContainer<Anima>
    {

        private Animation()
            : base()
        {
            InnerItems = new List<Anima>();
        }

        /// <summary>
        /// Allows for a combination of InnerItems to be played
        /// one after the other.
        /// </summary>
        /// <param name="loop_for">Determines how many subsequent times the data will loop.
        /// "-1" is infinite, "0" is none.</param>
        public Animation(int loop_for)
            : this()
        {
            LoopFor = loop_for;
            if (LoopFor <= INF_CONST)
                IsInfinite = true;
            else
                IsInfinite = false;
        }

        /// <summary>
        /// Allows for a combination of InnerItems to be played
        /// one after the other.
        /// </summary>
        /// <param name="animas">The list of animas for the animation</param>
        /// <param name="loop_for">Determines how many subsequent times the data will loop.
        /// "-1" is infinite, "0" is none.</param>
        public Animation(List<Anima> animas, int loop_for)
            : base(animas, loop_for)
        {}

        /// <summary>
        /// Take a list of frames and turns them into a Animation object
        /// </summary>
        /// <param name="Frames">The list of frames to add</param>
        /// <param name="anim_loop_for">Determines how many subsequent times the data wil loop.
        /// "-1" is infinite, "0" is none.</param>
        /// <param name="anima_loop_for">The same anim_loop_for, but for the automatically-generated anima</param>
        public Animation( List<Frame> Frames, int anim_loop_for, int anima_loop_for)
            : this(anim_loop_for)
        {
            if (anim_loop_for == -1)
            {
                if (anima_loop_for == -1)
                {
                    throw new LoopingException("No infinite sub-loops allowed");
                }
            }

            this.AddData(new Anima(Frames,anima_loop_for));
        }

        /// <summary>
        /// Accepts a base frame to automatically generate a Animation
        /// </summary>
        /// <param name="baseFrame">The base frame to use to add the rest</param>
        /// <param name="anim_loop_for">Determines how many subsequent times the data wil loop.
        /// "-1" is infinite, "0" is none.</param>
        /// <param name="anima_loop_for">The same as anim_loop_for, but for the automatically-genereated anima</param>
        /// <param name="ExtraFrames">The amount of subsequent frames after the intial frame</param>
        /// <param name="XSourceDelta">The amount of change in the source image file's X axis in relation to the
        /// initial frame</param>
        /// <param name="YSourceDelta">The amount of change in the source image file's Y axis in relation to the
        /// initial frame</param>
        public Animation(Frame baseFrame, int anim_loop_for,
            int anima_loop_for, int ExtraFrames, int XSourceDelta, int YSourceDelta)
            : this(anim_loop_for)
        {
            if (anim_loop_for == -1)
            {
                if (anima_loop_for == -1)
                {
                    throw new LoopingException("No infinite sub-loops allowed");
                }
            }
            this.AddData(new Anima(baseFrame,anima_loop_for,ExtraFrames,XSourceDelta,YSourceDelta));
        }

        /// <summary>
        /// Adds new Anima objects to the list of the animas
        /// </summary>
        /// <param name="data">The Anima object to be added</param>
        public void AddData(Anima data)
        {
            //Check for already existing infinitely-looping Anima
            foreach (Anima an in InnerItems)
            {
                //If it does...
                if (an.IsInfinite)
                {
                    //...we got problems.
                    throw new LoopingException();
                }
            }
            //However, if you make it here, you're in the clear!
            InnerItems.Add(data);
        }

        /// <summary>
        /// Runs the Activated event delegate
        /// </summary>
        protected override void OnActivate()
        {
            base.OnActivate();
            foreach(Anima an in InnerItems)
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
            foreach(Anima an in InnerItems)
            {
                an.IsActive = false;
            }
        }
    }
}