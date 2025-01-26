using UnityEngine;

public class Birth : MonoBehaviour
{
    public static Birth Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        Instance.gameObject.SetActive(false);
    }

    public void StartTheTHing()
    {
        gameObject.gameObject.SetActive(true);
    }
}
