using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyIntEvent : UnityEvent<int, TileEnum>
{
}

/// <summary>
/// The main mover for the puzzle system. sets dimensions/offset, percentage of certain tile types and validates connections. 
/// </summary>
public class GridManager : MonoBehaviour
{
    private GridTilePercentage gridTilePercentage; 
    private ComboManager comboManager;

    [Header("This is where you would want to add the potential presets you want to use. Leave it empty, if you dont want to use preset")]
    public BasePresetSO startingPreset; 
    

    
    [SerializeField]
    [Range(5, 8)]
    private int width, height;
  
    [SerializeField]
    [Range(1.1f, 2.125f)]
    private float offset = 1.5f;

    [SerializeField]
    private Tile _tilePrefab;
   
    [Header("These two fields force a min and max amount of tiles in the grid. A good default is min = 2 and max = 6.")]
    [SerializeField] [Range(1, 15)]
    private int minAmtTiles = 2;

    [SerializeField] [Range(1, 15)]
    private int maxAmtTiles = 6;

    private Tile[,] arrayOfTiles; // new Tile[width, height];


   
    private List<GameObject> allTilesInGrid = new List<GameObject>(); //private list must be initialized, reference to all tiles in the grid 

    [Header("The currently connected tiles by the player. Left exposed for debugging.")]
    public List<GameObject> connectedTiles; //currently connected tiles
    private Coroutine coroutine;

    public static event Action NewConnectionValidated;
    public static MyIntEvent lengthOfConnectionEvent;
    public static event Action BadConnectionMade;
   
