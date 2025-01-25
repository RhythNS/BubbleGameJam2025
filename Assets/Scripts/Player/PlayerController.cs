using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxHealth;
    public float maxSizeMult;
    public float minSizeMult;

    [HideInInspector] public float Health 
    { 
        get { return _health; }
        set 
        { 
            if (value < 0) { _health = 0; Kill(); }
            else if (value > maxHealth) { _health = maxHealth; }
            else { _health = value; }

            float mult = Mathf.Lerp(minSizeMult, maxSizeMult, _health / maxHealth);
            transform.localScale = baseScale * mult;
        }
    }
    private float _health;

    public float healthGroth;
    public float baseHealth;

    public float minHealthToBlowAir;
    public float blowAirSpeed;
    public float boostMaxSpeed;
    public float blowAirDisableTime;
    public float airLossPercent;
    public float airBubbleMinHealth;
    private float airBubbleCounter = 0;
    private float blowAirDisableTimer;
    private Vector2 boostVelocity;
    public float boostAccelertation;
    public float boostDeccelertation;

    public float invurnerableTime = 1f;
    private float invurnerableTimer = 0f;
    public float pushBackForce = 10f;


    public float upSpeed;
    public float horizontalMaxSpeed;
    public float horizontalMinSpeed;
    public float horizontalAcceleration;
    public float horizontalDeceleration;
    public float rotationSpeed;
    public Rigidbody2D rb;
    private CameraMovement camMovement;
    

    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    public KeyCode lookRightKey = KeyCode.RightArrow;
    public KeyCode lookDownKey = KeyCode.DownArrow;
    public KeyCode lookLeftKey = KeyCode.LeftArrow;
    public KeyCode lookUpKey = KeyCode.UpArrow;

    public KeyCode blowAirKey = KeyCode.Space;

    private Vector2 currentPos = Vector2.zero;
    private Vector2 moveVec = Vector2.zero;
    private Vector3 playerStartPos = Vector3.zero;
    private float horizontalMove = 0f;
    private Vector3 baseScale;
    private Vector3 direction = Vector3.up;

    [SerializeField] private GameObject blowBubble;

    private void Awake()
    {
        camMovement = Camera.main.GetComponent<CameraMovement>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Health -= 10f;
        }

        if (blowAirDisableTimer > 0)
        {
            blowAirDisableTimer -= Time.deltaTime;
        }
        if (invurnerableTimer > 0)
        {
            invurnerableTimer -= Time.deltaTime;
        }

        Grow();
        LookDirection();
    }

    public void Deactivate()
    {
        enabled = false;
        rb.simulated = false;
    }

    public void Activate()
    {
        enabled = true;
        rb.simulated = true;
        baseScale = transform.localScale;
        Health = baseHealth;
    }

    private void LookDirection()
    {
        Vector3 nextDirection = Vector3.zero;
        if (Input.GetKey(lookRightKey))
        {
            nextDirection += Vector3.right;
        }
        if (Input.GetKey(lookDownKey))
        {
            nextDirection += Vector3.down;
        }
        if (Input.GetKey(lookLeftKey))
        {
            nextDirection += Vector3.left;
        }
        if (Input.GetKey(lookUpKey))
        {
            nextDirection += Vector3.up;
        }
        if (nextDirection == Vector3.zero)
        {
            return;
        }
        direction = nextDirection;

        float currentRotation = rb.rotation;
        float targetRotation = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
        if (targetRotation < 0) { targetRotation = (180 - Mathf.Abs(targetRotation)) + 180; }
        float rotationDiff = targetRotation - currentRotation;
        if (Mathf.Abs(rotationDiff) < 3)
        {
            return;
        }
        int sign = 1;
        if ((rotationDiff < 0 || rotationDiff > 180) && rotationDiff > -180)
        {
            sign = -1;
        }
        rb.rotation = currentRotation + sign * rotationSpeed * Time.deltaTime;
        if (rb.rotation < 0) {  rb.rotation += 360; }
        else if (rb.rotation > 360) { rb.rotation -= 360;}
    }

    private void Grow()
    {
        Health += healthGroth * Time.deltaTime;
    }

    private void Movement()
    {
        currentPos = transform.position;
        moveVec = Vector2.zero;

        moveVec += BlowAir();
        moveVec += DefaultMovement();
        moveVec += ControledMovement();


        rb.MovePosition(currentPos + moveVec);
    }

    private Vector2 DefaultMovement()
    {
        Bounds bounds = camMovement.OrthographicBounds();
        Vector2 horizontalCorrection = Vector2.zero;
        if (transform.position.x > camMovement.transform.position.x + bounds.extents.x)
        { 
            horizontalCorrection = new Vector2(camMovement.transform.position.x + bounds.extents.x, transform.position.y) - rb.position; 
        }
        else if (transform.position.x < camMovement.transform.position.x - bounds.extents.x)
        {
            horizontalCorrection = new Vector2(camMovement.transform.position.x - bounds.extents.x, transform.position.y) - rb.position;
        }

        if (transform.position.y < camMovement.transform.position.y - bounds.extents.y) { Kill(); }

        
        return Vector2.up * upSpeed + horizontalCorrection;

    }

    private Vector2 ControledMovement()
    {
        int horizontalDirection = 0;
    
        if (Input.GetKey(moveRightKey)) 
        { 
            horizontalDirection += 1; 
      
        }
        if (Input.GetKey(moveLeftKey)) 
        { 
            horizontalDirection -= 1; 
        }

        if (horizontalDirection == 0) 
        {
            horizontalMove *= horizontalDeceleration;
            if (Mathf.Abs(horizontalMove) < horizontalMinSpeed) { horizontalMove = 0; } 
        }
        else
        {
            horizontalMove += horizontalDirection * horizontalAcceleration;
            horizontalMove = Mathf.Abs(horizontalMove) > horizontalMaxSpeed ? horizontalMaxSpeed * horizontalDirection : horizontalMove;
        }

        return Vector2.right * horizontalMove;
    }

    private Vector2 BlowAir()
    {
        if (blowAirDisableTimer > 0 || !Input.GetKey(blowAirKey))
        {
            boostVelocity *= boostDeccelertation;
            return boostVelocity;
        }
        if (Health <= minHealthToBlowAir)
        {
            blowAirDisableTimer = blowAirDisableTime;
            return boostVelocity;
        }
        Health -= blowAirSpeed;

        Vector2 boostDir = Quaternion.AngleAxis(rb.rotation, Vector3.back) * Vector2.up;
        boostDir = new Vector2(boostDir.x, -boostDir.y);

        boostVelocity += boostDir * boostAccelertation;
        if (boostVelocity.magnitude > boostMaxSpeed)
        {
            boostVelocity = boostVelocity.normalized * boostMaxSpeed;
        }

        airBubbleCounter += blowAirSpeed * (1f - airLossPercent);
        if (airBubbleCounter >= airBubbleMinHealth)
        {
            airBubbleCounter = 0;

            Vector3 bubbleSpawnPos = transform.position + new Vector3(-boostDir.x, -boostDir.y, 0) * transform.localScale.x * 2.5f;
            GameObject bubble = Instantiate(blowBubble, bubbleSpawnPos, Quaternion.identity);
            BlowBubble bubbleBehavoiur = bubble.GetComponent<BlowBubble>();

            bubbleBehavoiur.velocity = -boostVelocity;
            bubbleBehavoiur.Health = airBubbleMinHealth;
        }

        return boostVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (invurnerableTimer > 0) { return; }
        if (collision.gameObject.tag != "Enemy") { return; }

        invurnerableTimer = invurnerableTime;
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        Health -= enemy.DamagePhysicsMultiplier;
        Vector2 forceDir = rb.position - collision.otherRigidbody.position;
        forceDir = forceDir.normalized;
        rb.AddForce(forceDir * pushBackForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (invurnerableTimer > 0) { return; }
        if (collision.gameObject.tag != "Enemy") { return; }

        invurnerableTimer = invurnerableTime;
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        Health -= enemy.DamagePhysicsMultiplier;
        Vector2 forceDir = rb.position - (Vector2)collision.transform.position;
        forceDir = forceDir.normalized;
        rb.AddForce(forceDir * pushBackForce);
    }

    public void Kill()
    {
        Debug.Log("Player Died");
        GameManager.Instance.SwitchToGameOver();
    }
}
