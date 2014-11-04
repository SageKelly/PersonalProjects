using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PicAnimator_V1_1
{
    /// <summary>
    /// Used to register operations to active FrameData
    /// </summary>
    public delegate void HappeningEvent();

    /// <summary>
    /// A Texture2D object containing extra data used in animation
    /// </summary>
    public class FrameData
    {
        /// <summary>
        /// The actual image
        /// </summary>
        public Texture2D Image;

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
        private bool is_active;

        /// <summary>
        /// ...I don't know...I haven't seen this variable in
        /// a while....Don't use it for anything.
        /// </summary>
        public object Param;

        /// <summary>
        /// Tells whether or not the current frame is active
        /// </summary>
        public bool IsActive
        {
            get
            {
                return is_active;
            }
            set
            {
                is_active = value;
                if (is_active)
                    OnActive();
            }
        }

        /// <summary>
        /// The event to which to register methods
        /// </summary>
        public event HappeningEvent ActiveEvent;

        /// <summary>
        /// Creates data based on the given image based on the data provided.
        /// </summary>
        public FrameData() { }

        /// <summary>
        /// Creates data based on the given image based on the data provided.
        /// </summary>
        /// <param name="image">The image to be manipulated</param>
        /// <param name="source">The section of the image</param>
        /// <param name="fps">The frames per second in which the image can be animated
        /// (FrameData class is compatible with Anima and Animator classes)</param>
        /// <param name="delay">Anima/Animator specific:The delay, in milliseconds, before the FrameData.
        /// If 0, delay will be calculated from the given fps. Else it will be the literal delay</param>
        public FrameData(Texture2D image, Rectangle source, int fps, int delay = 0)
        {
            Image = image;
            Source = source;
            FPS = fps;
            MilliDelay = (1000 / fps) + delay;
        }

        private void OnActive()
        {
            if (ActiveEvent != null)
            {
                ActiveEvent();
            }

        }

    }
}
