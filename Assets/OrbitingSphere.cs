using System;
using System.Collections;
using Dan.Character.Collision;
using Dan.Manager;
using Dan.Weapon;
using DG.Tweening;
using UnityEngine;

public class OrbitingSphere : MonoBehaviour
{
    [SerializeField]
    private int activationLevel = 2;
    [SerializeField]
    private int orbitingLevel = 3;
    [SerializeField]
    private Transform master;
    [SerializeField]
    private float radius = 1f;
    [SerializeField]
    private float radiusSpeed = 0.5f;
    [SerializeField]
    private float rotationSpeed = 300f;
    [SerializeField]
    public Vector3 axis = Vector3.up;
    [SerializeField]
    private HitBox hitbox;

    private Transform _transform;
    private Vector3 _desiredPosition;
    private Vector3 _originalPosition;
    private bool _orbiting;
    private bool _active;
    private int _currentLevel;
    private IWeapon[] _weapons;
    private Vector3 MasterPosition => master.position;
    private Vector3 CurrentPosition => _transform.position;
    
    public void SetCurrentLevel(int currentLevel)
    {
        _currentLevel = currentLevel;
        Activate(currentLevel >= activationLevel);
        ActivateMovement(currentLevel >= orbitingLevel);
    }

    public void FireWeapons(Vector3 direction)
    {
        foreach (var weapon in _weapons)
            if (_currentLevel >= weapon.ActivationLevel)
                weapon.Fire(direction);
    }

    private void ActivateMovement(bool active)
    {
        if (!active || _orbiting)
            return;
        
        StartOrbit();
    }

    private void Activate(bool isActive)
    {
        if (!isActive || _active)
            return;
        
        _active = true;
        _transform.DOScale(Vector3.one, 0.4f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                hitbox.gameObject.SetActive(_active);
            });
    }

    private void StartOrbit()
    {
        _orbiting = true;
        StartCoroutine(StartOrbiting());
    }

    private void Awake()
    {
        _transform = transform;
        _originalPosition = _transform.localPosition;
        _weapons = GetComponentsInChildren<IWeapon>();
        hitbox.gameObject.SetActive(false);
        _transform.localScale = Vector3.zero;

        GameFlowManager.OnGameStart += ResetSphere;
        GameFlowManager.OnGameEnd += GameEnd;
    }

    private void OnDestroy()
    {
        GameFlowManager.OnGameStart -= ResetSphere;
        GameFlowManager.OnGameEnd -= GameEnd;
    }

    private void GameEnd()
    {
        _transform.localScale = Vector3.zero;
    }

    private void ResetSphere()
    {
        _transform.localPosition = _originalPosition;
        ResetPosition();
        _orbiting = false;
        _active = false;
        _currentLevel = 1;
        hitbox.gameObject.SetActive(false);
        _transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        ResetPosition();
    }

    private void ResetPosition()
    {
        _transform.position = (CurrentPosition - master.position).normalized * radius + MasterPosition;
    }

    private IEnumerator StartOrbiting()
    {
        while (_orbiting)
        {
            transform.RotateAround(master.position, axis, rotationSpeed * Time.deltaTime);
            _desiredPosition = (CurrentPosition - MasterPosition).normalized * radius + MasterPosition;
            transform.position = Vector3.MoveTowards(CurrentPosition, _desiredPosition, Time.deltaTime * radiusSpeed);
            yield return null;
        }
    }

    private void StopOrbiting()
    {
        _transform.DOKill();
        _transform.DOMove((CurrentPosition - master.position).normalized * radius + MasterPosition, 0.3f);
    }
}
