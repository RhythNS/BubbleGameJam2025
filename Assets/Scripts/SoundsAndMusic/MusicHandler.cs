using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
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

        isValid = true;

        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();

        Debug.Log("Start");
    }

    private void Update()
    {
        if (!isValid)
        {
            return;
        }

        float thing = GameManager.Instance.LevelLoader.GetThingForGradient();
        float value = curve.Evaluate(thing);

        musicInstance.setParameterByName("TrackSelector", value);
    }

    public void DoStop()
    {
        if (isValid)
        {
            //musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        isValid = false;
        Debug.Log("Stop");
    }

    public EventReference[] toPlayEvents;
    private List<EventInstance> toPlayInstances = new();

    public void TriggerEndThings()
    {
        Debug.Log("TriggerEndThings");
        toPlayInstances.ForEach(x => x.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT));
        toPlayInstances.Clear();

        for (int i = 0; i < toPlayEvents.Length; i++)
        {
            EventInstance musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
            musicInstance.start();
            toPlayInstances.Add(musicInstance);
        }
    }

    public void DeTriggerEndThings()
    {
        Debug.Log("DeTriggerEndThings");
        toPlayInstances.ForEach(x => x.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT));
        toPlayInstances.Clear();
    }
}
