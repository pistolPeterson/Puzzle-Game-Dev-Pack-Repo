using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicProgress : MonoBehaviour
{
    public enum StateMachine
    {
        NONE, 
        lvl1, 
        lvl2,
        lvl3,
        lvl4,
        lvl5,
    }
  
    [SerializeField] private AudioClip lvl1Clip;
    [SerializeField] private AudioClip lvl2Clip;
    [SerializeField] private AudioClip lvl3Clip;
    [SerializeField] private AudioClip lvl4Clip;
    [SerializeField] private AudioClip lvl5Clip;

    private AudioClip currentClipForLevel;
   [SerializeField] private StateMachine state;
    [SerializeField] private float timer;

    [SerializeField] private int connection; 

    private void Start()
    {
        connection = 0;
        timer = 0.0f; 
        currentClipForLevel = lvl1Clip;
        state = StateMachine.lvl1;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        MusicProgressStateMachine();
    }

    public void MusicProgressStateMachine()
    {
        switch (state)
        {
            case StateMachine.lvl1:        
                StateMachineMover(StateMachine.lvl2, 5, float.MaxValue);
                break; 

                case StateMachine.lvl2:
                currentClipForLevel = lvl2Clip;
                StateMachineDemover(5, 15.0f);
                StateMachineMover(StateMachine.lvl3, 5, 15.0f);
                             

                break;

            case StateMachine.lvl3:
                currentClipForLevel = lvl3Clip;
                StateMachineDemover(5, 12.0f);
                StateMachineMover(StateMachine.lvl4, 5, 12.0f);
               
                break;

            case StateMachine.lvl4:
                currentClipForLevel = lvl4Clip;
                StateMachineDemover(5, 10.0f);
                StateMachineMover(StateMachine.lvl5, 5, 10.0f);
                
                break;

            case StateMachine.lvl5:
                currentClipForLevel = lvl5Clip;
               
                Debug.Log("MAX LEVEL");
                //StateMachineMover(StateMachine.lvl5, 5, 10.0f);

                break;

        }
    }
    public void StateMachineDemover( int connectionsNeeded, float lessThanTime)
    {
        if ( timer > lessThanTime)
        {
            DropToLevel1();
        }

    }
    private void DropToLevel1()
    {
        Debug.Log("Dropping to lvl1");
        timer = 0.0f;
        connection = 0;
        state = StateMachine.lvl1;
        currentClipForLevel = lvl1Clip;

    }
    public void StateMachineMover(StateMachine newState, int connectionsNeeded, float lessThanTime )
    {
        if(connection > connectionsNeeded && timer < lessThanTime )
        {
            Debug.Log("Next level! " + newState.ToString());
            timer = 0.0f;
            connection = 0;
            state = newState;
        }
       
    }
    private void OnEnable()
    {
        GridManager.NewConnectionValidated += AddConnection;
        GridManager.BadConnectionMade += DropToLevel1; 
    }

    private void OnDisable()
    {
        GridManager.NewConnectionValidated -= AddConnection;
        GridManager.BadConnectionMade -= DropToLevel1;
    }
    public void AddConnection()
    {
        connection++;
    }
    public AudioClip GetCurrentClipForLevel() { return currentClipForLevel; }
}

