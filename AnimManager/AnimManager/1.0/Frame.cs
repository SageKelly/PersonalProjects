using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PicAnimator
{
    /// <summary>
    /// A class which contains information of a particular frame of animation
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// The actual image
        /// </summary>
        public Texture2D Image;

        /// <summary>
        /// Name the frame...
        /// </summary>
        public string Name;

        /// <summary>
        /// The location of the frame on the sprite sheet
        /// </summary>
        public Rectangle Source;

        /// <summary>
        /// The rate which this frame plays, or the latency of the frame
        /// </summary>
        public int FPS;

        /// <summary>
        /// Extra latency of the frame added to the FPS
        /// </summary>
        public int MilliDelay;
        private bool b_is_active;

        /// <summary>
        /// Tells whether or not the current frame is active
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
        /// The event to which to register methods
        /// </summary>
        public event HappeningEvent ActivatedFrame, DeactivatedFrame;

        /// <summary>
        /// Creates data based on the given image based on the data provided.
        /// </summary>
        /// <param name="image">The image to be manipulated</param>
        /// <param name="source">The section of the image</param>
        /// <param name="fps">The frames per second in which the image can be animated
        /// (FrameData class is compatible with Anima and Animator classes)</param>
        /// <param name="delay">Anima/Animator specific:The delay, in milliseconds, before the FrameData.
        /// If 0, delay will be calculated from the given fps. Else it will be the literal delay</param>
        public Frame(Texture2D image, Rectangle source, int fps, int delay = 0)
        {
            Image = image;
            Source = source;
            FPS = fps;
            MilliDelay = (1000 / fps) + delay;
            b_is_active = false;
            ActivatedFrame = null;
            Name = "";
        }

        private void OnActivate()
        {
            if (ActivatedFrame != null)
            {
                ActivatedFrame();
            }

        }

        private void OnDeactivate()
        {
            if (DeactivatedFrame != null)
            {
                DeactivatedFrame();
            }

        }
    }
}