using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PicAnimator;

namespace BulletHounds
{
    public class Attack : Game
    {
        /*
         Currently, the Attack-to-FrameData scheme is a little unintuitive.
         * Could we have an animation attached to the attack, then when 
         * the player shoots, and it's Attack-to-FrameData, it will play
         * the animation and work as already programmed.
         * 
         * In short, have an animation in the Attack class as well.
         */
        Vector2 position, velocity, acceleration;
        Bullet bullet;
        /// <summary>
        /// The initial setup of the bullets
        /// </summary>
        List<Bullet> SetupBullets;
        /// <summary>
        /// The bullets pending firing
        /// </summary>
        List<Bullet> ReadyBullets;
        /// <summary>
        /// The list to which to send the ready bullets
        /// </summary>
        List<Bullet> SendToList;
        bool AssignedToFrame;

        /// <summary>
        /// Creates special attack using bullets and different timing schemes
        /// </summary>
        /// <param name="sendToList">The list that the bullets will be sent
        /// to to be updated at each cycle</param>
        public Attack(List<Bullet> sendToList, Bullet b)
        {
            SendToList = sendToList;
            AssignedToFrame = false;
            bullet = b;
        }

        /// <summary>
        /// Creates special attack using bullets and differnt timing schemes
        /// </summary>
        /// <param name="sendToList">The list that the bullets will be sent
        /// to to be updated at each cycle</param>
        /// <param name="setupBullets"> The list of bullets with timers on them; 
        /// a means of staggering bullets to fit particular formations or to create
        /// a blast after a certain animation has run</param>
        public Attack(List<Bullet> sendToList, List<Bullet> setupBullets)
            : this(sendToList)
        {
            SetupBullets = setupBullets;
            AssignedToFrame = true;
        }

        /// <summary>
        /// Sends the entire list of timed bullets to 
        /// the SendToList to be fired and updated
        /// </summary>
        public void PushToList()
        {
            SendToList.AddRange(ReadyBullets);
        }

        /// <summary>
        /// Sends a particular bullet to the SendToList 
        /// to be fired and updated
        /// </summary>
        /// <param name="bullet">The particular bullet</param>
        private void PushToList(Bullet bullet)
        {
            SendToList.Add(new Bullet(bullet.BulletImage, bullet.Position, bullet.Velocity, bullet.Acceleration));
        }

        /// <summary>
        /// A means of adding Timedbullets to the list
        /// </summary>
        /// <param name="TB">The timed bullet formatted to the intial firing position</param>
        public void AddTimedBullet(Bullet TB)
        {
            SetupBullets.Add(TB);
        }

        /// <summary>
        /// Sets the list of ReadyBullets to a certain animation 
        /// frame to fire off when that frame is active
        /// </summary>
        /// <param name="FD">The particular trigger frame</param>
        public void AttackToFrame(Frame FD)
        {
            FD.ActivatedFrame+= new HappeningEvent(PushToList);
        }

        public void Shoot()
        {
            if (AssignedToFrame)
            {
                //AttackAnimation.PlayAnimation();
            }
            else
            {

            }
        }

        private void UpdateBullets(GameTime gameTime)
        {
            bool isReady;
            //AttackAnimation.IsAnimating = true;
            foreach (Bullet B in SetupBullets)
            {
                isReady = TB.Countdown(gameTime.ElapsedGameTime.Milliseconds);
                if (isReady)
                {
                    PushToList(TB.bullet);
                }
            }
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateBullets(gameTime);
        }
    }
}
