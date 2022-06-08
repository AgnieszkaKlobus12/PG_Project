using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SetAnimatorParameter : MonoBehaviour
{
    private Animator animator;
    public string player;
    private bool _movementEnabled;
    private PlayerActions _playerActions;
    private bool _groundCheckEnabled = true;
    private bool _jumping;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    public float _initialGravityScale;
    private Vector2 _boxCenter;
    private Vector2 _boxSize;
    private WaitForSeconds _wait;
    public float speed;
    public float jumpPower;
    public float jumpFallGravityMultiplier;
    private Vector2 lastRespawn;

    [Header("Ground Check")] public float groundOverlapHeight;
    public LayerMask groundMask;
    public float disableGCTime;

    [Header("Lives")] private int _health;
    public GameObject[] lives;

    private void Awake()
    {
        _playerActions = new PlayerActions();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _wait = new WaitForSeconds(disableGCTime);
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("X", 0f);
        _health = PlayerPrefs.GetInt("health");
        if (!PlayerPrefs.HasKey("health"))
        {
            _health = lives.Length;
        }
        for (var i = _health; i < lives.Length; i++)
        {
            lives[i].GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    private void Start()
    {
        lastRespawn = gameObject.transform.position;
        _movementEnabled = true;
        setAnimation("Idle");
    }

    void Update()
    {
        HandleGravity();
        if (_movementEnabled)
            if (player == "Orc")
            {
                OrcMove();
            }
            else if (player == "Human" && _playerActions.Multiplayer.enabled)
            {
                HumanMove();
            }
    }

    void OnEnable()
    {
        _playerActions.Multiplayer.Enable();
    }

    void OnDisable()
    {
        _playerActions.Multiplayer.Disable();
    }

    void OrcMove()
    {
        setAnimation("Walk");
        var moveInput = _playerActions.Multiplayer.OrcMove.ReadValue<Vector2>();
        _rigidbody.velocity = new Vector2(moveInput.x * speed, _rigidbody.velocity.y);
        if (moveInput.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (moveInput.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            setAnimation("Idle");
        }
    }

    void HumanMove()
    {
        setAnimation("Walk");
        var moveInput = _playerActions.Multiplayer.HumanMove.ReadValue<Vector2>();
        _rigidbody.velocity = new Vector2(moveInput.x * speed, _rigidbody.velocity.y);
        if (moveInput.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (moveInput.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            setAnimation("Idle");
        }
    }

    private void HandleGravity()
    {
        if (_groundCheckEnabled && IsGrounded())
        {
            _jumping = false;
        }
        else if (_jumping && _rigidbody.velocity.y < 0f)
        {
            _rigidbody.gravityScale = _initialGravityScale * jumpFallGravityMultiplier;
        }
        else
        {
            _rigidbody.gravityScale = _initialGravityScale;
        }
    }

    private bool IsGrounded()
    {
        var bounds = _collider.bounds;
        _boxCenter = new Vector2(bounds.center.x, bounds.center.y) +
                     (Vector2.down * (bounds.extents.y + (groundOverlapHeight / 2f)));
        _boxSize = new Vector2(bounds.size.x, groundOverlapHeight);
        var groundBox = Physics2D.OverlapBox(_boxCenter, _boxSize, 0f, groundMask);

        return groundBox != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(collision.gameObject.transform, true);
        }
        else if (collision.gameObject.CompareTag("Killzone"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            lastRespawn = other.transform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        transform.parent = null;
    }

    void Die()
    {
        setAnimation("Die");
        _movementEnabled = false;
        _rigidbody.velocity = new Vector2(0, 0);
        AfterDie();
    }

    void AfterDie()
    {
        _movementEnabled = true;
        _rigidbody.position = lastRespawn;
        _health -= 1;
        lives[_health].GetComponent<SpriteRenderer>().sprite = null;
        setAnimation("Idle");

        if (_health == 0)
        {
            // message.SetActive(true);
            // message.transform.GetChild(1).gameObject.SetActive(true);
            // message.transform.GetComponentInChildren<Close>().newScene = "Level 1";
            // PlayerPrefs.SetInt("health", lives.Length);
        }
    }

    void setAnimation(string name)
    {
        
        animator.SetBool("Attack", false);
        animator.SetBool("ChargedAttack", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Die", false);
        animator.SetBool("Jump", false);
        animator.SetBool("Dmg", false);
        animator.SetBool(name, true);
    }
}