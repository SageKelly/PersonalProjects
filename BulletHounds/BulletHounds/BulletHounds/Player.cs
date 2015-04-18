using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PicAnimator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BulletHounds
{
    public abstract class Player : DrawableGameComponent, IInput
    {
        private delegate void BulletUpdateEventHandler(GameTime gt);


        public Vector2 position;
        public List<Attack> attacks;
        protected List<Bullet> bullets;
        public List<Bullet> firedBullets;
        public int chosenAttack;
        public Dictionary<string, Mover> movers;
        public Animator animationSet;
        protected Game game;


        #region Properties
        public KeyboardState prevKey { get; private set; }

        public KeyboardState keyState { get; private set; }

        public GamePadState prevGamePadState { get; private set; }

        public GamePadState gamePadState { get; private set; }

        public PlayerIndex pIndex { get; private set; }

        protected Dictionary<Buttons, Action<GameTime>> gamePadDictionary { get; private set; }
        #endregion

        public Player(Game g)
            : base(g)
        {
            game = g;
            position = Vector2.Zero;
            bullets = new List<Bullet>();
            attacks = new List<Attack>();
            animationSet = new Animator(game, Vector2.Zero);
        }

        public Player(Game g, PlayerIndex player_index)
            : this(g)
        {
            pIndex = player_index;
            position = Vector2.Zero;
        }

        #region GameComponent Methods
        public override void Initialize()
        {
            base.Initialize();
            animationSet = new Animator(game, Vector2.Zero);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            keyState = Keyboard.GetState();
            gamePadState = GamePad.GetState(pIndex);



            prevKey = keyState;
            prevGamePadState = gamePadState;
        }

        public abstract void HandleControls(GameTime gameTime);

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion

        protected abstract void SetupBullets();

        protected abstract void SetupAttacks();

        protected abstract void SetupAnimations();

        protected virtual void SetupGamePad()
        {
            gamePadDictionary.Add(Buttons.A, null);
            gamePadDictionary.Add(Buttons.B, null);
            gamePadDictionary.Add(Buttons.X, null);
            gamePadDictionary.Add(Buttons.Y, null);
            gamePadDictionary.Add(Buttons.DPadUp, null);
            gamePadDictionary.Add(Buttons.DPadDown, null);
            gamePadDictionary.Add(Buttons.DPadLeft, null);
            gamePadDictionary.Add(Buttons.DPadRight, null);
            gamePadDictionary.Add(Buttons.LeftShoulder, null);
            gamePadDictionary.Add(Buttons.RightShoulder, null);
            gamePadDictionary.Add(Buttons.LeftTrigger, null);
            gamePadDictionary.Add(Buttons.RightTrigger, null);
            gamePadDictionary.Add(Buttons.Start, null);
        }

        public virtual void Attack()
        {
            attacks[chosenAttack].Shoot();
        }
    }
}
