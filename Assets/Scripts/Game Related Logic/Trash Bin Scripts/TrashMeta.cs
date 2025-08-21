using UnityEngine;

public class TrashMeta : MonoBehaviour
{
    public enum TrashType { Circle, Triangle, Square } // map to your 3 bins
    public TrashType type;

    // Optional: to reset after sorting / despawn
    public void Despawn()
    {
        gameObject.SetActive(false);
        // If pooling: return to pool here
    }
}
