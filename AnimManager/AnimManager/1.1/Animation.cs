using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using PicAnimator_V1_0;

namespace PicAnimator_V1_1
{
    /// <summary>
    /// Holds a collection of Anima data
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// List of Anima
        /// </summary>
        public List<Anima> Animas;
        /// <summary>
        /// Tells whether or not the Animation has looped
        /// </summary>
        public bool hasLooped;

        /// <summary>
        /// Tells how many times the Animation can loop
        /// </summary>
        public int LoopFor
        {
            get;
            private set;
        }

        /// <summary>
        /// Tells whether or not the Animation can loop
        /// </summary>
        public bool IsLooping
        {
            get;
            private set;
        }

        /// <summary>
        /// Tells whether or not the Animation loops infinitely
        /// </summary>
        public bool IsInfinite
        {
            get;
            private set;
        }
        private const int INF_CONST = -1;


        private Animation()
        {
            hasLooped = false;
            Animas = new List<Anima>();
        }

        /// <summary>
        /// Allows for a combination of Animas to be played
        /// one after the other.
        /// </summary>
        /// <param name="loop_for">Determines how many subsequent times the data will loop.
        /// "-1" is infinite, "0" is none.</param>
        public Animation(int loop_for)
            : this()
        {
            LoopFor = loop_for;
            if (LoopFor == INF_CONST)
            {
                IsInfinite = true;
            }
            else
            {
                IsInfinite = false;
                if (LoopFor == 0)
                    IsLooping = false;
                else
                    IsLooping = true;
            }
        }

        /// <summary>
        /// Allows for a combination of Animas to be played
        /// one after the other.
        /// </summary>
        /// <param name="animas">The list of animas for the animation</param>
        /// <param name="loop_for">Determines how many subsequent times the data will loop.
        /// "-1" is infinite, "0" is none.</param>
        public Animation(List<Anima> animas, int loop_for)
            : this(loop_for)
        {
            Animas = animas;
            LoopFor = loop_for;
            if (LoopFor == -1)
            {
                IsInfinite = true;
            }
            else
            {
                IsInfinite = false;
                if (LoopFor == 0)
                    IsLooping = false;
                else
                    IsLooping = true;
            }
        }

        /// <summary>
        /// Adds new Anima objects to the list of the animas
        /// </summary>
        /// <param name="data">The Anima object to be added</param>
        public void AddData(Anima data)
        {
            //Check for already existing infinitely-looping Anima
            foreach (Anima an in Animas)
            {
                //If it does...
                if (an.IsInfinite)
                {
                    //...we got problems.
                    throw new LoopingException();
                }
            }
            //However, if you make it here, you're in the clear!
            Animas.Add(data);
        }
    }
}