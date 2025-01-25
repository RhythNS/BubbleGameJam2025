using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    [SerializeField] private Vector2 speedRange = new Vector2(0.08f, 0.08f);
    [SerializeField] private Vector2 sizeRange = new Vector2(1.0f, 1.0f);

    public float DamagePhysicsMultiplier => damagePhysicsMultiplier;
    [SerializeField] private float damagePhysicsMultiplier;
    public float DamageOnHit => damageOnHit;
    [SerializeField] private float damageOnHit;

    private float speed;
    private float size;

    private void Awake()
    {
        speed = Random.Range(speedRange.x, speedRange.y);
        size = Random.Range(sizeRange.x, sizeRange.y);

        transform.localScale = new Vector3(size, size, 1);

        gameObject.tag = "Enemy";

        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }
}
