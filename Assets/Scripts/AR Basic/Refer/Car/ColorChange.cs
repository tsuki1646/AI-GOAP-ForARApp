using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorChange : MonoBehaviour
{
    public Material objMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColor()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        if (name == "yeallow")
        {
            objMaterial.color = new Color(255 / 255f, 236 / 255f, 0 / 255f);
        }
        else if (name == "white")
        {
            objMaterial.color = new Color(255 / 255f, 255 / 255f, 0 / 255f);
        }
        else if(name == "red")
        {
            objMaterial.color = new Color(224 / 255f, 24 / 255f, 24 / 255f);
        }
        else if (name == "blue")
        {
            objMaterial.color = new Color(36 / 255f, 120 / 255f, 231 / 255f);
        }
    }
}
