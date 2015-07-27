using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PicAnimator;

namespace BulletHounds
{
    public class Bullet
    {
        #region VARIABLES
        public Image2D bulletImage;

        public enum BulletTypes
        {
            Metal,
            Water,
            Ice,
            Fire,
            Energy,
            Electricity,
            Sound,
            Gum,
            Rubber,
            Wood,
            Glass
        }
        public BulletTypes bulletType { get; set; }

        #region Motion Variables
        private Vector2 staticPos;
        public Vector2 position;
        public Vector2 baseVelocity;
        public Vector2 deltaVelocity;
        public Vector2 maxVelocity { get; private set; }
        public Vector2 acceleration { get; private set; }
        private float baseRotation;
        public float rotation;
        private float baseScale;
        public float scale;
        /// <summary>
        /// The bullet's update method
        /// </summary>
        public delegate void UpdateDelegate(ref Bullet b);
        public UpdateDelegate updateAction { get; private set; }
        #endregion

        #region Animation
        /// <summary>
        /// Bullet animator for animated bullets
        /// </summary>
        public Animator bulletAnimator { get; private set; }
        /// <summary>
        /// Dictionary of Movers for the animator
        /// </summary>
        public Dictionary<string, Mover> bulletMovers;
        /// <summary>
        /// The currently selected Mover
        /// </summary>
        public Mover curMover;
        private Mover initialMover;
        /// <summary>
        /// Denotes an animation has been set, and
        /// this should use that instead of the image
        /// </summary>
        public bool isAnimated { get; private set; }
        #endregion

        public Dictionary<BulletTypes, Effect> bulletEffects;

        #region Collision Variables
        public delegate void GrazingDelegate(ref Bullet thisOne, ref Bullet other);
        private GrazingDelegate grazing_action;
        /// <summary>
        /// Used for bullets with grazing effects. The
        /// first paramenter is the sender, while the
        /// second is the recipient.
        /// </summary>
        public GrazingDelegate grazingAction
        {
            get
            {
                return grazing_action;
            }
            private set
            {
                grazing_action = value;
                if (grazing_action != null)
                {
                    grazingSet = true;
                }
                else grazingSet = false;
            }
        }
        /// <summary>
        /// The hitbounds used for bullets with grazing effects
        /// </summary>
        public Rectangle grazingHitbounds;
        /// <summary>
        /// Denotes whether or not grazing has been set up
        /// </summary>
        public bool grazingSet { get; private set; }
        /// <summary>
        /// Denotes whether or not this bullet has been grazed.
        /// </summary>
        public bool isGrazed;

        public delegate bool HitDelegate(ref Bullet thisOne);
        private HitDelegate hit_action;
        /// <summary>
        /// The method that will activate after the
        /// bullet is hit. Must return whether or not
        /// the bullet is active.
        /// </summary>
        public HitDelegate hitAction
        {
            get
            {
                return hit_action;
            }
            set
            {
                hit_action = value;
                if (hit_action != null)
                {
                    hitActionSet = true;
                }
                else hitActionSet = false;
            }
        }
        /// <summary>
        /// Denotes a hit action has been set;
        /// </summary>
        public bool hitActionSet { get; private set; }
        /// <summary>
        /// Denotes whether or not the bullet has been hit
        /// </summary>
        public bool isHit;
        /// <summary>
        /// The bullet's main hitbounds
        /// </summary>
        public Rectangle hitbounds;
        /// <summary>
        /// Denotes whether or not the bullet's hitbounds should be checked
        /// </summary>
        public bool checkCollision;
        /// <summary>
        /// The point on the bullet where ricochetting won't occur
        /// </summary>
        public Vector2 ricochetZeroPoint;
        #endregion
        #region Basic Bullet Stuff
        private bool b_is_active;
        /// <summary>
        /// Denotes whether or not the bullet is active,
        /// ergo, whether or not it will be updated and drawn
        /// </summary>
        public bool isActive
        {
            get
            {
                return b_is_active;
            }
            set
            {
                bool temp = value;
                if (temp && !b_is_active && bulletAnimator != null)
                {
                    bulletAnimator.StopAnimation(curMover);
                }
                b_is_active = temp;
            }
        }
        private int baseDamage;
        public int damage;
        private int baseHealth;
        public int health;
        #endregion

