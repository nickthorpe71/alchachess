using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{
    // CONFIG DATA
    [Tooltip("This prefab will only be spawned once and persisted between " +
    "scenes.")]
    private GameObject persistentObjectPrefab = null;

    // PRIVATE STATE
    static bool hasSpawned = false;

    // PRIVATE
    private void Awake()
    {
        if (hasSpawned) return;

        SpawnPersistentObjects();

        hasSpawned = true;
    }

    private void SpawnPersistentObjects()
    {
        GameObject persistentObject = Instantiate(Resources.Load("Utility/PersistentObjects") as GameObject);
        DontDestroyOnLoad(persistentObject);
    }
}