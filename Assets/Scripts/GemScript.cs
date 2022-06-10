using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    public bool[] correct;

    private int _nextIndex;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _finalPosition;
    private bool _showing;
    private bool[] _touching = { false, false };

    void Awake()
    {
        _nextIndex = 0;
        _showing = false;
        _finalPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        transform.position += Vector3.up * 2; //tp nie działa
    }

    void Update()
    {
        if (correct[correct.Length - 1] && !_showing)
        {
            _spriteRenderer.enabled = true;
            _showing = true;
            StartCoroutine(Show());
        }

        if (_touching[0] && _touching[1])
        {
            //load next level
            Debug.Log("next");
        }
    }

    IEnumerator Show()
    {
        while (transform.position != _finalPosition)
        {
            transform.position -= Vector3.up * 0.1f;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Show());
    }

    public void Completed()
    {
        correct[_nextIndex++] = true;
    }

    public void Failed()
    {
        if (_nextIndex > 0)
        {
            correct[--_nextIndex] = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_showing && other.CompareTag("Player"))
        {
            _touching[0] = true;
        }
        if (_showing && other.CompareTag("Human"))
        {
            _touching[01] = true;
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (_showing && other.CompareTag("Player"))
        {
            _touching[0] = true;
        }
        if (_showing && other.CompareTag("Human"))
        {
            _touching[01] = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_showing && other.CompareTag("Player"))
        {
            _touching[0] = false;
        }
        if (_showing && other.CompareTag("Human"))
        {
<<<<<<< Updated upstream
            _touching[01] = false;
=======
            case "Level 5":
                // Final
                break;
            case "Level 4":
                other.GetComponent<PlayerController>().UnlockChargedAttack();
                break;
            case "Level 3":
                other.GetComponent<PlayerController>().UnlockDoubleJump();
                break;
            case "Level 2":
                other.GetComponent<PlayerController>().UnlockAttack();
                break;
            case "Level 1":
                other.GetComponent<PlayerController>().UnlockJump();
                break;
>>>>>>> Stashed changes
        }
    }
}