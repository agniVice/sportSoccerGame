using DG.Tweening;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class Striker : MonoBehaviour
{
    [SerializeField] private Image _hitBar;
    [SerializeField] private CanvasGroup _barGroup;
    [SerializeField] private Image _alert;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private SpriteRenderer _strikerSprite;

    [SerializeField] private float _forceLaunch = 2f;

    private float _hitDelay;
    private float _currentDelay;
    private bool _hitted;
    private bool _launched;

    private Transform _gatePosition;
    private StrikerSet _set;

    private bool _isMissed;

    private void Awake()
    {
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        _alert.color = new Color32(255, 255, 255, 0);
        _alert.DOFade(1, 0.3f).SetLink(_alert.gameObject);
        _alert.transform.DORotate(new Vector3(0, 0, 720f), 1f, RotateMode.LocalAxisAdd).SetLink(_alert.gameObject);
        _alert.DOFade(0, 0.3f).SetLink(_alert.gameObject).SetDelay(1f);
        _strikerSprite.color = new Color32(255, 255, 255, 0);
        _strikerSprite.DOFade(1, 0.3f).SetDelay(1f).SetLink(_strikerSprite.gameObject);
        _barGroup.alpha = 0f;
        _barGroup.DOFade(1, 0.3f).SetDelay(1f).SetLink(_barGroup.gameObject);
    }
    public void Initialize(float delay, StrikerSet set, Transform pos)
    { 
        _gatePosition = pos;
        _set = set;
        _hitDelay = delay;
        _currentDelay = 0f;
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        _lineRenderer.positionCount = 2;
        if (transform.position.x > 0)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;
        if (_hitted)
            return;
        if (_currentDelay < _hitDelay)
        {
            _currentDelay += Time.fixedDeltaTime;
            _hitBar.fillAmount = _currentDelay / _hitDelay;
        }
        else
        { 
            if(!_launched)
                Hit();
        }

    }
    private void Hit()
    {
        SoundManger.Instance.PlaySound(SoundManger.Instance.Kick, 0.7f);
        _launched = true;
        var ball = Instantiate(_ballPrefab);
        ball.transform.position = transform.position;
        ball.GetComponent<Ball>().Launch(this, _gatePosition.position - transform.position, _forceLaunch);
    }
    public void Missed()
    {
        if (_hitted)
            return;
        _hitted = true;
        _set.OnStrikerHit(true);
    }
    public void Saved()
    {
        if (_hitted)
            return;
        _hitted = true;
        _set.OnStrikerHit(false);
    }
    public bool GetHit() => _hitted;
    public bool GetMissed() => _isMissed;
    public Vector3 GetGatePosition() => _gatePosition.position;
}
