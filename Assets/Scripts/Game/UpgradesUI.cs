using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesUI : MonoBehaviour
{
    public static UpgradesUI Instance { get; private set; }

    [SerializeField] private CanvasGroup _upgradesGroup;
    [SerializeField] private CanvasGroup _errorGroup;

    [SerializeField] private List<Image> _upgradeButtons;
    [SerializeField] private List<TextMeshProUGUI> _priceTexts;
    [SerializeField] private TextMeshProUGUI _errorText;

    [SerializeField] private Sprite _buttonActive;
    [SerializeField] private Sprite _buttonInactive;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void OpenUpgrades()
    {
        UpdateUpgrades();
        _upgradesGroup.blocksRaycasts = true;
        _upgradesGroup.alpha = 0f;
        _upgradesGroup.DOFade(1, 0.3f).SetLink(_upgradesGroup.gameObject);
    }
    public void CloseUpgrades() 
    {
        GameState.Instance.ChangeState(GameState.State.InGame);
        _upgradesGroup.blocksRaycasts = false;
        _upgradesGroup.DOFade(0, 0.3f).SetLink(_upgradesGroup.gameObject);
    }
    public void UpdateUpgrades()
    {
        for (int i = 0; i < _priceTexts.Count; i++)
        {
            if (Upgrades.Instance.GetOpened(i))
            {
                _upgradeButtons[i].sprite = _buttonActive;
            }
            else
            {
                _upgradeButtons[i].sprite = _buttonInactive;
                return;
            }
            _priceTexts[i].text = Upgrades.Instance.GetUpgradePrice(i).ToString();
        }
    }
    public void OnUpgrade(int id)
    {
        Upgrades.Instance.Upgrade(id);
        UpdateUpgrades();
    }
    public void ShowError(int count)
    {
        _errorText.text = count.ToString();
        _errorGroup.blocksRaycasts = true;
        _errorGroup.alpha = 0f;
        _errorGroup.DOFade(1, 0.3f).SetLink(_errorGroup.gameObject);
    }
    public void CloseError() 
    {
        _errorGroup.blocksRaycasts = false;
        _errorGroup.DOFade(0, 0.3f).SetLink(_errorGroup.gameObject);
    }
}
