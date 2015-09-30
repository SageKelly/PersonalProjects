using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletManager
{
    /// <summary>
    /// Determines the resulting effect from two different bullets colliding
    /// </summary>
    public static class BulletCollision
    {
        /*
         * If you want to kill any Bullet, set isDying to
         * true. The Manager will handle the rest from there.
        */
        private static Dictionary<string, Func<Bullet[], Bullet[]>> collideEffects;

        /// <summary>
        /// Denotes whether or not the collision dictionary has been populated.
        /// </summary>
        public static bool isSetup = false;

        /// <summary>
        /// Populates the collision dictionary
        /// </summary>
        public static void SetupDictionary()
        {
            collideEffects = new Dictionary<string, Func<Bullet[], Bullet[]>>();

            collideEffects.Add("FireWater", FireWater);
            collideEffects.Add("WaterFire", collideEffects["FireWater"]);
            isSetup = true;
        }

        /// <summary>
        /// Applies collision effects to incoming Bullets
        /// </summary>
        /// <param name="bullets">The incoming Bullets</param>
        /// <returns>The incoming Bullets with their applied effects</returns>
        public static Bullet[] Collide(ref Bullet[] bullets)
        {
            if (!isSetup)
            {
                SetupDictionary();
            }
            if (bullets.Length == 2 && (bullets[0] != null && bullets[1] != null))
            {
                //Find all the Type permutations that can be made from the two Bullets
                List<string> typePermutations = new List<string>();
                for (int a = 0; a < bullets[0].ETypes.Count; a++)
                {
                    for (int b = 0; b < bullets[1].ETypes.Count; b++)
                    {
                        typePermutations.Add(bullets[0].ETypes[a].ToString() + bullets[1].ETypes[b].ToString());
                    }
                }
                //Remove any duplicates so that each effect is added only once.
                for (int i = typePermutations.Count - 1; i >= 0; i--)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (typePermutations[i] == typePermutations[j])
                        {
                            typePermutations.RemoveAt(i);
                            break;
                        }
                    }
                }
                //Then apply all the unique effects to the Bullets
                foreach (string name in typePermutations)
                {
                    bullets = collideEffects[name](bullets);
                }
                return bullets;
            }
            return null;
        }

        private static Bullet[] FireWater(Bullet[] bullets)
        {
            return bullets;
        }

        private static Bullet[] WaterFire(Bullet[] bullets)
        {
            return FireWater(bullets);
        }
    }
}
