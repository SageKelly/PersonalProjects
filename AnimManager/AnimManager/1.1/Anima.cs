using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PicAnimator_V1_1
{
    /// <summary>
    /// Collection of FrameData
    /// </summary>
    public class Anima
    {
        /// <summary>
        /// Used to register events to active Anima (NOT CURRENTLY IMPLEMENTED)
        /// </summary>
        public event HappeningEvent ActiveAnima;

        /// <summary>
        /// Tells whether or not the Anima has looped in animation
        /// </summary>
        public bool hasLooped;

        /// <summary>
        /// The collection of FrameData
        /// </summary>
        public List<FrameData> Frames;
        /// <summary>
        /// How many times the Anima should play. 0 is infinite
        /// </summary>
        public int LoopFor
        {
            get;
            private set;
        }

        /// <summary>
        /// Tells whether or not the Anima can loop
        /// </summary>
        public bool IsLooping
        {
            get;
            private set;
        }

        /// <summary>
        /// Tell whether or not the Anima loops infinitely
        /// </summary>
        public bool IsInfinite
        {
            get;
            private set;
        }
        private const int INF_CONST = -1;
        bool is_active;

        /// <summary>
        /// Tells whether or not the Anima is active
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

        private Anima()
        {
            hasLooped = false;
            Frames = new List<FrameData>();
        }

        /// <summary>
        /// Creates Anima data which holds FrameData objects
        /// </summary>
        /// <param name="loop_for">Determines how many subsequent times the data will loop. "-1" is for infinite. "0" is none.</param>
        public Anima(int loop_for)
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
        /// Adds the frame the list of frames
        /// </summary>
        /// <param name="data">The frame data to be added to the list</param>
        public void AddFrameData(FrameData data)
        {
            Frames.Add(data);
        }

        /// <summary>
        /// Adds frames to the frame and any systematically different frames, assuming the frames
        /// are added in sequential order during method call
        /// </summary>
        /// <param name="data">The intial frame, which be used as a template for the rest of the frames</param>
        /// <param name="ExtraFrames">The amount of subsequent frames after the intial frame</param>
        /// <param name="XSourceDelta">The amount of change in the source image file's X axis in relation to the
        /// initial frame</param>
        /// <param name="YSourceDelta">The amount of change in the source image file's Y axis in relation to the
        /// initial frame</param>
        public void AddFrameData(FrameData data, int ExtraFrames, int XSourceDelta, int YSourceDelta)
        {
            Frames.Add(data);
            Rectangle temp_rect = data.Source;
            int fps = data.FPS;
            int delay = data.MilliDelay;
            for (int i = 1; i <= ExtraFrames; i++)
            {
                Frames.Add(new FrameData(data.Image,
                    new Rectangle(i * XSourceDelta, i * YSourceDelta, temp_rect.Width, temp_rect.Height),
                    fps, delay));
            }
        }

        private void OnActive()
        {
            if (ActiveAnima != null)
            {
                ActiveAnima();
            }
        }
    }
}
