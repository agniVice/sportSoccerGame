using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public static Upgrades Instance { get; private set; }

    [SerializeField] private List<int> _upgradePrices;
    [SerializeField] private List<int> _upgrades;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void Upgrade(int id)
    {
        if (_upgrades[id] >= _upgradePrices.Count - 1)
            return;
        if (PlayerBalance.Instance.Coins >= _upgradePrices[_upgrades[id + 1]]) 
        {
            _upgrades[id]++;
        }
        else
            UpgradesUI.Instance.ShowError(PlayerBalance.Instance.Coins - _upgradePrices[_upgrades[id + 1]]);
    }
    public int GetUpgradePrice(int id)
    {
        if (_upgrades[id] >= _upgradePrices.Count - 1)
            return -1;
        else
            return _upgradePrices[_upgrades[id]];
    }
    public bool GetOpened(int id) => _upgrades[id] < _upgradePrices.Count - 1;
}
