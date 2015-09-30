using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PicAnimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletManager
{
    public class Player:DrawableGameComponent
    {
        /*
         * To use your animations they must be added to the animator, or else they can't be played.
         */
        /// <summary>
        /// Represents the team ID for the Player
        /// </summary>
        public bool onTeamA { get; protected set; }
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
        public int baseHealth { get; protected set; }
        /// <summary>
        /// Represents the Player's current health
        /// </summary>
        public int deltaHealth;

        public Vector2 position;
        /// <summary>
        /// Represents the Player's stock Velocity
        /// </summary>
        public Vector2 baseVelocity { get; protected set; }
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

        public Player(Game game, bool teamA):base(game)
        {
            onTeamA = teamA;
            Attacks = new List<Attack>();
            Anims = new List<Mover>();
            animator = new Animator();
            receiveInput = true;
            isActive = true;
            if (onTeamA )
            {
                SprEff = SpriteEffects.None;
            }
            else
            {
                SprEff = SpriteEffects.FlipHorizontally;
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}
