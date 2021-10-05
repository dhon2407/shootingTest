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

        private ICharacterController _characterController;
        private IWeapon _weapon;

        private void Awake()
        {
            InitializePlayerController();
            InitializeWeapon();

            GameFlowManager.OnGameStart += GameStart;
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
    }
}