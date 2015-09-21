using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
//Last Edited: April 6, 2015
namespace PicAnimator
{
    /// <summary>
    /// Used to register to Playing Pausing and Stopping events
    /// </summary>
    public delegate void AnimatorEventHandler();

    /// <summary>
    /// Used to register operations to active FrameData
    /// </summary>
    public delegate void HappeningEvent(object sender);

    /// <summary>
    /// Holds the current state of a Mover's Play State
    /// </summary>
    internal enum PlayState
    {
        /// <summary>
        /// The Mover is Playing
        /// </summary>
        Play,
        /// <summary>
        /// The Mover has been paused
        /// </summary>
        Pause,
        /// <summary>
        /// The Mover has been stopped
        /// </summary>
        Stop,
        /// <summary>
        /// Default state: the Mover has not been setup or played yet.
        /// </summary>
        Inactive
    }



    /// <summary>
    /// Animates a collection of Animatinons, and allows the user to play, pause, stop and rewind animations at will.
    /// </summary>
    ///    
    public class Animator
    {
        /// <summary>
        /// The main list of all Mover objects associated with this Animator
        /// </summary>
        private List<Mover> MotherList;

        /// <summary>
        /// The most recent Movers used
        /// </summary>
        private List<Mover> RecentList;

        /// <summary>
        /// A list off all the Movers currently being played
        /// </summary>
        private List<MoverData> PlayList;

        /*
        /// <summary>
        /// Used to register the animation fully resetting
        /// </summary>
        public event AnimatorEventHandler ResettingingEvent;
        */

        /// <summary>
        /// A structure containing A particular Mover object's animation specs
        /// </summary>
        private struct MoverData
        {
            public Mover CurMover;
            public int i_mover_loop_counter, i_animation_index, i_animation_loop_counter,
                i_anima_index, i_anima_loop_counter, i_frame_index;
            public float SpeedPerc;
            public SpriteEffects SPEffects;
            public Animation CurAnimation;
            public Anima CurAnima;
            public Frame CurFrame;

            public MoverData(Mover data)
            {
                CurMover = data;
                i_animation_index = i_anima_index = i_frame_index = 0;
                i_mover_loop_counter = i_animation_loop_counter = i_anima_loop_counter = 0;
                SPEffects = SpriteEffects.None;
                CurMover.PState = PlayState.Play;
                if (CurMover.InnerItems.Count != 0)
                    CurAnimation = CurMover.InnerItems[i_animation_index];
                else
                {
                    CurAnimation = null;
                    throw new ArgumentOutOfRangeException("InnerItems must be added to the Mover before Animator creation");
                }
                if (CurAnimation != null && CurAnimation.InnerItems.Count != 0)
                    CurAnima = CurAnimation.InnerItems[i_anima_index];
                else
                {
                    CurAnima = null;
                    throw new ArgumentOutOfRangeException("Animas must be added to the Mover before Animator creation");
                }
                if (CurAnima != null && CurAnima.InnerItems.Count != 0)
                    CurFrame = CurAnima.InnerItems[i_frame_index];
                else
                {
                    CurFrame = null;
                    throw new ArgumentOutOfRangeException("Frames must be added to Anima before Animator creation");
                }
                SpeedPerc = 1.00f;
            }

            /// <summary>
            /// stops the mover
            /// </summary>
            public void Stop()
            {
                i_animation_index = i_anima_index = i_frame_index = 0;
                i_animation_loop_counter = i_anima_loop_counter = 0;
                SPEffects = SpriteEffects.None;
                CurMover.PState = PlayState.Stop;
                SpeedPerc = 1.00f;
                CurMover.IsActive = false;
            }

            /// <summary>
            /// Plays the Mover given
            /// </summary>
            /// <param name="SPE"></param>
            /// <param name="speedPerc"></param>
            public void Play(SpriteEffects SPE, float speedPerc)
            {
                SPEffects = SPE;
                if (CurMover.PState == PlayState.Stop)
                {
                    if (CurMover.InnerItems.Count != 0)
                        CurAnimation = CurMover.InnerItems[i_animation_index];
                    else
                    {
                        CurAnimation = null;
                        throw new LoopingException("InnerItems must be added to the Mover before Animator creation");
                    }

                    if (CurAnimation != null && CurAnimation.InnerItems.Count != 0)
                        CurAnima = CurAnimation.InnerItems[i_anima_index];
                    else
                    {
                        CurAnima = null;
                        throw new ArgumentOutOfRangeException("Animas must be added to the Mover before Animator creation");
                    }

                    if (CurAnima != null && CurAnima.InnerItems.Count != 0)
                        CurFrame = CurAnima.InnerItems[i_frame_index];
                    else
                    {
                        CurFrame = null;
                        throw new ArgumentOutOfRangeException("Frames must be added to Anima before Animator creation");
                    }
                }
                if (!CurMover.IsActive)
                    CurMover.IsActive = true;
                CurMover.PState = PlayState.Play;
                SpeedPerc = speedPerc;
            }
        }

