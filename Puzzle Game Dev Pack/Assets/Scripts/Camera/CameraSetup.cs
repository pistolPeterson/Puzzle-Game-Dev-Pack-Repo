using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// script working in the background to orient itself depending on the dimensions of the grid
/// </summary>
public class CameraSetup : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    // Start is called before the first frame update
    void Start()
    {
        SetPositionBasedOnOffset(); 
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void SetPositionBasedOnOffset()
    {       
        transform.position = FindPositionOfMiddleTile();
    }

    Vector3 FindPositionOfMiddleTile()
    {
        Vector3 pos = new Vector3(0,0,0);
        pos.z = transform.position.z;   
        var arrayOfTiles = gridManager.GetArrayOfTiles();
       
        Tile leftBottTile = arrayOfTiles[0, 0];
        Tile rightTopTile = arrayOfTiles[gridManager.getWidth() - 1, gridManager.getHeight() - 1];

        pos.x = (leftBottTile.gameObject.transform.position.x  + rightTopTile.gameObject.transform.position.x) / 2;
        pos.y = (leftBottTile.gameObject.transform.position.y + rightTopTile.gameObject.transform.position.y) / 2;
        //left side + right side / 2

        return pos;
    }
}
