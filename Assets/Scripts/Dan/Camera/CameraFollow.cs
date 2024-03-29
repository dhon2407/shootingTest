﻿using System;
using DG.Tweening;
using UnityEngine;

namespace Dan.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        private Func<Vector3> _getCameraFollowPositionFunc;
        private UnityEngine.Camera _camera;

        public void Shake()
        {
            _camera.DOShakePosition(0.3f, 0.05f, 15);
        }

        public void Setup(Func<Vector3> getCameraFollowPositionFunc)
        {
            _getCameraFollowPositionFunc = getCameraFollowPositionFunc;
        }

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
        }

        private void Update() => HandleMovement();

        private void HandleMovement()
        {
            if (_getCameraFollowPositionFunc == null)
                return;
            
            var cameraFollowPosition = _getCameraFollowPositionFunc();
            cameraFollowPosition.z = transform.position.z;

            var cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
            var distance = Vector3.Distance(cameraFollowPosition, transform.position);
            var cameraMoveSpeed = 3f;

            if (distance > 0.1f)
            {
                var newCameraPosition =
                    transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;

                var distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

                if (distanceAfterMoving > distance)
                    newCameraPosition = cameraFollowPosition;

                transform.position = newCameraPosition;
            }
        }
    }
}