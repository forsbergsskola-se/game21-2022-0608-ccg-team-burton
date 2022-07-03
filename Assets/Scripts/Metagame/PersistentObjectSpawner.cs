using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject persistentObjectPrefab;
    static bool hasSpawned = false;
    private void Awake()
    {
        if (hasSpawned) return;
        SpawnPersistObject();

        hasSpawned = true;
    }

    private void SpawnPersistObject()
    {
        GameObject persistObj = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistObj);
    }
}
