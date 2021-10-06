using System;
using Dan.Character.Collision;
using Dan.Character.Controllers;
using Dan.Manager;
using Dan.Weapon;
using UnityEngine;

namespace Dan.Character
{
    [AddComponentMenu("Dan/SpherePlayer")]
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float initialMovementSpeed;
        [SerializeField]
        private int hitPoints;

        private ICharacterController _characterController;
        private IWeapon _weapon;
        private int _currentHitPoints;
        private HitBox _hitBox;
        
        public event Action<int,int> OnUpdateHP;
        public event Action OnPlayerDie;
        
        public bool IsDead { get; private set; }


        private void Awake()
        {
            InitializePlayerController();
            InitializeWeapon();
            InitializeParameters();

            GameFlowManager.OnGameStart += GameStart;
        }

        private void InitializeParameters()
        {
            _currentHitPoints = hitPoints;
            _hitBox = GetComponentInChildren<HitBox>();
            if (_hitBox != null)
                _hitBox.OnHit += Hit;
        }

        private void InitializeWeapon()
        {
            _weapon = GetComponentInChildren<IWeapon>();
        }

        private void InitializePlayerController()
        {
            _characterController = gameObject.AddComponent<PlayerController>();
            _characterController.SetMovespeed(initialMovementSpeed);
            _characterController.OnFireWeapon += FireWeapon;
        }

        private void FireWeapon() => _weapon?.Fire(transform.up);

        private void GameStart()
        {
            _characterController.Activate(true);
        }
        
        private void Hit()
        {
            _currentHitPoints = Mathf.Max(0, _currentHitPoints - 1);
            OnUpdateHP?.Invoke(_currentHitPoints, hitPoints);
            if (_currentHitPoints <= 0)
                Die();
        }

        private void Die()
        {
            if (IsDead)
                return;
            
            IsDead = true;
            _characterController.Activate(false);
            OnPlayerDie?.Invoke();
        }
    }
}