using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _runSpeed = 1.0f;

    private CharacterController _characterController;

    public Vector2 _movement { get; private set; }

    public bool _isRunning { get; private set; }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _isRunning = false;
    }

    private void Update()
    {
        var resultSpeed = _isRunning ? _runSpeed : _speed;
        _characterController.Move(_movement * resultSpeed * Time.deltaTime);
    }

    private void OnMovement(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }

    private void OnShift(InputValue value)
    {
        _isRunning = !_isRunning;
    }

    public bool IsRunning() { return _isRunning; }
}