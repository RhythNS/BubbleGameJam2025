using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] EventReference enemySound;
    [SerializeField] private Vector2 speedRange = new Vector2(0.08f, 0.08f);
    [SerializeField] private Vector2 sizeRange = new Vector2(1.0f, 1.0f);

    public float DamageOnCollision => damageOnCollision;
    [SerializeField] private float damageOnCollision;
    public float DamageOnHurtbox => damageOnHurtbox;
    [SerializeField] private float damageOnHurtbox;

    private float speed;
    private float size;

    private void Awake()
    {
        speed = Random.Range(speedRange.x, speedRange.y);
        size = Random.Range(sizeRange.x, sizeRange.y);

        transform.localScale = new Vector3(size, size, 1);

        gameObject.tag = "Enemy";

        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void PlayEnemySound()
    { 
        if (enemySound.Path.Length > 0)
        {
            Debug.Log("Played Enemy Death sound");
            FMODUnity.RuntimeManager.PlayOneShot(enemySound);
        }
        else
        {
            Debug.Log("It's empty");
        }
    }
}
