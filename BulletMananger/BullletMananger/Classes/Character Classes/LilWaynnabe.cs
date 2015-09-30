using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletManager
{
    public class LilWaynnabe : Player
    {
        /*
        public LilWaynnabe(Game game, int id = 0) : base(game, id) { }

        public LilWaynnabe(Game game, Manager m, Vector2 pos, int id)
            : base(game, id)
        {
            position = pos;
            Setup(m);
        }
        */

        public LilWaynnabe(Texture2D image_texture, List<Texture2D> bullet_textures, bool team_A)

        private static void Setup(Manager m)
        {
            Attacks.Add(new BasicWaterShot(m, onTeamA));

        }

    }
}
