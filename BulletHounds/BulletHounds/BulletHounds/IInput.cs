using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletHounds
{
    public interface IInput
    {
        /// <summary>
        /// The previous keyboard state
        /// </summary>
        KeyboardState prevKey { get; }

        /// <summary>
        /// The current keyboard state
        /// </summary>
        KeyboardState keyState { get; }
        //TODO: customizable controls
        //TOOD: focus on gamepad
        /// <summary>
        /// The current gamepad state
        /// </summary>
        GamePadState gamePadState { get; }

        /// <summary>
        /// The previous gamepad stat
        /// </summary>
        GamePadState prevGamePadState { get; }
        /// <summary>
        /// The Player index for the gamepad
        /// </summary>
        PlayerIndex pIndex { get; }

        #region ButtonDelegates
        Dictionary<GamePadButtons, Action<GameTime>> gamePadDictionary { get; }
        /*
        public delegate void ButtonA(GameTime gt);
        public delegate void ButtonA();
        public delegate void ButtonB(GameTime gt);
        public delegate void ButtonB();
        public delegate void ButtonX(GameTime gt);
        public delegate void ButtonX();
        public delegate void ButtonY(GameTime gt);
        public delegate void ButtonY();

        public delegate void BumperLeft(GameTime gt);
        public delegate void BumperLeft();
        public delegate void BumperRight(GameTime gt);
        public delegate void BumperRight();

        public delegate void TriggerLeft(GameTime gt);
        public delegate void TriggerLeft();
        public delegate void TriggerRight(GameTime gt);
        public delegate void TriggerRight();

        public delegate void DPadUp(GameTime gt);
        public delegate void DPadUp();
        public delegate void DPadDown(GameTime gt);
        public delegate void DPadDown();
        public delegate void DPadLeft(GameTime gt);
        public delegate void DPadLeft();
        public delegate void DPadRight(GameTime gt);
        public delegate void DPadRight();
         * */
        #endregion

    }
}
