using JabberwockyWorld.AnimationSystem.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vices.Scripts.Core;

public class DoorOneScene : MonoBehaviour
{
    [SerializeField] private Controll _control;
    [SerializeField] private SpriteAnimator _animator;
    [SerializeField] private Transform _teleportPosition;

    private PlayerMovement _movement;
    private Collider _collider;
    private bool _isUpdating = false;
    private bool _inCollider = false;
    private int _cadrs = 1;
    private int _cntCadrs = 8;
    private int _fps = 12;
    private float _clock = 0.0f;

    void Start()
    {
        _animator.Set(_control.Name, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isUpdating)
        {
            _clock += Time.deltaTime;
            float secondsPerFrame = 1 / (float)_fps;
            while (_clock >= secondsPerFrame)
            {
                _clock -= secondsPerFrame;
                _cadrs++;
            }
        }
        if (_cadrs == _cntCadrs)
        {
            _animator.Set(_control.Name, 2);
            _isUpdating = false;
            _cadrs = 1;
            if (!_collider.CompareTag("Player")) return;
            StartCoroutine(TeleportDelay(_collider.GetComponent<CharacterController>()));
        }
    }

    private void OnDoorOpen()
    {
        if (_inCollider)
        {
            _animator.Set(_control.Name, 1);
            _movement.enabled = false;
            _isUpdating = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _collider = other;
        other.gameObject.TryGetComponent<PlayerMovement>(out _movement);
        _inCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _inCollider = false;
        _movement.enabled = true;
    }

    private IEnumerator TeleportDelay(CharacterController characterController)
    {
        characterController.enabled = false;

        characterController.transform.position = _teleportPosition.position;

        yield return new WaitForSeconds(0.65f);

        characterController.enabled = true;
    }
}
