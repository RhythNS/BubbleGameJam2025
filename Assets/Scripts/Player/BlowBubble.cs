using UnityEngine;
using UnityEngine.VFX;

public class BlowBubble : MonoBehaviour
{
    [HideInInspector] public Vector2 velocity;
    [HideInInspector] public float health;
    public float horizontalDecceleration;
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(velocity, Space.World);
        velocity *= horizontalDecceleration;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            default:
                Destroy(this);
                break;
            case "AirBubble":

                break;
            case "Player":
                break;
        }
    }


}
