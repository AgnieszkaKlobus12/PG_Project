using System.Collections;
using UnityEngine;

public class WizardBoss : MonoBehaviour
{
    public float moveSpeed;
    private Animator _animator;
    public GameObject allLives;
    private SpriteRenderer _spriteRenderer;
    private bool _attack;
    private bool _active;
    public GameObject[] lives;
    private int _idx;
    public GameObject attackParticle;
    public float boundXLeft;
    public float boundXRight;
    private bool _goRight;
    private bool _canDie;

    void Start()
    {
        _idx = lives.Length - 1;
        _attack = true;
        _canDie = true;
        _active = true;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        allLives.SetActive(false);
        StartCoroutine(Attack());
    }

    private void Update()
    {
        if (_active && _attack)
        {
            allLives.SetActive(true);
            if (_goRight)
            {
                if (transform.position.x >= boundXRight)
                {
                    _goRight = false;
                }
                else
                {
                    gameObject.transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(transform.position.x + 1f, gameObject.transform.position.y),
                        moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (transform.position.x <= boundXLeft)
                {
                    _goRight = true;
                }
                else
                {
                    gameObject.transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(transform.position.x - 1f, transform.position.y),
                        moveSpeed * Time.deltaTime);
                }
            }

            if (_animator.GetInteger("Action") != 0)
            {
                _animator.SetInteger("Action", 0);
            }

            _spriteRenderer.flipX = !_goRight;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Human"))
        {
            if (!_attack || !_active) return;
            if (other.gameObject.GetComponent<Animator>().GetInteger("Anim") < 2 && _canDie)
            {
                StartCoroutine(Die());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }

    private IEnumerator Attack()
    {
        _attack = false;
        yield return new WaitForSeconds(2f);
        _attack = true;
    }

    private IEnumerator Die()
    {
        _canDie = false;
        _attack = false;
        _animator.SetInteger("Action", 1);
        lives[_idx].GetComponent<SpriteRenderer>().enabled = false;
        _idx -= 1;
        if (_idx < 0)
        {
            _active = false;
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(2f);
        _attack = true;
        _canDie = true;
    }
}