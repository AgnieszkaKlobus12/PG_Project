using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustJumpJohn : MonoBehaviour
{
    private PlayerController _johnControl;
    private Rigidbody2D _john;


    private void Start()
    {
        var _johnHelp = GameObject.Find("Human");
        _john = _johnHelp.GetComponent<Rigidbody2D>();
        _johnControl = _johnHelp.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Human"))
        {
            _john.velocity = new Vector2(0, 2);
            _johnControl.Jump();
        }
    }
}
