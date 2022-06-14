using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GemScript : MonoBehaviour
{
    public bool[] correct;

    private int _nextIndex;
    protected SpriteRenderer _spriteRenderer;
    private Vector3 _finalPosition;
    protected bool _showing;
    protected List<int> order;

    void Awake()
    {
        order = new List<int>();
        _nextIndex = 0;
        _showing = false;
        _finalPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        transform.position += Vector3.up * 2;
    }

    protected virtual void Update()
    {
        if (correct.Length == 0 || correct[correct.Length - 1] && !_showing)
        {
            _spriteRenderer.enabled = true;
            _showing = true;
            StartCoroutine(Show());
        }
    }

    protected IEnumerator Show()
    {
        while (transform.position != _finalPosition)
        {
            transform.position -= Vector3.up * 0.1f;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Show());
        }
    }

    public virtual void Completed(int ind = -1)
    {
        if (ind == -1)
        {
            ind = _nextIndex++;
        }
        if (ind < correct.Length)
        {
            correct[ind] = true;
        }
    }

    public virtual void Failed(int ind = -1)
    {
        if (ind == -1)
        {
            ind = _nextIndex--;
        }

        if (ind < correct.Length)
        {
            correct[ind] = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_showing || (!other.CompareTag("Player") && !other.CompareTag("Human"))) return;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level 5":
                // TODO win screen, same as in player controller
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
        }
    }
}