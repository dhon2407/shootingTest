using System;
using System.Collections;
using Dan.Camera;
using Dan.Character.Input;
using UnityEngine;

namespace Dan.Character.Controllers
{
    public class PlayerController : MonoBehaviour, ICharacterController
    {
        [SerializeField]
        private float movementSpeed;
        
        public event Action OnFireWeapon;

        private IInputHandler _inputHandler;
        private Vector3 _targetPosition;
        private MovementLimiter _positionLimiter;
        private bool _active;
        
        public void Activate(bool active)
        {
            _active = active;

            if (active)
                StartCoroutine(StartMovement());
        }
        
        public void SetMovespeed(float speed)
        {
            movementSpeed = speed;
        }
        
        private void Awake()
        {
            _positionLimiter = FindObjectOfType<MovementLimiter>();
            _inputHandler = GetComponent<IInputHandler>();
            _inputHandler.Fire += FireWeapon;
        }

        private void OnDestroy()
        {
            _inputHandler.Fire -= FireWeapon;
        }

        private void FireWeapon()
        {
            if (_active)
                OnFireWeapon?.Invoke();
        }

        private IEnumerator StartMovement()
        {
            while (_active)
            {
                _targetPosition = transform.position + _inputHandler.MoveVector;
                _targetPosition = Vector3.Lerp(transform.position, _targetPosition,
                    movementSpeed * Time.deltaTime);
                
                _targetPosition.y = Mathf.Max(_targetPosition.y, _positionLimiter.LowerVerticalLimit);
                transform.position = _targetPosition;
                yield return null;
            }
        }
    }
}