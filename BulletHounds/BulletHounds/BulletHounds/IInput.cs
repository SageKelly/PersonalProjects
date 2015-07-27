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
        KeyboardState prevKey { get; set; }

        /// <summary>
        /// The current keyboard state
        /// </summary>
        KeyboardState keyState { get; set; }
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
    }
}
