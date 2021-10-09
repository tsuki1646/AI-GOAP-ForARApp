using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMaskTutorial : MonoBehaviour
{
    public LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            /*
            //Now, Layer 8: Enemy, Layer 9: Player
            if (Physics.Raycast(rayOrigin, out hitInfo, 100f, 1 << 8 | 1 << 9))
            {
                hitInfo.collider.GetComponent<Renderer>().material.color = Color.green;
            }
            */

            if (Physics.Raycast(rayOrigin, out hitInfo, 100f, enemyLayer.value))
            {
                hitInfo.collider.GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }
}
