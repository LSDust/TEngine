using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public interface IInputModule
    {
        InputSystem_Actions.PlayerActions Player { get; }
        InputSystem_Actions.UIActions UI { get; }
    }
}
