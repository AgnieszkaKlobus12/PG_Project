using System;
using System.Collections;
using UnityEngine;

public class PuzzleGem : MonoBehaviour
{
    public GameObject gem;
    private bool _completed;
    public int number;
    private Animator _animator;

    private void Start()
    {
        _completed = false;
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Human")) && !_completed)
        {
            if (other.gameObject.GetComponent<Animator>().GetInteger("Anim") < 2)
            {
                StartCoroutine(Complete());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }

    private IEnumerator Complete()
    {
        _completed = true;
        _animator.enabled = true;
        gem.GetComponent<GemAdvanced>().Completed(number);
        yield return new WaitForSeconds(5f);
        gem.GetComponent<GemAdvanced>().Failed(number);
        _completed = false;
        _animator.enabled = false;
    }
}
