using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public EventReference musicEvent;

    [SerializeField] private AnimationCurve curve;

    bool isValid = false;
    private EventInstance musicInstance;

    public void DoStart()
    {
        if (isValid)
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
    }

    private void Update()
    {
        if (!isValid)
        {
            return;
        }

        float thing = GameManager.Instance.LevelLoader.GetThingForGradient();
        
        float value = curve.Evaluate(thing);

        musicInstance.setParameterByName("Gradient", value);
    }

    public void DoStop()
    {
        if (isValid)
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        isValid = false;
    }
}
