using UnityEngine;

public class PickableMeta : MonoBehaviour
{
    public enum Owner { P1, P2 }
    public enum Shape { Circle, Triangle, Square }

    public Owner owner;
    public Shape shape;

    
    public System.Action<PickableMeta> OnReturnToPool;

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        OnReturnToPool?.Invoke(this);
    }
}
