using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicAnimator
{
    /// <summary>
    /// Abstract Container class
    /// </summary>
    public abstract class DataContainer
    {
        /// <summary>
        /// Used to register events to active Anima (NOT CURRENTLY IMPLEMENTED)
        /// </summary>
        public event HappeningEvent ActivationEvent, DeactivationEvent;

        /// <summary>
        /// Tells whether or not the Anima has looped in animation
        /// </summary>
        public bool hasLooped;

        /// <summary>
        /// How many times the Anima should play. 0 is infinite
        /// </summary>
        public int LoopFor
        {
            get;
            protected set;
        }

        /// <summary>
        /// Tell whether or not the Anima loops infinitely
        /// </summary>
        public bool IsInfinite
        {
            get;
            protected set;
        }
        /// <summary>
        /// Holds the standard value for an infinitely animating animation
        /// </summary>
        protected const int INF_CONST = -1;
        /// <summary>
        /// Determines whether or not the current DataContainer is active
        /// </summary>
        protected bool b_is_active;

        /// <summary>
        /// Tells whether or not the Anima is active
        /// </summary>
        public bool IsActive
        {
            get
            {
                return b_is_active;
            }
            set
            {
                bool b_prev_state = false;

                //hold previous value
                if (b_is_active)
                    b_prev_state = b_is_active;

                //obtain new value
                b_is_active = value;

                //check for activation
                if (b_is_active)
                    OnActivate();

                //check for deactivation (i.e. WAS active and NOW inactive)
                else if (b_prev_state && !b_is_active)
                    OnDeactivate();
            }
        }

        /// <summary>
        /// Holds animation data
        /// </summary>
        protected DataContainer()
        {
            hasLooped = false;
        }

        /// <summary>
        /// Creates Anima data which holds FrameData objects
        /// </summary>
        /// <param name="loop_for">Determines how many subsequent times the data will loop. "-1" is for infinite. "0" is none.</param>
        public DataContainer(int loop_for)
            : this()
        {
            LoopFor = loop_for;
            if (LoopFor <= INF_CONST)
                IsInfinite = true;
            else
                IsInfinite = false;
        }

        /// <summary>
        /// Runs the Activated event delegate
        /// </summary>
        protected void OnActivate()
        {
            if (ActivationEvent != null)
            {
                ActivationEvent();
            }
        }

        /// <summary>
        /// Runs the Deactivated event delegate
        /// </summary>
        protected void OnDeactivate()
        {
            if (DeactivationEvent != null)
            {
                DeactivationEvent();
            }
        }
    }
}
