using System.Collections.Generic;
using UnityEngine;

public class StrikerSet : MonoBehaviour
{
    public bool Finished;

    [SerializeField] private List<Striker> _strikers;
    [SerializeField] private Transform _commonPoint;

    [SerializeField] private float _minStrikerDistance;
    [SerializeField] private float _maxStrikerDistance;
    [SerializeField] private float _minGatePosition;

    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;

    private void Start()
    {
        float delay = GetDelay();

        Transform lastPos = Gate.Instance.GetRandomPosition();
        Transform gatePos = Gate.Instance.GetRandomPosition();

        SoundManger.Instance.PlaySound(SoundManger.Instance.Defender, 0.4f);
        foreach (var striker in _strikers)
        {
            while(Vector2.Distance(lastPos.position, gatePos.position) < _minGatePosition)
                gatePos = Gate.Instance.GetRandomPosition();

            striker.Initialize(delay, this, gatePos);
            lastPos = gatePos;
            delay *= 1.2f;
        }
        _commonPoint.position = new Vector2(Vector3.Lerp(_strikers[0].GetGatePosition(), _strikers[_strikers.Count - 1].GetGatePosition(), 0.5f).x, _commonPoint.position.y);

        foreach (var striker in _strikers)
        {
            striker.SetPosition((_commonPoint.position - striker.GetGatePosition()).normalized * Random.Range(_minStrikerDistance, _maxStrikerDistance));
        }
    }
    public void OnStrikerHit(bool missed)
    {
        if (missed)
        {
            SoundManger.Instance.PlaySound(SoundManger.Instance.Missed, 0.7f);
            PlayerBalance.Instance.OnMissed();
            HUD.Instance.OnMissed();
            StrikerManager.Instance.SpawnStrikers();
            Destroy(gameObject);
            return;
        }
        bool canSpawn = true;
        foreach (var item in _strikers) 
        {
            if (item.GetHit() == false)
                canSpawn = false;
        }
        if (canSpawn)
        {
            SoundManger.Instance.PlaySound(SoundManger.Instance.Saved, 0.5f);
            PlayerBalance.Instance.OnSaved();
            Missions.Instance.GiveMissionInfo(1);
            HUD.Instance.OnSaved();
            StrikerManager.Instance.SpawnStrikers();
            Destroy(gameObject);
        }
    }
    private float GetDelay() => Random.Range(_minDelay, _maxDelay);
}
