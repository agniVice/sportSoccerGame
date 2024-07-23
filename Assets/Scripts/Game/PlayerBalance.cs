using UnityEngine;

public class PlayerBalance : MonoBehaviour
{
    public static PlayerBalance Instance { get; private set; }

    public int Coins { get; private set; }

    public int Missed { get; private set; }

    public int Saved { get; private set; }

    [SerializeField] private int _savedReward;
    [SerializeField] private int _missedFine;


    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        Coins = PlayerPrefs.GetInt("Coins", 0);
    }
    public void ChangeCoins(int count)
    {
        if (Coins + count >= 0)
            Coins += count;
        else
            Coins = 0;
        Save();
    }
    public void OnMissed()
    {
        Missed++;
        ChangeCoins(-_missedFine);
    }
    public void OnSaved()
    {
        Saved++;
        ChangeCoins(_savedReward);
    }
    private void Save()
    {
        PlayerPrefs.SetInt("Coins", Coins);
    }
    public int GetReward() => _savedReward;
    public int GetFine() => _missedFine;
}
