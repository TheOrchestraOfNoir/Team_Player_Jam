using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpObject : MonoBehaviour
{
    [Header("Pickup Settings")]
    public LayerMask layerMask;
    public Transform pickupPoint;
    public float pickupRadius = 0.5f;
    public float throwForce = 3f;
    public SpriteRenderer spriteRender;

    [Header("Player Key Bind")]
    public KeyCode keyBind = KeyCode.P;

    private bool _isHoldingObject = false;
    private GameObject _heldObject;
    private PlayerMovement _playerMovement;
    private Collider2D _objectInRange;

    private void Start()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        _objectInRange = Physics2D.OverlapCircle(pickupPoint.position, pickupRadius, layerMask);
        spriteRender.enabled = !_isHoldingObject && _objectInRange != null;

        if (!_isHoldingObject && _objectInRange != null)
        {
            spriteRender.enabled = true;
            if (Input.GetKeyDown(keyBind))
            {
                PickUpObjectMethod(_objectInRange);
            }
        }
        else if (_isHoldingObject)
        {
            // NEW: Try deposit first
            if (Input.GetKeyDown(keyBind))
            {
                if (!TryDepositHeldObject())
                {
                    ThrowObjectMethod(); // fallback to throwing
                }
            }
        }
    }

    private void PickUpObjectMethod(Collider2D pickUp)
    {
        // If it's a trash can, only pick up if full
        TrashCan can = pickUp.GetComponent<TrashCan>();
        if (can != null && !can.IsFull)
        {
            Debug.Log("Can't pick up trash can unless it's full!");
            return;
        }

        _isHoldingObject = true;
        _heldObject = pickUp.gameObject;

        _heldObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _heldObject.GetComponent<Collider2D>().enabled = false;
        _heldObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        _heldObject.transform.position = pickupPoint.position;
        _heldObject.transform.SetParent(transform);
    }


    private void ThrowObjectMethod()
    {
        _isHoldingObject = false;

        _heldObject.GetComponent<Collider2D>().enabled = true;
        _heldObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        _heldObject.transform.SetParent(null);

        float snappedAngle = Mathf.Round((transform.eulerAngles.z + 90f) / 45f) * 45f;
        float rad = snappedAngle * Mathf.Deg2Rad;

        Vector2 velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        velocity.x = Mathf.Round(velocity.x);
        velocity.y = Mathf.Round(velocity.y);

        velocity *= throwForce;
        velocity += _playerMovement.GetVelocity();

        _heldObject.GetComponent<Rigidbody2D>().velocity = velocity;

        _heldObject = null;
    }

    // NEW: Try to deposit into nearby trash can
    private bool TryDepositHeldObject()
    {
        // If holding trash, try trash cans (old behavior)
        if (_heldObject != null && _heldObject.GetComponent<TrashCan>() == null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (Collider2D hit in hits)
            {
                TrashCan trashCan = hit.GetComponent<TrashCan>();
                if (trashCan != null)
                {
                    if (trashCan.TryDeposit(_heldObject))
                    {
                        _heldObject = null;
                        _isHoldingObject = false;
                        return true;
                    }
                }
            }
        }
        // If holding a trash can itself, try dumpsters
        else if (_heldObject != null && _heldObject.GetComponent<TrashCan>() != null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (Collider2D hit in hits)
            {
                Dumpster dumpster = hit.GetComponent<Dumpster>();
                if (dumpster != null)
                {
                    TrashCan can = _heldObject.GetComponent<TrashCan>();
                    if (dumpster.TryDump(can))
                    {
                        // Drop the can, teleport handled in EmptyBin()
                        _heldObject.transform.SetParent(null);
                        _heldObject = null;
                        _isHoldingObject = false;
                        return true;
                    }
                }
            }
        }
        return false;
    }


    private void OnDrawGizmosSelected()
    {
        if (pickupPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pickupPoint.position, pickupRadius);
    }


}
