using System;
using Dan.Character.Input;
using UnityEngine;

namespace Dan.Character.Controllers
{
    public class PlayerController : MonoBehaviour, ICharacterController
    {
        [SerializeField]
        private float movementSpeed;

        private IInputHandler _inputHandler;
        private Vector3 _targetPosition;
        private bool _active;

        public event Action OnFireWeapon;

        public void Activate(bool active)
        {
            _active = active;
        }
        
        public void SetMovespeed(float speed)
        {
            movementSpeed = speed;
        }
        
        private void Awake()
        {
            _inputHandler = GetComponent<IInputHandler>();
            _inputHandler.Fire += FireWeapon;
        }

        private void OnDestroy()
        {
            _inputHandler.Fire -= FireWeapon;
        }

        private void FireWeapon() => OnFireWeapon?.Invoke();

        private void Update()
        {
            if (!_active)
                return;
            
            _targetPosition = transform.position + _inputHandler.MoveVector;
            transform.position = Vector3.Lerp(transform.position, _targetPosition,
                movementSpeed * Time.deltaTime);
        }
    }
}