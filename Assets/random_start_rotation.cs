using UnityEngine;

public class random_start_rotation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomFloat = Random.value * 360.0F;
        transform.Rotate(0, 0, 6.0F * randomFloat * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
