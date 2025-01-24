using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovement PlayerMovement;
    private Transform playerTrans;

    private void Awake()
    {
        playerTrans = PlayerMovement.transform;
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
