using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PicAnimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletManager
{
    /// <summary>
    /// Keeps track of all the bullets in the game
    /// </summary>
    public class Manager : DrawableGameComponent
    {
        //Note: Do not create the Manager until all teams and players have been sorted.
        List<ShellPlayer> TeamAPlayers;
        List<ShellPlayer> TeamBPlayers;
        List<Bullet> ATeamBullets;
        List<Bullet> BTeamBullets;
        List<Bullet> Recycled;
        Dictionary<Bullet, List<Bullet>> PotentialGrazers;
        SpriteBatch spBa;
        bool friendlyFire;

        Game game;
        SpriteBatch SpBa;
        public Manager(Game game, bool ff)
            : base(game)
        {
            this.game = game;
            friendlyFire = ff;
            TeamAPlayers = new List<ShellPlayer>();
            TeamBPlayers = new List<ShellPlayer>();
            ATeamBullets = new List<Bullet>();
            BTeamBullets = new List<Bullet>();
            Recycled = new List<Bullet>();
            PotentialGrazers = new Dictionary<Bullet, List<Bullet>>();
        }

        public override void Initialize()
        {
            base.Initialize();
            spBa = new SpriteBatch(game.GraphicsDevice);
            Game.Components.Add(this);
            SpBa = new SpriteBatch(game.GraphicsDevice);
        }
        /// <summary>
        /// Updates the Bullets for all teams
        /// </summary>
        /// <param name="gameTime">GameTime used to help animate and update Bullets</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < ATeamBullets.Count; i++)
            {
                Bullet bu = ATeamBullets[i];
                //run the Bullet's update method
                bu.Update(gameTime);
                //Then the Type's update method
                bu.bulletType.Update(gameTime, ref bu);
            }

            for (int i = 0; i < BTeamBullets.Count; i++)
            {
                Bullet bu = BTeamBullets[i];
                //run the Bullet's update method
                bu.Update(gameTime);
                //Then the Type's update method
                bu.bulletType.Update(gameTime, ref bu);
            }
            if (friendlyFire)
            {
                CheckCollide(ref ATeamBullets, ref  ATeamBullets);
                CheckCollide(ref BTeamBullets, ref BTeamBullets);
                CheckCollide(ref ATeamBullets, ref BTeamBullets);
                CheckGraze(ref ATeamBullets, ref  ATeamBullets);
                CheckGraze(ref BTeamBullets, ref BTeamBullets);
                CheckGraze(ref ATeamBullets, ref BTeamBullets);
                CheckPlayerCollision(ref ATeamBullets, ref BTeamBullets);
                CheckPlayerCollision(ref BTeamBullets, ref ATeamBullets);

            }
            else
            {
                CheckCollide(ref ATeamBullets, ref BTeamBullets);
                CheckGraze(ref ATeamBullets, ref BTeamBullets);
                CheckPlayerCollision(ref BTeamBullets, ref ATeamBullets);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpBa.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointClamp, DepthStencilState.None, null);
            foreach (Bullet bu in ATeamBullets)
            {
                if (bu.isActive)
                {
                    Frame curFrame = bu.animator.FindCurFrame(bu.curMover);
                    SpBa.Draw(curFrame.Image, bu.position, curFrame.Source, Color.White,
                        bu.rotation, bu.center, bu.scale, bu.sprEff, 0);
                }
            }
            foreach (Bullet bu in BTeamBullets)
            {
                if (bu.isActive)
                {
                    Frame curFrame = bu.animator.FindCurFrame(bu.curMover);
                    SpBa.Draw(curFrame.Image, bu.position, curFrame.Source, Color.White,
                        bu.rotation, bu.center, bu.scale, bu.sprEff, 0);
                }
            }
            SpBa.End();
        }

        /// <summary>
        /// Adds the Player to the Manager for updating
        /// </summary>
        /// <param name="p">The Player to be added</param>
        /// <param name="TeamA">The team to which the Player belongs</param>
        public void RegisterPlayer(ShellPlayer p, bool TeamA = true)
        {
            if (TeamA)
            {
                TeamAPlayers.Add(p);
            }
            else TeamBPlayers.Add(p);
        }

        /// <summary>
        /// Adds a Bullet to the Manager's updating lists. If one like this
        /// already exists, the Manager will try to pull from the Recycled list of Bullets
        /// </summary>
        /// <param name="bu">The to be added</param>
        public void AddBulletToQueue(Bullet bu)
        {
            if (Recycled.Count > 0)
            {
                for (int i = Recycled.Count - 1; i >= 0; i--)
                {
                    Bullet buI = Recycled[i];

                    if (bu.GetType() == buI.GetType())
                    {
                        buI = Recycled[i];
                        buI.Reset();
                        buI.onTeamA = bu.onTeamA;
                        buI.position = bu.position;
                        Recycled[i] = buI;
                        if (bu.onTeamA)
                        {
                            ATeamBullets.Add(Recycled[i]);
                        }
                        else
                        {
                            BTeamBullets.Add(Recycled[i]);
                        }
                        Recycled.RemoveAt(i);
                    }
                    else if (bu.GetType() != buI.GetType() && i == 0)
                    {
                        //Create a Bullet of the same type as bu
                        if (bu.onTeamA)
                        {
                            buI.Reset(bu.bulletType);
                            Recycled[i] = buI;
                            ATeamBullets.Add(Recycled[i]);
                            ATeamBullets[ATeamBullets.Count - 1].DeactivationEvent += BulletCleanup;
                        }
                        else
                        {
                            buI.Reset(bu.bulletType);
                            Recycled[i] = buI;
                            BTeamBullets.Add(Recycled[i]);
                            BTeamBullets[BTeamBullets.Count - 1].DeactivationEvent += BulletCleanup;
                        }
                    }

                }
            }
            else
            {
                if (bu.onTeamA )
                {
                    ATeamBullets.Add(new Bullet(bu.position,bu.onTeamA, bu.bulletType));
                    ATeamBullets[ATeamBullets.Count - 1].DeactivationEvent += BulletCleanup;
                }
                else
                {
                    BTeamBullets.Add(new Bullet(bu.position,bu.onTeamA, bu.bulletType));
                    BTeamBullets[BTeamBullets.Count - 1].DeactivationEvent += BulletCleanup;
                }
            }
        }

        /// <summary>
        /// Check for Bullet collisions, and enact collision effects
        /// </summary>
        private void CheckCollide(ref List<Bullet> ListA, ref List<Bullet> ListB)
        {
            for (int a = ListA.Count - 1; a >= 0; a--)
            {
                Bullet buA = ListA[a];
                if (buA.isDying)
                    continue;
                for (int b = ListB.Count - 1; b >= 0; b--)
                {
                    Bullet buB = ListB[b];
                    if (buB.isDying)
                        continue;
                    //If hitbounds intersect...
                    if (buA.HitBounds.Intersects(buB.HitBounds))
                    {
                        //...we got a hit! Check the collision library for what to do to the Bullets
                        Bullet[] bullets = new Bullet[2] { buA, buB };
                        Bullet[] results = BulletCollision.Collide(ref bullets);
                        buA = results[0];
                        buB = results[1];
                        if (buA.isDying)
                        {
                            buA.Kill();
                            RemoveFromGrazeList(buA, true);
                            break;
                        }
                        if (buB.isDying)
                        {
                            buB.Kill();
                            RemoveFromGrazeList(buB);
                            continue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check for grazings and enact grazing effects
        /// </summary>
        public void CheckGraze(ref List<Bullet> ListA, ref List<Bullet> ListB)
        {
            for (int a = ListA.Count - 1; a >= 0; a--)
            {
                if (!ListA[a].hasGrazingAction)
                    continue;
                Bullet buA = ListA[a];
                for (int b = ListB.Count - 1; b >= 0; b--)
                {
                    if (!ListB[b].hasGrazingAction)
                        continue;
                    Bullet buB = ListB[b];
                    //If the grazeBounds intersect
                    if (buA.GrazeBounds.Intersects(buB.GrazeBounds))
                    {
                        /*...we MIGHT have a graze. Add to team A's bullet to the
                         * PotentialGrazers list and add Team B's bullet to the
                         * value's list. We only need one Bullet from one of the
                         * teams to be in the list, since the effects will be
                         * mutual.
                        */
                        if (PotentialGrazers.Keys.Contains(buA))
                        {
                            if (!PotentialGrazers[buA].Contains(buB))
                                PotentialGrazers[buA].Add(buB);
                        }
                        else
                        {
                            PotentialGrazers.Add(buA, new List<Bullet>());
                        }
                    }
                }
            }
            //Runs grazing effects on Bullets and removes inactive ones from the PotentialGrazers List
            foreach (KeyValuePair<Bullet, List<Bullet>> kvp in PotentialGrazers)
            {
                Bullet buA = kvp.Key;
                for (int i = kvp.Value.Count - 1; i >= 0; i--)
                {
                    if (!kvp.Value[i].isDying &&
                        (kvp.Key.hasGrazingAction && kvp.Value[i].hasGrazingAction))
                    {
                        Bullet buB = kvp.Value[i];
                        Bullet[] bullets = new Bullet[2] { buA, buB };
                        Bullet[] results = BulletGrazing.Graze(ref bullets);
                        buA = results[0];
                        buB = results[1];
                        kvp.Value.RemoveAt(i);
                        if (buA.isDying)
                        {
                            buA.Kill();
                            ListA.Remove(buA);

                        }
                        if (buB.isDying)
                        {
                            buB.Kill();
                            ListB.Remove(buB);
                        }
                    }
                    else
                    {
                        kvp.Value[i].Kill();
                        kvp.Value.RemoveAt(i);
                    }
                }
            }
        }

        public void BulletCleanup(Bullet sender)
        {
            Recycled.Add(sender);
            if (sender.onTeamA)
            {
                ATeamBullets.Remove(sender);
            }
            else
            {
                BTeamBullets.Remove(sender);
            }
        }

        /// <summary>
        /// Removes Bullet from the PotentialGrazers List
        /// </summary>
        /// <param name="bu">The Bullet to be removed</param>
        /// <param name="TeamA">Whether or not the Bullet was on Team A</param>
        public void RemoveFromGrazeList(Bullet bu, bool TeamA = false)
        {
            if (TeamA)
            {
                PotentialGrazers.Remove(bu);
            }
            else
            {
                foreach (KeyValuePair<Bullet, List<Bullet>> kvp in PotentialGrazers)
                {
                    if (kvp.Value.Contains(bu))
                    {
                        kvp.Value.Remove(bu);
                    }
                }
            }
        }

        /// <summary>
        /// Checks each team's Players to see if they were hit by opposing Bullets
        /// </summary>
        /// <param name="ABullets">Team A's opposing Bullets</param>
        /// <param name="BBullets">Team B's opposing Bullets</param>
        private void CheckPlayerCollision(ref List<Bullet> ABullets, ref List<Bullet> BBullets)
        {
            foreach (ShellPlayer p in TeamAPlayers)
            {
                for (int i = ABullets.Count - 1; i >= 0; i--)
                {
                    Bullet bu = ABullets[i];
                    if (bu.HitBounds.Intersects(p.HitBounds))
                    {
                        p.deltaHealth -= bu.deltaDamage;
                        ABullets.Remove(bu);
                        if (p.deltaHealth <= 0)
                        {
                            p.Kill();
                            break;
                        }
                    }
                }
            }

            foreach (ShellPlayer p in TeamBPlayers)
            {
                for (int i = BBullets.Count - 1; i >= 0; i--)
                {
                    Bullet bu = BBullets[i];
                    if (bu.HitBounds.Intersects(p.HitBounds))
                    {
                        p.deltaHealth -= bu.deltaDamage;
                        BBullets.Remove(bu);
                        if (p.deltaHealth <= 0)
                        {
                            p.Kill();
                            break;
                        }
                    }
                }
            }
        }

    }
}
