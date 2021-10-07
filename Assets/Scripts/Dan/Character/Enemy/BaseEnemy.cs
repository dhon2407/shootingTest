using System;
using System.Collections;
using Dan.Character.Collision;
using Dan.Helper.Utils;
using Dan.Manager;
using DG.Tweening;
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
        [SerializeField]
        private GameObject explosionObj;
        [SerializeField]
        private float initialForwardMovementDistance = 5;
        
        public int MaxHitPoints => maxHitPoints;
        public int HitPoints { get; protected set; }
        public bool IsDead { get; protected set; }
        public float MoveSpeed => moveSpeed;
        
        protected bool OutOfCamera { get; set; }
        protected Vector3 MoveDirection { get; set; }

        public event Action OnCharacterDeath;
        public event Action OnCharacterHit;

        protected Player TargetPlayer;
        private CameraVisibility _camVisibility;
        private HitBox _hitBox;
        private Coroutine _removeRoutine;

        public abstract void Hit();
        protected abstract void StartFighting();

        public void StartMoving()
        {
            IsDead = false;
            OutOfCamera = false;
            
            transform.DOMove(transform.position + MoveDirection * initialForwardMovementDistance, 1.5f)
                .SetEase(Ease.OutSine)
                .OnComplete(StartFighting);
        }
        
        public void SetMoveDirection(Vector3 direction)
        {
            MoveDirection = direction;
        }

        public void SetPlayer(Player player)
        {
            TargetPlayer = player;
        }
        
        public virtual void Die()
        {
            IsDead = true;
            GameFlowManager.AddScore(enemyKillScore);
            InvokeOnCharacterDeath();
            Destroy(Instantiate(explosionObj, transform.position, Quaternion.identity, FreeObjectPool.Transform), 2f);
            
            transform.rotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            gameObject.SetActive(false);
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
            {
                if (_removeRoutine != null)
                    StopCoroutine(_removeRoutine);
            }
            else
            {
                _removeRoutine = StartCoroutine(RemoveFromPlay());
            }
        }

        private IEnumerator RemoveFromPlay()
        {
            var timeLapse = 0f;

            while (timeLapse < timeToRemove)
            {
                yield return null;
                timeLapse += Time.deltaTime;
            }

            OutOfCamera = true;
            gameObject.SetActive(false);
        }
    }
}