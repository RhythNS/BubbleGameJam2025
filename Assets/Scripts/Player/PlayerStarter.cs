using System.Collections;
using UnityEngine;

public class PlayerStarter : MonoBehaviour
{
    [SerializeField] private float timePlayOther;
    private Animation animation1;
    private Animation animation2;
    private CameraMovement movement;

    private void Start()
    {
        animation1 = GameManager.Instance.Player.GetComponent<Animation>();
        animation2 = Camera.main.GetComponent<Animation>();
        movement = Camera.main.GetComponent<CameraMovement>();

        DoTheThing();
    }

    public void DoTheThing()
    {
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        PlayerController player = GameManager.Instance.Player;
        Whale whale = GameManager.Instance.Whale;
        player.transform.position = whale.StartLocation.position;
        player.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);

        animation2.Play();

        yield return new WaitForSeconds(timePlayOther);
        movement.enabled = true;

        animation1.Play();

        while (animation1.isPlaying)
        {
            yield return null;
        }

        Debug.Log("Done");
        GameManager.Instance.SwitchToGame();
    }
}
