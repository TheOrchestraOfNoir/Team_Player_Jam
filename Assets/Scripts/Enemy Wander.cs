using System.Collections;
using UnityEngine;

/// <summary>
/// Makes an enemy wander around randomly in 2D space.
/// Attach to an enemy with a Rigidbody2D.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyWander : MonoBehaviour
{
    private Rigidbody2D _rb;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;          // Speed of movement
    public float changeDirectionTime = 2f; // How often to pick a new direction

    private Vector2 _moveDirection;
    private float _timer;

    [Header("Optional Sprite Flip")]
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        PickNewDirection();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= changeDirectionTime)
        {
            PickNewDirection();
            _timer = 0f;
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveDirection * moveSpeed;
    }

    private void PickNewDirection()
    {
        // Pick a random direction inside a circle
        _moveDirection = Random.insideUnitCircle.normalized;

        // Optional: flip sprite left/right
        if (spriteRenderer != null)
        {
            if (_moveDirection.x > 0.1f) spriteRenderer.flipX = false;
            else if (_moveDirection.x < -0.1f) spriteRenderer.flipX = true;
        }
    }
}
