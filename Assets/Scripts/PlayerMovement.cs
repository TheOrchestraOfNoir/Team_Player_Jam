using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles 2D player movement and rotation based on input.
/// Supports two players with separate input axes.
/// </summary>
public class PlayerMovement : MonoBehaviour {
    private Rigidbody2D _rb;

    [Header("Player Settings")]
    public bool isPlayerOne = true;   // True if this is player 1, false for player 2
    public float xSpeed = 3f;         // Horizontal movement speed
    public float ySpeed = 3f;         // Vertical movement speed
    
    // Called when the game starts
    private void Start() { _rb = GetComponent<Rigidbody2D>(); }

    // Called once per frame
    private void FixedUpdate() {
        // Get player input
        float xInput = 0f;
        float yInput = 0f;

        if (isPlayerOne) {
            xInput = Input.GetAxis("Horizontal_P1");
            yInput = Input.GetAxis("Vertical_P1");
        }
        else {
            xInput = Input.GetAxis("Horizontal_P2");
            yInput = Input.GetAxis("Vertical_P2");
        }

        // Apply movement to the Rigidbody
        _rb.velocity = new Vector2(xInput * xSpeed, yInput * ySpeed);

        // Rotate the player to face the direction of movement
        Vector2 movement = new Vector2(xInput, yInput);
        if (movement != Vector2.zero) {
            // Calculate the angle in degrees (0° = up, clockwise rotation)
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            // Adjust so that 0° points up
            angle -= 90f;

            // Snap the rotation to the nearest 45 degrees for 8-directional movement
            float snappedAngle = Mathf.Round(angle / 45f) * 45f;

            // Apply rotation to the player
            transform.rotation = Quaternion.Euler(0, 0, snappedAngle);
        }
    }

    /// <summary>
    /// Returns the player's current velocity
    /// </summary>
    public Vector2 GetVelocity() {
        return _rb.velocity;
    }
}