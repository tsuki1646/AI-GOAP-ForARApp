using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlacementObject))]
public class PlacementBoundingArea_ForTable_Test : MonoBehaviour
{
    private PlacementObject placementObject;

    private bool initialized = false;
    private Transform childObj;

    void Start()
    {
        placementObject = GetComponent<PlacementObject>();
        initialized = true;
        childObj = this.transform.Find("SelectionVisualization");
    }

    void Update()
    {
        if (initialized)
        {
            DrawBoundingArea(placementObject.Selected);
        }
    }

    void DrawBoundingArea(bool isActive)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(2).gameObject.SetActive(true); // or false
        }
    }
}

