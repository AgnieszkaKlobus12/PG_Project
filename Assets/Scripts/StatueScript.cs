using System.Collections;
using UnityEngine;

public class StatueScript : MonoBehaviour
{
    public float secondsTillTurn;
    public bool flipX;
    public GameObject gem;
    public int rotationIdx;
    public Sprite activeSprite;
    private bool _completed;
    private Settings Settings;
    private SpriteRenderer _spriteRenderer;
    private bool _perm;
    public BoxCollider2D leftCollider, rightCollider;
    private Sprite inactiveSprite;

    void Start()
    {
        _perm = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Settings = new Settings();
        _completed = false;
        inactiveSprite = _spriteRenderer.sprite;
        StartCoroutine(turn());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<SpriteRenderer>().flipX == flipX && !_completed)
        {
            gem.GetComponent<GemScript>().Completed();
            _completed = true;
            _spriteRenderer.sprite = activeSprite;
            if (Settings.GetMode(PlayerPrefs.GetInt("Slot")) != "multiplayer" &&
                (other.CompareTag("Player") || other.CompareTag("Human")))
            {
                _perm = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_completed && !_perm)
        {
            _spriteRenderer.sprite = inactiveSprite;
            _completed = false;
            gem.GetComponent<GemScript>().Failed();
        }
    }

    IEnumerator turn()
    {
        yield return new WaitForSeconds(secondsTillTurn);
        rotationIdx = (rotationIdx + 1) % 2;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
        if (flipX)
        {
            if (_spriteRenderer.flipX)
            {
                leftCollider.enabled = true;
                rightCollider.enabled = false;
            }
            else
            {
                leftCollider.enabled = false;
                rightCollider.enabled = true;
            }
        }
        else
        {
            if (_spriteRenderer.flipX)
            {
                leftCollider.enabled = false;
                rightCollider.enabled = true;
            }
            else
            {
                leftCollider.enabled = true;
                rightCollider.enabled = false;
            }
        }

        StartCoroutine(turn());
    }
}