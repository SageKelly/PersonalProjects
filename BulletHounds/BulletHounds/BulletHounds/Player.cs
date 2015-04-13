using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PicAnimator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BulletHounds
{
    public class Player : DrawableGameComponent, IInput
    {
        private delegate void BulletUpdateEventHandler(GameTime gt);


        public Vector2 position;
        public Animator image;
        public List<Attack> attacks;
        public Attack chosenAttack;
        public Dictionary<string, Mover> playerAnims;
        public List<Effect> effects;
        public Animator playerAnimations;
        protected Game game;


        #region Properties
        public KeyboardState prevKey { get; private set; }

        public KeyboardState keyState { get; private set; }

        public GamePadState prevGamePadState { get; private set; }

        public GamePadState gamePadState { get; private set; }

        public PlayerIndex pIndex { get; private set; }

        Dictionary<GamePadButtons, Action<GameTime>> IInput.gamePadDictionary { get; private set; }
        #endregion

        public Player(Game g)
            : base(g)
        {
            game = g;
            position = Vector2.Zero;
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
            playerAnimations = new Animator(game, Vector2.Zero);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            keyState = Keyboard.GetState();
            gamePadState = GamePad.GetState(pIndex);



            prevKey = keyState;
            prevGamePadState = gamePadState;
        }

        public void HandleControls(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion

        protected virtual void SetupAttacks()
        {

        }

        protected virtual void SetupAnimations()
        {

        }



        public virtual void Attack()
        {
            chosenAttack.Shoot();
        }


    }
}
