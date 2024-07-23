using System.Collections.Generic;
using UnityEngine;

public class StrikerManager : MonoBehaviour
{
    public static StrikerManager Instance { get; private set; }

    [SerializeField] private List<GameObject> _strikersPrefabs;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        SpawnStrikers();
    }
    public void SpawnStrikers()
    {
        var strikers = Instantiate(GetRandomStrikers());
    }
    private GameObject GetRandomStrikers() => _strikersPrefabs[Random.Range(0, _strikersPrefabs.Count)];
}
