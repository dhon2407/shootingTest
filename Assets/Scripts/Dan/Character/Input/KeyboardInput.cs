using System;
using UnityEngine;

namespace Dan.Character.Input
{
    public class KeyboardInput : MonoBehaviour, IInputHandler
    {
        public event Action Fire;
        public event Action Pause;
        public Vector3 MoveVector => _currentMovement;

        private Vector3 _currentMovement;

        private void Update()
        {
            _currentMovement.x = UnityEngine.Input.GetAxis("Horizontal");
            _currentMovement.y = UnityEngine.Input.GetAxis("Vertical");
            
            if (UnityEngine.Input.GetKey(KeyCode.Z))
                Fire?.Invoke();
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                Pause?.Invoke();
        }
    }
}