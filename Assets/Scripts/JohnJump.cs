using UnityEngine;

public class JohnJump : MonoBehaviour
{
    private PlayerController _johnControl;
    private Rigidbody2D _john;
    private GameObject _johnObj;


    private void Start()
    {
        var _johnObj = GameObject.Find("Human");
        _john = _johnObj.GetComponent<Rigidbody2D>();
        _johnControl = _johnObj.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Platform"))
        {
            _john.velocity = new Vector2(0, 2);
            _johnControl.Jump();
        }
    }
}