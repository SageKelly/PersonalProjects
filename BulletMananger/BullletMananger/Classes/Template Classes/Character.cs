using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PicAnimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletManager
{
    public class Character : DrawableGameComponent
    {
        /*
         * To use your animations they must be added to the animator, or else they can't be played.
         */
        /// <summary>
        /// Represents the team ID for the Character
        /// </summary>
        public bool onTeamA { get; protected set; }
        /// <summary>
        /// The list of Attacks for the Character
        /// </summary>
        public List<Attack> Attacks;
        /// <summary>
        /// Represents the currently-selected Attack
        /// </summary>
        public int curAttack;

        #region Anims
        public Animator animator;
        /// <summary>
        /// Represents the currently-playing Mover
        /// </summary>
        public Mover curMover;

        /// <summary>
        /// Mover for when the Character enters the stadium
        /// </summary>
        public Mover enterMover;
        /// <summary>
        /// Animation for when the Character is hit, if necessary.
        /// </summary>
        public Mover hitMover;
        /// <summary>
        /// Idling animation
        /// </summary>
        public Mover idleMover { get; protected set; }
        /// <summary>
        /// Animation for if the Character dies
        /// </summary>
        public Mover loseMover { get; protected set; }
        /// <summary>
        /// Animation for being shocked the first time.
        /// The Character should not be able to move,
        /// while this is playing
        /// </summary>
        public Mover shocked1Mover;
        /// <summary>
        /// Animation for being shocked the second time.
        /// The Character should not be able to move,
        /// while this is playing
        /// </summary>
        public Mover shocked2Mover;
        /// <summary>
        /// Animation for being shocked the first time.
        /// The Character should not be able to move,
        /// while this is playing
        /// </summary>
        public Mover shocked3Mover;
        /// <summary>
        /// Animation for the Character's basic shot.
        /// </summary>
        public Mover attack1Mover;
        /// <summary>
        /// Animation for the Character's "stand-still" shot
        /// </summary>
        public Mover attack2Mover;
        /// <summary>
        /// The anim for the third attack
        /// </summary>
        public Mover attack3Mover;
        /// <summary>
        /// The Mover for if the Character wins
        /// </summary>
        public Mover winMover;
        #endregion

        #region Stats
        /// <summary>
        /// Hitbounds for the Character's trinket
        /// </summary>
        public Rectangle HitBounds;
        public Vector2 HitBoundOffset;
        /// <summary>
        /// Represents the Character's stock health amount
        /// </summary>
        public int baseHealth { get; protected set; }
        /// <summary>
        /// Represents the Character's current health
        /// </summary>
        public int deltaHealth;

        public Vector2 position;
        /// <summary>
        /// Represents the Character's stock Velocity
        /// </summary>
        public Vector2 baseVelocity { get; protected set; }
        /// <summary>
        /// Represents the Character's current Velocity
        /// </summary>
        public Vector2 deltaVelocity;
        /// <summary>
        /// Represents the Character's fastest possible Velocity
        /// </summary>
        public Vector2 maxVelocity { get; protected set; }
        public Vector2 acceleration;
        protected Vector2 slowing_acceleration;
        /// <summary>
        /// Used to determine when the first shock threshold
        /// has been passed. This will be divided by the full
        /// health to determine so.
        /// </summary>
        protected int Shock1DivisionFactor;
        /// <summary>
        /// Used to determine when the second shock threshold
        /// has been passed. This will be divided by the full
        /// health to determine so.
        /// </summary>
        protected int Shock2DivisionFactor;

        #endregion
        /// <summary>
        /// Represents whether or not the Character can currently recieve input
        /// </summary>
        public bool receiveInput;
        /// <summary>
        /// Represents whether or not the Character is currently active
        /// </summary>
        public bool isActive;
        /// <summary>
        /// The horizontal facing of the Character
        /// </summary>
        public SpriteEffects SprEff;
        /// <summary>
        /// The Bullets used by this Character in her attacks
        /// </summary>
        List<Bullet> Bullets;

        public Character(Game game, ImageLoader.CharacterData cd, bool teamA)
            : base(game)
        {
            onTeamA = teamA;
            Attacks = cd.CharacterAttacks;
            Bullets = cd.Bullets;
            List<Mover> Anims = new List<Mover>();
            enterMover = cd.CharacterAnims.Keys.Contains(ImageLoader.ENTER) ? cd.CharacterAnims[ImageLoader.ENTER] : null;
            hitMover = cd.CharacterAnims.Keys.Contains(ImageLoader.HIT) ? cd.CharacterAnims[ImageLoader.HIT] : null;
            idleMover = cd.CharacterAnims.Keys.Contains(ImageLoader.IDLE) ? cd.CharacterAnims[ImageLoader.IDLE] : null;
            loseMover = cd.CharacterAnims.Keys.Contains(ImageLoader.LOSE) ? cd.CharacterAnims[ImageLoader.LOSE] : null;
            shocked1Mover = cd.CharacterAnims.Keys.Contains(ImageLoader.SHOCK1) ? cd.CharacterAnims[ImageLoader.SHOCK1] : null;
            shocked2Mover = cd.CharacterAnims.Keys.Contains(ImageLoader.SHOCK2) ? cd.CharacterAnims[ImageLoader.SHOCK2] : null;
            shocked3Mover = cd.CharacterAnims.Keys.Contains(ImageLoader.SHOCK3) ? cd.CharacterAnims[ImageLoader.SHOCK3] : null;
            shocked1Mover = cd.CharacterAnims.Keys.Contains(ImageLoader.ATTACK1) ? cd.CharacterAnims[ImageLoader.ATTACK1] : null;
            shocked2Mover = cd.CharacterAnims.Keys.Contains(ImageLoader.ATTACK2) ? cd.CharacterAnims[ImageLoader.ATTACK2] : null;
            shocked3Mover = cd.CharacterAnims.Keys.Contains(ImageLoader.ATTACK3) ? cd.CharacterAnims[ImageLoader.ATTACK3] : null;
            winMover = cd.CharacterAnims.Keys.Contains(ImageLoader.WIN) ? cd.CharacterAnims[ImageLoader.WIN] : null;

            Anims.Add(attack1Mover);
            Anims.Add(attack2Mover);
            Anims.Add(attack3Mover);
            Anims.Add(enterMover);
            Anims.Add(hitMover);
            Anims.Add(idleMover);
            Anims.Add(loseMover);
            Anims.Add(shocked1Mover);
            Anims.Add(shocked2Mover);
            Anims.Add(shocked3Mover);
            Anims.Add(winMover);

            animator = new Animator(Anims);
            receiveInput = true;
            isActive = true;
            SprEff = onTeamA ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void Kill()
        {
            animator.Stop(curMover);
            curMover = loseMover;
            animator.Play(curMover,SprEff);
        }

    }
}
