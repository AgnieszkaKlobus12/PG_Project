using System;
using System.Collections;
using UnityEngine;

public class BirdBossController : MonoBehaviour
{
    public float moveSpeed;
    private Animator _animator;
    public GameObject allLives;
    private GameObject _target;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;
    private GameObject _orc;
    private GameObject _human;
    public float distance;
    public LayerMask groundMask;
    public float groundOverlapHeight;
    private bool _fight;
    private bool _active;
    public GameObject[] lives;
    private int _idx;

    void Start()
    {
        _idx = lives.Length - 1;
        _fight = true;
        _active = true;
        _animator = GetComponent<Animator>();
        _target = null;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _orc = GameObject.Find("Orc");
        _human = GameObject.Find("Human");
        allLives.SetActive(false);
    }

    private void Update()
    {
        if (!_active) return;
        if (_target != null)
        {
            allLives.SetActive(true);
            transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(_target.transform.position.x, _target.transform.position.y),
                moveSpeed * Time.deltaTime);
            if (_animator.GetInteger("Action") != 1)
            {
                _animator.SetInteger("Action", 1);
            }
        }
        else
        {
            allLives.SetActive(false);
            if (!IsGrounded())
            {
                if (_animator.GetInteger("Action") != 1)
                {
                    _animator.SetInteger("Action", 1);
                }

                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x, transform.position.y - 1f),
                    moveSpeed * Time.deltaTime);
            }
            else if (_animator.GetInteger("Action") != 0)
            {
                _animator.SetInteger("Action", 0);
            }
        }

        _spriteRenderer.flipX = _rigidbody2D.velocity.x <= 0;
        if (Vector2.Distance(transform.position, _human.transform.position) < distance)
        {
            _target = _human;
        }
        else if (Vector2.Distance(transform.position, _orc.transform.position) < distance)
        {
            _target = _orc;
        }
        else
        {
            _target = null;
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
            else
            {
                other.gameObject.GetComponent<PlayerController>().Die(); // Kill
            }
        }
        else if (other.CompareTag("Ground"))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }

    private IEnumerator Die()
    {
        _target = null;
        _animator.SetInteger("Action", 0);
        _fight = false;
        lives[_idx].GetComponent<SpriteRenderer>().enabled = false;
        _idx -= 1;
        if (_idx < 0)
        {
            _animator.SetInteger("Action", 2);
            _active = false;
        }

        yield return new WaitForSeconds(1.5f);
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