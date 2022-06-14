using System.Collections;
using UnityEngine;

public class StatueScript : MonoBehaviour
{
    public float secondsTillTurn;
    public bool flipX;
    public GameObject gem;
    public int rotationIdx;
    private bool _completed;
    private float[] _rotations = { 0, 180f };
    private Settings Settings;
    private SpriteRenderer _spriteRenderer;
    private bool _perm;
    private BoxCollider2D _collider;

    void Start()
    {
        _perm = false;
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Settings = new Settings();
        StartCoroutine(turn());
        _completed = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<SpriteRenderer>().flipX == flipX && !_completed)
        {
            gem.GetComponent<GemScript>().Completed();
            _completed = true;
            if (Settings.GetMode(PlayerPrefs.GetInt("Slot")) == "multiplayer" ||
                Settings.GetMode(PlayerPrefs.GetInt("Slot")) == "empty" && other.CompareTag("Player"))
            {
                _perm = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_completed && !_perm)
        {
            _completed = false;
            gem.GetComponent<GemScript>().Failed();
        }
    }

    IEnumerator turn()
    {
        yield return new WaitForSeconds(secondsTillTurn);
        rotationIdx = (rotationIdx + 1) % 2;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
        _collider.transform.localPosition =
            _spriteRenderer.flipX ? new Vector3(-1f, 0f) : new Vector3(1f, 0f);
        transform.rotation = new Quaternion(transform.rotation.x, _rotations[rotationIdx], transform.rotation.z, 1);
        StartCoroutine(turn());
    }
}