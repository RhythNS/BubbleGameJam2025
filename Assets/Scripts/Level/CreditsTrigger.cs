using UnityEngine;

public class CreditsTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.GradientBackground.Credits();
    }

}
