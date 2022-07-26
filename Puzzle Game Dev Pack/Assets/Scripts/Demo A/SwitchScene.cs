using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Basic scene management script. Will have more as more demo scenes are made.
/// </summary>
public class SwitchScene : MonoBehaviour
{

    public void SwitchScenes()
    {
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
    }

    public void SwitchScenes1()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
