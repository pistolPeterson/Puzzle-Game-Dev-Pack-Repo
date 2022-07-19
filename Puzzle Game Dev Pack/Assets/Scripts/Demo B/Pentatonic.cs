using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentatonic : MonoBehaviour
{
    public List<Note_SO> C_Pentatonic;
    public AudioClip clippy;
   [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
       
    }
    private void OnEnable()
    {
        GridManager.NewConnectionValidated += PlayNote; 
    }
    private void OnDisable()
    {
        GridManager.NewConnectionValidated -= PlayNote;
    }

    public AudioClip SelectNote(List<Note_SO> notes, int velocity = 2)
    {
        Debug.Log(1);
        if (notes.Count <= 0) return null;
      
        switch (velocity)
        {
            case 1:
                Debug.Log(4);
                return notes[Random.Range(0, notes.Count)].smallLength;
                break;
            case 2:
                return notes[Random.Range(0, notes.Count)].normalLength;
                break;
            case 3:
                return notes[Random.Range(0, notes.Count)].largeLength;
                break;

            default:
                Debug.Log("default case reached");
                return null;
                break;
        }

    }

    public void PlayNote()
    {
        Debug.Log(3);
        audioSource.PlayOneShot(SelectNote(C_Pentatonic,2));
    }

}
