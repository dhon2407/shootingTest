using System;
using UnityEngine;

namespace Dan.Character.Input
{
    public interface IInputHandler
    {
        event Action Fire;
        event Action Pause;
        Vector3 MoveVector { get; }
    }
}