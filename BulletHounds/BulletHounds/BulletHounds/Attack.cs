using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PicAnimator;

namespace BulletHounds
{
    public class Attack
    {
        /*
         Currently, the Attack-to-FrameData scheme is a little unintuitive.
         * Could we have an animation attached to the attack, then when 
         * the player shoots, and it's Attack-to-FrameData, it will play
         * the animation and work as already programmed.
         * 
         * In short, have an animation in the Attack class as well.
         */
        /// <summary>
        /// Defines the firing spot for the attack.
        /// </summary>
        public Vector2 fireSpot { get; private set; }
        /// <summary>
        /// the delay before the attack can be used again
        /// </summary>
        public int cooldown { get; set; }
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
        /// <param name="b">the type of bullet used in the attack</param>
        /// <param name="firingSpot">the point around where to centralize the attack</param>
        public Attack(List<Bullet> sendToList, Bullet b,Vector2 firingSpot)
        {
            SendToList = sendToList;
            AssignedToFrame = false;
            bullet = b;
            fireSpot = firingSpot;
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
            SendToList.Add(bullet.CopyBullet());
        }

        /// <summary>
        /// Sets the list of ReadyBullets to a certain animation 
        /// frame to fire off when that frame is active
        /// </summary>
        /// <param name="FD">The particular trigger frame</param>
        public void AttachToFrame(Frame FD)
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

        /// <summary>
        /// Adds another bullet to the attack
        /// </summary>
        /// <param name="b">the bullet to add</param>
        public void AddBullet(Bullet b)
        {
            SetupBullets.Add(b);
        }

        private void UpdateBullets(GameTime gameTime)
        {
            bool isReady;
            //AttackAnimation.IsAnimating = true;
            foreach (Bullet B in SetupBullets)
            {
                isReady = B.Countdown(gameTime.ElapsedGameTime.Milliseconds);
                if (isReady)
                {
                    PushToList(B.CopyBullet());
                }
            }
        }        
    }
}
