using UnityEngine;

public class GoJohn : MonoBehaviour
{
    private PlayerController _johnControl;
    private Rigidbody2D _john;
    private GameObject _johnObj;
    public bool up;
    public bool platform;

    private void Start()
    {
        var _johnObj = GameObject.Find("Human");
        _john = _johnObj.GetComponent<Rigidbody2D>();
        _johnControl = _johnObj.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Human"))
        {
            if (_johnControl.mode == 1)
            {
                _johnControl.mode = 3;
                if (up)
                {
                    _john.velocity = new Vector2(_johnControl.speed, _john.velocity.y);
                    // _johnObj.transform.position = Vector2.MoveTowards(_johnObj.transform.position,
                    //     new Vector2(_johnObj.transform.position.x + 5f, _johnObj.transform.position.y+5f),
                    //     _johnControl.speed * Time.deltaTime);
                }
                else
                {
                    // _johnObj.transform.position = Vector2.MoveTowards(_johnObj.transform.position,
                    //     new Vector2(_johnObj.transform.position.x + 10f, _johnObj.transform.position.y),
                    //     _johnControl.speed * Time.deltaTime);
                    _john.velocity = new Vector2(_johnControl.speed, _john.velocity.y);
                }
                if (_john.velocity.x > 0)
                {
                    _johnControl.setAnimation("Walk");
                    _johnControl._spriteRenderer.flipX = false;
                }
                else if (_john.velocity.x < 0)
                {
                    _johnControl.setAnimation("Walk");
                    _johnControl._spriteRenderer.flipX = true;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!platform)
        {
            OnTriggerEnter2D(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_johnControl.mode == 3)
        {
            _johnControl.mode = 1;
        }
    }
}