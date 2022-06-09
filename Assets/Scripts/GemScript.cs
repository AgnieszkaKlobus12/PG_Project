using System.Collections;
using UnityEngine;

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

    IEnumerator Show()
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
        if (_showing && (other.CompareTag("Player") || other.CompareTag("Human")))
        {
            other.GetComponent<SetAnimatorParameter>().UnlockJump();
        }
    }
}