using UnityEngine;

public class Dumpster : MonoBehaviour
{
    public bool TryDump(TrashCan trashCan)
    {
        if (trashCan != null && trashCan.IsFull)
        {
            trashCan.EmptyBin();
            return true;
        }
        return false;
    }
}
