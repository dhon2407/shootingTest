using System;
using System.Collections;
using Dan.Character.Collision;
using Dan.Helper.Utils;
using Dan.Manager;
using UnityEngine;

namespace Dan.Character.Enemy
{
    public abstract class BaseEnemy : PoolableMonobehaviour, ICharacter
    {
        [SerializeField]
        private float timeToRemove = 2f;
        [SerializeField]
        protected int maxHitPoints = 5;
        [SerializeField]
        protected int enemyKillScore = 10;
        [SerializeField]
        private float moveSpeed = 10f;

        public int MaxHitPoints => maxHitPoints;
        public int HitPoints { get; protected set; }
        public bool IsDead { get; protected set; }
        public float MoveSpeed => moveSpeed;

        public event Action OnCharacterDeath;
        public event Action OnCharacterHit;

        protected Player TargetPlayer;
        
        private CameraVisibility _camVisibility;
        private HitBox _hitBox;

        public abstract void SetMoveDirection(Vector3 direction);
        public abstract void StartMoving();
        public abstract void Die();
        public abstract void Hit();

        public void SetPlayer(Player player)
        {
            TargetPlayer = player;
        }
        
        protected virtual void Initialize()
        {
            HitPoints = MaxHitPoints;
        }
        
        protected virtual void GameEnds()
        {
            TargetPlayer = null;
        }

        protected void InvokeOnCharacterDeath() => OnCharacterDeath?.Invoke();
        protected void InvokeOnCharacterHit() => OnCharacterHit?.Invoke();
        
        protected IEnumerator LookAtPlayer()
        {
            while (true)
            {
                if (TargetPlayer == null)
                    yield break;

                transform.LookAt(TargetPlayer.transform.position);   
                yield return null;
            }
        }

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
            
            GameFlowManager.OnGameEnd += GameEnds;
        }

        private void ClearEvents()
        {
            if (_camVisibility != null)
                _camVisibility.OnVisibilityChange -= ChangeCameraVisibility;
            if (_hitBox != null)
                _hitBox.OnHit -= Hit;
        }

        private void ChangeCameraVisibility(bool isVisible)
        {
            if (!gameObject.activeSelf || maxHitPoints <= 0)
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