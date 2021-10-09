using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    public Transform parent;

    List<Transform> objectstoColor;

    void Start()
    {
        objectstoColor = new List<Transform>();

        foreach (Transform item in parent)
        {
            objectstoColor.Add(item);
        }
    } 

    //<summary>Method for changing the material color</summary>
    public void SwitchColors()
    {
        foreach (Transform item in objectstoColor)
        {
            Renderer rend = item.gameObject.GetComponent<Renderer>();
            rend.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }
}
