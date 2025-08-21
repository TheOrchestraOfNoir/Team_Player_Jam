using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [Header("Trash Can Settings")]
    public string acceptedTag;   // "Plastic", "Metal", "Paper"
    public int maxCapacity = 5;
    public int currentCapacity = 0;

    [Header("Spawn Settings")]
    public Transform spawnPoint; // set in inspector (empty object at original location)

    public bool IsFull => currentCapacity >= maxCapacity;

    public bool TryDeposit(GameObject item)
    {
        if (currentCapacity >= maxCapacity)
        {
            Debug.Log($"{acceptedTag} trash can is full!");
            return false;
        }

        if (item.CompareTag(acceptedTag))
        {
            Destroy(item); // remove from scene
            currentCapacity++;
            Debug.Log($"{acceptedTag} trash can now has {currentCapacity}/{maxCapacity}");
            return true;
        }
        else
        {
            Debug.Log("Wrong trash can for this item!");
            return false;
        }
    }

    public void EmptyBin()
    {
        currentCapacity = 0;

        // Reset physics so the can acts normal again
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Collider2D col = GetComponent<Collider2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        if (col != null)
        {
            col.enabled = true;
        }

        // Make sure it's no longer parented to the player
        transform.SetParent(null);

        // Teleport back to spawn
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }

        Debug.Log($"{acceptedTag} trash can emptied and reset to spawn.");
    }

}