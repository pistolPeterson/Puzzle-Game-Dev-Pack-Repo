using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Music system taken from Unity documentation: https://docs.unity3d.com/ScriptReference/AudioSource.PlayScheduled.html
/// Uses the MusicProgress Script in order to decide what music to switch to every 4 bars. 
/// </summary>
public class AudioExample : MonoBehaviour
{
    public float bpm = 120.0f;
    public int numBeatsPerSegment = 16;
    public AudioClip[] clips = new AudioClip[2];

    private double nextEventTime;
    private int flip = 0;
    private AudioSource[] audioSources = new AudioSource[2];
    private bool running = false;

    private MusicProgress musicProgress; 
    void Start()
    {
        musicProgress = GetComponent<MusicProgress>();
        if (musicProgress == null) Debug.Log("No Music Progress Script in the gameobject!");
        for (int i = 0; i < 2; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;
            audioSources[i] = child.AddComponent<AudioSource>();
        }

        nextEventTime = AudioSettings.dspTime + 0.0f;
        running = true;
    }

    void Update()
    {
        if (!running)
        {
            return;
        }

        double time = AudioSettings.dspTime;

        if (time + 1.0f > nextEventTime)
        {
            // We are now approx. 1 second before the time at which the sound should play,
            // so we will schedule it now in order for the system to have enough time
            // to prepare the playback at the specified time. This may involve opening
            // buffering a streamed file and should therefore take any worst-case delay into account.
            // audioSources[flip].clip = clips[flip];
            audioSources[flip].clip = musicProgress.GetCurrentClipForLevel();
            audioSources[flip].PlayScheduled(nextEventTime);


            // Place the next event 16 beats from here at a rate of 140 beats per minute
            nextEventTime += 60.0f / bpm * numBeatsPerSegment;

            // Flip between two audio sources so that the loading process of one does not interfere with the one that's playing out
            flip = 1 - flip;
        }
    }
}