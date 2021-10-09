using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToSignBoard_Canvas : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private GameObject boardCanvas;
    RaycastHit hit;

    void Start()
    {
        //boardCanvas.SetActive(false);
    }
    void Update()
    {
        if (Input.touchCount <= 0) return;

        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            // arCameraカメラからrayを照射
            var ray = arCamera.ScreenPointToRay(touch.position);

            // Cubeをタップした場合は削除する
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {
                var target = hit.collider.gameObject;

                if (target.name.Contains("BoardCanvas"))
                {
                    boardCanvas.SetActive(false);
                }

            }
        }
    }
}
