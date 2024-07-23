using DG.Tweening;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance { get; private set; }

    [SerializeField] private CanvasGroup _tutorialGroup;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        if (PlayerPrefs.GetInt("IsToturialCompleted", 0) == 0)
            OpenTutorial();
    }
    private void OpenTutorial()
    {
        GameState.Instance.ChangeState(GameState.State.Paused);
        _tutorialGroup.alpha = 1f;
        _tutorialGroup.blocksRaycasts = true;
    }
    public void CloseToturial()
    {
        PlayerPrefs.SetInt("IsToturialCompleted", 1);
        GameState.Instance.ChangeState(GameState.State.InGame);
        _tutorialGroup.DOFade(0, 0.3f).SetLink(_tutorialGroup.gameObject);
        _tutorialGroup.blocksRaycasts = false;
    }
}
