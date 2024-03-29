﻿using System;
using System.Collections;
using Dan.Helper.Utils;
using Dan.Manager;
using Dan.Weapon.Projectile;
using UnityEngine;

namespace Dan.Weapon
{
    [AddComponentMenu("Dan/Sphere Shooter (Weapon)")]
    public class SphereShooter : MonoBehaviour, IWeapon
    {
        [SerializeField]
        private BaseProjectile projectilePrefab;
        [SerializeField]
        private int projectilePoolCount = 100;
        [SerializeField]
        private float weaponRange = 10f;
        [SerializeField]
        private float fireSpeed;
        [SerializeField]
        private int activationLevel = 1;
        [SerializeField]
        private bool invertFireDirection;

        public float FireSpeed => fireSpeed;
        public int ActivationLevel => activationLevel;

        private bool IsReadyToFire { get; set; } = false;
        private GameObjectPool<BaseProjectile> _bulletPool;

        public void Fire(Vector3 direction)
        {
            if (!IsReadyToFire)
                return;
            
            var bullet = _bulletPool.GetObject();
            bullet.SetOwner(transform)
                .SetRange(weaponRange);
            bullet.Fire(transform.position, direction * (invertFireDirection ? -1 : 1));

            StartCoroutine(StartCooldown());
        }

        public void ResetWeapon()
        {
            IsReadyToFire = true;
        }

        private IEnumerator StartCooldown()
        {
            IsReadyToFire = false;
            yield return new WaitForSeconds(1 / Mathf.Max(FireSpeed, Mathf.Epsilon));
            IsReadyToFire = true;
        }

        private void Awake()
        {
            _bulletPool = GameObjectPool<BaseProjectile>.CreateInstance(projectilePrefab, projectilePoolCount, FreeObjectPool.Transform);
        }

        private void Start()
        {
            IsReadyToFire = true;
        }
    }
}