using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class SwitchPlacementPrefab_Test : MonoBehaviour
{
    // Storage for all the resources.
    public GameObject[] allResources;

    public GameObject m_CubePrefab;
    public GameObject m_SpherePrefab;
    public GameObject m_CapsulePrefab;

    public GameObject m_BookPrefab;
    public GameObject m_TablePrefab;
    public GameObject m_ToiletPrefab;



    public ARPlacementInteractableSingle m_PlacementInteractable;

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

    public void SwapToBook()
    {
        if (m_PlacementInteractable == null)
            return;

        m_PlacementInteractable.placementPrefab = m_BookPrefab;
    }

    public void SwapToTable()
    {
        if (m_PlacementInteractable == null)
            return;

        m_PlacementInteractable.placementPrefab = m_TablePrefab;
    }

    public void SwapToToilet()
    {
        if (m_PlacementInteractable == null)
            return;

        m_PlacementInteractable.placementPrefab = m_ToiletPrefab;
    }

}

