using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace PicAnimator
{
    /// <summary>
    /// Used to register to Playing Pausing and Stopping events
    /// </summary>
    public delegate void AnimatorEventHandler();

    /// <summary>
    /// Used to register operations to active FrameData
    /// </summary>
    public delegate void HappeningEvent();

    /// <summary>
    /// Holds the current state of a Mover's Play State
    /// </summary>
    public enum PlayState
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
    public class Animator : DrawableGameComponent
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
                if (CurMover.Animations.Count != 0)
                    CurAnimation = CurMover.Animations[i_animation_index];
                else
                {
                    CurAnimation = null;
                    throw new ArgumentOutOfRangeException("Animations must be added to the Mover before Animator creation");
                }
                if (CurAnimation != null && CurAnimation.Animas.Count != 0)
                    CurAnima = CurAnimation.Animas[i_anima_index];
                else
                {
                    CurAnima = null;
                    throw new ArgumentOutOfRangeException("Animas must be added to the Mover before Animator creation");
                }
                if (CurAnima != null && CurAnima.Frames.Count != 0)
                    CurFrame = CurAnima.Frames[i_frame_index];
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
                    if (CurMover.Animations.Count != 0)
                        CurAnimation = CurMover.Animations[i_animation_index];
                    else
                    {
                        CurAnimation = null;
                        throw new LoopingException("Animations must be added to the Mover before Animator creation");
                    }

                    if (CurAnimation != null && CurAnimation.Animas.Count != 0)
                        CurAnima = CurAnimation.Animas[i_anima_index];
                    else
                    {
                        CurAnima = null;
                        throw new ArgumentOutOfRangeException("Animas must be added to the Mover before Animator creation");
                    }

                    if (CurAnima != null && CurAnima.Frames.Count != 0)
                        CurFrame = CurAnima.Frames[i_frame_index];
                    else
                    {
                        CurFrame = null;
                        throw new ArgumentOutOfRangeException("Frames must be added to Anima before Animator creation");
                    }
                }
                CurMover.PState = PlayState.Play;
                SpeedPerc = speedPerc;
            }
        }

        private TimeSpan timer;

        private SpriteBatch spriteBatch;

        /// <summary>
        /// The rotation, in radians, of the Mover
        /// </summary>
        public float Rotation;

        /// <summary>
        /// The rotation origin for the Mover
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// The position of the Mover: for drawing and movement purposes
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// The maximum velocity for the Mover: should be used for player movement
        /// </summary>
        public Vector2 MaxVelocity;

        /// <summary>
        /// The default vector for player movement. Holds how much local positional change occurs
        /// </summary>
        public Vector2 DeltaVelocity;

        /// <summary>
        /// The acceleration for movement purposes
        /// </summary>
        public Vector2 Acceleration;

        /// <summary>
        /// The scale for the Mover
        /// </summary>
        public Vector2 Scale;

        /// <summary>
        /// The tint for drawing the Mover
        /// </summary>
        public Color Tint;

        /// <summary>
        /// Animates Movers
        /// </summary>
        /// <param name="game">The game you're using (Just do it...I dont know)</param>
        /// <param name="pos">Drawing position for the Mover. Velocity and
        /// Accleration are also available (See SetupVectors()).</param>
        public Animator(Game game, Vector2 pos)
            : base(game)
        {
            MotherList = new List<Mover>();
            PlayList = new List<MoverData>();
            RecentList = new List<Mover>();
            Origin = Vector2.Zero;
            Position = pos;
            MaxVelocity = Vector2.Zero;
            DeltaVelocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0.00f;
            Tint = Color.White;
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

        /// <summary>
        /// Sets up the positional vectors for drawing and moving the overall animation
        /// </summary>
        /// <param name="vel">Velocity</param>
        public void SetupVectors(Vector2 vel)
        {
            DeltaVelocity = vel;
        }

        /// <summary>
        /// Sets up the positional vectors for drawing and moving the overall animation
        /// </summary>
        /// <param name="max_vel">Max Velocity: with Acceleration used, this should be used to 
        /// keep track of how fast the can ever move.</param>
        /// <param name="accel">Acceleration: if used this should be 1/60 of the desired speed,
        /// since XNA runs at 60 fps.</param>
        public void SetupVectors(Vector2 max_vel, Vector2 accel)
        {
            MaxVelocity = max_vel;
            Acceleration = accel;
        }

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
            if (md.i_animation_index == md.CurMover.Animations.Count)
            {
                md.i_animation_index = 0;
                if (md.CurMover.IsInfinite)
                {
                    md.CurMover.hasLooped = true;
                    md.CurAnimation = md.CurMover.Animations[md.i_animation_index];
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
                        md.CurAnimation = md.CurMover.Animations[md.i_animation_index];
                        md.CurAnimation.IsActive = true;
                        return true;
                    }
                    else
                    {
                        md.CurMover.hasLooped = true;
                        //If there are more animations
                        if (NextMover(ref md))
                        {
                            md.CurAnimation = md.CurMover.Animations[md.i_animation_index];
                            md.CurAnimation.IsActive = true;
                            return true;
                        }
                        else return false;
                    }
                }
            }
            else
            {
                md.CurAnimation = md.CurMover.Animations[md.i_animation_index];
                md.CurAnimation.IsActive = true;
                return true;
            }
        }

        private bool NextAnima(ref MoverData md)
        {
            md.i_anima_index++;
            md.CurAnima.IsActive = false;
            //If infinite
            if (md.i_anima_index == md.CurAnimation.Animas.Count)
            {
                md.i_anima_index = 0;
                if (md.CurAnimation.IsInfinite)
                {
                    md.CurAnimation.hasLooped = true;
                    md.CurAnima = md.CurAnimation.Animas[md.i_anima_index];
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
                        md.CurAnima = md.CurAnimation.Animas[md.i_anima_index];
                        md.CurAnima.IsActive = true;
                        return true;
                    }
                    else
                    {
                        md.CurAnimation.hasLooped = true;
                        //If there are more animations
                        if (NextAnimation(ref md))
                        {
                            md.CurAnima = md.CurAnimation.Animas[md.i_anima_index];
                            md.CurAnima.IsActive = true;
                            return true;
                        }
                        else return false;
                    }
                }
            }
            else
            {
                md.CurAnima = md.CurAnimation.Animas[md.i_anima_index];
                md.CurAnima.IsActive = true;
                return true;
            }
        }

        private bool NextFrame(ref MoverData md)
        {
            //Unset current frame
            md.CurFrame.IsActive = false;
            md.i_frame_index++;

            if (md.i_frame_index == md.CurAnima.Frames.Count)
            {
                md.i_frame_index = 0;
                //If infinite
                if (md.CurAnima.IsInfinite)
                {
                    md.CurAnima.hasLooped = true;
                    md.CurFrame = md.CurAnima.Frames[md.i_frame_index];
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
                        md.CurFrame = md.CurAnima.Frames[md.i_frame_index];
                        md.CurFrame.IsActive = true;
                        return true;
                    }
                    else
                    {
                        md.CurAnima.hasLooped = true;
                        //If there are more anima
                        if (NextAnima(ref md))
                        {
                            md.CurFrame = md.CurAnima.Frames[md.i_frame_index];
                            md.CurFrame.IsActive = true;
                            return true;
                        }
                        else
                        {
                            StopAnimation(md.CurMover);
                            return false;
                        }
                    }
                }
            }
            else
            {
                md.CurFrame = md.CurAnima.Frames[md.i_frame_index];
                md.CurFrame.IsActive = true;
                return true;
            }
        }

        /// <summary>
        /// ...Plays...Animation
        /// </summary>
        /// <param name="data">The data which will be animated</param>
        /// <param name="SPE">The Sprite effect for the animation</param>
        /// <param name="SpeedPercentage">The speed at which to play the animation. 1.00 is regular speed.</param>
        public void PlayAnimation(Mover data, SpriteEffects SPE, float SpeedPercentage = 1.00f)
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
        public void PauseAnimation(Mover data)
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
        public void StopAnimation(Mover data)
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
        private void Animate(GameTime gameTime)
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

        /// <summary>
        /// Loads content
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }


        /// <summary>
        /// Updates the current frame for animation
        /// </summary>
        /// <param name="gameTime">Update's GameTime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Animate(gameTime);
        }

        /// <summary>
        /// Draws animation (NOT READY)
        /// </summary>
        /// <param name="gameTime">Draw's GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            /* */
            spriteBatch.Begin();
            for (int i = 0; i < PlayList.Count; i++)
            {
                MoverData md = PlayList[i];
                if (md.CurMover.PState == PlayState.Play)
                {
                    spriteBatch.Draw(md.CurFrame.Image, Position, md.CurFrame.Source,
                        Tint, Rotation, Origin, Scale, md.SPEffects, 0);
                }
            }
            spriteBatch.End();

        }


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