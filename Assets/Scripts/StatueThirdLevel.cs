using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueThirdLevel : MonoBehaviour
{
    public float secondsTillTurn;
    public GameObject target;
    public int rotationIdx;
    private bool _completed;
    private readonly float[] _rotations = { 0, 180f };

    void Start()
    {
        StartCoroutine(turn());
        _completed = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Animator>().GetInteger("Anim") == 0 && !_completed)
        {
            if (target.CompareTag("Gem"))
            {
                target.GetComponent<GemScript>().Completed();
            }
            else
            {
                target.GetComponent<CompleteThirdPuzzle>().Completed();
            }
            _completed = true;
        }
    }


    IEnumerator turn()
    {
        yield return new WaitForSeconds(secondsTillTurn);
        rotationIdx = (rotationIdx + 1) % 2;
        transform.rotation = new Quaternion(transform.rotation.x, _rotations[rotationIdx], transform.rotation.z, 1);
        StartCoroutine(turn());
    }
}