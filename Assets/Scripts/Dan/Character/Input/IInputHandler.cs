using System;
using UnityEngine;

namespace Dan.Character.Input
{
    public interface IInputHandler
    {
        event Action Fire;
        Vector3 MoveVector { get; }
    }
}