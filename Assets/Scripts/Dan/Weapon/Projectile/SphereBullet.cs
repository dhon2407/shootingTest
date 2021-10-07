using System;
using System.Collections;
using Dan.Character.Collision;
using PolygonArsenal;
using UnityEngine;

namespace Dan.Weapon.Projectile
{
    public class SphereBullet : BaseProjectile
    {
        [SerializeField]
        private GameObject explosionPrefab;
        [SerializeField]
        private float speed;

        [SerializeField]
        private PolygonSoundSpawn soundSpawn;

        private Vector3 _direction;
        private Vector3 _firedPosition;

        protected override void Hit()
        {
            var impactP = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(impactP, 0.5f);
            
            gameObject.SetActive(false);
        }

        public override void Fire(Vector3 startPosition, Vector3 direction)
        {
            _firedPosition = startPosition;
            transform.position = _firedPosition;
            _direction = direction;
            StartCoroutine(Fire());

            soundSpawn.Play();
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