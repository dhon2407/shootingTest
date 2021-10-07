using System;
using UnityEngine;

namespace Dan.Character.Collision
{
    public class AttackBox : MonoBehaviour
    {
        public event Action OnHit;
        public void SetOwnerTag(string tagName) => tag = tagName;
        public void Hit() => OnHit?.Invoke();
    }
}