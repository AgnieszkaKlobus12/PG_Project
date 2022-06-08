using System.Collections;
using UnityEngine;

public class StatueScript : MonoBehaviour
{
    public float secondsTillTurn;
    public bool flipX;
    public GameObject gem;
    public int rotationIdx;
    private bool _completed;
    private float[] _rotations = {0, 180f};

    void Start()
    {
        StartCoroutine(turn());
        _completed = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<SpriteRenderer>().flipX == flipX && !_completed)
        {
            gem.GetComponent<GemScript>().Completed();
            _completed = true;
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<SpriteRenderer>().flipX == flipX && !_completed)
        {
            gem.GetComponent<GemScript>().Completed();
            _completed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_completed)
        {
            _completed = false;
            gem.GetComponent<GemScript>().Failed();
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