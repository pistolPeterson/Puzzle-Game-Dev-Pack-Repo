using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchScenes()
    {
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
    }

    public void SwitchScenes1()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
