using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Awareness_Controller : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }

    [SerializeField] float _playerAwarenessDistance = 10f;

    // Assign in Inspector or found automatically in Awake
    [SerializeField] private Transform _player;
    public Transform Player => _player;   // read-only access for other scripts

    void Awake()
    {
        if (_player == null)
        {
            var tagged = GameObject.FindWithTag("Player");
            if (tagged) _player = tagged.transform;

            if (_player == null)
            {
#if UNITY_2022_1_OR_NEWER
                var cc = FindAnyObjectByType<Character_Controller>();
#else
                var cc = FindObjectOfType<Character_Controller>();
#endif
                if (cc) _player = cc.transform;
            }
        }
    }

    void Update()
    {
        if (_player == null)
        {
            AwareOfPlayer = false;
            DirectionToPlayer = Vector2.zero;
            return;
        }

        Vector2 toPlayer = (Vector2)(_player.position - transform.position);
        DirectionToPlayer = toPlayer.sqrMagnitude > 0f ? toPlayer.normalized : Vector2.zero;
        AwareOfPlayer = toPlayer.magnitude <= _playerAwarenessDistance;
    }

    // visualize the detection radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = AwareOfPlayer ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _playerAwarenessDistance);
    }
}