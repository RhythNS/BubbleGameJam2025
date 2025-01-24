using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float upSpeed;
    public float horizontalMaxSpeed;
    public float horizontalMinSpeed;
    public float horizontalAcceleration;
    public float horizontalDeceleration;
    public Rigidbody2D rb;
    [SerializeField] private CameraMovement camMovement;
    

    public const KeyCode moveLeftKey = KeyCode.A;
    public const KeyCode moveRightKey = KeyCode.D;

    private Vector2 currentPos = Vector2.zero;
    private Vector2 moveVec = Vector2.zero;
    private Vector3 playerStartPos = Vector3.zero;
    private float horizontalMove = 0f;

    void Start()
    {
        playerStartPos = transform.position;
    }

    private void FixedUpdate()
    {
        currentPos = transform.position;
        moveVec = Vector2.zero;

        moveVec += DefaultMovement();
        moveVec += ControledMovement();

        rb.MovePosition(currentPos + moveVec);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Movement()
    {

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

        if (Input.GetKey(KeyCode.Space)) { return Vector2.up * upSpeed * 1.5f; }
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
    }
}
