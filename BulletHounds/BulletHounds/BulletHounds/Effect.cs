using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletHounds
{
    public class Effect
    {
        public int counter;
        public bool isActive;
        public int damage;
        /// <summary>
        /// The effect method
        /// </summary>
        public Action<Player> playerEffectMethod;
        public Action<Bullet> bulletEffectMethod;

        public Effect()
        {

        }

        public void ApplyPlayerEffect(Player P)
        {
            if (playerEffectMethod != null)
            {
                playerEffectMethod(P);
            }
        }

        public void ApplyBulletEffect(Bullet b)
        {
            if(playerEffectMethod!=null)
            {
                bulletEffectMethod(b);
            }
        }

        public void UpdateEffect(GameTime gameTime)
        {
            if(counter>0)
            {

            }
        }
    }
}