    void Awake()
    {
        if(startingPreset != null)
        {
            width = startingPreset.width;
            height = startingPreset.height;
            offset = startingPreset.offset;
            minAmtTiles = startingPreset.minAmtTiles;
            maxAmtTiles = startingPreset.maxAmtTiles;
        }

        if (gridTilePercentage == null)
            gridTilePercentage = GetComponent<GridTilePercentage>();
        
        if (comboManager == null)
            comboManager = GetComponent<ComboManager>();

        GenerateGrid();        
    }

  
    private void GenerateGrid()
    {    
        arrayOfTiles = new Tile[width, height];
        allTilesInGrid.Clear();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
              
                var gridPlacement = new Vector3(x * offset, y *offset);
                var spawnedTile = Instantiate(_tilePrefab, gridPlacement, Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.transform.parent = transform; //all tiles are now a child of the gridmanager object
                
                spawnedTile.transform.localScale += new Vector3(-0.1f, -0.1f, -0.1f);
                if (spawnedTile == null)
                    Debug.Log("spawn tile");

                if (gridTilePercentage == null)
                    Debug.Log("grid tile");


                spawnedTile.Init(gridTilePercentage.GenerateRandomColorType());
                spawnedTile.SetTileId(x, y);
                spawnedTile.transform.localPosition = new Vector3(x * offset, y * offset);
                allTilesInGrid.Add(spawnedTile.gameObject);      
                arrayOfTiles[x, y] = spawnedTile.GetComponent<Tile>();
            }
        }
        ForceTilesToRange(minAmtTiles, maxAmtTiles);
    }


    public void RegenerateGrid()
    {
        foreach (var tile in allTilesInGrid)
        {
            if(tile.gameObject != null)
            Destroy(tile.gameObject);
        }
        connectedTiles = new List<GameObject>();
        GenerateGrid();
        comboManager.ClearCombo();      
    }

    public void RegenerateGridColors()//assumption that grid size will be the same, maybe add parameters for grid size 
    {     
        //delete all line objects in the grid 
        RemoveLineObjectsInList(allTilesInGrid, true);

        //rechange the "type" of all the tiles 
        ReassignColorTypeInGrid(allTilesInGrid);      
    }

    private void ReassignColorTypeInGrid(List<GameObject> list)
    {
        foreach (var tile in list)
        {
            if(tile.gameObject != null)
            tile.GetComponent<Tile>().Init(gridTilePercentage.GenerateRandomColorType());
        }
            
    }


    public void AddConnectedTiles(GameObject tile)
    {
        if (tile == null)
            Debug.Log("ay yo, tile is null"); 

        if (tile.gameObject.GetComponent<Tile>().GetTileColorIdentity() != TileEnum.BLANK_TILE)
        {
            if(connectedTiles.Count > 0) //this should be removed and implemented elsewhere
            {
                if (connectedTiles[0] == null)
                    return;
                //if the currently selected tile is not the same type as the first tile selected
                if (tile.gameObject.GetComponent<Tile>().GetTileColorIdentity() != connectedTiles[0].gameObject.GetComponent<Tile>().GetTileColorIdentity()) 
                {
                   
                    RemoveLineObjectsInList(connectedTiles);
                    RemoveTilesInList();
                  
                    connectedTiles.Add(tile);
                    return;
                }
            }
        }
        connectedTiles.Add(tile);
    }


    public void ValidateConnection()
    {
        for (int i = connectedTiles.Count- 2; i >= 1; i--)
        {         
            if(connectedTiles[i].gameObject.GetComponent<Tile>().GetPrevTile() == null)
            {
                Debug.Log("Invalid Connection");
                RemoveLineObjectsInList(connectedTiles);
                connectedTiles.Clear();
                return;
            }
        }

        foreach (var tile in connectedTiles)
        {
            tile.gameObject.GetComponent<Tile>().SetInUse(true);
        }
       
        comboManager.AddToCombo(connectedTiles);
        NewConnectionValidated?.Invoke();
        lengthOfConnectionEvent.Invoke(connectedTiles.Count, connectedTiles[connectedTiles.Count - 1].GetComponent<Tile>().GetTileColorIdentity());
        connectedTiles.Clear ();
        
    }

   

    public void RemoveLineObjectsInList(List<GameObject> list, bool completeRemove = false)
    {
        Debug.Log("missed note lul");
        BadConnectionMade?.Invoke();
        if (completeRemove == true)
        {
            foreach (var tile in list)
            {
                if(tile.gameObject != null)
                tile.gameObject.GetComponent<Tile>().DestroyLineObject();
            }
               
        }
        else
        {
            
            foreach (var tile in list)
            {
                if (tile.gameObject != null)
                {
                    if (tile.gameObject.GetComponent<Tile>().IsInUse() == false)
                    {
                       
                        tile.gameObject.GetComponent<Tile>().DestroyLineObject(); 
                    }
                       
                }
                    
            }
        }
        connectedTiles.Clear(); 
    }

    private void ForceTilesToRange(int minAmount, int maxAmount)
    {
        ForceMinTiles(minAmount, TileEnum.RED_TILE);
        ForceMinTiles(minAmount, TileEnum.GREEN_TILE);
        ForceMinTiles(minAmount, TileEnum.BLUE_TILE);
        ForceMaxTiles(maxAmount, TileEnum.RED_TILE);
        ForceMaxTiles(maxAmount, TileEnum.GREEN_TILE);
        ForceMaxTiles(maxAmount, TileEnum.BLUE_TILE);
    }
    private void ForceMinTiles(int minAmount, TileEnum colorType)
    {
        int amtOfColorTile = 0;
        foreach (var tile in allTilesInGrid)
        {
            if (tile.gameObject.GetComponent<Tile>().GetTileColorIdentity() == colorType) 
                amtOfColorTile++;
        }

       
        while (amtOfColorTile <= minAmount)
        {
           
            List<GameObject> blackTiles = new List<GameObject>(); 
            foreach(var tile in allTilesInGrid) //get all black tiles 
            {
                if (tile.gameObject.GetComponent<Tile>().GetTileColorIdentity() == TileEnum.BLANK_TILE) 
                    blackTiles.Add(tile);
            }

            //randomly change one of them to that color 
            int randNum = UnityEngine.Random.Range(0, blackTiles.Count -1);
            blackTiles[randNum].gameObject.GetComponent<Tile>().SetTileColorIdentity(colorType);  


            amtOfColorTile++;          
        }
    }
    private void ForceMaxTiles(int maxAmount, TileEnum colorType)
    {
        int amtOfColorTile = 0;
        foreach (var tile in allTilesInGrid)
        {
            if (tile.gameObject.GetComponent<Tile>().GetTileColorIdentity() == colorType)
                amtOfColorTile++;
        }


        while (amtOfColorTile >= maxAmount)
        {

            List<GameObject> blackTiles = new List<GameObject>();
            foreach (var tile in allTilesInGrid) //get all black tiles 
            {
                if (tile.gameObject.GetComponent<Tile>().GetTileColorIdentity() == colorType)
                    blackTiles.Add(tile);
            }

            //randomly change one of them to black
            int randNum = UnityEngine.Random.Range(0, blackTiles.Count - 1);
            blackTiles[randNum].gameObject.GetComponent<Tile>().SetTileColorIdentity(TileEnum.BLANK_TILE); 


            amtOfColorTile--;
        }
    }
    //GETTERS AND SETTERS
    public GameObject getFirstConnectedTile()
    {
        if (connectedTiles.Count == 0)
        {
            return null;
        }

        return connectedTiles[0];
    }

    public void RemoveTilesInList()
    {
        connectedTiles.Clear();
    }  

    public int getWidth()
    {
        return width;
    }
    public int getHeight()
    {
        return height;
    }

    public List<GameObject> getAllTilesInGrid()
    {
        return allTilesInGrid;
    }

    public List<GameObject> getConnectedTiles()
    {
        return connectedTiles;
    }

    public void SetGridOffset(float newOffset)
    {
        offset = newOffset;
    }

    public void SetGridWidth(int newWidth)
    {
        width = newWidth;
    }
    public void SetGridHeight(int newHeight)
    {
        height = newHeight;
    }
    public float GetGridOffset()
    {
        return offset;
    }

    public Tile[,] GetArrayOfTiles() { return arrayOfTiles; }

}
