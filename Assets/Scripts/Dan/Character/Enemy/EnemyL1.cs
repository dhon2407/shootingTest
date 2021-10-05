using System.Collections;
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
        private float moveSpeed = 10f;
        [SerializeField]
        private float initialForwardMovementDistance = 5;
        
        private Player _player;
        private Vector3 _targetDirection;
        private IWeapon _weapon;

        public override void Initialize()
        {
            _weapon = GetComponentInChildren<IWeapon>();
        }

        public override void SetPlayer(Player player)
        {
            _player = player;
        }

        public override void StartMoving()
        {
            transform.DOMove(transform.position + _targetDirection * initialForwardMovementDistance, 1.5f)
                .SetEase(Ease.OutSine)
                .OnComplete(StartFighting);
        }

        private void StartFighting()
        {
            if (IsDead)
                return;
            
            StartCoroutine(LookAtPlayer());
            StartCoroutine(ContinueMoving());
            StartCoroutine(StartFiring());
            StartRotating();
        }

        public override void Die()
        {
            transform.DOKill();
            IsDead = true;
            gameObject.SetActive(false);
        }

        private IEnumerator ContinueMoving()
        {
            while (gameObject.activeSelf)
            {
                transform.position = transform.position + _targetDirection * moveSpeed * Time.deltaTime;
                yield return null;
            }
        }
        
        private IEnumerator StartFiring()
        {
            if (_weapon == null)
                yield break;
            
            _weapon.ResetWeapon();
            while (gameObject.activeSelf)
            {
                _weapon.Fire(transform.forward); 
                yield return null;
            }
        }

        public override void SetMoveDirection(Vector3 direction)
        {
            _targetDirection = direction;
        }

        private IEnumerator LookAtPlayer()
        {
            while (true)
            {
                if (_player == null)
                    yield break;

                transform.LookAt(_player.transform.position);   
                yield return null;
            }
        }

        private void StartRotating()
        {
            model.DOLocalRotate(Quaternion.Euler(0, 0, 180).eulerAngles, 0.7f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public override void OnDisable()
        {
            ResetParameters();
            base.OnDisable();
        }

        private void ResetParameters()
        {
            model.DOKill(true);
            model.localRotation = Quaternion.identity;
            transform.rotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            IsDead = false;
            CurrentHitPoints = hitPoints;
        }
    }
}