using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Player PlayerMovement;
    public Camera cam;


    public float targetOffset;
    public float maxSlowDownPercent;
    public float maxSpeedUpPercent;
    public float maxDistUp;
    public float maxDistDown;

    private Transform playerTrans;

    private float cameraSpeed;

    private void Awake()
    {
        playerTrans = PlayerMovement.transform;
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Movement();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    private void Movement()
    {
        cameraSpeed = PlayerMovement.upSpeed;
        float playerDistVert = playerTrans.position.y - transform.position.y;


        if (playerDistVert > targetOffset)
        {
            cameraSpeed *= 1 + Mathf.Lerp(0f, maxSpeedUpPercent, Mathf.Clamp01(Mathf.Abs(playerDistVert - targetOffset) / maxDistUp));
        }
        else
        {
            cameraSpeed *= 1 - Mathf.Lerp(0f, maxSlowDownPercent, Mathf.Clamp01(Mathf.Abs(playerDistVert - targetOffset) / maxDistDown));
        }

        transform.position += Vector3.up * cameraSpeed;
    }

    public Bounds OrthographicBounds()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = cam.orthographicSize * 2;
        Bounds bounds = new Bounds(
            cam.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}
