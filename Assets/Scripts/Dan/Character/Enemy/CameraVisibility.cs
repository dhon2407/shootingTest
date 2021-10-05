using System;
using UnityEngine;

namespace Dan.Character.Enemy
{
    public class CameraVisibility : MonoBehaviour
    {
        private bool _quittingApplication;
        public event Action<bool> OnVisibilityChange;

        private void OnBecameVisible()
        {
            if (_quittingApplication)
                return;
            
            OnVisibilityChange?.Invoke(true);
        }

        private void OnBecameInvisible()
        {
            if (_quittingApplication)
                return;
            
            OnVisibilityChange?.Invoke(false);
        }

        private void OnApplicationQuit()
        {
            _quittingApplication = true;
        }
    }
    
}