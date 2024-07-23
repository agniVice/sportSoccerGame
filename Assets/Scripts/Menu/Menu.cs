using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("MainMenu")]
    [SerializeField] private CanvasGroup _mainMenuGroup;
    [SerializeField] private List<Transform> _mainMenuTransforms;

    [Space]
    [Header("Settings")]
    [SerializeField] private CanvasGroup _settingsGroup;
    [SerializeField] private List<Transform> _settingsTransforms;
    [SerializeField] private Image _soundImage;
    [SerializeField] private Image _musicImage;
    [SerializeField] private Sprite[] _soundSprites;
    [SerializeField] private Sprite[] _musicSprites;

    [Space]
    [Header("Other")]
    [SerializeField] private Image _fadeFx;

    private void Start()
    {
        _fadeFx.DOFade(1, 0.01f).SetLink(_fadeFx.gameObject);
        _fadeFx.DOFade(0, 1f).SetLink(_fadeFx.gameObject);
        OpenMenu();
    }
    public void OpenMenu()
    {
        _mainMenuGroup.blocksRaycasts = true;
        _mainMenuGroup.alpha = 0f;
        _mainMenuGroup.DOFade(1, 0.3f).SetLink(_mainMenuGroup.gameObject);
        float delay = 0f;
        foreach (var item in _mainMenuTransforms)
        {
            Vector3 startScale = item.localScale;
            item.localScale = Vector3.zero;
            item.DOScale(startScale, 0.2f).SetDelay(delay).SetEase(Ease.OutBack).SetLink(item.gameObject);
            delay += 0.1f;
        }
    }
    public void CloseMenu()
    {
        _mainMenuGroup.blocksRaycasts = false;
        _mainMenuGroup.DOFade(0, 0.3f).SetLink(_mainMenuGroup.gameObject);
    }
    public void OpenSettigns()
    {
        UpdateSettings();
        _settingsGroup.blocksRaycasts = true;
        _settingsGroup.alpha = 0f;
        _settingsGroup.DOFade(1, 0.3f).SetLink(_settingsGroup.gameObject);
        float delay = 0f;
        foreach (var item in _settingsTransforms)
        {
            Vector3 startScale = item.localScale;
            item.localScale = Vector3.zero;
            item.DOScale(startScale, 0.2f).SetDelay(delay).SetEase(Ease.OutBack).SetLink(item.gameObject);
            delay += 0.1f;
        }
    }
    public void CloseSettings()
    {
        _settingsGroup.blocksRaycasts = false;
        _settingsGroup.DOFade(0, 0.3f).SetLink(_settingsGroup.gameObject);
    }
    private void UpdateSettings()
    {
        _soundImage.sprite = _soundSprites[Convert.ToInt32(SoundManger.Instance.IsSoundEnabled)];
        _musicImage.sprite = _musicSprites[Convert.ToInt32(SoundManger.Instance.IsMusicEnabled)];
    }
    public void OnPlayButtonClikced()
    {
        _fadeFx.DOFade(1, 0.5f).SetLink(_fadeFx.gameObject);
        SceneLoader.Instance.LoadScene("Game", 0.5f);
    }
    public void OnExitButtonClicled() => Application.Quit();
    public void OnSoundToggled()
    {
        SoundManger.Instance.SoundToggle();
        UpdateSettings();
    }
    public void OnMusicToggled()
    {
        SoundManger.Instance.MusicToggle();
        UpdateSettings();
    }
}
