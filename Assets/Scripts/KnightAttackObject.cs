using UnityEngine;

public class KnightAttackObject : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    public float attackSpeed;
    public float rotation;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        attackSpeed = GameObject.Find("BodilessKnight").GetComponent<SpriteRenderer>().flipX
            ? -attackSpeed
            : attackSpeed;
    }

    void Update()
    {
        _rigidbody.velocity = new Vector2(-attackSpeed, -0.1f);
        _rigidbody.rotation += rotation;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}