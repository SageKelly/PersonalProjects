using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PicAnimator;

namespace BulletHounds
{
    public class Bullet : DrawableGameComponent
    {
        private Game game;
        public Image2D bulletImage;

        public Vector2 position, baseVelocity, deltaVelocity, maxVelocity, acceleration;
        public float rotation;
        public float scale;
        /// <summary>
        /// Bullet animator for animated bullets
        /// </summary>
        public Animator bulletAnimator { get; private set; }
        public List<Mover> bulletMovers;
        public Mover curMover;
        /// <summary>
        /// The bullet's main hitbounds
        /// </summary>
        public Rectangle hitbounds;
        /// <summary>
        /// The hitbounds used for bullets with grazing effects
        /// </summary>
        public Rectangle grazingHitbounds;
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
            }
        }
        public int damage;
        public int health;
        private int counter;
        private Vector2 ricochetZeroPoint;
        /// <summary>
        /// Denotes how long, in milliseconds, the bullet
        /// will live. -1 is until it is hit
        /// </summary>
        public int lifetimeCounter;
        /// <summary>
        /// Holds the millisecond-based delay for spawning bullet
        /// </summary>
        private int delay;
        /// <summary>
        /// Denotes a delay has been set
        /// </summary>
        private bool delaySet;
        /// <summary>
        /// The method that will activate after the
        /// bullet is hit. Must return whether or not
        /// the bullet is active.
        /// </summary>
        public Func<Bullet, bool> hitAction;
        /// <summary>
        /// Denotes a hit action has been set;
        /// </summary>
        private bool hitActionSet;
        /// <summary>
        /// Used for bullets with grazing effects. The
        /// first paramenter is the sender, while the
        /// second is the recipient.
        /// </summary>
        private Action<Bullet, Bullet> grazingAction;
        /// <summary>
        /// The bullet's update method
        /// </summary>
        private Action<Bullet> updateAction;
        /// <summary>
        /// Denotes an animation has been set, and
        /// this should use that instead of the image
        /// </summary>
        private bool isAnimated;
        /// <summary>
        /// Denotes the drawing for this bullet is
        /// complex, and more attributes have been
        /// defined.
        /// </summary>
        private bool isComplexDraw;

        SpriteBatch spriteBatch;

        public Bullet(Game game)
            : base(game)
        {
            this.game = game;
            bulletImage = new Image2D();
            position = Vector2.Zero;
            baseVelocity = Vector2.Zero;
            maxVelocity = Vector2.Zero;
            acceleration = Vector2.Zero;
            rotation = 0.0f;
            hitbounds = Rectangle.Empty;
            bulletAnimator = new Animator(game, position);
            isActive = true;
            scale = 1.0f;
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
        public Bullet(Game game, Image2D image, int id, float rot = 0)
            : this(game)
        {
            bulletImage = image;
            rotation = rot;
        }
        /// <summary>
        /// Creates a bullet
        /// </summary>
        /// <param name="game">Just do it</param>
        /// <param name="image">How the bullet looks</param>
        /// <param name="bullet_damage">How much damage the bullet deals</param>
        /// <param name="bullet_health">How much health the bullet has</param>
        /// <param name="updateMethod">The method to update the bullet's position</param>
        /// <param name="rot">the bullet's intial rotation</param>
        public Bullet(Game game, Image2D image, float rot = 0)
            : this(game)
        {
            bulletImage = image;
            hitbounds = new Rectangle((int)position.X, (int)position.Y,
                bulletImage.image.Width, bulletImage.image.Height);
            rotation = rot;
        }

        #region SetupMethods
        /// <summary>
        /// Sets the bullet up to be animated
        /// </summary>
        /// <param name="bullet_animator">the animator for the bullet</param>
        /// <param name="intialMover">the initial animation for the bullet.
        /// Can be pulled from the public animations Dictionary</param>
        /// <param name="SprEff">The spriteeffects for</param>
        public void SetupAnimation(Animator bullet_animator, Mover intialMover, SpriteEffects SprEff, float speed_percentage)
        {
            bulletAnimator = bullet_animator;
            curMover = intialMover;

            if (bullet_animator != null)
                isAnimated = true;
        }

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

        public void UpdateBullet(GameTime gameTime)
        {
            if (isActive)
            {
                if (isAnimated)
                    bulletAnimator.PlayAnimation(curMover, bulletImage.spriteEf);
                #region Update bullet movement
                /*
            if (acceleration != Vector2.Zero)
            {
                deltaVelocity += acceleration;
                if (deltaVelocity.X >= maxVelocity.X)
                    deltaVelocity.X = maxVelocity.X;
                if (deltaVelocity.Y >= maxVelocity.Y)
                    deltaVelocity.Y = maxVelocity.Y;

            }
            position.X += deltaVelocity.X * (float)Math.Cos(rotation);
            position.Y += deltaVelocity.Y * (float)Math.Sin(rotation);
            */
                #endregion
                updateAction(this);
                //Update HitBounds
                hitbounds.Offset(
                    (int)(deltaVelocity.X * (float)Math.Cos(rotation)),
                    (int)(deltaVelocity.Y * (float)Math.Sin(rotation))
                    );
                if (hitActionSet)
                {
                    hitAction(this);
                }
            }
        }

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

        /// <summary>
        /// resets the bullet's data
        /// </summary>
        public void ResetBullet()
        {
            bulletAnimator.StopAnimation(curMover);
            counter = 0;
            isActive = true;
            isActive = true;
        }


        public override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (isActive)
            {
                switch (bulletImage.drawType)
                {
                    case 1:
                        spriteBatch.Draw(bulletImage.image, position, Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(bulletImage.image, position, bulletImage.sourceBounds, Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(bulletImage.image, position, bulletImage.sourceBounds, Color.White,
                            rotation, bulletImage.center, scale, bulletImage.spriteEf, 0);
                        break;
                }
            }
        }
    }
}