using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles 2D player movement, rotation, and animations.
/// Supports two players with separate input axes.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;

    [Header("Player Settings")]
    public bool isPlayerOne = true;   // True if this is player 1, false for player 2
    public float xSpeed = 3f;         // Horizontal movement speed
    public float ySpeed = 3f;         // Vertical movement speed

    [Header("Sprite & Animation")]
    public SpriteRenderer spriteRenderer; // assign in Inspector
    public Animator animator;             // assign in Inspector

    // Called when the game starts
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Called once per frame
    private void FixedUpdate()
    {
        // Get player input
        float xInput = 0f;
        float yInput = 0f;

        if (isPlayerOne)
        {
            xInput = Input.GetAxis("Horizontal_P1");
            yInput = Input.GetAxis("Vertical_P1");
        }
        else
        {
            xInput = Input.GetAxis("Horizontal_P2");
            yInput = Input.GetAxis("Vertical_P2");
        }

        // Apply movement to the Rigidbody
        Vector2 velocity = new Vector2(xInput * xSpeed, yInput * ySpeed);
        _rb.velocity = velocity;

        // Rotate the player to face the direction of movement
        Vector2 movement = new Vector2(xInput, yInput);
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            angle -= 90f;
            float snappedAngle = Mathf.Round(angle / 45f) * 45f;
            transform.rotation = Quaternion.Euler(0, 0, snappedAngle);
        }

        // 🔹 Force sprite to stay upright (ignore parent rotation)
        if (spriteRenderer != null)
        {
            spriteRenderer.transform.rotation = Quaternion.identity;
        }

        // 🔹 Update animator
        if (animator != null)
        {
            bool isWalking = velocity.magnitude > 0.1f; // small threshold
            animator.SetBool("isWalking", isWalking);
        }
    }

    /// <summary>
    /// Returns the player's current velocity
    /// </summary>
    public Vector2 GetVelocity()
    {
        return _rb.velocity;
    }
}
