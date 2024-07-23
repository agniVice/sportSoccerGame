using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Shop : MonoBehaviour
{
    public static Shop Instance { get; private set; }

    [SerializeField] private List<int> _prices;
    [SerializeField] private List<int> _maxUpgrades;
    [SerializeField] private List<int> _upgrades;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        Initialize();
    }
    private void Initialize()
    {
        for (int i = 0; i < _upgrades.Count; i++)
            _upgrades[i] = PlayerPrefs.GetInt(i + "Upgrade", 0);
    }
    public void Upgrade(int id)
    {
        if (PlayerBalance.Instance.Coins >= _prices[_upgrades[id]])
        {
            PlayerBalance.Instance.ChangeCoins(-_prices[_upgrades[id]]);

            HUD.Instance.UpdateCoins();
            _upgrades[id]++;
            SoundManger.Instance.PlaySound(SoundManger.Instance.Upgrade, 0.5f);

            Save();
            DefenderManager.Instance.UpdateDefenderCount(false);
            HUD.Instance.UpdateDefenders();
        }
    }
    private void Save()
    {
        for (int i = 0; i < _upgrades.Count; i++)
            PlayerPrefs.SetInt(i + "Upgrade", _upgrades[i]);
    }
    public bool IsLastUpgrade(int id) => (_upgrades[id] == _maxUpgrades[id] - 1);
    public int GetPrice(int id) => _prices[_upgrades[id]];
}
