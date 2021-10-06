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

        public void ResetGame()
        {
            InitializeParameters();
        }
        
        private void Awake()
        {
            InitializeComponents();
            InitializeParameters();

            GameFlowManager.OnGameStart += GameStart;
        }

        private void InitializeParameters()
        {
            _currentHitPoints = hitPoints;
            transform.position = Vector3.zero;
            IsDead = false;
            _characterController.Activate(false);
        }

        private void InitializeComponents()
        {
            _characterController = gameObject.AddComponent<PlayerController>();
            _characterController.SetMovespeed(initialMovementSpeed);
            _characterController.OnFireWeapon += FireWeapon;
            
            _weapon = GetComponentInChildren<IWeapon>();
            _hitBox = GetComponentInChildren<HitBox>();
            if (_hitBox != null)
                _hitBox.OnHit += Hit;
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