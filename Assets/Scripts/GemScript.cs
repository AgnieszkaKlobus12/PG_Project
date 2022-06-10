using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GemScript : MonoBehaviour
{
    public bool[] correct;

    private int _nextIndex;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _finalPosition;
    private bool _showing;

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
    }

    public IEnumerator Show()
    {
        while (transform.position != _finalPosition)
        {
            transform.position -= Vector3.up * 0.1f;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Show());
        }
    }

    public void Completed()
    {
        if (_nextIndex < correct.Length)
        {
            correct[_nextIndex++] = true;
        }
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
        if (!_showing || (!other.CompareTag("Player") && !other.CompareTag("Human"))) return;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level 5":
                // Final
                break;
            case "Level 4":
                other.GetComponent<SetAnimatorParameter>().UnlockChargedAttack();
                break;
            case "Level 3":
                other.GetComponent<SetAnimatorParameter>().UnlockDoubleJump();
                break;
            case "Level 2":
                other.GetComponent<SetAnimatorParameter>().UnlockAttack();
                break;
            case "Level 1":
                other.GetComponent<SetAnimatorParameter>().UnlockJump();
                break;
        }
    }
}