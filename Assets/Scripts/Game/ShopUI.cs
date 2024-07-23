using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance { get; private set; }

    [SerializeField] private CanvasGroup _shopGroup;
    [SerializeField] private List<Transform> _shopTransforms;

    [SerializeField] private List<TextMeshProUGUI> _prices;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void OpenShop()
    {
        GameState.Instance.ChangeState(GameState.State.Paused);
        UpdateShop();
        _shopGroup.alpha = 0f;
        _shopGroup.blocksRaycasts = true;
        _shopGroup.DOFade(1, 0.3f).SetLink(_shopGroup.gameObject);

        float delay = 0f;
        foreach (var item in _shopTransforms)
        {
            Vector2 startScale = item.localScale;
            item.localScale = Vector2.zero;
            item.DOScale(startScale, 0.15f).SetEase(Ease.OutBack).SetLink(item.gameObject).SetDelay(delay);
            delay += 0.05f;
        }
    }
    public void CloseShop()
    {
        GameState.Instance.ChangeState(GameState.State.InGame);
        _shopGroup.blocksRaycasts = false;
        _shopGroup.DOFade(0, 0.3f).SetLink(_shopGroup.gameObject);
    }
    private void UpdateShop()
    {
        for (int i = 0; i < _prices.Count; i++)
        {
            if (Shop.Instance.IsLastUpgrade(i))
                _prices[i].text = "MAX";
            else
                _prices[i].text = Shop.Instance.GetPrice(i).ToString();
        }
    }
    public void OnUpgradeClicked(int id)
    {
        if (Shop.Instance.IsLastUpgrade(id))
            return;
        Shop.Instance.Upgrade(id);
        UpdateShop();
    }
}
