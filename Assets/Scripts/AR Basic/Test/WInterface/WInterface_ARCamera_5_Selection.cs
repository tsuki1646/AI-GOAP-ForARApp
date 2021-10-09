using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class WInterface_ARCamera_5_Selection : MonoBehaviour
{
    public NavMeshSurface surface;
    public GameObject Environment;
    //ScaleAndRotateSlider scaleAndrotate;

    [SerializeField] private Button bookButton;
    [SerializeField] private Button tableButton;
    [SerializeField] private Button toiletButton;
    [SerializeField] private Camera arCamera;
    //[SerializeField] private Button dismissButton;

    //[SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject floor;
    [SerializeField] private Button toggleButton;

    [SerializeField] private TextMeshProUGUI selectionText;

    private GameObject placedPrefab;
    private GameObject PlacedPrefab
    {
        get
        {
            return placedPrefab;
        }
        set
        {
            placedPrefab = value;
        }
    }
    private GameObject spawnedObj;
    private ARRaycastManager arRaycastManager;
    private ARSessionOrigin aRSessionOrigin;

    //[SerializeField] private Color activeColor = Color.red;
    //[SerializeField] private Color inactiveColor = Color.gray;
    //private PlacementObject[] placedObjects;
    private PlacementObject lastSelectedObject;
    private bool onTouchHold = false;
    [SerializeField] private bool applyScalingPerObject = false;
    [SerializeField] private Slider scaleSlider;
    [SerializeField] private TextMeshProUGUI scaleTextValue;

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        aRSessionOrigin = GetComponent<ARSessionOrigin>();
        // set initial prefab
        ChangePrefabTo("Book_Test5");

        bookButton.onClick.AddListener(() => ChangePrefabTo("Book_Test5"));
        tableButton.onClick.AddListener(() => ChangePrefabTo("Table_Test5"));
        toiletButton.onClick.AddListener(() => ChangePrefabTo("Toilet_Test5"));
        //dismissButton.onClick.AddListener(Dismiss);

        //ChangeSelectedObject(placedObjects[0]);

        scaleSlider.onValueChanged.AddListener(ScaleChanged);
    }

    //private void Dismiss() => welcomePanel.SetActive(false);

    void ChangePrefabTo(string prefabName)
    {
        placedPrefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");

        if (placedPrefab == null)
        {
            Debug.LogError($"Prefab with name {prefabName} could not be loaded, make sure you check the naming of your prefabs...");
        }

        switch (prefabName)
        {
            case "ARBlue":
                selectionText.text = $"Selected: <color='blue'>{prefabName}</color>";
                break;
            case "ARRed":
                selectionText.text = $"Selected: <color='red'>{prefabName}</color>";
                break;
            case "ARGreen":
                selectionText.text = $"Selected: <color='green'>{prefabName}</color>";
                break;
        }
    }


    private void ScaleChanged(float newValue)
    {
        if (applyScalingPerObject)
        {
            if (lastSelectedObject != null && lastSelectedObject.Selected)
            {
                lastSelectedObject.transform.parent.localScale = Vector3.one * newValue;
            }
        }
        else
            aRSessionOrigin.transform.localScale = Vector3.one * newValue;

        scaleTextValue.text = $"Scale {newValue}";
    }

    public void Toggle()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
        var toggleButtonText = toggleButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        toggleButtonText.text = uiPanel.activeSelf ? "UI OFF" : "UI ON";
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    void Update()
    {
        //if (placedPrefab == null || welcomePanel.gameObject.activeSelf)
        if (placedPrefab == null) return;
        if (!TryGetTouchPosition(out Vector2 touchPosition)) return;
        Touch touch = Input.GetTouch(0);
        var touchPos = touch.position;

        //Block Raycast on UI
        //bool isOverUI = touchPos.IsPointOverUIObject();
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        if (touch.phase == TouchPhase.Began)
        {
            // rayを照射
            Ray ray = arCamera.ScreenPointToRay(touch.position);

            // Cubeをタップした場合は削除する
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {
                lastSelectedObject = hit.transform.GetComponent<PlacementObject>();
                if (lastSelectedObject != null)
                {
                    PlacementObject[] allOtherObjects = FindObjectsOfType<PlacementObject>();
                    foreach (PlacementObject placementObject in allOtherObjects)
                    {
                        if (placementObject != lastSelectedObject)
                        {
                            placementObject.Selected = false;
                        }
                        else
                            placementObject.Selected = true;
                    }
                }

            }


            //var isHitPlane = arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon);
            var isHitPlane = arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon);
            if (isHitPlane)
            {
                Pose pose = hits[0].pose;
                // 配置すべき座標
                Vector3 placePos = pose.position;
                Vector3 newPlacePos = new Vector3(placePos.x, floor.transform.position.y, placePos.z);
                /*
                if (lastSelectedObject != null)
                {
                    lastSelectedObject.transform.parent.position = newPlacePos; //MOVE OBJECT
                }
                else
                {
                    spawnedObj = Instantiate(placedPrefab, newPlacePos, Quaternion.identity);
                    
                    spawnedObj.transform.parent = Environment.transform;
                    surface.BuildNavMesh();
                    GWorld.Instance.GetQueue("toilets").AddResource(spawnedObj);
                    GWorld.Instance.GetWorld().ModifyState("FreeToilet", 1);
                }*/

                if (lastSelectedObject == null)
                {
                    lastSelectedObject = Instantiate(placedPrefab, newPlacePos, pose.rotation).GetComponent<PlacementObject>();
                }
                else
                {
                    lastSelectedObject.transform.parent.position = newPlacePos; //MOVE OBJECT
                }

            }
        }

        if (touch.phase == TouchPhase.Moved)
        {
            var isHitPlane = arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon);
            if (isHitPlane)
            {
                Pose pose = hits[0].pose;
                Vector3 placePos = pose.position;
                Vector3 newPlacePos = new Vector3(placePos.x, floor.transform.position.y, placePos.z);

                if (lastSelectedObject != null && lastSelectedObject.Selected)
                {
                    lastSelectedObject.transform.parent.position = newPlacePos;
                    lastSelectedObject.transform.parent.rotation = pose.rotation;
                }
            }
        }

        /*
        if (touch.phase == TouchPhase.Ended)
        {
            if (spawnedObj != null)
            {
                spawnedObj.transform.parent = Environment.transform;
                surface.BuildNavMesh();
                GWorld.Instance.GetQueue("toilets").AddResource(spawnedObj);
                GWorld.Instance.GetWorld().ModifyState("FreeToilet", 1);
            }
        }*/

    }    

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

}