        /// <summary>
        /// Denotes how long, in milliseconds, the bullet
        /// will live. -1 is until it is hit
        /// </summary>
        public int lifetimeCounter;

        #region Timing variables
        private int counter;
        /// <summary>
        /// Holds the millisecond-based delay for spawning bullet
        /// </summary>
        private int delay
        {
            get
            {
                return i_delay;
            }
            set
            {
                i_delay = value;
                if (i_delay != 0)
                {
                    delaySet = true;
                }
            }
        }
        private int i_delay;
        /// <summary>
        /// Denotes a delay has been set
        /// </summary>
        public bool delaySet { get; private set; }
        #endregion

        /// <summary>
        /// Denotes the drawing for this bullet is
        /// complex, and more attributes have been
        /// defined.
        /// </summary>
        private bool isComplexDraw;

        public PlayerIndex bulletID { get; private set; }

        SpriteBatch spriteBatch;
        #endregion

        #region Constructors
        private Bullet()
        {
            bulletImage = new Image2D();
            position = Vector2.Zero;
            baseVelocity = Vector2.Zero;
            maxVelocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            baseRotation = 0.0f;
            hitbounds = Rectangle.Empty;
            isActive = true;
            baseScale = 1.0f;
        }

        /// <summary>
        /// Creates a bullet
        /// </summary>
        /// <param name="game">Just do it</param>
        /// <param name="image">How the bullet looks</param>
        /// <param name="pos">The initial position of the bullet</param>
        /// <param name="base_vel">the initial velocity of the bullet</param>
        /// <param name="bullet_damage">How much damage the bullet deals</param>
        /// <param name="bullet_health">How much health the bullet has</param>
        /// <param name="updateMethod">The method to update the bullet's position</param>
        /// <param name="rot">the bullet's intial rotation</param>
        public Bullet(Image2D image, PlayerIndex id, float rot = 0)
        {
            bulletImage = image;
            baseRotation = rot;
            bulletID = id;
        }

        #endregion

        #region SetupMethods
        /// <summary>
        /// Sets the bullet up to be animated
        /// </summary>
        /// <param name="bullet_animator">the animator for the bullet</param>
        /// <param name="intialMover">the initial animation for the bullet.
        /// Can be pulled from the public animations Dictionary. This
        /// one will be used to reset the bullet.</param>
        /// <param name="mover_name">the name of the initial mover</param>
        /// <param name="bulletPosition">the position of the bullet</param>
        public virtual void SetupAnimation(Animator bullet_animator, Mover intialMover, Vector2 bulletPosition)
        {
            bulletMovers = new Dictionary<string, Mover>();
            bulletAnimator = bullet_animator;
            curMover = intialMover;
            bulletMovers.Add(curMover.name, curMover);

            if (bullet_animator != null)
                isAnimated = true;

            position = bulletPosition;
            bulletAnimator.Position = position;
        }

        /// <summary>
        /// Sets up the bullet to move.
        /// </summary>
        /// <param name="pos">The initial position of the bullet</param>
        /// <param name="base_vel">the initial velocity of the bullet</param>
        /// <param name="max_vel">the fastest the bullet could every become via acceleration</param>
        /// <param name="accel">the accleration of the bullet. THIS WILL BE DIVIVED BY 60</param>
        public void SetupMovement(Vector2 pos, Vector2 base_vel, Vector2 max_vel, Vector2 accel, UpdateDelegate update_method)
        {
            acceleration = accel;
            baseVelocity = base_vel;
            maxVelocity = max_vel;
            position = staticPos;
            staticPos = pos;
            updateAction = update_method;
        }

        public virtual void SetupEffects()
        {
            bulletEffects = new Dictionary<BulletTypes, Effect>();
        }

        /// <summary>
        /// Sets up the hitbounds for the bullet
        /// </summary>
        /// <param name="hitbounds">the hitbounds for the bullet</param>
        public virtual void setupCollision(Rectangle hitbounds)
        {
            this.hitbounds = hitbounds;
        }

