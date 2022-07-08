using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The main script for test scene 1. Handles all the common data extraction and logic you can create and customize with the 
/// Grid Manager.
/// </summary>
public class GridManagerDebug : MonoBehaviour
{
    
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GridTilePercentage gridTilePercentage;

    [SerializeField] private Slider widthSlider;
    [SerializeField] private Text widthText;

    [SerializeField] private Slider heightSlider;
    [SerializeField] private Text heightText;

    [SerializeField] private Slider offsetSlider;
    [SerializeField] private Text offsetText;

    [SerializeField] private Slider A_percentSlider;
    [SerializeField] private Text A_percentText;

    [SerializeField] private Slider B_percentSlider;
    [SerializeField] private Text B_percentText;

    [SerializeField] private Slider C_percentSlider;
    [SerializeField] private Text C_percentText;


    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        gridTilePercentage = FindObjectOfType<GridTilePercentage>();

        if (gridManager == null)
            Debug.LogWarning("Grid Manager Debug is active, yet there is not Grid Manager in the scene! ");
        if (gridTilePercentage == null)
            Debug.LogWarning("Grid Manager Debug is active, yet there is not gridTilePercentage script in the scene! ");
    }

    private void Start()
    {
        SliderSetup(); 
    }

    //Used by the reset grid button on scene
    public void RegenerateGrid() 
    { 
        gridManager.RegenerateGrid();
       var cameraScript = FindObjectOfType<CameraSetup>(); 
        if (cameraScript != null)
            cameraScript.SetPositionBasedOnOffset();
    }

    //Randomize all customizations possible 
    public void RandomizeAll()
    {
        int r1 = Random.Range(5, 9); 
        gridManager.SetGridWidth(r1);
        widthSlider.value = r1;

        int r2 = Random.Range(5, 9);
        gridManager.SetGridHeight(r2);
        heightSlider.value = r2;

        float r3 = Random.Range(1.1f, 2.125f);
        gridManager.SetGridOffset(r3);
        offsetSlider.value = r3;

        int r4 = Random.Range(1, 33);
        gridTilePercentage.SetAPercentage(r4);
        A_percentSlider.value = r4;

        int r5 = Random.Range(1, 33);
        gridTilePercentage.SetBPercentage(r5);
        B_percentSlider.value = r5;

        int r6 = Random.Range(1, 33);
        gridTilePercentage.SetCPercentage(r6);
        C_percentSlider.value = r6;

        RegenerateGrid();
    }


    //Register the sliders and use their on value change feature
    private void SliderSetup()
    {
        widthSlider.onValueChanged.AddListener((v) =>
        {
            widthText.text = "Width: " + v.ToString();
            gridManager.SetGridWidth((int)v);
        });

        heightSlider.onValueChanged.AddListener((v) =>
        {
            heightText.text = "Height: " + v.ToString();
            gridManager.SetGridHeight((int)v);
        });

        offsetSlider.onValueChanged.AddListener((v) =>
        {
            offsetText.text = "Offset: " + v.ToString();
            gridManager.SetGridOffset(v);
        });

        A_percentSlider.onValueChanged.AddListener((v) =>
        {
            A_percentText.text = "Tile A %: " + v.ToString();
            gridTilePercentage.SetAPercentage((int)v);
        });

        B_percentSlider.onValueChanged.AddListener((v) =>
        {
            B_percentText.text = "Tile B %: " + v.ToString();
            gridTilePercentage.SetBPercentage((int)v);
        });

        C_percentSlider.onValueChanged.AddListener((v) =>
        {
            C_percentText.text = "Tile C %: " + v.ToString();
            gridTilePercentage.SetCPercentage((int)v);
        });
    }
}
