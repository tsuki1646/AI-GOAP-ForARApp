using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ToggleAR_Test : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public ARPointCloudManager pointCloudManager;

    public void OnValueChanged(bool isOn)
    {
        VisualizePlanes(isOn);
        VisualizePoints(isOn);
    }

    void VisualizePlanes(bool active)
    {
        planeManager.enabled = active;
        foreach (ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(active);
        }
    }

    void VisualizePoints(bool active)
    {
        pointCloudManager.enabled = active;
        foreach (ARPointCloud pointCloud in pointCloudManager.trackables)
        {
            pointCloud.gameObject.SetActive(active);
        }
    }
}
