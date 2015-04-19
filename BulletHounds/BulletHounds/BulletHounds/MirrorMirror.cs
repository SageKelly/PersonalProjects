using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletHounds
{
    public class MirrorMirror : Player
    {
        Texture2D images;
        public MirrorMirror(Game game)
            : base(game)
        {

        }

        public MirrorMirror(Game g, PlayerIndex player_index)
            : base(g, player_index)
        {

        }

        protected override void SetupAttacks()
        {


        }

        protected override void LoadContent()
        {
            base.LoadContent();
            images = game.Content.Load<Texture2D>("Infini-Ballistic");
        }

        public override void HandleControls(GameTime gameTime)
        {
            
        }

        protected override void SetupBullets()
        {
            Bullet bulletFire = new Bullet(game,
                new Image2D(images, new Rectangle(135, 80, 7, 5)),
                this.pIndex);
            bulletFire.setupCollision(new Rectangle(0, 0, 7, 5));
            bulletFire.setupGrazing(2,new Bullet.GrazingDelegate(grazingEffect));
            bulletFire.SetupMovement(Vector2.Zero, new Vector2(7, 0), Vector2.Zero, Vector2.Zero,
                new Bullet.UpdateDelegate(UpdateBullet));
            bulletFire.bulletType = Bullet.BulletTypes.Fire;

            Attack attackFire = new BulletHounds.Attack(firedBullets, bulletFire, position);
            attackFire.AddBullet(bulletFire.CopyBullet(new Vector2(attackFire.fireSpot.X, attackFire.fireSpot.Y - 40)));
            attackFire.AddBullet(bulletFire.CopyBullet(attackFire.fireSpot));
            attackFire.AddBullet(bulletFire.CopyBullet(new Vector2(attackFire.fireSpot.X, attackFire.fireSpot.Y + 40)));
            attackFire.cooldown = 250;
            bulletFire.SetupEffects();

        }

        private void UpdateBullet(ref Bullet b)
        {
            b.position += b.baseVelocity;
        }

        private void grazingEffect(ref Bullet thisOne,ref Bullet other)
        {
            other.health -= thisOne.damage/2;
        }

        protected override void SetupAnimations()
        {
            throw new NotImplementedException();
        }
    }
}
