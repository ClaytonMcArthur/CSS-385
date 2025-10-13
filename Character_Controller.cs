using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
public class Character_Controller : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _moveSpeed = 3f;      // units / second
    [SerializeField] float _inputDeadzone = 0.12f;

    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;

    Vector2 _moveDir;

    void Awake()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // Old Input Manager (Project Settings > Player > Active Input Handling = Both or Input Manager)
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // kill micro input to avoid creeping
        if (_moveDir.sqrMagnitude < _inputDeadzone * _inputDeadzone)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        // set exact velocity (no deltaTime)
        _rb.linearVelocity = _moveDir.normalized * _moveSpeed;
    }
}