using UnityEngine;

namespace Dan.Weapon
{
    public interface IWeapon
    {
        float FireSpeed { get; }
        void Fire(Vector3 direction);
        void ResetWeapon();
    }
}