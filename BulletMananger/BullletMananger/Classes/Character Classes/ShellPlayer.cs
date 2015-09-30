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
    /// Creates a changeable Player than can be of any type
    /// </summary>
    public class ShellPlayer
    {
        IPlayer playerType;
        /// <summary>
        /// Represents the team ID for the Player
        /// </summary>
        public int ID { get; protected set; }
        //public Game game;
        /// <summary>
        /// The list of Attacks for the Player
        /// </summary>
        public List<Attack> Attacks;
        /// <summary>
        /// The list of Animations (Movers) for the Player
        /// </summary>
        public List<Mover> Anims;
        public Animator animator;
        /// <summary>
        /// Represents the currently-playing Mover
        /// </summary>
        public Mover curMover;
        /// <summary>
        /// Represents the currently-selected Attack
        /// </summary>
        public int curAttack;
        /// <summary>
        /// Idling animation
        /// </summary>
        public Mover idlingMover { get; protected set; }
        /// <summary>
        /// Animation for being shocked the first time.
        /// The Player should not be able to move,
        /// while this is playing
        /// </summary>
        public Mover shocked1Mover;
        /// <summary>
        /// Animation for being shocked the second time.
        /// The Player should not be able to move,
        /// while this is playing
        /// </summary>
        public Mover shocked2Mover;
        /// <summary>
        /// Animation for being shocked the first time.
        /// The Player should not be able to move,
        /// while this is playing
        /// </summary>
        public Mover shocked3Mover;
        /// <summary>
        /// Animation for if the Player loses by not being shocked.
        /// </summary>
        public Mover loseMover;
        /// <summary>
        /// Animation for the Player's basic shot.
        /// </summary>
        public Mover shootMover;
        /// <summary>
        /// Animation for the Player's "stand-still" shot
        /// </summary>
        public Mover longShootMover;
        /// <summary>
        /// Animation for when the Player is hit, if necessary.
        /// </summary>
        public Mover hitMover;
        /// <summary>
        /// Animation for if the Player dies
        /// </summary>
        public Mover dyingMover { get; protected set; }
        /// <summary>
        /// Hitbounds for the Player's trinket
        /// </summary>
        public Rectangle HitBounds;
        /// <summary>
        /// Represents the Player's stock health amount
        /// </summary>
        protected int baseHealth;
        /// <summary>
        /// Represents the Player's current health
        /// </summary>
        public int deltaHealth;

        public Vector2 position;
        /// <summary>
        /// Represents the Player's stock Velocity
        /// </summary>
        protected Vector2 baseVelocity;
        /// <summary>
        /// Represents the Player's current Velocity
        /// </summary>
        public Vector2 deltaVelocity;
        /// <summary>
        /// Represents the Player's fastest possible Velocity
        /// </summary>
        public Vector2 maxVelocity;
        public Vector2 acceleration;
        /// <summary>
        /// Represents whether or not the Player can currently recieve input
        /// </summary>
        public bool receiveInput;
        /// <summary>
        /// Represents whether or not the Player is currently active
        /// </summary>
        public bool isActive;
        /// <summary>
        /// The horizontal facing of the Player
        /// </summary>
        public SpriteEffects SprEff;

        public ShellPlayer(Manager m, IPlayer type, int id = 0)
        {
            playerType = type;
            ID = id;

        }

        public void Reset(IPlayer type)
        {
            playerType = type;
            Reset();
        }

        public void Reset()
        {
            this.acceleration = playerType.acceleration;
            this.Anims = playerType.Anims;
            this.Attacks = playerType.Attacks;
            this.baseHealth = playerType.baseHealth;
            this.baseVelocity = playerType.baseVelocity;
            this.dyingMover = playerType.dyingMover;
            this.HitBounds = playerType.HitBounds;
            this.hitMover = playerType.hitMover;
            this.idlingMover = playerType.idlingMover;
            this.longShootMover = playerType.longShootMover;
            this.loseMover = playerType.loseMover;
            this.maxVelocity = playerType.maxVelocity;
            this.shocked1Mover = playerType.shocked1Mover;
            this.shocked2Mover = playerType.shocked2Mover;
            this.shocked3Mover = playerType.shocked3Mover;
            this.shootMover = playerType.shootMover;
            this.SprEff = playerType.SprEff;

            curAttack = 0;
            curMover = idlingMover;
            deltaHealth = baseHealth;
            deltaVelocity = baseVelocity;

        }


        public virtual void Kill()
        {
            isActive = false;
            animator.Stop(curMover);
            curMover = dyingMover;
            animator.Play(dyingMover, SprEff);
        }
    }
}
