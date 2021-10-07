using Dan.Character.Collision;
using Dan.Helper.Utils;
using UnityEngine;

namespace Dan.Weapon.Projectile
{
    public abstract class BaseProjectile : PoolableMonobehaviour
    {
        protected float Range;
        private AttackBox _attackBox;

        public abstract void Fire(Vector3 startPosition, Vector3 direction);
        protected abstract void Hit();

        public BaseProjectile SetRange(float range)
        {
            Range = range;
            return this;
        }

        public BaseProjectile SetOwner(Transform ownerTransform)
        {
            _attackBox.SetOwnerTag(ownerTransform.tag);
            return this;
        }

        protected virtual void Initialization()
        {
            _attackBox = GetComponentInChildren<AttackBox>();
            if (_attackBox != null)
                _attackBox.OnHit += Hit;
        }

        protected virtual void TearDown()
        {
            if (_attackBox != null)
                _attackBox.OnHit -= Hit;
        }
        

        private void Awake() => Initialization();

        private void OnDestroy() => TearDown();
    }
}