using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

public class UIBlock: MonoBehaviour
{
    public bool IsOverUI(TouchState touch)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = touch.position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.GetComponent<CanvasRenderer>())
                {
                    return true;
                }
            }
        }

        return false;
    }
}
