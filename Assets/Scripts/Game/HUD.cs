using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD: MonoBehaviour
{
    public static HUD Instance { get; private set; }

    [SerializeField] private CanvasGroup _hudGroup;
    [SerializeField] private TextMeshProUGUI _coins;
    [SerializeField] private TextMeshProUGUI _defenders;
    [SerializeField] private TextMeshProUGUI _defendersMax;
    [SerializeField] private TextMeshProUGUI _missionInfo;
    [SerializeField] private TextMeshProUGUI _missionReward;
    [SerializeField] private Image _missionProgress;
    [SerializeField] private Image _fadeFX;

    [SerializeField] private TextMeshProUGUI _missedCount;
    [SerializeField] private TextMeshProUGUI _savedCount;

    [SerializeField] private CanvasGroup _savedGroup;
    [SerializeField] private TextMeshProUGUI _savedInfo;
    [SerializeField] private ParticleSystem _savedParticle;

    [SerializeField] private CanvasGroup _missedGroup;
    [SerializeField] private TextMeshProUGUI _missedInfo;

    private int _startCoins;
    private int _currentCoins;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        _fadeFX.DOFade(1, 0.01f).SetLink(_fadeFX.gameObject);
        _fadeFX.DOFade(0, 0.5f).SetLink(_fadeFX.gameObject);

        ShowHUD();
    }
    public void ShowHUD()
    {
        UpdateCoins();
        UpdateDefenders();
        _hudGroup.blocksRaycasts = true;
        _hudGroup.alpha = 0f;
        _hudGroup.DOFade(1, 0.5f).SetLink(_hudGroup.gameObject);
    }
    public void HideHUD() 
    {
        _hudGroup.blocksRaycasts = false;
        _hudGroup.DOFade(0, 0.5f).SetLink(_hudGroup.gameObject);
    }
    public void UpdateCoins()
    {
        _currentCoins = _startCoins;
        DOTween.To(() => _currentCoins, x => _currentCoins = x, PlayerBalance.Instance.Coins, 1f).SetEase(Ease.Linear).OnUpdate(UpdateCoinsText);
    }
    public void UpdateCoinsText()
    {
        _coins.text = _currentCoins.ToString();
        _startCoins = PlayerBalance.Instance.Coins;
    }
    public void UpdateDefenders()
    {
        _defenders.text = DefenderManager.Instance.CurrentDefenders.ToString();
        _defendersMax.text = DefenderManager.Instance.MaxDefenders.ToString();
    }
    public void OnMenuClicked()
    {
        SceneLoader.Instance.LoadScene("Menu");
    }
    public void OnMissed()
    {
        _missedInfo.text = "-"+PlayerBalance.Instance.GetFine();
        _missedCount.text = PlayerBalance.Instance.Missed.ToString();
        UpdateCoins();
        _missedGroup.DOFade(1, 0.3f).SetLink(_missedGroup.gameObject);
        _missedGroup.DOFade(0, 0.3f).SetLink(_missedGroup.gameObject).SetDelay(2f);
    }
    public void OnSaved()
    {
        _savedInfo.text = "+"+PlayerBalance.Instance.GetReward();
        _savedCount.text = PlayerBalance.Instance.Saved.ToString();
        UpdateCoins();
        _savedGroup.DOFade(1, 0.3f).SetLink(_savedGroup.gameObject);
        _savedParticle.Play();
        _savedGroup.DOFade(0, 0.3f).SetLink(_savedGroup.gameObject).SetDelay(2f);
    }
}
