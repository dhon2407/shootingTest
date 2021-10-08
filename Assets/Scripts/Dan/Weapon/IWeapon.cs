using UnityEngine;

namespace Dan.Weapon
{
    public interface IWeapon
    {
        float FireSpeed { get; }
        int ActivationLevel { get; }
        void Fire(Vector3 direction);
        void ResetWeapon();
    }
}