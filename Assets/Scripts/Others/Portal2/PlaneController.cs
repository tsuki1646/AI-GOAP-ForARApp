using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;

    public Text buttonText;

    private void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
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
        m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;

        //If plane detection is enabled
        if (m_ARPlaneManager.enabled)
        {
            SetAllPlaneActiveOrDeactive(true);
            GetComponent<PortalPlace>().enabled = true;

            buttonText.text = "Disable Plane Detection And Hide Existing";
        }
        else
        {
            SetAllPlaneActiveOrDeactive(false);
            GetComponent<PortalPlace>().enabled = false;

            buttonText.text = "Enable Plane Detection And Show Existing";
        }
        //*　⇧If this way, We will not be able to move the portal once we hide the detected place.
    }

    void SetAllPlaneActiveOrDeactive(bool value)
    {
        foreach(var plane in m_ARPlaneManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }
}
