using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private float _loadingTime;
    [SerializeField] private Image _loadingBar;
    [SerializeField] private TextMeshProUGUI _percentText;

    private void Start()
    {
        _percentText.text = "0%";
        _loadingBar.fillAmount = 0f;
        Load();
    }
    private void Load()
    {
        _loadingBar.DOFillAmount(0.63f, _loadingTime / 2).SetLink(_loadingBar.gameObject);
        _loadingBar.DOFillAmount(1, _loadingTime / 2).SetLink(_loadingBar.gameObject).SetDelay(_loadingTime/2);

        DOTween.To(() => 0f, value => _percentText.text = $"{Mathf.RoundToInt(value)}%", 63f, _loadingTime/2);
        DOTween.To(() => 63f, value => _percentText.text = $"{Mathf.RoundToInt(value)}%", 100f, _loadingTime / 2).SetDelay(_loadingTime/2);

        SceneLoader.Instance.LoadScene("Menu", _loadingTime);
    }
}
