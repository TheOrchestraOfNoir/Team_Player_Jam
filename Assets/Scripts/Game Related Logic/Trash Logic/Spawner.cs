using System.Collections.Generic;
using UnityEngine;

public class Spawner2D : MonoBehaviour
{
    public enum Direction { Left, Right }
    public enum SpawnMode { All, RandomOne, Cycle }

    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject prefab;           // must have Rigidbody2D (optional)
        public Transform spawnPoint;        // optional; falls back to Spawner2D.transform
        public Direction direction = Direction.Right;
        public float throwForce = 10f;

        [Header("Optional Auto-Despawn")]
        public bool enableLifetime = false;
        public float lifetimeSeconds = 10f;

        [Header("Random Weight (RandomOne mode)")]
        public float weight = 1f;           // ignored in All/Cycle
    }

    [Header("Spawn Settings")]
    public List<SpawnEntry> entries = new List<SpawnEntry>();
    public float spawnInterval = 1.25f;
    public bool autoStart = true;
    public SpawnMode mode = SpawnMode.All;

    [Header("Limits")]
    public int maxActive = 12;          // counts all spawned objects from all entries

    private readonly HashSet<GameObject> _active = new HashSet<GameObject>();
    private int _cycleIndex = 0;

    void Start()
    {
        if (autoStart && entries.Count > 0)
            InvokeRepeating(nameof(SpawnOnce), 0f, spawnInterval);
    }

    public void SpawnOnce()
    {
        CleanupNulls();
        if (_active.Count >= maxActive || entries.Count == 0) return;

        switch (mode)
        {
            case SpawnMode.All:
                // spawn each entry once (respecting maxActive)
                for (int i = 0; i < entries.Count && _active.Count < maxActive; i++)
                    SpawnFromEntry(entries[i]);
                break;

            case SpawnMode.RandomOne:
                // spawn a single entry chosen by weights
                var e = PickWeighted(entries);
                if (e != null) SpawnFromEntry(e);
                break;

            case SpawnMode.Cycle:
                // spawn entries[i] then next call uses the next one
                var entry = entries[_cycleIndex % entries.Count];
                SpawnFromEntry(entry);
                _cycleIndex = (_cycleIndex + 1) % entries.Count;
                break;
        }
    }

    private void SpawnFromEntry(SpawnEntry e)
    {
        if (e.prefab == null) return;
        if (_active.Count >= maxActive) return;

        Vector3 pos = e.spawnPoint ? e.spawnPoint.position :
                       (transform.position);

        GameObject go = Instantiate(e.prefab, pos, Quaternion.identity);

        // Track object so we respect maxActive
        var hook = go.AddComponent<SpawnerLifetimeHook2D>();
        hook.owner = this;
        _active.Add(go);

        // Reset physics state + throw
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            Vector2 dir = (e.direction == Direction.Right)
                ? (Vector2)transform.right
                : -(Vector2)transform.right;

            rb.AddForce(dir.normalized * e.throwForce, ForceMode2D.Impulse);
        }

        // Optional timed despawn
        if (e.enableLifetime && e.lifetimeSeconds > 0f)
            hook.BeginLifetime(e.lifetimeSeconds);
    }

    private static SpawnEntry PickWeighted(List<SpawnEntry> list)
    {
        float total = 0f;
        for (int i = 0; i < list.Count; i++)
        {
            float w = Mathf.Max(0f, list[i].weight);
            total += w;
        }
        if (total <= 0f) return list[Random.Range(0, list.Count)];

        float r = Random.value * total;
        float acc = 0f;
        for (int i = 0; i < list.Count; i++)
        {
            acc += Mathf.Max(0f, list[i].weight);
            if (r <= acc) return list[i];
        }
        return list[list.Count - 1];
    }

    internal void NotifyDespawn(GameObject go)
    {
        _active.Remove(go);
    }

    private void CleanupNulls()
    {
        _active.RemoveWhere(g => g == null);
    }
}

/// <summary>
/// Helper for lifetime + notifying spawner when despawned/destroyed.
/// </summary>
public class SpawnerLifetimeHook2D : MonoBehaviour
{
    [HideInInspector] public Spawner2D owner;
    private bool notified;
    private Coroutine routine;

    public void BeginLifetime(float seconds)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Lifetime(seconds));
    }

    private System.Collections.IEnumerator Lifetime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject); // replace with SetActive(false) if pooling
    }

    void OnDisable()
    {
        if (!notified && owner != null)
        {
            notified = true;
            owner.NotifyDespawn(gameObject);
        }
    }

    void OnDestroy()
    {
        if (!notified && owner != null)
        {
            notified = true;
            owner.NotifyDespawn(gameObject);
        }
    }
}
