using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PicAnimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletManager
{
    public class LilWaynnabe : Character
    {
        public LilWaynnabe(Game game, ImageLoader.CharacterData character_info, bool team_A)
            : base(game,character_info, team_A)
        {
            HitBounds = new Rectangle((int)position.X, (int)position.Y, 12, 12);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HitBounds.Offset(new Point((int)position.X, (int)position.Y));
        }

    }
}
