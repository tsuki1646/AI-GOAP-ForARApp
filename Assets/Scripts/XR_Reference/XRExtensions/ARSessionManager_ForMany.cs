using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;

public class ARSessionManager_ForMany : MonoBehaviour
{
    [SerializeField] private ARPlacementInteractableMany arPlacementInteractableMany;
    [SerializeField] private ARPlaneManager arPlaneManager;

    [SerializeField] private Button togglePlaneButton;
    [SerializeField] private Button ClearButton;

    public void ClearAllObjects()
    {
        Logger.Instance.LogInfo("ClearAllObjects executed");
        arPlacementInteractableMany.DestroyPlacementObject();

    }

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
