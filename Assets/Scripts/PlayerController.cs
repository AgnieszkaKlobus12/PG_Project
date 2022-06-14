using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    public string player;
    public GameObject _orc;
    private bool _movementEnabled;
    public PlayerActions PlayerActions;
    private bool _groundCheckEnabled = true;
    private bool _jumping;
    private bool _jumpEnabled;
    private bool _doubleJumpEnabled;
    public GameObject attackArea;
    private bool _chargedAttackEnabled;
    private int _jumps;
    private bool _attackEnabled;
    public GameObject lose;
    public GameObject livesObj;
    private bool _dieEnabled;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    public float initialGravityScale;
    private Vector2 _boxCenter;
    private Vector2 _boxSize;
    private WaitForSeconds _wait;
    private SpriteRenderer _spriteRenderer;
    public float speed;
    public float jumpPower;
    public float jumpFallGravityMultiplier;
    private Vector2 _lastRespawn;
    public GameObject chargedAttack;
    private ParticleSystem _chargedAttackSystem;
    private Animator _chargedAttackAnimator;
    private CircleCollider2D _chargedAttackCollider;
    public int mode;

    [Header("Ground Check")] public float groundOverlapHeight;
    public LayerMask groundMask;
    public float disableGCTime;

    [Header("Lives")] private int _health;
    public GameObject[] lives;

    private Settings _settings;

    private void Awake()
    {
        _settings = new Settings();
        _jumps = 0;
        _orc = GameObject.Find("Orc");
        _dieEnabled = true;
        PlayerActions = new PlayerActions();
        _rigidbody = GetComponent<Rigidbody2D>();
        _chargedAttackSystem = chargedAttack.GetComponent<ParticleSystem>();
        _chargedAttackAnimator = chargedAttack.GetComponent<Animator>();
        _chargedAttackCollider = chargedAttack.GetComponent<CircleCollider2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _wait = new WaitForSeconds(disableGCTime);
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat("X", 0f);
        PlayerActions.Multiplayer.OrcJump.performed += ctx => Jump();
        PlayerActions.Singleplayer.Jump.performed += ctx => Jump();
        PlayerActions.Multiplayer.HumanJump.performed += ctx => Jump();
        PlayerActions.Multiplayer.OrcFire.performed += Attack;
        PlayerActions.Singleplayer.Fire.performed += Attack;
        PlayerActions.Multiplayer.HumanFire.performed += Attack;
        PlayerActions.Multiplayer.HumanCharged.performed += ChargedAttack;
        PlayerActions.Multiplayer.OrcCharged.performed += ChargedAttack;
        PlayerActions.Singleplayer.Charged.performed += ChargedAttack;
        _health = gameObject.CompareTag("Player")
            ? _settings.GetOrcLives(PlayerPrefs.GetInt("Slot"))
            : _settings.GetHumanLives(PlayerPrefs.GetInt("Slot"));
        if (_health == 0)
        {
            _health = 3;
        }

        for (var i = _health; i < lives.Length; i++)
        {
            lives[i].GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    private void Start()
    {
        _lastRespawn = gameObject.transform.position;
        _movementEnabled = true;
        setAnimation("Idle");
    }

    void Update()
    {
        HandleGravity();
        if (_movementEnabled)
        {
            if (player == "Orc")
            {
                OrcMove();
            }
            else if (player == "Human")
            {
                HumanMove();
            }
        }
    }

    void OnEnable()
    {
        _chargedAttackEnabled = false;
        _jumpEnabled = false;
        _attackEnabled = false;
        _doubleJumpEnabled = false;
        if (_settings.GetMode(PlayerPrefs.GetInt("Slot")) == "multiplayer")
        {
            PlayerActions.Multiplayer.Enable();
            mode = 2;
        }
        else
        {
            PlayerActions.Singleplayer.Enable();
            mode = 1;
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Level 5":
                _jumpEnabled = true;
                _attackEnabled = true;
                _doubleJumpEnabled = true;
                _chargedAttackEnabled = true;
                break;
            case "Level 4":
                _jumpEnabled = true;
                _attackEnabled = true;
                _doubleJumpEnabled = true;
                break;
            case "Level 3":
                _jumpEnabled = true;
                _attackEnabled = true;
                break;
            case "Level 2":
                _jumpEnabled = true;
                break;
        }
    }

    void OnDisable()
    {
        mode = 0;
        PlayerActions.Singleplayer.Disable();
        PlayerActions.Multiplayer.Disable();
    }

    void OrcMove()
    {
        var moveInput = mode == 1
            ? PlayerActions.Singleplayer.Move.ReadValue<Vector2>()
            : PlayerActions.Multiplayer.OrcMove.ReadValue<Vector2>();

        _rigidbody.velocity = new Vector2(moveInput.x * speed, _rigidbody.velocity.y);
        if (moveInput.x > 0)
        {
            setAnimation("Walk");
            _spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            setAnimation("Walk");
            _spriteRenderer.flipX = true;
        }
        else
        {
            setAnimation("Idle");
        }
    }

    void HumanMove()
    {
        if (mode == 2)
        {
            var moveInput = PlayerActions.Multiplayer.HumanMove.ReadValue<Vector2>();
            _rigidbody.velocity = new Vector2(moveInput.x * speed, _rigidbody.velocity.y);
        }
        else if (mode == 1)
        {
            var x = _orc.transform.position.x < transform.position.x
                ? +1f
                : -1f;
            var bounds = _collider.bounds;
            _boxCenter = new Vector2(bounds.center.x - x/2, bounds.center.y);
            var groundBox = Physics2D.OverlapBox(_boxCenter, _boxSize, 0f, groundMask);
            if (groundBox != null && _jumpEnabled)
            {
                PerformJump();
            }

            _rigidbody.velocity = Math.Abs(transform.position.x - _orc.transform.position.x) > 0.4f
                ? new Vector2(speed * -x, _rigidbody.velocity.y)
                : new Vector2(0, _rigidbody.velocity.y);
        }

        if (_rigidbody.velocity.x > 0)
        {
            setAnimation("Walk");
            _spriteRenderer.flipX = false;
        }
        else if (_rigidbody.velocity.x < 0)
        {
            setAnimation("Walk");
            _spriteRenderer.flipX = true;
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
            _jumps = 0;
        }
        else if (_jumping && _rigidbody.velocity.y < 0f)
        {
            _rigidbody.gravityScale = initialGravityScale * jumpFallGravityMultiplier;
        }
        else
        {
            _rigidbody.gravityScale = initialGravityScale;
        }
    }

    public bool IsGrounded()
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
            _lastRespawn = other.transform.position;
        }
        else if (other.gameObject.CompareTag("Killzone") || other.gameObject.CompareTag("Attack"))
        {
            Die();
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        transform.parent = null;
    }

    public void Die(bool back = true)
    {
        if (_dieEnabled)
        {
            _dieEnabled = false;
            _movementEnabled = false;
            _attackEnabled = false;
            _jumpEnabled = false;
            setAnimation("Die");
            _rigidbody.velocity = new Vector2(0, 0);
            StartCoroutine(AfterDie(back));
        }
    }

    IEnumerator AfterDie(bool back = true)
    {
        yield return new WaitForSeconds(2f);
        _movementEnabled = true;
        _attackEnabled = true;
        _jumpEnabled = true;
        if (back)
        {
            _rigidbody.position = _lastRespawn;
        }

        _health -= 1;
        lives[_health].GetComponent<SpriteRenderer>().sprite = null;
        setAnimation("Idle");
        _dieEnabled = true;
        if (_health == 0)
        {
            livesObj.SetActive(false);
            lose.SetActive(true);
            PlayerActions.Singleplayer.Disable();
            PlayerActions.Multiplayer.Disable();
            PlayerActions.UI.Enable();
        }
    }

    private void Jump()
    {
        if ((PlayerActions.Multiplayer.OrcJump.triggered || PlayerActions.Singleplayer.Jump.triggered) &&
            player == "Orc")
        {
            PerformJump();
        }
        else if (player == "Human" && PlayerActions.Multiplayer.enabled &&
                 PlayerActions.Multiplayer.HumanJump.triggered)
        {
            PerformJump();
        }
    }

    public void PerformJump()
    {
        if ((_jumps < 1 || _jumps > 0 && _doubleJumpEnabled) && _movementEnabled &&
            _jumpEnabled)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpPower);
            _jumping = true;
            setAnimation("Jump");
            _jumps++;
            StartCoroutine(EnableGroundCheckAfterJump());
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (_attackEnabled)
        {
            if ((PlayerActions.Multiplayer.OrcFire.triggered || PlayerActions.Singleplayer.Fire.triggered) &&
                player == "Orc")
            {
                PerformAttack();
            }
            else if (player == "Human" && PlayerActions.Multiplayer.enabled &&
                     PlayerActions.Multiplayer.HumanFire.triggered)
            {
                PerformAttack();
            }
        }
    }

    private void PerformChargedAttack()
    {
        _movementEnabled = false;
        _attackEnabled = false;
        chargedAttack.tag = "Player";
        _chargedAttackSystem.transform.localPosition =
            _spriteRenderer.flipX ? new Vector3(-3f, 0f) : new Vector3(3f, 0f);
        _chargedAttackCollider.enabled = true;
        _chargedAttackAnimator.enabled = true;
        _chargedAttackSystem.Play();
        setAnimation("ChargedAttack");
        StartCoroutine(EnableAttack());
    }

    private void ChargedAttack(InputAction.CallbackContext context)
    {
        if (_chargedAttackEnabled && _attackEnabled)
        {
            if ((PlayerActions.Multiplayer.OrcCharged.triggered || PlayerActions.Singleplayer.Charged.triggered) &&
                player == "Orc")
            {
                PerformChargedAttack();
            }
            else if (player == "Human" && PlayerActions.Multiplayer.enabled &&
                     PlayerActions.Multiplayer.HumanCharged.triggered)
            {
                PerformChargedAttack();
            }
        }
    }

    private void PerformAttack()
    {
        _movementEnabled = false;
        _attackEnabled = false;
        _chargedAttackSystem.Stop();
        setAnimation("Attack");
        attackArea.SetActive(true);
        StartCoroutine(EnableAttack());
    }

    private IEnumerator EnableAttack()
    {
        yield return new WaitForSeconds(0.4f);
        _attackEnabled = true;
        _movementEnabled = true;
        attackArea.SetActive((false));
        _chargedAttackCollider.enabled = false;
        _chargedAttackAnimator.enabled = false;
        chargedAttack.tag = "Player";
        setAnimation("Idle");
    }

    private IEnumerator EnableGroundCheckAfterJump()
    {
        _groundCheckEnabled = false;
        yield return _wait;
        _groundCheckEnabled = true;
    }

    private void setAnimation(string name)
    {
        switch (name)
        {
            case "Attack":
                if (_animator.GetInteger("Anim") != 0)
                {
                    _animator.SetInteger("Anim", 0);
                }

                break;
            case "ChargedAttack":
                if (_animator.GetInteger("Anim") != 1)
                {
                    _animator.SetInteger("Anim", 1);
                }

                break;
            case "Dmg":
                if (_animator.GetInteger("Anim") != 2)
                {
                    _animator.SetInteger("Anim", 2);
                }

                break;
            case "Walk":
                if (_animator.GetInteger("Anim") != 3)
                {
                    _animator.SetInteger("Anim", 3);
                }

                break;
            case "Die":
                if (_animator.GetInteger("Anim") != 4)
                {
                    _animator.SetInteger("Anim", 4);
                }

                break;
            case "Jump":
                if (_animator.GetInteger("Anim") != 5)
                {
                    _animator.SetInteger("Anim", 5);
                }

                break;
            default: //Idle
                if (_animator.GetInteger("Anim") != 6)
                {
                    _animator.SetInteger("Anim", 6);
                }

                break;
        }
    }

    public void UnlockAttack()
    {
        _attackEnabled = true;
    }

    public void UnlockJump()
    {
        _jumpEnabled = true;
    }

    public void UnlockDoubleJump()
    {
        _jumpEnabled = true;
        _doubleJumpEnabled = true;
    }

    public void UnlockChargedAttack()
    {
        _chargedAttackEnabled = true;
        _attackEnabled = true;
    }

    public bool IsJumpEnabled()
    {
        return _jumpEnabled;
    }

    public bool IsDoubleJumpEnabled()
    {
        return _doubleJumpEnabled;
    }

    public bool IsChargedAttackEnabled()
    {
        return _chargedAttackEnabled;
    }

    public bool IsAttackEnabled()
    {
        return _attackEnabled;
    }

    public bool MovementEnabled
    {
        get => _movementEnabled;
        set => _movementEnabled = value;
    }
}