using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main mover for the puzzle system. sets dimensions/offset, percentage of certain tile types and validates connections. 
/// </summary>
public class GridManager : MonoBehaviour
{
    private GridTilePercentage gridTilePercentage; 

    [SerializeField]
    [Range(5, 8)]
    private int width, height;
  
    [SerializeField]
    [Range(1.1f, 2.125f)]
    private float offset = 1.5f;

    [SerializeField]
    private Tile _tilePrefab;
   
    [Header("These two fields force a min and max amount of tiles in the grid. A good default is min = 2 and max = 6.")]
    [SerializeField] [Range(1, 10)]
    private int minAmtTiles = 2;

    [SerializeField] [Range(1, 10)]
    private int maxAmtTiles = 6;

    private Tile[,] arrayOfTiles; // new Tile[width, height];


    public SpriteRenderer dimmer;
    private List<GameObject> allTilesInGrid = new List<GameObject>(); //private list must be initialized, reference to all tiles in the grid 

    [Header("The currently connected tiles by the player. Left exposed for debugging.")]
    public List<GameObject> connectedTiles; //currently connected tiles
    private Coroutine coroutine;

    private bool falling = false;
    public bool Falling {
        get { return falling; }
    }

    void Awake()
    {
        GenerateGrid();        
    }

    private void Update()
    {
        if (dimmer != null) {
            dimmer.enabled = falling;
        }
    }

    private void GenerateGrid()
    {

        if (gridTilePercentage == null)
        {
            gridTilePercentage = GetComponent<GridTilePercentage>();
        }
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
        connectedTiles.Clear ();
        
    }

   

