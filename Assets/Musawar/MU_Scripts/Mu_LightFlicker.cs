using System.Collections;
using UnityEngine;

public class Mu_LightFlicker : MonoBehaviour
{
    [SerializeField] Light lightA;
    [SerializeField] float interval = 1f;

    //local
    private float timer;

    private void Awake()
    {
        lightA.enabled = true;
    }


    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > interval)
        {
            lightA.enabled = !lightA.enabled;
            interval = Random.Range(0f, 1f);
            timer = 0f;
        }
    }
}
