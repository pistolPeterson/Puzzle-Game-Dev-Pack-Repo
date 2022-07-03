using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script that assigns a certain percentage of the tiles. A good default value is all 15 percent.
/// </summary>
public class GridTilePercentage : MonoBehaviour
{

    [SerializeField]
    [Range(10, 25)]
    private int RedPercentage = 15, BluePercentage = 15, GreenPercentage = 15;


    public TileEnum GenerateRandomColorType()
    {
        int num = UnityEngine.Random.Range(1, 101);

        int none = 100 - (RedPercentage + BluePercentage + GreenPercentage);

        if (num <= none)
            return TileEnum.BLANK_TILE;
        else if (num <= RedPercentage + none)
            return TileEnum.A_TILE;
        else if (num <= BluePercentage + RedPercentage + none)
            return TileEnum.C_TILE;
        else if (num <= GreenPercentage + BluePercentage + RedPercentage + none)
            return TileEnum.B_TILE;

        return TileEnum.BLANK_TILE;
    }
}
