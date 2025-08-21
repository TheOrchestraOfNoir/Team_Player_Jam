using UnityEngine;
using System;

public class TrashCan : MonoBehaviour
{
    public static event Action<TrashCan> OnAnyEmptied;

    [Header("Trash Can Settings")]
    public string acceptedTag;
    public int maxCapacity = 5;
    public int currentCapacity = 0;

    [Header("Spawn Settings")]
    public Transform spawnPoint;

    public bool IsFull => currentCapacity >= maxCapacity;

    public bool TryDeposit(GameObject item)
    {
        if (currentCapacity >= maxCapacity) { Debug.Log($"{acceptedTag} trash can is full!"); return false; }

        if (item.CompareTag(acceptedTag))
        {
            Destroy(item);
            currentCapacity++;
            Debug.Log($"{acceptedTag} trash can now has {currentCapacity}/{maxCapacity}");
            return true;
        }
        Debug.Log("Wrong trash can for this item!");
        return false;
    }

    public void EmptyBin()
    {
        currentCapacity = 0;

        var rb = GetComponent<Rigidbody2D>();
        var col = GetComponent<Collider2D>();
        if (rb) { rb.bodyType = RigidbodyType2D.Dynamic; rb.velocity = Vector2.zero; rb.angularVelocity = 0f; }
        if (col) col.enabled = true;

        transform.SetParent(null);
        if (spawnPoint) transform.position = spawnPoint.position;

        Debug.Log($"{acceptedTag} trash can emptied and reset to spawn.");

        OnAnyEmptied?.Invoke(this);
    }
}
