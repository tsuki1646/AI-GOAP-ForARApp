using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlacementObject))]
public class PlacementBoundingArea_Test : MonoBehaviour
{
    private PlacementObject placementObject;

    private bool initialized = false;
    //private Transform childObj;
    [SerializeField]private GameObject childObj;

    void Start()
    {
        placementObject = GetComponent<PlacementObject>();
        initialized = true;
        //childObj = this.transform.Find("SelectionVisualization");
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
        childObj.gameObject.SetActive(true);
    }


}

