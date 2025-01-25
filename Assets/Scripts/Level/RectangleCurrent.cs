using UnityEngine;

public class RectangleCurrent : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    private Vector2 appliedVector = Vector2.zero;
    [SerializeField] private float strength;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "AirBubble") { return; }
        //collision.attachedRigidbody.MovePosition(appliedVector * Time.deltaTime);
        collision.attachedRigidbody.AddForce(appliedVector);
    }

    void Start()
    {
        appliedVector = direction.normalized * strength;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
