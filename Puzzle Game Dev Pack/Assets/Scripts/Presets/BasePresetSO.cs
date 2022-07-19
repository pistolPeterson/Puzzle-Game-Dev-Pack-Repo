using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Preset", menuName = "Preset")]
public class BasePresetSO : ScriptableObject
{
   
    [Range(5, 8)]
    public int width, height = 6;

    [SerializeField]
    [Range(1.1f, 2.125f)]
    public float offset = 1.5f;


    [Header("These two fields force a min and max amount of tiles in the grid. A good default is min = 2 and max = 6.")]
    [Range(1, 15)]
    public int minAmtTiles = 2;

    [Range(1, 15)]
    public int maxAmtTiles = 6;

    [Header("The percentage that you would like for each tile to appear.")]
    [Range(1, 33)]
    public int aPercentage = 15;
    [Range(1, 33)]
    public int bPercentage = 15;
    [Range(1, 33)]
    public int cPercentage = 15;
}
