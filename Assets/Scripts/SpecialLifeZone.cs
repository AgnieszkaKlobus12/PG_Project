using UnityEngine;

public class SpecialLifeZone : MonoBehaviour
{
    private bool _PlayerDown;
    private bool _HumanDown;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.gameObject.GetComponent<SetAnimatorParameter>().IsGrounded())
            {
                _PlayerDown = col.gameObject.GetComponent<Rigidbody2D>().velocity.y >= 0;
            }
            else
            {
                _PlayerDown = false;
            }
        }
        else if (col.gameObject.CompareTag("Human"))
        {
            if (col.gameObject.GetComponent<SetAnimatorParameter>().IsGrounded())
            {
                _HumanDown = col.gameObject.GetComponent<Rigidbody2D>().velocity.y >= 0;
            }
            else
            {
                _HumanDown = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }

    public bool IsPlayerDown()
    {
        return _PlayerDown;
    }

    public bool IsHumanDown()
    {
        return _HumanDown;
    }
}