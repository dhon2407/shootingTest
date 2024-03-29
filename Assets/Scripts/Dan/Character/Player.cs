﻿using System;
using Dan.Character.Collision;
using Dan.Character.Controllers;
using Dan.Manager;
using Dan.Weapon;
using UnityEngine;

namespace Dan.Character
{
    [AddComponentMenu("Dan/SpherePlayer")]
    public class Player : MonoBehaviour, ICharacter
    {
        [SerializeField]
        private float initialMovementSpeed;
        [SerializeField]
        private int hitPoints;
        [SerializeField]
        private Transform cameraFocus;
        [SerializeField]
        private GameObject explosionObj;

        [SerializeField]
        private OrbitingSphere[] orbitingSpheres;

        private ICharacterController _characterController;
        private IWeapon _weapon;
        private HitBox _hitBox;
        
        public event Action OnCharacterDeath;
        public event Action OnCharacterHit;

        public Transform CameraFocus => cameraFocus;
        public int MaxHitPoints => hitPoints;
        public int HitPoints { get; protected set; }
        public float MoveSpeed => initialMovementSpeed;
        public bool IsDead { get; private set; }

        public void ResetGame()
        {
            InitializeParameters();
        }
        
        public void Hit()
        {
            HitPoints = Mathf.Max(0, HitPoints - 1);
            OnCharacterHit?.Invoke();
            if (HitPoints <= 0)
                Die();
        }

        public void Die()
        {
            if (IsDead)
                return;
            
            IsDead = true;
            OnCharacterDeath?.Invoke();
            Destroy(Instantiate(explosionObj, transform.position, Quaternion.identity), 2f);
            transform.localScale = Vector3.zero;
            _characterController.Activate(false);
        }
        
        private void Awake()
        {
            InitializeComponents();
            InitializeParameters();

            GameFlowManager.OnGameStart += GameStart;
            GameFlowManager.OnLevelChange += LevelIncrease;
        }

        private void InitializeParameters()
        {
            HitPoints = MaxHitPoints;
            transform.position = Vector3.zero;
            transform.localScale = Vector3.one;
            IsDead = false;
            _characterController.Activate(false);
        }

        private void InitializeComponents()
        {
            _characterController = gameObject.AddComponent<PlayerController>();
            _characterController.SetMovespeed(MoveSpeed);
            _characterController.OnFireWeapon += FireWeapon;
            
            _weapon = GetComponentInChildren<IWeapon>();
            _hitBox = GetComponentInChildren<HitBox>();
            if (_hitBox != null)
                _hitBox.OnHit += Hit;
        }

        private void FireWeapon()
        {
            _weapon?.Fire(transform.up);
            foreach (var orbitingSphere in orbitingSpheres)
                orbitingSphere.FireWeapons(transform.up);
        }

        private void GameStart()
        {
            _characterController.Activate(true);
        }
        
        private void LevelIncrease(int currentLevel)
        {
            foreach (var orbitingSphere in orbitingSpheres)
                orbitingSphere.SetCurrentLevel(currentLevel);
        }
    }
}