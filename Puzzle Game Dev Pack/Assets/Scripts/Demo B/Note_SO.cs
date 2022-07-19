using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Note_SO : ScriptableObject
{
    public string noteName;

    public AudioClip normalLength;
    public AudioClip smallLength;
    public AudioClip largeLength;
}
