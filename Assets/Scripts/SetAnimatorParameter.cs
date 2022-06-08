using System;
using System.Collections;
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
    private bool _jumpEnabled;
    private bool _doubleJumpEnabled;
    private int _jumps;
    private bool _attackEnabled;
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
        _jumpEnabled = true; //w zależności od poziomu! - to do testów
        _attackEnabled = true; //w zależności od poziomu! - to do testów
        _doubleJumpEnabled = true; //w zależności od poziomu! - to do testów
        _jumps = 0;
        _playerActions = new PlayerActions();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _wait = new WaitForSeconds(disableGCTime);
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("X", 0f);
        _playerActions.Multiplayer.OrcJump.performed += Jump;
        _playerActions.Singleplayer.Jump.performed += Jump;
        _playerActions.Multiplayer.HumanJump.performed += Jump;
        _playerActions.Multiplayer.OrcFire.performed += Attack;
        _playerActions.Singleplayer.Fire.performed += Attack;
        _playerActions.Multiplayer.HumanFire.performed += Attack;
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
        var moveInput = _playerActions.Multiplayer.OrcMove.ReadValue<Vector2>();
        _rigidbody.velocity = new Vector2(moveInput.x * speed, _rigidbody.velocity.y);
        if (moveInput.x > 0)
        {
            setAnimation("Walk");
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (moveInput.x < 0)
        {
            setAnimation("Walk");
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            setAnimation("ChargedAttack");
        }
    }

    void HumanMove()
    {
        var moveInput = _playerActions.Multiplayer.HumanMove.ReadValue<Vector2>();
        _rigidbody.velocity = new Vector2(moveInput.x * speed, _rigidbody.velocity.y);
        if (moveInput.x > 0)
        {
            setAnimation("Walk");
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (moveInput.x < 0)
        {
            setAnimation("Walk");
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            setAnimation("ChargedAttack");
        }
    }

    private void HandleGravity()
    {
        if (_groundCheckEnabled && IsGrounded())
        {
            _jumping = false;
            _jumps = 0;
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
        StartCoroutine(AfterDie());
    }

    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(2f);
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

    private void Jump(InputAction.CallbackContext context)
    {
        if ((_playerActions.Multiplayer.OrcJump.triggered || _playerActions.Singleplayer.Jump.triggered) &&
            player == "Orc")
        {
            PerformJump();
        }
        else if (player == "Human" && _playerActions.Multiplayer.enabled &&
                 _playerActions.Multiplayer.HumanJump.triggered)
        {
            PerformJump();
        }
    }

    private void PerformJump()
    {
        if ((!IsGrounded() && _jumps>1) || (!IsGrounded() && !_doubleJumpEnabled) || !_movementEnabled || !_jumpEnabled) return;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpPower);
        _jumping = true;
        setAnimation("Jump");
        _jumps++;
        StartCoroutine(EnableGroundCheckAfterJump());
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (_attackEnabled)
        {
            if ((_playerActions.Multiplayer.OrcFire.triggered || _playerActions.Singleplayer.Fire.triggered) &&
                player == "Orc")
            {
                PerformAttack();
            }
            else if (player == "Human" && _playerActions.Multiplayer.enabled &&
                     _playerActions.Multiplayer.HumanFire.triggered)
            {
                PerformAttack();
            }
        }
    }

    private void PerformAttack()
    {
        _movementEnabled = false;
        _attackEnabled = false;
        setAnimation("Attack");
        StartCoroutine(EnableAttack());
    }

    private IEnumerator EnableAttack()
    {
        yield return new WaitForSeconds(0.25f);
        _attackEnabled = true;
        _movementEnabled = true;
        setAnimation("Idle");
    }

    private IEnumerator EnableGroundCheckAfterJump()
    {
        _groundCheckEnabled = false;
        yield return _wait;
        _groundCheckEnabled = true;
    }

    void setAnimation(string name)
    {
        Debug.Log(animator.GetInteger("Anim"));
        switch (name)
        {
            case "Attack":
                if (animator.GetInteger("Anim") != 0)
                {
                    animator.SetInteger("Anim", 0);
                }

                break;
            case "ChargedAttack":
                if (animator.GetInteger("Anim") != 1)
                {
                    animator.SetInteger("Anim", 1);
                }

                break;
            case "Dmg":
                if (animator.GetInteger("Anim") != 2)
                {
                    animator.SetInteger("Anim", 2);
                }

                break;
            case "Walk":
                if (animator.GetInteger("Anim") != 3)
                {
                    animator.SetInteger("Anim", 3);
                }

                break;
            case "Die":
                if (animator.GetInteger("Anim") != 4)
                {
                    animator.SetInteger("Anim", 4);
                }

                break;
            case "Jump":
                if (animator.GetInteger("Anim") != 5)
                {
                    animator.SetInteger("Anim", 5);
                }

                break;
            default: //Idle
                if (animator.GetInteger("Anim") != 6)
                {
                    animator.SetInteger("Anim", 6);
                }

                break;
        }
    }
}