using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PortalPlace2 : MonoBehaviour
{
    [SerializeField] private GameObject placementPrefab;
    [SerializeField] private Camera arCamera;
    [SerializeField] private ARRaycastManager raycastManager;
    private GameObject spawnedPortal;
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    bool flg = false;

    public static event Action onPlacedObject;

    /*
    [SerializeField]
    int m_MaxNumberOfObjectsToPlace = 1;

    int m_NumberOfPlacedObjects = 0;
    */

    private void Update()
    {
        if (Input.touchCount <= 0) return;

        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            // rayを照射
            var ray = arCamera.ScreenPointToRay(touch.position);
            if (raycastManager.Raycast(ray, raycastHits, TrackableType.PlaneWithinPolygon))
            {
                // 複数のPlaneがあった場合、最も近いPlaneが0番目に入っている
                Pose pose = raycastHits[0].pose;
                // 配置すべき座標
                Vector3 placePosition = pose.position;

                //Instantiate(placementPrefab, placePosition, Quaternion.identity);
                
                if (spawnedPortal == null)
                {
                    spawnedPortal = Instantiate(placementPrefab, placePosition, Quaternion.Euler(0, 0, 0));

                    var rotationOfPortal = spawnedPortal.transform.rotation.eulerAngles;
                    spawnedPortal.transform.eulerAngles = new Vector3(rotationOfPortal.x, Camera.main.transform.rotation.eulerAngles.y, rotationOfPortal.z);
                }
                else
                {
                    spawnedPortal.transform.position = placePosition;

                    var rotationOfPortal = spawnedPortal.transform.rotation.eulerAngles;
                    spawnedPortal.transform.eulerAngles = new Vector3(rotationOfPortal.x, Camera.main.transform.rotation.eulerAngles.y, rotationOfPortal.z);
                }
                

                /*
                if (spawnedPortal == null)
                {
                    if (m_NumberOfPlacedObjects < m_MaxNumberOfObjectsToPlace)
                    {
                        spawnedPortal = Instantiate(placementPrefab, placePosition, Quaternion.Euler(0, 0, 0));

                        var rotationOfPortal = spawnedPortal.transform.rotation.eulerAngles;
                        spawnedPortal.transform.eulerAngles = new Vector3(rotationOfPortal.x, Camera.main.transform.rotation.eulerAngles.y, rotationOfPortal.z);

                        m_NumberOfPlacedObjects++;
                    }
                    else
                    {
                        spawnedPortal.transform.SetPositionAndRotation(pose.position, pose.rotation);

                        var rotationOfPortal = spawnedPortal.transform.rotation.eulerAngles;
                        spawnedPortal.transform.eulerAngles = new Vector3(rotationOfPortal.x, Camera.main.transform.rotation.eulerAngles.y, rotationOfPortal.z);
                    }
                    
                }
                else
                {
                    spawnedPortal.transform.position = placePosition;

                    var rotationOfPortal = spawnedPortal.transform.rotation.eulerAngles;
                    spawnedPortal.transform.eulerAngles = new Vector3(rotationOfPortal.x, Camera.main.transform.rotation.eulerAngles.y, rotationOfPortal.z);
                }*/

                if (onPlacedObject != null)
                {
                    onPlacedObject();
                }
            }
        }
    }
}
