using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;

public class PointCloudController : MonoBehaviour
{
    ARPointCloudManager m_ARPointCloudManager;

    //public Text buttonText;

    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField]
    GameObject screenspaceUI;

    private void Awake()
    {
        m_ARPointCloudManager = GetComponent<ARPointCloudManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TogglePlaneDetectionAndVisibility()
    {
        //with this way, arPlane detection is enabled disable
        //but if plane detection is disabled be enabled again
        m_ARPointCloudManager.enabled = !m_ARPointCloudManager.enabled;

        //If plane detection is enabled
        if (m_ARPointCloudManager.enabled)
        {
            SetAllPlaneActiveOrDeactive(true);
            GetComponent<PortalPlace2>().enabled = true;

            buttonText.text = "Disable Plane Detection And Hide Existing";
            screenspaceUI.SetActive(true);
        }
        else
        {
            SetAllPlaneActiveOrDeactive(false);
            GetComponent<PortalPlace2>().enabled = false;

            buttonText.text = "Enable Plane Detection And Show Existing";
            screenspaceUI.SetActive(false);

        }
        //*　⇧If this way, We will not be able to move the portal once we hide the detected place.
    }

    void SetAllPlaneActiveOrDeactive(bool value)
    {
        foreach (var pointCloud in m_ARPointCloudManager.trackables)
        {
            pointCloud.gameObject.SetActive(value);
        }
    }
}
