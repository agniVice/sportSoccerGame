using UnityEngine;

public class Missions : MonoBehaviour
{
    public static Missions Instance {  get; private set; }

    [SerializeField] private int _startMissionTarget;
    [SerializeField] private int _increaseMissionTarget;

    [SerializeField] private int _startMissionReward;
    [SerializeField] private int _increaseMissionReward;

    private int _currentInfo;
    private int _currentMission;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        _currentInfo = PlayerPrefs.GetInt("CurrentInfo", 0);
        _currentMission = PlayerPrefs.GetInt("CurrentMission", 0);
    }
    private void CheckMissionComplete()
    {
        if (_currentInfo >= (_startMissionTarget + _increaseMissionTarget * _currentMission))
            MissionComplete();
    }
    private void MissionComplete()
    {
        SoundManger.Instance.PlaySound(SoundManger.Instance.MissionComplete, 0.7f);
        PlayerBalance.Instance.ChangeCoins(_startMissionReward + _increaseMissionReward * _currentMission);
        MissionsUI.Instance.OnMissionCompleted();
        _currentInfo = 0;
        _currentMission++;
    }
    public void GiveMissionInfo(int info)
    {
        _currentInfo += info;
        CheckMissionComplete();
        Save();
        MissionsUI.Instance.UpdateMission();
    }
    public int GetCurrentInfo() => _currentInfo;
    public int GetCurrentTarget() => _startMissionTarget + _increaseMissionTarget * _currentMission;
    public int GetCurrentReward() => _startMissionReward + _increaseMissionReward * _currentMission;
    private void Save()
    {
        PlayerPrefs.SetInt("CurrentInfo", _currentInfo);
        PlayerPrefs.SetInt("CurrentMission", _currentMission);
    }
}
