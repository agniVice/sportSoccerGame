using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public static Gate Instance { get; private set; }
    [SerializeField] private List<Transform> _gatePositions;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public Transform GetRandomPosition() => _gatePositions[Random.Range(0, _gatePositions.Count)];
}