        private TimeSpan timer;

        /// <summary>
        /// Animates Movers
        /// </summary>
        public Animator()
        {
            MotherList = new List<Mover>();
            PlayList = new List<MoverData>();
            RecentList = new List<Mover>();
        }

        /// <summary>
        /// Adds Animation data to the collection
        /// </summary>
        /// <param name="Data">The Animation data to be added</param>
        public void AddData(Mover Data)
        {
            MotherList.Add(Data);
        }

        /// <summary>
        /// Allows you to assign a list to the in-class collection of Mover data
        /// </summary>
        /// <param name="Data">The list of Mover Data</param>
        public void RegisterData(List<Mover> Data)
        {
            MotherList = Data;
        }

        #region Next Methods
        private bool NextMover(ref MoverData md)
        {
            md.CurMover.IsActive = false;
            if (md.CurMover.IsInfinite)
                return true;
            else
                return false;
        }

        private bool NextAnimation(ref MoverData md)
        {
            md.i_animation_index++;
            md.CurAnimation.IsActive = false;


            //If infinite
            if (md.i_animation_index == md.CurMover.InnerItems.Count)
            {
                md.i_animation_index = 0;
                if (md.CurMover.IsInfinite)
                {
                    md.CurMover.hasLooped = true;
                    md.CurAnimation = md.CurMover.InnerItems[md.i_animation_index];
                    md.CurAnimation.IsActive = true;
                    return true;
                }
                //else if finite
                else
                {
                    if (md.i_mover_loop_counter != md.CurMover.LoopFor)
                    {
                        //increment loop counter
                        md.i_mover_loop_counter++;
                        md.CurAnimation = md.CurMover.InnerItems[md.i_animation_index];
                        md.CurAnimation.IsActive = true;
                        return true;
                    }
                    else
                    {
                        md.CurMover.hasLooped = true;
                        //If there are more animations
                        if (NextMover(ref md))
                        {
                            md.CurAnimation = md.CurMover.InnerItems[md.i_animation_index];
                            md.CurAnimation.IsActive = true;
                            return true;
                        }
                        else return false;
                    }
                }
            }
            else
            {
                md.CurAnimation = md.CurMover.InnerItems[md.i_animation_index];
                md.CurAnimation.IsActive = true;
                return true;
            }
        }

        private bool NextAnima(ref MoverData md)
        {
            md.i_anima_index++;
            md.CurAnima.IsActive = false;
            //If infinite
            if (md.i_anima_index == md.CurAnimation.InnerItems.Count)
            {
                md.i_anima_index = 0;
                if (md.CurAnimation.IsInfinite)
                {
                    md.CurAnimation.hasLooped = true;
                    md.CurAnima = md.CurAnimation.InnerItems[md.i_anima_index];
                    md.CurAnima.IsActive = true;
                    return true;
                }
                //else if finite
                else
                {
                    if (md.i_animation_loop_counter != md.CurAnimation.LoopFor)
                    {
                        //increment loop counter
                        md.i_animation_loop_counter++;
                        md.CurAnima = md.CurAnimation.InnerItems[md.i_anima_index];
                        md.CurAnima.IsActive = true;
                        return true;
                    }
                    else
                    {
                        md.CurAnimation.hasLooped = true;
                        //If there are more animations
                        if (NextAnimation(ref md))
                        {
                            md.CurAnima = md.CurAnimation.InnerItems[md.i_anima_index];
                            md.CurAnima.IsActive = true;
                            return true;
                        }
                        else return false;
                    }
                }
            }
            else
            {
                md.CurAnima = md.CurAnimation.InnerItems[md.i_anima_index];
                md.CurAnima.IsActive = true;
                return true;
            }
        }

