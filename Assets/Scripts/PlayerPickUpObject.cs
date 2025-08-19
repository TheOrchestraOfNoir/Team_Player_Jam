using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows a player to pick up and throw objects in 2D.
/// It supports two players using different keys.
/// </summary>
public class PlayerPickUpObject : MonoBehaviour {
    [Header("Pickup Settings")]
    public LayerMask layerMask;           // Which objects can be picked up
    public Transform pickupPoint;         // Where the object will be held
    public float pickupRadius = 0.5f;    // How far the player can reach to pick up
    public float throwForce = 3f;        // How strong the throw is
    public SpriteRenderer spriteRender;  // Shows if the player can pick somehting up
    
    [Header("Player Key Bind")]
    public KeyCode playerOneKey = KeyCode.P;
    public KeyCode playerTwoKey = KeyCode.Q;
    
    private bool _isPlayerOne = true;      // Set to true if this is player 1

    private bool _isHoldingObject = false;
    private GameObject _heldObject;       // The object currently being held
    private PlayerMovement _playerMovement;
    private Collider2D _objectInRange;    // Object in pickup range
    // Called when the game starts
    private void Start() {
        _playerMovement = GetComponentInParent<PlayerMovement>();  
        _isPlayerOne = _playerMovement.isPlayerOne;
    }

    // Called every frame
    private void Update() {
        // Check if there is an object in front of the player
        _objectInRange = Physics2D.OverlapCircle(pickupPoint.position, pickupRadius, layerMask);

        // Makes the idea sprite turn on/off based on if you can pick up an item
        spriteRender.enabled = !_isHoldingObject && _objectInRange != null;
        
        if (!_isHoldingObject && _objectInRange != null) {
            spriteRender.enabled = true;
            // Pick up object when the correct key is pressed
            if ((_isPlayerOne && Input.GetKeyDown(playerOneKey)) || (!_isPlayerOne && Input.GetKeyDown(playerTwoKey))) {
                PickUpObjectMethod(_objectInRange);
            }
        }
        else if (_isHoldingObject) {
            // Throw object when the correct key is pressed
            if ((_isPlayerOne && Input.GetKeyDown(playerOneKey)) || (!_isPlayerOne && Input.GetKeyDown(playerTwoKey))) {
                ThrowObjectMethod();
            }
        }
    }

    /// <summary>
    /// Picks up an object
    /// </summary>
    private void PickUpObjectMethod(Collider2D pickUp) {
        _isHoldingObject = true;
        _heldObject = pickUp.gameObject;
        

        // Disable physics so the object can be carried
        _heldObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _heldObject.GetComponent<Collider2D>().enabled = false;
        _heldObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        // Move object to player's hand
        _heldObject.transform.position = pickupPoint.position;
        _heldObject.transform.SetParent(transform);
    }

    /// <summary>
    /// Throws the currently held object
    /// </summary>
    private void ThrowObjectMethod() {
        _isHoldingObject = false;

        // Re-enable physics
        _heldObject.GetComponent<Collider2D>().enabled = true;
        _heldObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // Remove parent
        _heldObject.transform.SetParent(null);

        // Calculate throw direction based on player's rotation snapped to 45-degree increments
        float snappedAngle = Mathf.Round((transform.eulerAngles.z + 90f) / 45f) * 45f;
        float rad = snappedAngle * Mathf.Deg2Rad;

        Vector2 velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        // Convert velocity to -1, 0, or 1 for direction only
        velocity.x = Mathf.Round(velocity.x);
        velocity.y = Mathf.Round(velocity.y);

        // Apply throw force and add player's movement
        velocity *= throwForce;
        velocity += _playerMovement.GetVelocity();

        // Set the object's velocity
        _heldObject.GetComponent<Rigidbody2D>().velocity = velocity;

        // Clear held object reference
        _heldObject = null;
    }

    // Draw a circle in the editor to show pickup range
    private void OnDrawGizmosSelected() {
        if (pickupPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pickupPoint.position, pickupRadius);
    }
}