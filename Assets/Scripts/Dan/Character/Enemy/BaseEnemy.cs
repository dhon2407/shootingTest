using System.Collections;
using Dan.Character.Collision;
using Dan.Helper.Utils;
using UnityEngine;

namespace Dan.Character.Enemy
{
    public abstract class BaseEnemy : PoolableMonobehaviour
    {
        [SerializeField]
        private float timeToRemove = 2f;
        [SerializeField]
        protected float hitPoints = 5;
        [SerializeField]
        protected int enemyKillScore = 10;

        public bool IsDead { get; protected set; }
        
        protected Transform PlayerTransform;
        protected float CurrentHitPoints; 
        private CameraVisibility _camVisibility;
        private HitBox _hitBox;

        public abstract void Initialize();
        public abstract void SetPlayer(Player player);
        public abstract void SetMoveDirection(Vector3 direction);
        public abstract void StartMoving();
        public abstract void Die();

        private void Awake()
        {
            Initialize();
            SetupEvents();
        }
        
        private void OnDestroy() => ClearEvents();


        private void SetupEvents()
        {
            _camVisibility = GetComponentInChildren<CameraVisibility>();
            if (_camVisibility != null)
                _camVisibility.OnVisibilityChange += ChangeCameraVisibility;

            _hitBox = GetComponentInChildren<HitBox>();
            if (_hitBox != null)
                _hitBox.OnHit += Hit;
        }

        private void ClearEvents()
        {
            if (_camVisibility != null)
                _camVisibility.OnVisibilityChange -= ChangeCameraVisibility;
            if (_hitBox != null)
                _hitBox.OnHit -= Hit;
        }

        private void Hit()
        {
            CurrentHitPoints = Mathf.Max(0, CurrentHitPoints - 1);
            if (CurrentHitPoints <= 0)
                Die();
        }

        private void ChangeCameraVisibility(bool isVisible)
        {
            if (!gameObject.activeSelf || hitPoints <= 0)
                return;
            
            if (isVisible)
                StopCoroutine(RemoveFromPlay());
            else
                StartCoroutine(RemoveFromPlay());
        }

        private IEnumerator RemoveFromPlay()
        {
            var timeLapse = 0f;

            while (timeLapse < timeToRemove)
            {
                yield return null;
                timeLapse += Time.deltaTime;
            }
            
            gameObject.SetActive(false);
        }
    }
}