        private bool NextFrame(ref MoverData md)
        {
            //Unset current frame
            md.CurFrame.IsActive = false;
            md.i_frame_index++;

            if (md.i_frame_index == md.CurAnima.InnerItems.Count)
            {
                md.i_frame_index = 0;
                //If infinite
                if (md.CurAnima.IsInfinite)
                {
                    md.CurAnima.hasLooped = true;
                    md.CurFrame = md.CurAnima.InnerItems[md.i_frame_index];
                    md.CurFrame.IsActive = true;
                    return true;
                }
                //else if finite
                else
                {
                    if (md.i_anima_loop_counter != md.CurAnima.LoopFor)
                    {
                        //increment loop counter
                        md.i_anima_loop_counter++;
                        md.CurFrame = md.CurAnima.InnerItems[md.i_frame_index];
                        md.CurFrame.IsActive = true;
                        return true;
                    }
                    else
                    {
                        md.CurAnima.hasLooped = true;
                        //If there are more anima
                        if (NextAnima(ref md))
                        {
                            md.CurFrame = md.CurAnima.InnerItems[md.i_frame_index];
                            md.CurFrame.IsActive = true;
                            return true;
                        }
                        else
                        {
                            Stop(md.CurMover);
                            return false;
                        }
                    }
                }
            }
            else
            {
                md.CurFrame = md.CurAnima.InnerItems[md.i_frame_index];
                md.CurFrame.IsActive = true;
                return true;
            }
        }
        #endregion

        #region Animation Methods
        /// <summary>
        /// ...Plays...Animation
        /// </summary>
        /// <param name="data">The data which will be animated</param>
        /// <param name="SPE">The Sprite effect for the animation</param>
        /// <param name="SpeedPercentage">The speed at which to play the animation. 1.00 is regular speed.</param>
        public void Play(Mover data, SpriteEffects SPE, float SpeedPercentage = 1.00f)
        {
            bool found = false;
            Mover temp = data;
            //Look for if the Mover in question is already playing
            foreach (MoverData md in PlayList)
            {
                if (md.CurMover == data && md.CurMover.PState == PlayState.Play)
                    return;
                else
                {
                    //Start playing from where you left off
                    md.Play(SPE, SpeedPercentage);
                }
            }
            //Search for the data in question to see if it has been played recently
            foreach (Mover move in RecentList)
            {
                if (move == data)
                {
                    found = true;
                    temp = move;
                    break;
                }
            }

            if (found)
            {
                RecentList.Remove(temp);
                //Check for if an unused MoverData exists
                foreach (MoverData md in PlayList)
                {
                    if (md.CurMover == null)
                    {
                        md.Play(SPE, SpeedPercentage);
                        return;
                    }
                }
                PlayList.Add(new MoverData(temp));
            }
            else
            {
                //Look in MotherList
                foreach (Mover move in MotherList)
                {
                    if (move == data)
                    {
                        //Check for if an unused MoverData exists
                        foreach (MoverData md in PlayList)
                        {
                            if (md.CurMover == null)
                            {
                                md.Play(SPE, SpeedPercentage);
                                return;
                            }
                        }
                        PlayList.Add(new MoverData(temp));
                    }
                }
            }
        }

        /// <summary>
        /// Pauses the animation at the current frame; does not stop or reset it
        /// </summary>
        public void Pause(Mover data)
        {
            //Look for if the Mover in question is already playing
            foreach (MoverData md in PlayList)
            {
                if (md.CurMover == data && md.CurMover.PState == PlayState.Play)
                {
                    md.CurMover.PState = PlayState.Pause;
                }
            }
        }

        /// <summary>
        /// Stops and resets animation
        /// </summary>
        public void Stop(Mover data)
        {
            Mover temp = data;
            for (int i = PlayList.Count - 1; i >= 0; i--)
            {
                MoverData md = PlayList[i];
                if (md.CurMover == data)
                {
                    temp = md.CurMover;
                    md.Stop();
                    RecentList.Add(temp);
                    PlayList.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Continues to animate the Anima data until the animation has stopped
        /// </summary>
        /// <param name="gameTime">GameTime parameter for persistent animation</param>
        public void Animate(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;

            for (int i = 0; i < PlayList.Count; i++)
            {
                MoverData temp = PlayList[i];
                if (temp.CurMover.PState != PlayState.Pause)
                {
                    if (timer.TotalMilliseconds >= temp.CurFrame.MilliDelay * temp.SpeedPerc)
                    {
                        timer = TimeSpan.Zero;
                        if (NextFrame(ref temp))
                            PlayList[i] = temp;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Find the current frame for a particular Mover
        /// </summary>
        /// <param name="mover">The Mover holding the Frame</param>
        /// <returns>Returns the current Frame for the particular Mover, if found. Otherwise, returns null</returns>
        public Frame FindCurFrame(Mover mover)
        {
            foreach (MoverData md in PlayList)
            {
                if (md.CurMover == mover)
                {
                    return md.CurFrame;
                }
            }
            return null;
        }
    }
}