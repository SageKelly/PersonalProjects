using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Last edited: April 6, 2015
namespace PicAnimator
{
    /// <summary>
    /// Abstract Container class
    /// </summary>
    public abstract class DataContainer<T>
    {
        /// <summary>
        /// Used to register activation events to this object
        /// </summary>
        public event HappeningEvent ActivationEvent;

        /// <summary>
        /// Used to register deactivation events to this object
        /// </summary>
        public event HappeningEvent DeactivationEvent;

        /// <summary>
        /// Tells whether or not this object has looped in animation
        /// </summary>
        internal bool hasLooped;

        internal List<T> InnerItems;

        /// <summary>
        /// How many times this object should play. 0 is infinite
        /// </summary>
        public int LoopFor
        {
            get;
            protected set;
        }

        /// <summary>
        /// Tell whether or not this object loops infinitely
        /// </summary>
        public bool IsInfinite
        {
            get;
            protected set;
        }
        /// <summary>
        /// Holds the standard value for an infinitely animating object class
        /// </summary>
        protected const int INF_CONST = -1;

        /// <summary>
        /// Determines whether or not this current object is active
        /// </summary>
        protected bool b_is_active;

        /// <summary>
        /// Tells whether or not this object is active
        /// </summary>
        internal virtual bool IsActive
        {
            get
            {
                return b_is_active;
            }
            set
            {
                //hold previous value
                bool b_prev_state = b_is_active;

                //obtain new value
                b_is_active = value;

                //check for activation
                if (!b_prev_state && b_is_active)
                    OnActivate();

                //check for deactivation (i.e. WAS active and NOW inactive)
                else if (b_prev_state && !b_is_active)
                    OnDeactivate();
            }
        }

        /// <summary>
        /// Holds lower-level data
        /// </summary>
        protected DataContainer()
        {
            hasLooped = false;
            InnerItems = new List<T>();
        }

        /// <summary>
        /// Creates an upper-level object which holds lower-level objects
        /// </summary>
        /// <param name="loop_for">Determines how many subsequent times
        /// the data will loop. "-1" is for infinite. "0" is none.</param>
        public DataContainer(List<T> data, int loop_for)
            : this()
        {
            InnerItems = data;
            LoopFor = loop_for;
            if (LoopFor <= INF_CONST)
                IsInfinite = true;
            else
                IsInfinite = false;
        }

        /// <summary>
        /// Runs the Activated event delegate
        /// </summary>
        protected virtual void OnActivate()
        {
            if (ActivationEvent != null)
            {
                ActivationEvent(this);
            }
        }

        /// <summary>
        /// Runs the Deactivated event delegate
        /// </summary>
        protected virtual void OnDeactivate()
        {
            if (DeactivationEvent != null)
            {
                DeactivationEvent(this);
            }
        }
    }
}
