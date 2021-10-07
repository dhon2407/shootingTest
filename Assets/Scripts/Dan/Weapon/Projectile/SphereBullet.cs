using System;
using System.Collections;
using Dan.Character.Collision;
using UnityEngine;

namespace Dan.Weapon.Projectile
{
    public class SphereBullet : BaseProjectile
    {
        [SerializeField]
        private float speed;

        private Vector3 _direction;
        private Vector3 _firedPosition;

        protected override void Hit()
        {
            gameObject.SetActive(false);
        }

        public override void Fire(Vector3 startPosition, Vector3 direction)
        {
            _firedPosition = startPosition;
            transform.position = _firedPosition;
            _direction = direction;
            StartCoroutine(Fire());
        }

        private IEnumerator Fire()
        {
            while (gameObject.activeSelf)
            {
                transform.Translate(_direction * speed * Time.deltaTime);

                if (IsOutOfRange())
                {
                    gameObject.SetActive(false);
                    yield break;
                }

                yield return null;
            }
        }

        private bool IsOutOfRange()
        {
            return Vector3.Distance(_firedPosition, transform.position) > Range;
        }

        public override void OnDisable()
        {
            StopAllCoroutines();
            base.OnDisable();
        }
    }
}