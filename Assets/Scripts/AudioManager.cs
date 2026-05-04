using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public EventReference click;
    public EventReference move;
    public EventReference die;
    public StudioEventEmitter musicEmitter;

    public static AudioManager GetInstance()
    {
        return instance;
    }

    public void Awake()
    {
        
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void PlayClickSound()
    {
        RuntimeManager.PlayOneShot(click);
    }

    public void PlayMoveSound()
    {
        RuntimeManager.PlayOneShot(move);
    }

    public void PlayDieSound()
    {
        RuntimeManager.PlayOneShot(die);
    }

    public void PlayMetaMusic()
    {
        musicEmitter.SetParameter("ParamGameplay", 0);
    }

    public void PlayCoreMusic()
    {
        musicEmitter.SetParameter("ParamGameplay", 1);
    }
}
