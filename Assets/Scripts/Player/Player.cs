using UnityEngine;

public class Player : MonoBehaviour
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

    public float upSpeed;
    public float horizontalMaxSpeed;
    public float horizontalMinSpeed;
    public float horizontalAcceleration;
    public float horizontalDeceleration;
    public Rigidbody2D rb;
    [SerializeField] private CameraMovement camMovement;
    

    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    public KeyCode lookRight = KeyCode.RightArrow;
    public KeyCode lookDown = KeyCode.DownArrow;
    public KeyCode lookLeft = KeyCode.LeftArrow;
    public KeyCode lookUp = KeyCode.UpArrow;

    private Vector2 currentPos = Vector2.zero;
    private Vector2 moveVec = Vector2.zero;
    private Vector3 playerStartPos = Vector3.zero;
    private float horizontalMove = 0f;
    private Vector3 baseScale;
    private Vector3 direction = Vector3.up;

    void Start()
    {
        playerStartPos = transform.position;
        baseScale = transform.localScale;
        Health = baseHealth;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Health -= 10f;
        }
        Grow();
        LookDirection();
    }

    private void LookDirection()
    {
        Vector3 nextDirection = Vector3.zero;
        if (Input.GetKey(lookRight))
        {
            nextDirection += Vector3.right;
        }
        if (Input.GetKey(lookDown))
        {
            nextDirection += Vector3.down;
        }
        if (Input.GetKey(lookLeft))
        {
            nextDirection += Vector3.left;
        }
        if (Input.GetKey(lookUp))
        {
            nextDirection += Vector3.up;
        }
        if (nextDirection != direction && nextDirection != Vector3.zero)
        {
            direction = nextDirection;
        }
        rb.rotation = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
    }

    private void Grow()
    {
        Health += healthGroth * Time.deltaTime;
    }

    private void Movement()
    {
        currentPos = transform.position;
        moveVec = Vector2.zero;

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

    public void Kill()
    {
        transform.position = playerStartPos;
        camMovement.transform.position = playerStartPos + Vector3.down * camMovement.targetOffset + camMovement.transform.position.z * Vector3.forward;
        Health = baseHealth;
    }
}
