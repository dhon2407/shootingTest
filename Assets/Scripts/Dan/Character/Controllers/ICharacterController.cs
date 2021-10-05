using System;

namespace Dan.Character.Controllers
{
    public interface ICharacterController
    {
        event Action OnFireWeapon;
        void Activate(bool active);
        void SetMovespeed(float speed);
    }
}