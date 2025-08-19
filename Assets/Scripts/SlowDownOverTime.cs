using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gradually slows down a 2D object over time.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class SlowDownOverTime : MonoBehaviour {
    public float deceleration = 1f; // How fast the object slows down (units per second)
    private Rigidbody2D _rb;

    private void Start() { _rb = GetComponent<Rigidbody2D>(); }

    private void FixedUpdate() {
        // Only slow down if the object is moving
        if (_rb.velocity != Vector2.zero) {
            // Calculate the new velocity
            Vector2 newVelocity = _rb.velocity;

            // Reduce velocity gradually
            newVelocity -= newVelocity.normalized * (deceleration * Time.fixedDeltaTime);

            // Make sure it doesn't reverse direction
            if (Vector2.Dot(newVelocity, _rb.velocity) < 0) {
                newVelocity = Vector2.zero;
            }

            _rb.velocity = newVelocity;
        }
    }
}