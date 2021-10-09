using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class SwitchPlacementPrefab_ForMany : MonoBehaviour
{
    public GameObject m_CubePrefab;
    public GameObject m_SpherePrefab;
    public GameObject m_CapsulePrefab;

    public ARPlacementInteractableMany m_PlacementInteractable;

    public void SwapToCube()
    {
        if (m_PlacementInteractable == null)
            return;

        m_PlacementInteractable.placementPrefab = m_CubePrefab;
    }

    public void SwapToSphere()
    {
        if (m_PlacementInteractable == null)
            return;

        m_PlacementInteractable.placementPrefab = m_SpherePrefab;
    }

    public void SwapToCylinder()
    {
        if (m_PlacementInteractable == null)
            return;

        m_PlacementInteractable.placementPrefab = m_CapsulePrefab;
    }
}
