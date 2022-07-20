using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentatonic : MonoBehaviour
{
    public List<Note_SO> C_Pentatonic;
    public List<Note_SO> F_Pentatonic;
    public List<Note_SO> G_Pentatonic;


   [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (GridManager.lengthOfConnectionEvent == null)
            GridManager.lengthOfConnectionEvent = new MyIntEvent();


        GridManager.lengthOfConnectionEvent.AddListener(PlayNote);

    }
    private void OnEnable()
    {
       // GridManager.NewConnectionValidated += PlayNote; 
    }
    private void OnDisable()
    {
       // GridManager.NewConnectionValidated -= PlayNote;
    }

    public AudioClip SelectNote(List<Note_SO> notes, int velocity = 2)
    {
        
        if (notes.Count <= 0) return null;
      
        switch (velocity)
        {
            case 1:
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

    public void PlayNote(int length, TileEnum tile)
    {
        int num = Random.Range(0, 2);

        if(tile == TileEnum.A_TILE)
            audioSource.PlayOneShot(SelectNote(C_Pentatonic,2));
        if (tile == TileEnum.B_TILE)
            audioSource.PlayOneShot(SelectNote(F_Pentatonic, 2));
        if (tile == TileEnum.C_TILE)
            audioSource.PlayOneShot(SelectNote(G_Pentatonic, 2));
    }

}
