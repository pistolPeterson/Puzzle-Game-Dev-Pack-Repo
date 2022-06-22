using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float offset = gridManager.GetGridOffset();
        Vector3 newPos = new Vector2(); 

        newPos.x = (offset * 1.428f) + 1.428f;
        newPos.y = (offset * 2.85714f) - 0.89285f;
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
