using Dan.Helper.Utils;
using UnityEngine;

namespace Dan.Weapon.Projectile
{
    public abstract class BaseProjectile : PoolableMonobehaviour
    {
        protected float Range;

        public abstract void Fire(Vector3 startPosition, Vector3 direction);
        
        public void SetRange(float range)
        {
            Range = range;
        }
    }
}