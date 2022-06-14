using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnHelperJump : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(gameObject.CompareTag("Human"))
        {
            collision.gameObject.GetComponent<PlayerController>().PerformJump();
        }
    }
}
