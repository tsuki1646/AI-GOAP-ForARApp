using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class Shoot_Test2 : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    //public GameObject explosion;

    RaycastHit hit;
    void Start()
    {
        //arCamera = GetComponent<Camera>();
    }
    void Update()
    {
        /*
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit))
            {
                if (hit.transform.tag == "Cube")
                {
                    Destroy(hit.transform.gameObject);
                    Instantiate(explosion, hit.transform.position, hit.transform.rotation);
                    Destroy(explosion, 2f);  // nothing gets left behind
                }
            }
        }*/

        var touch = Input.GetTouch(0);
        // arCameraカメラからrayを照射
        var ray = arCamera.ScreenPointToRay(touch.position);

        // Cubeをタップした場合は削除する
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            var target = hit.collider.gameObject;

            if (target.name.Contains("Cube"))
            {
                Destroy(target);
                //Instantiate(explosion, hit.transform.position, hit.transform.rotation);
                //Destroy(explosion, 2f);  // nothing gets left behind
                return;
            }
        }

        /*
        if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit))
        {
            if (hit.transform.tag == "Cube")
            {
                Destroy(hit.transform.gameObject);
                Instantiate(explosion, hit.transform.position, hit.transform.rotation);
                Destroy(explosion, 2f);  // nothing gets left behind
            }
        }*/
    }
}
