using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletManager
{
    public abstract class Attack
    {
        /* 1. When creating your attack, place the intial position of the Bullet within it.
         * 2. Use the Bullets within the class to make the attack
        */
        public delegate void StoppedAttack();
        public event StoppedAttack StoppedAttackEvent;

        public delegate void StartedAttack();
        public event StartedAttack StartedAttackEvent;

        private bool onTeamA;


        public List<Bullet> BulletsUsed { get; protected set; }

        /// <summary>
        /// The list of Bullets involved in the Attack.
        /// </summary>
        public List<TimerBullet> Bullets { get; protected set; }

        /// <summary>
        /// Represents the longest Time delay in the Attack
        /// </summary>
        private int longestTimer = 0;

        protected TimeSpan TimeElapsed;

        private Manager bulletManager;

        public Attack(Manager m,bool teamA)
        {
            bulletManager = m;
            this.onTeamA = teamA;
            Bullets = new List<TimerBullet>();
            /*
             * Find the timer with the longest delay.
             * This will be used to denote when the
             * attack has finished.
            */
            foreach (TimerBullet TB in Bullets)
            {
                if (TB.delay > longestTimer)
                    longestTimer = TB.delay;
            }
        }

        public void Update(GameTime gT)
        {
            TimeElapsed = gT.ElapsedGameTime;
            foreach (TimerBullet TB in Bullets)
            {
                if (!TB.isDone && TB.delay <= TimeElapsed.Milliseconds)
                {
                    TB.bullet.onTeamA = onTeamA;
                    bulletManager.AddBulletToQueue(TB.bullet);
                    TB.isDone = true;
                }
            }
        }
    }
}
