using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
/// <summary>
/// A basic audio system for the gridmanager. Use it to input your sfx clips and change the randomize parameters to help generate more variety of sounds.
/// </summary>
public class AudioEvents : MonoBehaviour
{

    public AudioClip clippy;
    [Header("The SFX Files for the snaps and connections")]
    [SerializeField] private AudioClip[] snapClips;
    [SerializeField] private AudioClip[] connectionClips;

    [Header("Option to randomize the pitch and the volume")]
    [SerializeField] private bool randomizePitch;
    [SerializeField] private bool randomizeVolume;

    [Header("Randomize the SFX Parameters ")]
    [SerializeField] [Range(0.01f, 0.25f)] private float volumeVariance = 0.05f;
    [SerializeField] [Range(0.01f, 1.0f)] private float pitchVarience = 0.05f;

    private AudioSource audioSource;

    private void Awake() => audioSource = GetComponent<AudioSource>();

    private void OnEnable()
    {
        Tile.snapSound += PlaySnapAudio;
        GridManager.NewConnectionValidated += PlayConnectionSound; 
    }

    private void OnDisable() 
    {
        Tile.snapSound += PlaySnapAudio;
        GridManager.NewConnectionValidated -= PlayConnectionSound;
    }

    private void PlaySnapAudio()
    {
        RandomizeSound();
        if(snapClips.Length > 0)
            audioSource.PlayOneShot(snapClips[Random.Range(0, snapClips.Length)]);
    }

    private void PlayConnectionSound()       
    {
        RandomizeSound();
        if (connectionClips.Length > 0)
            audioSource.PlayOneShot(connectionClips[Random.Range(0, connectionClips.Length)]);

        audioSource.PlayOneShot(clippy);
    }


    private void RandomizeSound()
    {
        if (randomizePitch)
        {
            audioSource.pitch = Random.Range( 1f - pitchVarience, 1f+ pitchVarience);
        }

        if (randomizeVolume)
        {
            audioSource.volume = Random.Range(0.5f - volumeVariance, 0.5f + volumeVariance);
        }

    }
}
