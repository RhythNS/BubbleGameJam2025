using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float upSpeed;
    public float horizontalMaxSpeed;
    public float horizontalMinSpeed;
    public float horizontalAcceleration;
    public float horizontalDeceleration;
    public Rigidbody2D rb;

    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    private Vector2 currentPos = Vector2.zero;
    private Vector2 moveVec = Vector2.zero;
    private float horizontalMove = 0f;


    void Start()
    {
        
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
        return Vector2.up * upSpeed;
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
}
