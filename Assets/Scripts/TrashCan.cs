using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [Header("Trash Can Settings")]
    public string acceptedTag;   // e.g. "Plastic", "Metal", "Paper"
    public int maxCapacity = 5;
    private int currentCapacity = 0;

    public bool TryDeposit(GameObject item)
    {
        // Check capacity
        if (currentCapacity >= maxCapacity)
        {
            Debug.Log("Trash can is full!");
            return false;
        }

        // Check tag
        if (item.CompareTag(acceptedTag))
        {
            Destroy(item); // remove object from scene
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
}
