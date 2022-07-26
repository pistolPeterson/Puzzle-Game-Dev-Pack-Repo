using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A simple crude script that resets the grid every 8 seconds for Demo B. I would probably couple it with the music progress script instead later. 
/// </summary>
public class ResetGridEveryMeasure : MonoBehaviour
{
    private GridManager gridManager;
    private float timer; 
    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        gridManager = FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 8.0f)
        {
            timer = 0.0f;
            ResetGrid();

        }
    }

    public void ResetGrid()
    {
        gridManager.RegenerateGrid();
    }

}
