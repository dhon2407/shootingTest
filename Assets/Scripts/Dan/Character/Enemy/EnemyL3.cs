﻿using System.Collections;
using Dan.Weapon;
using DG.Tweening;
using UnityEngine;

namespace Dan.Character.Enemy
{
    public class EnemyL3 : BaseEnemy
    {
        [SerializeField]
        private Transform model;
        [SerializeField]
        private float initialForwardMovementDistance = 5;
        
        private Vector3 _moveDirection;
        private IWeapon _weapon;
        private bool _fireMode;

        public override void StartMoving()
        {
            IsDead = false;
            OutOfCamera = false;
            
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
            ResetParameters();
            
            base.Die();
        }

        private void StartFighting()
        {
            if (IsDead || OutOfCamera)
                return;

            StartCoroutine(RandomlyFireAtPlayer());
        }

        private IEnumerator RandomlyFireAtPlayer()
        {
            while (!IsDead)
            {
                _fireMode = true;
                StartRotating();
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(StartFiring());
                yield return new WaitForSeconds(3f);
                _fireMode = false;
                StopRotating();
                StartCoroutine(ContinueMoving());
                yield return new WaitForSeconds(5f);
            }
        }

        private IEnumerator ContinueMoving()
        {
            transform.DOLookAt(_moveDirection, 0.2f);
            while (gameObject.activeSelf && !_fireMode)
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
            while (gameObject.activeSelf && TargetPlayer != null && _fireMode)
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

        private void StopRotating()
        {
            model.DOKill(true);
            model.DOLocalRotate(Quaternion.identity.eulerAngles, 0.1f);
        }

        private void ResetParameters()
        {
            model.DOKill(true);
            model.localRotation = Quaternion.identity;
            HitPoints = maxHitPoints;
        }
    }
}