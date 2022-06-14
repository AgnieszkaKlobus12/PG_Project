using UnityEngine;

public class KnightAttackObject : MonoBehaviour
{
    public float timeToDestroy;
    private Rigidbody2D _rigidbody;
    public float attackSpeed;
    public float rotation;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        attackSpeed = GameObject.Find("BodilessKnight").GetComponent<SpriteRenderer>().flipX
            ? -attackSpeed
            : attackSpeed;
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        _rigidbody.velocity = new Vector2(-attackSpeed, -0.1f);
        _rigidbody.rotation += rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Human") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
    }
}