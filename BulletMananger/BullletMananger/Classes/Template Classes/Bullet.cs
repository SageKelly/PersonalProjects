using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PicAnimator;

namespace BulletManager
{
    /// <summary>
    /// Used for creating Bullets of different types and behaviors
    /// </summary>
    public class Bullet
    {
        /*
         * When manipulating the Animator:
         * 1. It would behoove you to play a Mover only once.
         * 2. No Movers will play until they are added to the animator.
         * When manipulating a Bullet
         * 1. The dying order is Dying, Kill => Deactivate
         * 2. The dying animation triggers the Bullet's deactivation.
        */
        public Game game;

        public Bullet bulletType;

        public Vector2 position;
        /// <summary>
        /// The base, unchanging velocity of the bullet
        /// </summary>
        public Vector2 baseVelocity { get; protected set; }
        /// <summary>
        /// The current velocity of the bullet, which may or may not be changed by acceleration
        /// </summary>
        public Vector2 deltaVelocity;
        /// <summary>
        /// The ceiling velocity for a bullet
        /// </summary>
        public Vector2 maxVelocity;
        /// <summary>
        /// Represents the rotational center of the Bullet
        /// </summary>
        public Vector2 center;
        public Vector2 acceleration;
        public float rotation;
        public float scale;
        public SpriteEffects sprEff;
        /// <summary>
        /// Bullet animator for animated bullets
        /// </summary>
        public Animator animator { get; protected set; }
        /// <summary>
        /// The default Animation for the bullet while it's flying
        /// </summary>
        public Mover idlingAnim;
        /// <summary>
        /// The default Animation for the bullet while it's dying
        /// </summary>
        public Mover dyingAnim;
        /// <summary>
        /// A list of effect animations that are applied to the Bullet via grazing
        /// </summary>
        public List<Mover> EffectAnims;
        public Mover curMover;
        /// <summary>
        /// The bullet's main hitbounds
        /// </summary>
        public int radius;
        /// <summary>
        /// The bounds for the Bullet testing for collision
        /// </summary>
        public Rectangle HitBounds { get; protected set; }
        /// <summary>
        /// The bounds for the Bullet testing for grazing.
        /// One is for top level
        /// This run through the HitBounds and extrude past it on either side of the bullet.
        /// It should a be a small protrusion, however.
        /// </summary>
        public Rectangle GrazeBounds { get; protected set; }

        /// <summary>
        /// Represents the ownership for this Bullet
        /// </summary>
        public bool onTeamA;
        private bool b_is_active;
        /// <summary>
        /// Denotes whether or not the bullet is active,
        /// ergo, whether or not it will be updated and drawn
        /// </summary>
        public bool isActive;
        /// <summary>
        /// The base, unchanging damage for the bullet
        /// </summary>
        public int baseDamage { get; protected set; }
        /// <summary>
        /// The current damage of the bullet once affected by collision effects
        /// </summary>
        public int deltaDamage;

        public bool isDying;

        /// <summary>
        /// The base, unchanging health for the bullet
        /// </summary>
        public int baseHealth { get; protected set; }
        /// <summary>
        /// The current health of the bullet once damaged
        /// </summary>
        public int deltaHealth;

        public bool hasGrazingAction;

        /// <summary>
        /// The elemental type of bullet this can be
        /// </summary>
        public enum ElementTypes
        {
            Fire,
            Ice,
            Water,
            Metal,
            Glass,
            Energy,
            Electricity,
            Wood,
            Sound,
            Gum,
            Rubber
        }
        /// <summary>
        /// The starting elemental type of the Bullet before extra types are added.
        /// </summary>
        public ElementTypes startingEType { get; protected set; }
        /// <summary>
        /// The list of acquired elemental types
        /// </summary>
        public List<ElementTypes> ETypes;
        /*
         * It is important, at creation of the final Bullet
         * template, to add the startingBType to the BTypes
         * list, else the starting type will never be
         * considered during collision or graze checking.
        */
        /// <summary>
        /// They X/Y coordinate that represents the point of the Bullet
        /// </summary>
        public Vector2 ricochetZeroPoint { get; protected set; }
        /// <summary>
        /// Denotes how long, in milliseconds, the bullet
        /// will live. -1 is until it is hit
        /// </summary>
        public double lifetime;
        /// <summary>
        /// The current amount of life remaining within the Bullet
        /// </summary>
        protected double lifetimeCounter;

        public delegate void DeactivationHandler(Bullet sender);
        public event DeactivationHandler DeactivationEvent;

        /// <summary>
        /// Creates a Bullet template
        /// </summary>
        protected Bullet()
        {
            position = Vector2.Zero;
            baseVelocity = Vector2.Zero;
            maxVelocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            rotation = 0.0f;
            radius = 3;
            animator = new Animator();
            animator.AddData(idlingAnim);
            animator.AddData(dyingAnim);
            isActive = true;
            scale = 1.0f;
            ETypes = new List<ElementTypes>();
            deltaDamage = baseDamage;
            deltaHealth = baseHealth;
            curMover = idlingAnim;
            animator.Play(curMover, sprEff);
        }

