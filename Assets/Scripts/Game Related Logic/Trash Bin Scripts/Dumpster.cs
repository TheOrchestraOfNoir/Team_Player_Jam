using UnityEngine;

public class Dumpster : MonoBehaviour
{
    private const int DumpPoints = 100;

    public bool TryDump(TrashCan trashCan)
    {
        if (trashCan != null && trashCan.IsFull)
        {
            trashCan.EmptyBin();

            // add score on success
            ScoreManagerTMP.Instance?.Add(DumpPoints);

            return true;
        }
        return false;
    }
}
