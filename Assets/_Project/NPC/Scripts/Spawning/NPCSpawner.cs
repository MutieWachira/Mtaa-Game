using UnityEngine;
using UnityEngine.AI;

///<summary
/// Spawns NPC Instances at configure spawn points.
/// 
/// The spwaner owns NPC creatio. It does not control NPC Behaviour.
/// </summary>

public sealed class NPCSpawner : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField]
    private GameObject npcPrefab;

    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private int maximumNPCs = 10;

    [SerializeField]
    private float spawnValidationRadius = 1f;

    private int _spawnedNPCCount;

    private void Start()
    {
        SpawnInitialNPCs();
    }

    private void SpawnInitialNPCs()
    {
        if (npcPrefab == null)
        {
            Debug.LogError(
                "[NPCSpawner] NPC prefab is missing"
            );
            return;
        }
        if(spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[NPCSpawner] No spawn points configured.");
            return;
        }
        foreach(Transform spawnPoint in spawnPoints)
        {
            if (_spawnedNPCCount >= maximumNPCs)
            {
                break;
            }
            SpawnNPC(spawnPoint);
        }
    }

    private void SpawnNPC(Transform spawnPoint)
{
    if (spawnPoint == null)
    {
        Debug.LogWarning(
            "[NPCSpawner] A spawn point is missing."
        );

        return;
    }

    if (!TryGetValidNavMeshPosition(
            spawnPoint.position,
            out Vector3 position))
    {
        Debug.LogWarning(
            $"[NPCSpawner] Invalid spawn point: " +
            $"{spawnPoint.name}"
        );

        return;
    }

    Instantiate(
        npcPrefab,
        position,
        spawnPoint.rotation
    );

    _spawnedNPCCount++;
}
    private bool TryGetValidNavMeshPosition(
        Vector3 sourcePosition,
        out Vector3 validPosition)
    {
        if (NavMesh.SamplePosition(
                sourcePosition,
                out NavMeshHit hit,
                spawnValidationRadius,
                NavMesh.AllAreas))
        {
            validPosition = hit.position;

            return true;
        }

        validPosition = Vector3.zero;

        return false;
    }
}