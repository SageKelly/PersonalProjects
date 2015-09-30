using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletManager
{
    /// <summary>
    /// Determines the resulting effect from two different bullets grazing each other
    /// </summary>
    public static class BulletGrazing
    {
        private static Dictionary<string, Func<Bullet[], Bullet[]>> grazeEffects;

        /// <summary>
        /// Denotes whether or not the grazing dictionary has been populated.
        /// </summary>
        private static bool isSetup = false;

        /// <summary>
        /// Populates the grazing dictionary
        /// </summary>
        private static void SetupDictionary()
        {
            grazeEffects = new Dictionary<string, Func<Bullet[], Bullet[]>>();
            isSetup = true;
        }

        /// <summary>
        /// Applies grazing effects to incoming Bullets
        /// </summary>
        /// <param name="bullets">The incoming Bullets</param>
        /// <returns>The incoming Bullets with their applied effects</returns>
        public static Bullet[] Graze(ref Bullet[] bullets)
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
                    bullets = grazeEffects[name](bullets);
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
