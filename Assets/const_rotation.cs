using UnityEngine;

public class const_rotation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    
    void Update()
    {
        float rotationsPerMinute = 10;
        transform.Rotate(0,0, 6.0F * rotationsPerMinute * Time.deltaTime);
    }
}
