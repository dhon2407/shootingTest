using System;
using UnityEngine;

namespace Dan.Character.Controllers
{
    public class EnemyController : MonoBehaviour, ICharacterController
    {
        public event Action OnFireWeapon;

        public void Activate(bool active)
        { }

        public void SetMovespeed(float speed)
        { }
    }
}