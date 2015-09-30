using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletManager
{
    public class TimerBullet
    {
        public int delay { get; private set; }
        public Bullet bullet;
        /// <summary>
        /// Used to signify to the acquiring Attack that it
        /// has attempted to spawn its Bullet
        /// </summary>
        public bool isDone;
        /// <summary>
        /// Creates a Bullet with a Spawn delay timer
        /// </summary>
        /// <param name="delay">How long the Bullet should wait before it attempts to spawn a Bullet</param>
        /// <param name="bullet">The Bullet to be spawned</param>
        public TimerBullet(int delay, Bullet bullet)
        {
            if (delay >= 0)
                this.delay = delay;
            else
                this.delay = 0;
            this.bullet = bullet;
            isDone = false;
        }
    }
}