        /// <summary>
        /// Creates a Bullet
        /// </summary>
        /// <param name="game">The game in which this bullet will be used</param>
        /// <param name="position">The initial position of the bBllet</param>
        /// <param name="teamA">The ID derived from the player or team</param>
        /// <param name="image">How the bullet looks</param>
        /// <param name="SE">The visual orientation of the Bullet</param>
        /// <param name="rot">the Bullet's intial rotation</param>
        public Bullet(Vector2 pos, bool teamA,  Bullet bullet_type, float rot = 0)
        {
            onTeamA = teamA;
            sprEff = onTeamA ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            rotation = rot;
            bulletType = bullet_type;
            onTeamA = teamA;
            position = pos;
            dyingAnim.DeactivationEvent += Deactivate;
        }

        /// <summary>
        /// Copies a Bullet
        /// </summary>
        /// <param name="bu">The Bullet to be copied</param>
        public Bullet(ref Bullet bu)
        {
            baseVelocity = bu.baseVelocity;
            maxVelocity = bu.maxVelocity;
            center = bu.center;
            acceleration = bu.acceleration;
            rotation = bu.rotation;
            scale = bu.scale;
            sprEff = bu.sprEff;
            idlingAnim = bu.idlingAnim;
            dyingAnim = bu.dyingAnim;
            EffectAnims = bu.EffectAnims;
            curMover = bu.curMover;
            radius = bu.radius;
            HitBounds = bu.HitBounds;
            GrazeBounds = bu.GrazeBounds;
            onTeamA = bu.onTeamA;
            baseDamage = bu.baseDamage;
            baseHealth = bu.baseHealth;
            hasGrazingAction = bu.hasGrazingAction;
            startingEType = bu.startingEType;
            ETypes = bu.ETypes;
            ricochetZeroPoint = bu.ricochetZeroPoint;
            lifetime = bu.lifetime;
            isActive = true;
        }


        #region SetupMethods

        /// <summary>
        /// Sets up the bullet to move
        /// </summary
        /// <param name="pos">The initial position of the bullet</param>
        /// <param name="base_vel">the initial velocity of the bullet</param>
        /// <param name="max_vel">the fastest the bullet could every become via acceleration</param>
        /// <param name="accel">the accleration of the bullet. THIS WILL BE DIVIVED BY 60</param>
        public void SetupMovement(Vector2 pos, Vector2 base_vel, Vector2 max_vel, Vector2 accel)
        {
            position = pos;
            baseVelocity = base_vel;
            maxVelocity = max_vel;
            acceleration = accel;
        }
        #endregion

        /// <summary>
        /// Updates multiple aspects of the Bullet
        /// </summary>
        /// <param name="gameTime">The GameTime's timer to aid in the Bullet's updating</param>
        public abstract void Update(GameTime gameTime, ref Bullet bu);

        public void Update(GameTime gameTime)
        {
            if (lifetimeCounter > 0)
            {
                lifetimeCounter -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (lifetimeCounter <= 0)
                {
                    isActive = false;
                }
            }
            animator.Animate(gameTime);
        }

        /// <summary>
        /// Sets the Bullet to be inactive
        /// </summary>
        /// <param name="Draw">Determines whether or not the dying animation is drawn</param>
        public void Kill(bool Draw = true)
        {
            isDying = true;
            if (Draw)
            {
                animator.Stop(curMover);
                curMover = dyingAnim;
                animator.Play(curMover, sprEff);
            }
            else
            {
                Deactivate(this);
            }
        }

        public void Reset(Bullet type)
        {
            this.bulletType = type;
            Reset();
        }

        public void Reset()
        {
            acceleration = bulletType.acceleration;
            baseDamage = bulletType.baseDamage;
            baseHealth = bulletType.baseHealth;
            baseVelocity = bulletType.baseVelocity;
            center = bulletType.center;
            deltaDamage = baseDamage;
            deltaHealth = baseHealth;
            deltaVelocity = baseVelocity;
            dyingAnim = bulletType.dyingAnim;
            EffectAnims = bulletType.EffectAnims;
            ETypes = bulletType.ETypes;
            GrazeBounds = bulletType.GrazeBounds;
            hasGrazingAction = bulletType.hasGrazingAction;
            HitBounds = bulletType.HitBounds;
            idlingAnim = bulletType.idlingAnim;
            lifetime = bulletType.lifetime;
            lifetimeCounter = lifetime;
            maxVelocity = bulletType.maxVelocity;
            radius = bulletType.radius;
            ricochetZeroPoint = bulletType.ricochetZeroPoint;
            rotation = bulletType.rotation;
            scale = bulletType.scale;
            sprEff = bulletType.sprEff;
            startingEType = bulletType.startingEType;
            //personal resetting
            curMover = idlingAnim;
            animator = new Animator();
        }

        private void Deactivate(object sender)
        {
            isActive = false;
            if (DeactivationEvent != null)
                DeactivationEvent(this);
        }


        public abstract Bullet CopyBullet(Vector2 pos);
    }
}