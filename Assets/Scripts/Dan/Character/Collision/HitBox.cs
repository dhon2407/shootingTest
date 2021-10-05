using System;
using UnityEngine;

namespace Dan.Character.Collision
{
    public class HitBox : MonoBehaviour
    {
        public event Action OnHit;
        private void OnTriggerEnter(Collider other)
        {
            var attackBox = other.GetComponent<AttackBox>();
            if (attackBox == null || attackBox.CompareTag(tag))
                return;
            
            attackBox.OnHit();
            OnHit?.Invoke();
        }
    }
}