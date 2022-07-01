using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Use this class to customize the colors of the tiles to your desired aesthetic. Make sure the alpha ('A') value is set to 255 
/// or at a high enough value.
/// </summary>
public class TileCustomization : MonoBehaviour
{
    [SerializeField] private Color tileAColor;
    [SerializeField] private Color tileBColor;
    [SerializeField] private Color tileCColor;
    [SerializeField] private Color blankTileColor;


    public Color TileAColor  {  get => tileAColor;  }
    public Color TileBColor { get => tileBColor; }
    public Color TileCColor { get => tileCColor; }
    public Color BlankTileColor { get => blankTileColor; }

}
