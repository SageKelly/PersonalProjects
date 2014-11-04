using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PicAnimator_V1_1
{
    /// <summary>
    /// Animates a collection of Animatinons, and allows the user to play, pause, stop and rewind animations at will.
    /// </summary>
    public class Animator : Game
    {
        private List<Animation> AnimationData;
        private Animation CurAnimation;
        private Anima CurAnima;
        /// <summary>
        /// The current frame being animated
        /// </summary>
        public FrameData CurFrameData
        {
            get;
            private set;
        }

        private int i_frame_index, i_anima_index, i_animation_index;
        private int i_anima_loop_counter, i_animation_loop_counter;
        private const int LOOP_CONST = -1;

        private TimeSpan timer;
        private float speed_perc = 1.00f;

        /// <summary>
        /// Tells whether or not the animation is paused
        /// </summary>
        public bool Paused
        {
            get;
            private set;
        }

        /// <summary>
        /// Tells whether or not the animation has stopped
        /// </summary>
        public bool HasStopped
        {
            get;
            private set;
        }

        /// <summary>
        /// Tells whether or not the animation is currently playing
        /// </summary>
        public bool IsAnimating;

        /// <summary>
        /// Tells whether or not the animation has run at least once
        /// already, even if it was paused or stopped midway.
        /// </summary>
        public bool HasRun
        {
            get;
            private set;
        }

        private Animator()
        {
            i_frame_index = i_anima_index = i_animation_index = 0;
            CurAnimation = AnimationData[i_animation_index];
            CurAnima = CurAnimation.Animas[i_anima_index];
            CurFrameData = CurAnima.Frames[i_frame_index];
            HasRun = false;
            StopAnimation();
        }

        /// <summary>
        /// Animates Anima data
        /// </summary>
        /// <param name="data">The Animation data to be animated</param>
        /// <param name="speedPerc">The speed at which the data should be animated</param>
        public Animator(Animation data, float speedPerc = 1.00f)
            : this()
        {
            AnimationData.Add(data);
        }

        /// <summary>
        /// Animates Anima data
        /// </summary>
        /// <param name="data">The list of Animation data to be animated</param>
        /// <param name="speedPerc">The speed at which the data should be animated</param>
        public Animator(List<Animation> data, float speedPerc = 1.00f)
            : this()
        {
            AnimationData = data;
        }

        /// <summary>
        /// ...Plays...Animation
        /// </summary>
        /// <param name="SpeedPercentage">The speed at which to play the animation. 1.00 is regular speed.</param>
        public void PlayAnimation(float SpeedPercentage = 1.00f)
        {
            HasStopped = false;
            Paused = false;
            IsAnimating = true;
            speed_perc = SpeedPercentage;
            CurFrameData.IsActive = true;
            CurAnima.IsActive = false;
        }

        /// <summary>
        /// Rewinds animation back to the first frame
        /// </summary>
        /// <param name="ResetHistory">Determines if HasRun should be reset to false</param>
        public void ResetAnimation(bool ResetHistory = false)
        {
            i_animation_loop_counter = 0;
            i_anima_loop_counter = 0;
            i_frame_index = 0;
            i_anima_index = 0;
            i_animation_index = 0;
            CurAnimation = AnimationData[i_animation_index];
            CurAnima = CurAnimation.Animas[i_animation_index];
            CurFrameData = CurAnima.Frames[i_frame_index];
            CurFrameData.IsActive = false;
            CurAnima.IsActive = false;
            if (ResetHistory)
                HasRun = false;
        }

        /// <summary>
        /// Pauses the animation at the current frame; does not stop or reset it
        /// </summary>
        public void PauseAnimation()
        {
            Paused = true;
            IsAnimating = false;
        }

        /// <summary>
        /// Stops and resets animation
        /// </summary>
        public void StopAnimation()
        {
            HasStopped = true;
            /*
             * Not sure how I want to use HasRun yet...
            if (IsAnimating && HasStopped)
                HasRun = true;
            */
            IsAnimating = false;
            ResetAnimation();
        }

        private bool NextAnimation()
        {
            i_animation_index++;
            //If infinite
            if (i_animation_index == AnimationData.Count)
                return false;
            else return true;
        }

        ///<summary>
        /// Sets the current frame to be displayed while the current anima can be displayed.
        /// </summary>
        private bool NextAnima()
        {
            #region Old Code
            /*
            if (CurAnimation.IsInfinite)
            {
                //set the previous frame to inactive
                if (CurAnimation.LoopFor == 0 || (CurAnimation.LoopFor > 0 && i_animation_loop_counter < CurAnimation.LoopFor))
                {
                    if (i_anima_index != 0)
                        CurAnimation.Animas[i_anima_index - 1].IsActive = false;
                    else
                    {
                        CurAnimation.Animas[CurAnimation.Animas.Count - 1].IsActive = false;
                    }
                    //set the new frame to active
                    CurAnimation.Animas[i_anima_index].IsActive = true;

                    if (i_anima_index == CurAnimation.Animas.Count)
                    {
                        CurAnimation.hasLooped = true;
                        i_anima_index = 0;

                    }

                    CurAnima = CurAnimation.Animas[i_anima_index];

                    i_anima_index++;
                }
                else
                {
                    if (AnimationData.Count > 1)
                    {
                        NextAnimation();
                    }
                    else
                        StopAnimation();
                    i_animation_loop_counter = 0;
                }
            }
            else
                StopAnimation();
            */
            #endregion
            i_anima_index++;
            //If infinite
            if (i_anima_index == CurAnimation.Animas.Count)
            {
                i_anima_index = 0;
                if (CurAnimation.IsInfinite)
                {
                    CurAnimation.hasLooped = true;
                    CurAnima = CurAnimation.Animas[i_anima_index];
                    CurAnima.IsActive = true;
                    return true;
                }
                //else if finite
                else if (CurAnimation.IsLooping)
                {
                    if (i_animation_loop_counter != CurAnimation.LoopFor)
                    {
                        //increment loop counter
                        i_animation_loop_counter++;
                        CurAnima = CurAnimation.Animas[i_anima_index];
                        CurAnima.IsActive = true;
                        return true;
                    }
                    else
                    {
                        CurAnimation.hasLooped = true;
                        //If there are more animations
                        if (NextAnimation())
                        {
                            CurAnima = CurAnimation.Animas[i_anima_index];
                            CurAnima.IsActive = true;
                            return true;
                        }
                        else return false;
                    }
                }
                //else...(one-time)
                else
                {
                    //If there are more animations
                    if (NextAnimation())
                    {
                        CurAnima = CurAnimation.Animas[i_anima_index];
                        CurAnima.IsActive = true;
                        return true;
                    }
                    else return false;
                }
            }
            else
            {
                CurAnima = CurAnimation.Animas[i_anima_index];
                CurAnima.IsActive = true;
                return true;
            }
        }

        /// <summary>
        /// Sets the current frame to be displayed while the current anima can be displayed.
        /// </summary>
        private void NextFrame()
        {
            #region Old Code
            /*
            if (i_frame_index == CurAnima.Frames.Count)//Once you've reach the end of the Anima
            {
                //Deactive
                if (i_frame_index != 0)
                    CurAnima.Frames[i_frame_index - 1].IsActive = false;
                else
                {
                    CurAnima.Frames[CurAnima.Frames.Count - 1].IsActive = false;
                }

                if (CurAnima.isLooping)
                {
                    CurAnima.hasLooped = true;
                    i_anima_loop_counter++;
                    i_frame_index = 0;
                    
             * If it:
             * A) Loops infinitely, or
             * B) Loops finitely but has not reached its last loop
             * then give the next frame.
             * Otherwise, stop the animation and reset the loop counter
             
                    if (CurAnima.IsInfinite || (CurAnima.LoopFor > LOOP_CONST && i_anima_loop_counter != CurAnima.LoopFor))
                    {
                        //set the new frame to active
                        CurAnima.Frames[i_frame_index].IsActive = true;

                        CurFrameData = CurAnima.Frames[i_frame_index++];
                    }

                }
                else if (CurAnimation.Animas.Count > 1)
                {
                    NextAnima();
                }
                else
                    StopAnimation();

                i_anima_loop_counter = 0;
            }
            */
            #endregion
            //Unset current frame
            CurFrameData.IsActive = false;
            i_frame_index++;
            //If infinite
            if (i_frame_index == CurAnima.Frames.Count)
            {
                i_frame_index = 0;
                if (CurAnima.IsInfinite)
                {
                    CurAnima.hasLooped = true;
                    CurFrameData = CurAnima.Frames[i_frame_index];
                    CurFrameData.IsActive = true;
                }
                //else if finite
                else if (CurAnima.IsLooping)
                {
                    if (i_anima_loop_counter != CurAnima.LoopFor)
                    {
                        //increment loop counter
                        i_anima_loop_counter++;
                        CurFrameData = CurAnima.Frames[i_frame_index];
                        CurFrameData.IsActive = true;
                    }
                    else
                    {
                        CurAnima.hasLooped = true;
                        //If there are more anima
                        if (NextAnima())
                        {
                            CurFrameData = CurAnima.Frames[i_frame_index];
                            CurFrameData.IsActive = true;
                        }
                        else StopAnimation();
                    }
                }
                //else...(one-time)
                else
                {
                    //If there are more animations
                    if (NextAnima())
                    {
                        CurFrameData = CurAnima.Frames[i_frame_index];
                        CurFrameData.IsActive = true;
                    }
                    else StopAnimation();
                }
            }
            else
            {
                CurFrameData = CurAnima.Frames[i_frame_index];
                CurFrameData.IsActive = true;
            }
        }

        /// <summary>
        /// Continues to animate the Anima data until the animation has stopped
        /// </summary>
        /// <param name="gameTime">GameTime parameter for persistent animation</param>
        private void Animate(GameTime gameTime)
        {
            if (!HasStopped && !Paused)
            {
                timer += gameTime.ElapsedGameTime;
                if (timer.TotalMilliseconds >= CurFrameData.MilliDelay * speed_perc)
                {
                    timer = TimeSpan.Zero;
                    NextFrame();
                }
            }
        }

        /// <summary>
        /// Updates the current frame for animation
        /// </summary>
        /// <param name="gameTime">Update's GameTime</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsAnimating)
                Animate(gameTime);
        }

        /// <summary>
        /// Draws animation (NOT READY)
        /// </summary>
        /// <param name="gameTime">Draw's GameTime</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            /*
            spriteBatch.Begin();

            spriteBatch.End();
            */
        }
    }
}

