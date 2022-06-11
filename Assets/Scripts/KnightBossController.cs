using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBossController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask groundMask;
    public float groundOverlapHeight;
    private CapsuleCollider2D _collider;
    private Animator _animator;
    public GameObject allLives;
    private GameObject _target;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private GameObject _orc;
    private GameObject _human;
    public float distance;
    private bool _fight;
    private bool _active;
    public GameObject[] lives;
    private int _idx;
    public GameObject smokeParticle;

    void Start()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        _idx = lives.Length - 1;
        _fight = true;
        _active = true;
        _animator = GetComponent<Animator>();
        _target = null;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _orc = GameObject.Find("Orc");
        _human = GameObject.Find("Human");
        allLives.SetActive(false);
    }

    private void Update()
    {
        if (_target != null && _active)
        {
            allLives.SetActive(true);
            transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(_target.transform.position.x, transform.position.y),
                moveSpeed * Time.deltaTime);
            if (_animator.GetInteger("Action") != 1)
            {
                _animator.SetInteger("Action", 1);
            }
        }
        else
        {
            allLives.SetActive(false);
            if (_animator.GetInteger("Action") != 0)
            {
                _animator.SetInteger("Action", 0);
            }
        }

        _spriteRenderer.flipX = _rigidbody2D.velocity.x <= 0;
        if (Vector2.Distance(transform.position, _human.transform.position) < distance)
        {
            _target = _human;
            _active = true;
        }
        else if (Vector2.Distance(transform.position, _orc.transform.position) < distance)
        {
            _target = _orc;
            _active = true;
        }
        else
        {
            _target = null;
        }

        if (!IsGrounded())
        {
            transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(transform.position.x, transform.position.y - 0.1f),
                moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Human"))
        {
            if (!_fight || !_active) return;
            if (other.gameObject.GetComponent<Animator>().GetInteger("Anim") < 2)
            {
                StartCoroutine(Die());
            }
        }
        else if (other.gameObject.CompareTag("Killzone"))
        {
            StartCoroutine(Die());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }

    private IEnumerator Die()
    {
        _target = null;
        _fight = false;
        lives[_idx].GetComponent<SpriteRenderer>().enabled = false;
        _idx -= 1;
        if (_idx < 0)
        {
            _spriteRenderer.enabled = false;
            smokeParticle.GetComponent<ParticleSystem>().Play();
            _active = false;
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }

        yield return new WaitForSeconds(1f);
        _fight = true;
    }

    private bool IsGrounded()
    {
        var bounds = _collider.bounds;
        var boxCenter = new Vector2(bounds.center.x, bounds.center.y) +
                        (Vector2.down * (bounds.extents.y + (groundOverlapHeight / 2f)));
        var boxSize = new Vector2(bounds.size.x, groundOverlapHeight);
        var groundBox = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundMask);
        return groundBox != null;
    }
}