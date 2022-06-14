using System.Collections;
using UnityEngine;

public class EasyEnemyController : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("UserAttack"))
        {
            _animator.SetInteger("Action", 2);
        }
        if (col.CompareTag("Player") || col.CompareTag("Human"))
        {
            var playerAct = col.GetComponent<Animator>().GetInteger("Anim");
            if (playerAct == 0 || playerAct == 1)
            {
                _animator.SetInteger("Action", 2);
            }
            else
            {
                _animator.SetInteger("Action", 1);
                col.GetComponent<PlayerController>().Die();
            }
        }
    }

    private IEnumerator OnTriggerExit2D(Collider2D other)
    {
        yield return new WaitForSeconds(0.5f);
        _animator.SetInteger("Action", 0);
    }
}