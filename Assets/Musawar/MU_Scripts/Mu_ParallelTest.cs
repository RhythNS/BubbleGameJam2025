using UnityEngine;

public class Mu_ParallelTest : MonoBehaviour
{
    [SerializeField] GameObject camObject;
    [SerializeField] float parSpeed = 2f;

    //local variables
    private float length;
    private float startPos;

    private void Awake()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (camObject.transform.position.x * (1 - parSpeed));
        float dist = (camObject.transform.position.x * parSpeed);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
        
    }
}
