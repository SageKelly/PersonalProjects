using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletHounds
{
    /// <summary>
    /// In charge of the updating each bullet currently active or counting down in the game
    /// </summary>
    public class BulletManager : GameComponent
    {
        public List<Bullet> bulletsTeamA;
        public List<Bullet> bulletsTeamB;
        Game game;
        public BulletManager(Game game)
            : base(game)
        {
            bulletsTeamA = new List<Bullet>();
            this.game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
            game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < bulletsTeamA.Count; i++)
            {
                Bullet b = bulletsTeamA[i];
                if (!b.isActive && b.delaySet)
                {
                    b.isActive = b.Countdown(gameTime.ElapsedGameTime.Milliseconds);
                }
                if (b.isActive)
                {
                    if (b.isAnimated)
                        b.bulletAnimator.PlayAnimation(b.curMover, b.bulletImage.spriteEf);
                    b.updateAction(ref b);
                    //Update HitBounds
                    b.hitbounds.Offset((int)b.position.X, (int)b.position.Y);
                }
            }
            for (int i = 0; i < bulletsTeamB.Count; i++)
            {
                Bullet b = bulletsTeamA[i];
                if (!b.isActive && b.delaySet)
                {
                    b.isActive = b.Countdown(gameTime.ElapsedGameTime.Milliseconds);
                }
                if (b.isActive)
                {
                    if (b.isAnimated)
                        b.bulletAnimator.PlayAnimation(b.curMover, b.bulletImage.spriteEf);
                    b.updateAction(ref b);
                    //Update HitBounds
                    b.hitbounds.Offset((int)b.position.X, (int)b.position.Y);
                }
            }
            for (int a = bulletsTeamA.Count; a >= 0; a--)
            {
                Bullet bA = bulletsTeamA[a];
                for (int b = bulletsTeamB.Count; b >= 0; b--)
                {
                    Bullet bB = bulletsTeamB[b];
                    if (bA.hitbounds.Intersects(bB.hitbounds) && (!bA.isHit || !bB.isHit))
                    {
                        bA.isHit = bB.isHit = true;
                    }
                    else
                    {
                        bA.isHit = bB.isHit = false;
                    }
                    if (bA.grazingHitbounds.Intersects(bB.grazingHitbounds) && (!bA.isHit && !bB.isHit))
                    {
                        bA.isGrazed = true;
                        bB.isGrazed = true;
                    }
                    if (bA.isHit)
                    {
                        if (bA.hitActionSet)
                        {
                            bA.hitAction(ref bA);
                        }
                    }
                    else
                    {
                        bA.isActive = false;
                        bulletsTeamA.RemoveAt(a);
                    }
                    if (bB.isHit)
                    {
                        if (bB.hitActionSet)
                        {
                            bB.hitAction(ref bB);
                        }
                    }
                    else
                    {
                        bB.isActive = false;
                        bulletsTeamB.RemoveAt(b);
                    }
                }
            }
        }
    }
}
