using UnityEngine;

public class WhirlpoolCurrent : MonoBehaviour
{
    [SerializeField] private float whirlStrength;
    [SerializeField] private float pullStrength;
    [SerializeField] private bool positivRotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "AirBubble") { return; }
        Vector2 pull = (Vector2)transform.position - collision.attachedRigidbody.position;
        pull = pull.normalized;
        Vector2 whirl = Quaternion.AngleAxis(90, Vector3.forward) * pull;
        whirl = whirl.normalized;
        if (!positivRotation) { whirl = -whirl; }
        collision.attachedRigidbody.AddForce(whirl * whirlStrength + pull * pullStrength);
    }


}
