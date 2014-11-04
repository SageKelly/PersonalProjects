using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PicAnimator
{

    /// <summary>
    /// Collection of FrameData
    /// </summary>
    public class Anima : DataContainer
    {
        /// <summary>
        /// The collection of FrameData
        /// </summary>
        public List<Frame> Frames;

        private Anima()
        {
            hasLooped = false;
            Frames = new List<Frame>();
        }

        /// <summary>
        /// Creates Anima data which holds FrameData objects
        /// </summary>
        /// <param name="loop_for">Determines how many subsequent times the data will loop. "-1" is for infinite. "0" is none.</param>
        public Anima(int loop_for)
            : this()
        {
            LoopFor = loop_for;
            if (LoopFor <= INF_CONST)
                IsInfinite = true;
            else
                IsInfinite = false;
        }

        /// <summary>
        /// Creates Anima data which holds FrameData objects
        /// </summary>
        /// <param name="frames">A given list of frames to add to the anima</param>
        /// <param name="loop_for">Determines how many subsequent times the data will loop. "-1" is for infinite. "0" is none.</param>
        public Anima(List<Frame> frames, int loop_for)
            : this(loop_for)
        {
            Frames = frames;
        }

        /// <summary>
        /// Adds frames to the frame and any systematically different frames, assuming the frames
        /// are added in sequential order during method call
        /// </summary>
        /// <param name="baseFrame">The intial frame data, which will be used as a template for the rest of the frames</param>
        /// <param name="loop_for">Determines how many subsequent times the data will loop. "-1" is for infinite. "0" is none.</param>
        /// <param name="ExtraFrames">The amount of subsequent frames after the intial frame</param>
        /// <param name="XSourceDelta">The amount of change in the source image file's X axis in relation to the
        /// initial frame</param>
        /// <param name="YSourceDelta">The amount of change in the source image file's Y axis in relation to the
        /// initial frame</param>
        public Anima(Frame baseFrame,int loop_for, int ExtraFrames, int XSourceDelta, int YSourceDelta)
            :this(loop_for)
        {
            AddFrames(baseFrame, ExtraFrames, XSourceDelta, YSourceDelta);
        }



        /// <summary>
        /// Adds the frame the list of frames
        /// </summary>
        /// <param name="data">The Frame object to add</param>
        public void AddFrame(Frame data)
        {
            Frames.Add(data);
        }

        /// <summary>
        /// Adds frames to the frame and any systematically different frames, assuming the frames
        /// are added in sequential order during method call
        /// </summary>
        /// <param name="data">The intial frame data, which will be used as a template for the rest of the frames</param>
        /// <param name="ExtraFrames">The amount of subsequent frames after the intial frame</param>
        /// <param name="XSourceDelta">The amount of change in the source image file's X axis in relation to the
        /// initial frame</param>
        /// <param name="YSourceDelta">The amount of change in the source image file's Y axis in relation to the
        /// initial frame</param>
        public void AddFrames(Frame data, int ExtraFrames, int XSourceDelta, int YSourceDelta)
        {
            Frame temp = data;
            Frames.Add(data);
            Rectangle temp_rect = temp.Source;
            int fps = temp.FPS;
            int delay = temp.MilliDelay;
            for (int i = 1; i <= ExtraFrames; i++)
            {
                Frames.Add(new Frame(temp.Image,
                    new Rectangle(i * XSourceDelta, i * YSourceDelta, temp_rect.Width, temp_rect.Height),
                    fps, delay));
            }
        }
    }
}