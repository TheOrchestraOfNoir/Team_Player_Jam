using System.Collections;
using UnityEngine;

/// <summary>
/// Stuns the player for a few seconds when touching an enemy.
/// Disables movement & flashes sprite.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHit : MonoBehaviour
{
    [Header("Settings")]
    public float stunDuration = 3f;          // How long player is stunned
    public float flashInterval = 0.2f;       // Flash speed (seconds)

    [Header("References")]
    public PlayerMovement playerMovement;    // Assign in inspector
    public SpriteRenderer spriteRenderer;    // Assign in inspector

    private bool isStunned = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isStunned)
        {
            StartCoroutine(StunPlayer());
        }
    }

    private IEnumerator StunPlayer()
    {
        isStunned = true;

        // Disable movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Flash effect
        float timer = 0f;
        while (timer < stunDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle visibility
            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        // Ensure sprite is visible again
        spriteRenderer.enabled = true;

        // Re-enable movement
        if (playerMovement != null)
            playerMovement.enabled = true;

        isStunned = false;
    }
}
