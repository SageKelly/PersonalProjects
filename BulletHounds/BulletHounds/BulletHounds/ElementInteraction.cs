using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletHounds
{
    public static class ElementInteraction
    {
        public static void InteractWithElectricity(ref Bullet elec, ref Bullet other)
        {
            switch (other.bulletType)
            {
                case Bullet.BulletTypes.Electricity:
                    break;
                case Bullet.BulletTypes.Energy:
                    break;
                case Bullet.BulletTypes.Fire:
                    break;
                case Bullet.BulletTypes.Glass:
                    break;
                case Bullet.BulletTypes.Gum:
                    break;
                case Bullet.BulletTypes.Ice:
                    break;
                case Bullet.BulletTypes.Metal:
                    break;
                case Bullet.BulletTypes.Rubber:
                    break;
                case Bullet.BulletTypes.Sound:
                    break;
                case Bullet.BulletTypes.Water:
                    break;
                case Bullet.BulletTypes.Wood:
                    break;
            }
        }

        public static void InteractWithFire(ref Bullet fire, ref Bullet other)
        {
            switch (other.bulletType)
            {
                case Bullet.BulletTypes.Electricity:
                    /*The bullets pass through each other, imbuing each other with
                     * their own energies, and giving the other bullet their own
                     * damage.
                     */ 
                    break;
                case Bullet.BulletTypes.Energy:
                    /*The fire is absorbed by the energy bullet. The energy bullet
                     * increases in size and damage, and slows in speed. Once the
                     * speed reaches zero, the bullet dissipates.
                     */ 
                    break;
                case Bullet.BulletTypes.Fire:
                    /*
                     * Bullets collide, making a supernova at the point of collision.
                     * It grows with each subsequent hit, and starts moving in the
                     * direction of the last bullet that hits it. Each subsequent
                     * bullet strike adds or subtracts from the supernova's velocity.
                     */
                    break;
                case Bullet.BulletTypes.Glass:
                    /*
                     * Consecutive hits will build up pressure inside the glass
                     * bullet until it explodes. The shards can affect both opponents.
                     */
                    break;
                case Bullet.BulletTypes.Gum:
                    // It would bubble and increase in size, but not damage.
                    break;
                case Bullet.BulletTypes.Ice:
                    /*
                     * Fire melts ice to water. Water persists, and fire dissipates.
                     * If the bullet grazes the ice the ice will still melt, but the
                     * fire bullet will survive.
                     */
                    break;
                case Bullet.BulletTypes.Metal:
                    //Bullet melts away after three hits. Grazing metal does double damage
                    break;
                case Bullet.BulletTypes.Rubber:
                     //They burn slowly and bubble away over time.
                    break;
                case Bullet.BulletTypes.Sound:
                    // Both are dissipated in the most magnificent of ways
                    break;
                case Bullet.BulletTypes.Water:
                    //They both dissipate. Grazing will evaporate water
                    break;
                case Bullet.BulletTypes.Wood:
                    //Bullet burns over time, diminishing in both size and damage.
                    break;
            }
        }

    }
}
