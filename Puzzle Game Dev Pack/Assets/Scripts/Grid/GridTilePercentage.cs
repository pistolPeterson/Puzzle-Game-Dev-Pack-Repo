using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script that assigns a certain percentage of the tiles. A good default value is all 15 percent.
/// </summary>
public class GridTilePercentage : MonoBehaviour
{

    [SerializeField]
    [Range(1, 33)]
    private int aPercentage = 15, bPercentage = 15, cPercentage = 15;


    public TileEnum GenerateRandomColorType()
    {
        int num = UnityEngine.Random.Range(1, 101);

        int none = 100 - (aPercentage + bPercentage + cPercentage);

        if (num <= none)
        {
            return TileEnum.BLANK_TILE;
        }
            
        else if (num <= aPercentage + none)
            return TileEnum.A_TILE;
        else if (num <= bPercentage + aPercentage + none)
            return TileEnum.C_TILE;
        else if (num <= cPercentage + bPercentage + aPercentage + none)
            return TileEnum.B_TILE;

        return TileEnum.BLANK_TILE;
    }

    public void SetAPercentage(int newA_percentage) { aPercentage = newA_percentage;  }
    public void SetBPercentage(int new_Bpercentage) { bPercentage = new_Bpercentage; }

    public void SetCPercentage(int newCPercentage) { cPercentage = newCPercentage; }

}
