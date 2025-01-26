using FMODUnity;
using UnityEngine;

public class MenuBubbleSounds_Test : MonoBehaviour
{
    [SerializeField] EventReference eventReference;

    public void PlayBubbleMenuSounds()
    {
        FMODUnity.RuntimeManager.PlayOneShot(eventReference);
    }

    public void StopEventReference()
    {

    }
}
