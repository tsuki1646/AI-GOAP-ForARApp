using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;

public class ARSessionManager : MonoBehaviour
{
    [SerializeField] private ARPlacementInteractableSingle arPlacementInteractableSingle;
    private ARSelectionInteractable_Extend arSelectionInteractable_Extend;
    [SerializeField] private ARPlaneManager arPlaneManager;

    [SerializeField] private Button togglePlaneButton;
    //[SerializeField] private Button ClearButton;

    /*
    bool deleteResource = false;

    // Method for checkingit we're over the trashcan icon
    public void MouseOnHoverTrash()
    {
        deleteResource = true;
    }

    // We've now left the area of the trashcan
    public void MouseOutHoverTrash()
    {
        deleteResource = false;
    }*/

    public void ClearAllObjects()
    {
        Logger.Instance.LogInfo("ClearAllObjects executed");
        arPlacementInteractableSingle.DestroyPlacementObject();
    }

    /*
    public void ClearAllObjects_Extend()
    {
        if (arSelectionInteractable_Extend.canDelete)
        {
            Logger.Instance.LogInfo("ClearAllObjects executed");
            arPlacementInteractableSingle.DestroyPlacementObject();
        }
    }*/

    public void TogglePlanes()
    {
        Logger.Instance.LogInfo("TogglePlanes executed");

        arPlaneManager.enabled = !arPlaneManager.enabled;
        ChageStateOfPlanes(arPlaneManager.enabled);

        var textForToggle = togglePlaneButton.GetComponentInChildren<TextMeshProUGUI>();
        textForToggle.text = arPlaneManager.enabled ? "Disable ARPlane" : "Enable ARPlane";

        Logger.Instance.LogInfo($"AR Plane State: {arPlaneManager.enabled}");
    }

    private void ChageStateOfPlanes(bool state)
    {
        var planes = arPlaneManager.trackables;
        foreach (ARPlane arPlane in planes)
        {
            arPlane.gameObject.SetActive(state);
        }
    }


}
