using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(ARRaycastManager))]
public class WInterface_ARCamera : MonoBehaviour
{
    [SerializeField] GameObject m_PlacedPrefab; //placementPrefab or newResourcePrefab
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }
    [SerializeField] private Camera arCamera;
    [SerializeField] private ARRaycastManager m_RaycastManager;
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private GameObject spawnedObj; //focusObj

    [SerializeField]int m_MaxNumberOfObjectsToPlace = 1;
    int m_NumberOfPlacedObjects = 0;

    [SerializeField] private GameObject uiPanel;
    [SerializeField] private Button toggleButton;
    [SerializeField] private TextMeshProUGUI log;

    //bool flg = false;
    //public static event Action onPlacedObject;

    //Vector3 placePos; // Selected location Vector3 goalPos

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    public void Toggle()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
        var toggleButtonText = toggleButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        toggleButtonText.text = uiPanel.activeSelf ? "UI OFF" : "UI ON";
    }

    private void Update()
    {
        if (Input.touchCount <= 0) return;

        var touch = Input.GetTouch(0);
        
        if (touch.phase == TouchPhase.Began)
        {
            //Block Raycast on UI
            var touchPos = touch.position;
            bool isOverUI = touchPos.IsPointOverUIObject();
            //bool isOverUI = touchPos.IsOverUI();

            /*Delete Object*/
            // arCameraカメラからrayを照射
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            // Cubeをタップした場合は削除する
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {
                var target = hit.collider.gameObject;
                if (target.name.Contains("Cube"))
                {
                    Destroy(target);
                    return;
                }
            }

            if (isOverUI)
            {
                log.text = $"{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")} blocked raycast";
            }

            /*Place Object*/
            // Cubeを配置
            var isHitPlane = m_RaycastManager.Raycast(ray, raycastHits, TrackableType.PlaneWithinPolygon);
            if (isHitPlane && !isOverUI)
            {
                // 複数のPlaneがあった場合、最も近いPlaneが0番目に入っている
                Pose pose = raycastHits[0].pose;
                // 配置すべき座標
                Vector3 placePos = pose.position;

                if (m_NumberOfPlacedObjects < m_MaxNumberOfObjectsToPlace)
                {
                    //spawnedObj = Instantiate(newResourcePrefab, placePos, Quaternion.identity);
                    spawnedObj = Instantiate(m_PlacedPrefab, placePos, m_PlacedPrefab.transform.rotation);
                    m_NumberOfPlacedObjects++;
                    /*Portal*/
                    //spawnedObj = Instantiate(newResourcePrefab, placePos, Quaternion.Euler(0, 0, 0));
                    //var rotationOfPortal = spawnedObj.transform.rotation.eulerAngles;
                    //spawnedObj.transform.eulerAngles = new Vector3(rotationOfPortal.x, Camera.main.transform.rotation.eulerAngles.y, rotationOfPortal.z);
                }
                else
                {
                    spawnedObj.transform.position = placePos;

                    /*Portal*/
                    //var rotationOfPortal = spawnedObj.transform.rotation.eulerAngles;
                    //spawnedObj.transform.eulerAngles = new Vector3(rotationOfPortal.x, Camera.main.transform.rotation.eulerAngles.y, rotationOfPortal.z);
                }

                //flg = true;
                /*
                if (onPlacedObject != null)
                {
                    onPlacedObject();
                }*/
            }
        }
    }
}
