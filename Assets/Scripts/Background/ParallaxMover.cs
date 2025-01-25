using UnityEngine;

public class ParallaxMover : MonoBehaviour
{
    [SerializeField]
    private Transform toTrack;

    [SerializeField]
    private float parallaxFactor = 0.5f;

    private Vector3 trackStartPosition;
    private Vector3 ownStartPosition;
    private float atZ = 0.0f;

    private void Awake()
    {
        if (toTrack == null)
        {
            toTrack = Camera.main.transform;
        }

        atZ = transform.position.z;
        trackStartPosition = toTrack.position; 
        ownStartPosition = transform.position;
    }

    private void Update()
    {
        Vector3 delta = toTrack.position - trackStartPosition;
        delta.z = atZ;
        transform.position = ownStartPosition + delta * parallaxFactor;
    }
}
