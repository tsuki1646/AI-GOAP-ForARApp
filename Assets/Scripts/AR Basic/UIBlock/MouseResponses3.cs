using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseResponses3 : MonoBehaviour
{
    bool isRed;

    public void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            isRed = !isRed;
            if (isRed)
            {
                GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.white;
            }

        }
    }

    /*
     * Unity UI - Blocking clicks
     * www.youtube.com/watch?v=EVZiv7DLU6E
     */
}
