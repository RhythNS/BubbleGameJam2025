using UnityEngine;
using UnityEngine.VFX;

public class BlowBubble : MonoBehaviour
{
    [SerializeField] private GameObject blowBubblePrefab;
    [SerializeField] private float defaultUpVelocity;
    [SerializeField] private float lifespan;
    [HideInInspector] public Vector2 velocity;
    [HideInInspector] public bool consumed = false;
    private float _health;

    [SerializeField] private float healthSizeRatio = 1f;
    [HideInInspector]
    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            float scaleX = Mathf.Sqrt(_health / healthSizeRatio);
            transform.localScale = new Vector3(scaleX, scaleX, 1);
        }
    }
    public float horizontalDecceleration;
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(velocity, Space.World);
        velocity *= horizontalDecceleration;
        velocity.y = defaultUpVelocity;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        lifespan -= Time.deltaTime;
        if (lifespan < 0) { Destroy(gameObject); }
        // Evil Hack do not delete 
        consumed = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            default:
                Destroy(gameObject);
                break;
            case "AirBubble":
                BlowBubble otherBubble = collision.gameObject.GetComponent<BlowBubble>();
                if (consumed || otherBubble.consumed) { return; }
                consumed = true;
                otherBubble.consumed = true;

                GameObject bubble = Instantiate(blowBubblePrefab, transform.position, Quaternion.identity);
                BlowBubble blowBubble = bubble.GetComponent<BlowBubble>();
                blowBubble.velocity = velocity + otherBubble.velocity;
                blowBubble.Health = Health + otherBubble.Health;
                Destroy(collision.gameObject);
                Destroy(gameObject);
                break;
            case "Player":
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.Health += Health;
                Destroy(gameObject);
                break;
        }
    }

}
