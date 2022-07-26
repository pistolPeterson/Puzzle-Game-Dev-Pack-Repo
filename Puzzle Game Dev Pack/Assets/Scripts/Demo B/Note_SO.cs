using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// An attempt at making a fully realized musical instrument. This scriptable object is for holding audio data for a specific note. Use it if you want to create more
/// notes for more options in your audio gameplay. 
/// </summary>
[CreateAssetMenu]
public class Note_SO : ScriptableObject
{
    public string noteName;

    public AudioClip normalLength;
    public AudioClip smallLength;
    public AudioClip largeLength;
}
