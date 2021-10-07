using System.Collections;
using Dan.Manager;
using Dan.Weapon;
using DG.Tweening;
using UnityEngine;

namespace Dan.Character.Enemy
{
    public class EnemyL1 : BaseEnemy
    {
        [SerializeField]
        private Transform model;
        [SerializeField]
        private float initialForwardMovementDistance = 5;
        
        private Vector3 _moveDirection;
        private IWeapon _weapon;

        public override void StartMoving()
        {
            transform.DOMove(transform.position + _moveDirection * initialForwardMovementDistance, 1.5f)
                .SetEase(Ease.OutSine)
                .OnComplete(StartFighting);
        }
        
        public override void SetMoveDirection(Vector3 direction)
        {
            _moveDirection = direction;
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            _weapon = GetComponentInChildren<IWeapon>();
        }
        
        public override void Hit()
        {
            InvokeOnCharacterHit();
            HitPoints = Mathf.Max(0, HitPoints - 1);
            if (HitPoints > 0)
                return;
            
            Die();
        }
        
        public override void Die()
        {
            transform.DOKill();
            IsDead = true;
            GameFlowManager.AddScore(enemyKillScore);
            ResetParameters();
            InvokeOnCharacterDeath();
            gameObject.SetActive(false);
        }

        private void StartFighting()
        {
            if (IsDead || OutOfCamera)
                return;
            
            StartCoroutine(LookAtPlayer());
            StartCoroutine(ContinueMoving());
            StartCoroutine(StartFiring());
            StartRotating();
        }

        private IEnumerator ContinueMoving()
        {
            while (gameObject.activeSelf)
            {
                transform.position += _moveDirection * MoveSpeed * Time.deltaTime;
                yield return null;
            }
        }
        
        private IEnumerator StartFiring()
        {
            if (_weapon == null)
                yield break;
            
            _weapon.ResetWeapon();
            while (gameObject.activeSelf && TargetPlayer != null)
            {
                _weapon.Fire(transform.forward); 
                yield return null;
            }
        }

        private void StartRotating()
        {
            model.DOLocalRotate(Quaternion.Euler(0, 0, 180).eulerAngles, 0.7f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void ResetParameters()
        {
            model.DOKill(true);
            model.localRotation = Quaternion.identity;
            transform.rotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            IsDead = false;
            HitPoints = maxHitPoints;
        }
    }
}