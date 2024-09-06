using JabberwockyWorld.AnimationSystem.Scripts;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using Vices.Scripts.Core;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoorBetweenScenes : MonoBehaviour
{
    [SerializeField] private Controll _control;
    [SerializeField] private SpriteAnimator _animator;
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _afterTeleport;
    [SerializeField] private string _sceneNameAfterTeleport;

    private PlayerMovement _movement;
    private bool _isUpdating = false;
    private bool _inCollider = false;
    private int _cadrs = 1;
    private int _cntCadrs = 8;
    private int _fps = 12;
    private float _clock = 0.0f;

    void Start()
    {
        //if (GameContext.Context.isTeleported)
        //{           
        //    _player.transform.position = GameContext.Context.PreviousTeleportPosition;
        //    GameContext.Context.isTeleported = false;
        //}
        _animator.Set(_control.Name, 0);
    }

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
            //GameContext.Context.isTeleported = true;
            GameContext.Context.PreviousTeleportPosition = _afterTeleport;
            SceneSystem.Singleton.LoadScene(_sceneNameAfterTeleport);
            _cadrs = 1;
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
        other.gameObject.TryGetComponent<PlayerMovement>(out _movement);
        _inCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _inCollider = false;
        _movement.enabled = true;
    }
}
