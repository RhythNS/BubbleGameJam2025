using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.GradientBackground.Finish();
    }

}
