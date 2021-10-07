using System.Collections;
using Dan.Helper.Utils;
using Dan.Manager;
using Dan.Weapon.Projectile;
using UnityEngine;

namespace Dan.Weapon
{
    public class SpreadShooter : MonoBehaviour, IWeapon
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
        private int spreadCount = 5; 

        public float FireSpeed => fireSpeed;

        private bool IsReadyToFire { get; set; } = false;
        private GameObjectPool<BaseProjectile> _bulletPool;

        public void Fire(Vector3 direction)
        {
            if (!IsReadyToFire)
                return;

            var angleStep = 360f / spreadCount;
            var angle = 0f;

            for (int i = 0; i < spreadCount; i++)
            {
                var xPos = transform.position.x + Mathf.Sin(angle * Mathf.PI / 180) * 5f;
                var yPos = transform.position.y + Mathf.Cos(angle * Mathf.PI / 180) * 5f;

                var pVector = new Vector2(xPos, yPos);
                direction = (pVector - (Vector2)transform.position).normalized;
                
                var bullet = _bulletPool.GetObject();
                bullet.SetOwner(transform)
                    .SetRange(weaponRange);
                bullet.Fire(transform.position, direction);

                angle += angleStep;
            }

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