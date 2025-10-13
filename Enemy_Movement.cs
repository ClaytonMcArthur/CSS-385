using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player_Awareness_Controller))]
public class Enemy_Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _maxSpeed = 1.25f;     // top speed
    [SerializeField] float _accel = 4f;           // how fast it reaches speed
    [SerializeField] float _decel = 6f;           // how fast it slows
    [SerializeField] float _rotationSpeed = 360f; // deg/sec to face player
    [SerializeField] float _stoppingDistance = 0.4f;

    Rigidbody2D _rb;
    Player_Awareness_Controller _awareness;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = 0f;
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        _awareness = GetComponent<Player_Awareness_Controller>();
    }

    void FixedUpdate()
    {
        Transform playerT = _awareness.Player;
        if (playerT == null) return;

        Vector2 toPlayer = (Vector2)(playerT.position - transform.position);
        if (toPlayer.magnitude <= _stoppingDistance)
        {
            // stop near player
            return;
        }

        Vector2 dir = toPlayer.normalized;

        // face the player
        float targetAngle = Vector2.SignedAngle(Vector2.up, dir);
        float newAngle = Mathf.MoveTowardsAngle(_rb.rotation, targetAngle, _rotationSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(newAngle);

        // go straight toward player (no collisions)
        Vector2 step = dir * _maxSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + step);
    }
}