    public void RemoveLineObjectsInList(List<GameObject> list, bool completeRemove = false)
    {
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
                       
                        tile.gameObject.GetComponent<Tile>().DestroyLineObject(); //duplicated code, refactor later 
                    }
                       
                }
                    
            }
        }
        connectedTiles.Clear(); 
    }

    private void ForceTilesToRange(int minAmount, int maxAmount)
    {
        ForceMinTiles(minAmount, TileEnum.A_TILE);
        ForceMinTiles(minAmount, TileEnum.B_TILE);
        ForceMinTiles(minAmount, TileEnum.C_TILE);
        ForceMaxTiles(maxAmount, TileEnum.A_TILE);
        ForceMaxTiles(maxAmount, TileEnum.B_TILE);
        ForceMaxTiles(maxAmount, TileEnum.C_TILE);
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

    public void PlayFallAnimation() {
        if (coroutine != null) {
            Debug.Log("Drop : Fall Animation Already Playing");
            return;
        }
        coroutine = StartCoroutine(DropDown());
    }

    private IEnumerator DropDown() {
        List<GameObject> usedTiles = new List<GameObject>();
        falling = true;
        //Stop Timer

        //Freeze Player

        //Init Drops : Amount of Falldown for each 
        List<int> drops = new List<int>();
        for (int i = 0; i < allTilesInGrid.Count; i++){
            drops.Add(0);
        }

        int[] topDrops = new int[width];
        //Init Top Drops
        for (int i = 0; i < width; i++) {
            topDrops[i] = 0;
        }

        Debug.Log("Drop : Start of Initial Setup");
        //Turn Gray or Calculate Drop
        for (int h = 0; h < allTilesInGrid.Count; h++) {
            allTilesInGrid[h].GetComponent<Tile>().DestroyLineObject();

            //Turn Gray If in Use & Delet Line
            if (allTilesInGrid[h].GetComponent<Tile>().IsInUse()) {
                allTilesInGrid[h].GetComponent<SpriteRenderer>().color = Color.gray;
                topDrops[allTilesInGrid[h].GetComponent<Tile>().GetXID()]++;
                usedTiles.Add(allTilesInGrid[h]);
                continue;
            }

            //Calculate Drop if in Use
            for (int i = 0; i < allTilesInGrid.Count; i++) {

                if (allTilesInGrid[h].GetComponent<Tile>().GetXID() == allTilesInGrid[i].GetComponent<Tile>().GetXID()) {
                    if (allTilesInGrid[h].GetComponent<Tile>().GetYID() > allTilesInGrid[i].GetComponent<Tile>().GetYID()) {
                        if (allTilesInGrid[i].GetComponent<Tile>().IsInUse()) {
                            drops[h] = drops[h] + 1;
                        }
                    }
                }
            }
            yield return new WaitForSecondsRealtime(1 / ((float) allTilesInGrid.Count));
        } Debug.Log("Drop : Reach End of Initial Setup");

        yield return new WaitForSecondsRealtime(1/(30.0f));
        //Remove Tile

        Debug.Log("Drop: Start of Remove Tile");
        foreach (GameObject g in usedTiles)
        {
            //Delete Tile
            g.GetComponent<Tile>().SetTileColorIdentity(TileEnum.BLANK_TILE);
            g.transform.localPosition = new Vector2 (g.transform.localPosition.x, height * offset);
            g.GetComponent<Tile>().SetInUse(false);
            g.SetActive(false);
            yield return new WaitForSecondsRealtime(1 /( 60.0f));
        }
        Debug.Log("Drop: End of Remove Tile");

        //Lerp Trackers
        float lerpTime = 0.0f;
        float lerpFull = 0.25f;

        //Calculate Falls
        Debug.Log("Drop: Start of Calc Falls");
        for (int i = 0; i < allTilesInGrid.Count; i++) {
            if (drops[i] > 0) {
                lerpTime = 0;
                Tile tile = allTilesInGrid[i].GetComponent<Tile>();
                Vector3 startLoc = tile.transform.localPosition;
                tile.SetTileId(tile.GetXID(), tile.GetYID() - drops[i]); //Set new ID
                Vector3 gridPlacement = new Vector3(tile.GetXID() * offset, tile.GetYID() * offset);

                //Lerp Block to Position
                while (lerpTime < lerpFull) {
                    lerpTime += 1 / 60.0f;
                    if (lerpTime >= lerpFull)
                    {
                        tile.transform.localPosition = gridPlacement;
                        break;
                    }
                    else {
                        tile.transform.localPosition = Vector3.Lerp(startLoc, gridPlacement, lerpTime/lerpFull);
                    }
                    yield return new WaitForSecondsRealtime(1 / ((60.0f) * drops.Count));
                }//End of Lerp

            }
        }
        Debug.Log("Drop: End of Calc Falls");


        //Generate New tiles
        Debug.Log("Drop: Start of New Tiles");
        float tileTime = 1 + ((usedTiles.Count/5)/2.0f);//Increase Time for tile falls if more tiles involved

        for (int i = 0; i < usedTiles.Count; i++)
        {
            usedTiles[i].SetActive(true);
            Tile tile = usedTiles[i].GetComponent<Tile>();

            tile.GetComponent<Tile>().Init(gridTilePercentage.GenerateRandomColorType());// Assign New Color

            if (topDrops[tile.GetXID()] > 0)
            {
                lerpTime = 0;
                
                Vector3 startLoc = tile.transform.localPosition;
                tile.SetTileId(tile.GetXID(), height - topDrops[tile.GetXID()]); //Set new ID
                topDrops[tile.GetXID()] -= 1;

                Vector3 gridPlacement = new Vector3(tile.GetXID() * offset, tile.GetYID() * offset);

                //Lerp Block to Position
                while (lerpTime < lerpFull)
                {
                    lerpTime += 1 / 60.0f;
                    if (lerpTime >= lerpFull)
                    {
                        tile.transform.localPosition = gridPlacement;
                        break;
                    }
                    else
                    {
                        tile.transform.localPosition = Vector3.Lerp(startLoc, gridPlacement, lerpTime / lerpFull);
                    }
                    yield return new WaitForSecondsRealtime(1.0f / ((60.0f) * usedTiles.Count));
                }//End of Lerp

            }
        }
        Debug.Log("Drop: End of New Tiles");

        //Unfreeze Player & set timer to solving speed

        //Clear Connected Tiles
        RemoveTilesInList();
        yield return new WaitForSecondsRealtime(0.33f);
        falling = false;
        coroutine = null;
        yield break;
    }

    public float GetGridOffset()
    {
        return offset;
    }

    public Tile[,] GetArrayOfTiles() { return arrayOfTiles; }

}
