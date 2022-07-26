using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A script made for holding grouping of musical notes called pentatonics. However you can use to make groupings of any types of scales, modes and keys! 
/// </summary>
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
            GridManager.lengthOfConnectionEvent = new ConnectionEvent();


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
        audioSource.pitch = 1;

    }

    public AudioClip SelectNote(List<Note_SO> notes, int velocity = 2)
    {
        
        if (notes.Count <= 0) return null;
      
        switch (velocity)
        {
            case 1:
                return notes[Random.Range(0, notes.Count)].smallLength;
            case 2:
                return notes[Random.Range(0, notes.Count)].normalLength;
            case 3:
                return notes[Random.Range(0, notes.Count)].largeLength;
            default:
                Debug.Log("default case reached");
                return null;
        }

    }

    public void PlayNote(int length, TileEnum tile)
    {
        int num = Random.Range(0, 2);     
            audioSource.PlayOneShot(SelectNote(A_Pentatonic,2));
        
    }

}
