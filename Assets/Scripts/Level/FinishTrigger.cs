using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.FinishGame();
        GameManager.Instance.MusicHandler.DoStop();
        GameManager.Instance.MusicHandler.DeTriggerEndThings();
    }
}
