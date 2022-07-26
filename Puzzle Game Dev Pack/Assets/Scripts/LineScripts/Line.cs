using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The Line Object, that is set on top of any tile object. Uses the Unity Linerenderer to create the illusion of connecting lines across the tiles.
/// </summary>
public class Line : MonoBehaviour
{
   
    public LineRenderer lineRenderer;
    private bool isPlaced = false;
    private int lnXID;
    private int lnYID;
     
    private void Start()
    {
        lineRenderer.sortingLayerName = "Display";
    }

    // Update is called once per frame
    void Update()
    {       
        LineDrag();
    }
    

    private void LineDrag()
    {
        if (isPlaced == true)
            return;
            FollowMouse();
    }


    public void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition; // returns "mouse coordinates", must convert to game vector position 
        Vector3 convertedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        convertedMousePosition.z = 0; //making sure the z position is 0         
        transform.position = convertedMousePosition;
        Vector3 positionDifference = convertedMousePosition - lineRenderer.transform.position;
        lineRenderer.SetPosition(2, positionDifference);
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tile"))
        {
            var tile = collision.GetComponent<Tile>();
            if (tile != null)
            {
                if ((GetLineXID() == tile.GetXID()) && (GetLineYID() == tile.GetYID()))
                {
                    
                    isPlaced = false;
                }
                else if (tile.GetInUseLine() == true)
                {
                    isPlaced = false;
                }
                else
                {
                   
                    SetIsPlaced();
                }
                   
            }
        }
        
    }

    //GETTERS AND SETTERS 
    public void SetIsPlaced()
    {
        
        isPlaced = true;
    }

    public int GetLineXID()
    {
        return lnXID;
    }
    
    public int GetLineYID()
    {
        return lnYID;
    }

    public void SetLineID(int xId, int yId)
    {
        lnXID = xId;
        lnYID = yId;
    }
}
