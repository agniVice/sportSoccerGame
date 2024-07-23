using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionsUI : MonoBehaviour
{
    public static MissionsUI Instance { get; private set; }

    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _info;
    [SerializeField] private TextMeshProUGUI _reward;

    [SerializeField] private CanvasGroup _missionCompleted;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        UpdateMission();
    }
    public void OnMissionCompleted()
    {
        _missionCompleted.DOFade(1, 0.5f).SetLink(_missionCompleted.gameObject);
        _missionCompleted.DOFade(0, 0.5f).SetLink(_missionCompleted.gameObject).SetDelay(2f);
    }
    public void UpdateMission()
    {
        _progressBar.fillAmount = Missions.Instance.GetCurrentInfo() / (float)Missions.Instance.GetCurrentTarget();
        _reward.text = Missions.Instance.GetCurrentReward().ToString();
        _info.text = "Save " + Missions.Instance.GetCurrentTarget() + " attacks";
    }
}
