using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentatonic : MonoBehaviour
{
    public List<Note_SO> C_Pentatonic;
    public List<Note_SO> F_Pentatonic;
    public List<Note_SO> G_Pentatonic;
    public List<Note_SO> A_Pentatonic;

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
        GridManager.BadConnectionMade += PlayBadNote; 
    }
    private void OnDisable()
    {
        // GridManager.NewConnectionValidated -= PlayNote;
        GridManager.BadConnectionMade -= PlayBadNote;

    }


    public void PlayBadNote()
    {
        audioSource.pitch = Random.Range(0.85f, 1.15f);
        audioSource.PlayOneShot(C_Pentatonic[Random.Range(0, C_Pentatonic.Count)].normalLength);
        StartCoroutine(WaitBeforeFixingPitch());

    }

    private IEnumerator WaitBeforeFixingPitch()
    {
        yield return new WaitForSeconds(1.5f);

        //optional fade pitch back to 1 
        audioSource.pitch = 1;

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

       
            audioSource.PlayOneShot(SelectNote(A_Pentatonic,2));
        
    }

}
