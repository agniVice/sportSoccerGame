using DG.Tweening;
using System.Xml.Serialization;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Striker _striker;
    private SpriteRenderer _spriteRenderer;

    private bool _collided;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Launch(Striker striker, Vector2 direction, float force)
    {
        _striker = striker;
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.AddForce(direction * force, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_collided)
            return;
        if (collision.CompareTag("Gate"))
        {
            _collided = true;
            _rigidBody.velocity = -_rigidBody.velocity * 0.1f;
            _spriteRenderer.DOFade(0, 0.5f).SetLink(gameObject);
            Destroy(gameObject, 0.6f);
            if(_striker != null)
                _striker.Missed();
        }
        if (collision.CompareTag("Defender"))
        {
            _collided = true;
            _rigidBody.velocity = -_rigidBody.velocity * 0.1f;
            _spriteRenderer.DOFade(0, 0.5f).SetLink(gameObject);
            Destroy(gameObject, 0.6f);
            if (_striker != null)
                _striker.Saved();
        }
    }
}