        /// <summary>
        /// sets up hitbounds and an extra method for special actions for when the bullet is hit
        /// </summary>
        /// <param name="hitbounds">Defines the bullet's hitbounds</param>
        /// <param name="hitmethod">the hit action for the bullet</param>
        public virtual void setupCollision(Rectangle hitbounds, HitDelegate hitmethod)
        {
            this.hitbounds = hitbounds;
            this.hitAction = hitmethod;
            hitActionSet = true;
        }

        /// <summary>
        /// Sets up for grazing effects. SHOULD BE INITLIAZED AFTER COLLISION SETUP
        /// </summary>
        /// <param name="grazeWidth">the width of the collision bounds on either side of the bullet</param>
        /// <param name="grazingAction">the method that runs when bullet is grazed</param>
        public void setupGrazing(int grazeWidth, GrazingDelegate grazingMethod)
        {
            grazingHitbounds = new Rectangle(0, -grazeWidth, hitbounds.Width, hitbounds.Height + (grazeWidth * 2));
            this.grazingAction = grazingMethod;
        }

        #endregion

        /// <summary>
        /// Sets the internal counter until it equals the delay
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool Countdown(int milliseconds)
        {
            counter = milliseconds;
            if (counter >= delay)
                return true;
            return false;
        }

        public Bullet CopyBullet()
        {
            Bullet temp = new Bullet(bulletImage, this.bulletID, baseRotation);
            temp.acceleration = acceleration;
            temp.baseVelocity = baseVelocity;
            temp.bulletEffects = bulletEffects;
            temp.bulletMovers = bulletMovers;
            temp.bulletType = bulletType;
            temp.baseDamage = baseDamage;
            temp.delay = delay;
            temp.delaySet = delaySet;
            temp.grazingAction = grazingAction;
            temp.grazingHitbounds = grazingHitbounds;
            temp.baseHealth = baseHealth;
            temp.hitAction = hitAction;
            temp.hitbounds = hitbounds;
            temp.isAnimated = isAnimated;
            temp.isComplexDraw = isComplexDraw;
            temp.lifetimeCounter = lifetimeCounter;
            temp.maxVelocity = maxVelocity;
            temp.staticPos = staticPos;
            temp.ricochetZeroPoint = ricochetZeroPoint;
            temp.baseScale = baseScale;
            temp.SetupAnimation(bulletAnimator, curMover, position);
            temp.updateAction = updateAction;
            return temp;
        }

        public Bullet CopyBullet(Vector2 pos, int countdown = 0, float rotation = 0)
        {
            Bullet temp = new Bullet(bulletImage, this.bulletID, rotation);
            temp.acceleration = acceleration;
            temp.baseVelocity = baseVelocity;
            temp.bulletEffects = bulletEffects;
            temp.bulletMovers = bulletMovers;
            temp.bulletType = bulletType;
            temp.baseDamage = baseDamage;
            temp.delay = delay;
            temp.delaySet = delaySet;
            temp.grazingAction = grazingAction;
            temp.grazingHitbounds = grazingHitbounds;
            temp.baseHealth = baseHealth;
            temp.hitAction = hitAction;
            temp.hitActionSet = hitActionSet;
            temp.hitbounds = hitbounds;
            temp.isAnimated = isAnimated;
            temp.isComplexDraw = isComplexDraw;
            temp.lifetimeCounter = lifetimeCounter;
            temp.maxVelocity = maxVelocity;
            temp.staticPos = staticPos;
            temp.ricochetZeroPoint = ricochetZeroPoint;
            temp.baseScale = baseScale;
            temp.SetupAnimation(bulletAnimator, curMover, position);
            temp.updateAction = updateAction;
            return temp;
        }

        /// <summary>
        /// resets the bullet's data
        /// </summary>
        public void ResetBullet()
        {
            position = staticPos;
            grazingHitbounds.Offset((int)position.X, (int)position.Y);
            hitbounds.Offset((int)position.X, (int)position.Y);
            isGrazed = false;
            isHit = false;
            checkCollision = true;
            deltaVelocity = baseVelocity;
            rotation = baseRotation;
            scale = baseScale;
            bulletAnimator.StopAnimation(curMover);
            curMover = initialMover;
            counter = 0;
        }
    }
}