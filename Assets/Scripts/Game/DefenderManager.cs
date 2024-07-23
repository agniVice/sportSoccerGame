using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DefenderManager : MonoBehaviour
{
    public static DefenderManager Instance { get; private set; }

    public int MaxDefenders { get; private set; }
    public int CurrentMaxDefenders { get; private set; }
    public int CurrentDefenders { get; private set; }

    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private float[] _spawnIntervals;
    [SerializeField] private int _startDefendersCount = 7;

    private Vector2 _startPoint;
    private Vector2 _endPoint;
    private bool _isDrawing = false;
    private bool _canOnlyDecreaseLine = false;
    private List<Vector2> _spawnPoints = new List<Vector2>();
    private List<DefenderWall> _defenderWalls = new List<DefenderWall>();

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        UpdateDefenderCount(true);

        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("DefenderZone"))
            {
                _startPoint = mousePos;
                _endPoint = _startPoint;
                _isDrawing = true;
                _canOnlyDecreaseLine = false;

                _lineRenderer.positionCount = 2;
                _lineRenderer.SetPosition(0, _startPoint);
                _lineRenderer.SetPosition(1, _endPoint);

                var wall = Instantiate(_wallPrefab, _startPoint, Quaternion.identity).GetComponent<DefenderWall>();
                _defenderWalls.Add(wall);
            }
        }

        if (Input.GetMouseButton(0) && _isDrawing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 newEndPoint = new Vector2(mousePos.x, _startPoint.y); // ограничение по оси Y

            if (_canOnlyDecreaseLine)
            {
                if (Vector2.Distance(newEndPoint, _startPoint) <= Vector2.Distance(_endPoint, _startPoint))
                {
                    _endPoint = newEndPoint;
                    _lineRenderer.SetPosition(1, _endPoint);
                    RemovePrefabsOutsideLine();
                }
            }
            else
            {
                _endPoint = newEndPoint;
                _lineRenderer.SetPosition(1, _endPoint);
                TrySpawnPrefab(_endPoint);
            }

            _canOnlyDecreaseLine = _defenderWalls[_defenderWalls.Count - 1].GetDefendersCount() == CurrentMaxDefenders;
        }

        if (Input.GetMouseButtonUp(0) && _isDrawing)
        {
            _isDrawing = false;
            _lineRenderer.positionCount = 0;
            _spawnPoints.Clear();
        }
    }
    public void RemoveWall(DefenderWall wall)
    {
        _defenderWalls.Remove(wall);
        Destroy(wall.gameObject);
    }

    private void TrySpawnPrefab(Vector2 newPoint)
    {
        if (_spawnPoints.Count == 0 || Vector2.Distance(_spawnPoints[_spawnPoints.Count - 1], newPoint) >= _spawnIntervals[PlayerPrefs.GetInt("2Upgrade", 0)])
        {
            _spawnPoints.Add(newPoint);
            if (CurrentDefenders > 0)
            {
                SoundManger.Instance.PlaySound(SoundManger.Instance.Defender, 0.2f);
                CurrentDefenders--;
                GameObject spawnedPrefab = Instantiate(_prefabs[PlayerPrefs.GetInt("3Upgrade",0)], newPoint, Quaternion.identity);
                _defenderWalls[_defenderWalls.Count - 1].SetDefender(spawnedPrefab);
                HUD.Instance.UpdateDefenders();
            }
            else
            {
                var defender = _defenderWalls[0].GetDefender();
                if(defender == null)
                    defender = _defenderWalls[0].GetDefender();
                Destroy(defender);
                GameObject spawnedPrefab = Instantiate(_prefabs[PlayerPrefs.GetInt("3Upgrade", 0)], newPoint, Quaternion.identity);
                _defenderWalls[_defenderWalls.Count - 1].SetDefender(spawnedPrefab);
                SoundManger.Instance.PlaySound(SoundManger.Instance.Defender, 0.2f);
            }
        }
    }

    private void RemovePrefabsOutsideLine()
    {
        for (int i = _defenderWalls[_defenderWalls.Count-1].GetDefendersCount() - 1; i >= 0; i--)
        {
            if (!IsPointOnLine(_defenderWalls[_defenderWalls.Count-1].GetDefender(i).transform.position))
            {
                var defender = _defenderWalls[_defenderWalls.Count-1].GetComponent<DefenderWall>().GetDefender();
                Destroy(defender);
                CurrentDefenders++;
                HUD.Instance.UpdateDefenders();
            }
        }
    }

    private bool IsPointOnLine(Vector2 point)
    {
        float lineLength = Vector2.Distance(_startPoint, _endPoint);
        float distanceToStart = Vector2.Distance(_startPoint, point);
        float distanceToEnd = Vector2.Distance(_endPoint, point);
        float buffer = 0.1f;

        return Mathf.Abs(lineLength - (distanceToStart + distanceToEnd)) < buffer;
    }
    public void UpdateDefenderCount(bool initialize)
    {
        int usedDefenders = 0;

        foreach (var item in _defenderWalls)
            usedDefenders += item.GetDefendersCount();

        if (initialize)
        {
            MaxDefenders = _startDefendersCount + PlayerPrefs.GetInt("1Upgrade", 0);
            CurrentMaxDefenders = MaxDefenders;
            CurrentDefenders = MaxDefenders;
        }
        else
        {
            MaxDefenders = _startDefendersCount + PlayerPrefs.GetInt("1Upgrade", 0);
            CurrentMaxDefenders = MaxDefenders;
            if (usedDefenders + CurrentDefenders < CurrentMaxDefenders)
                CurrentDefenders += CurrentMaxDefenders - (usedDefenders + CurrentDefenders);
        }
    }
}
