using UnityEngine;

public class CreditsTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelLoader.Instance.DeleteAllBackgrounds();
        //GameManager.Instance.GradientBackground.Credits();
        GameManager.Instance.MusicHandler.DoStop();
        GameManager.Instance.MusicHandler.TriggerEndThings();
    }

}
