using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

/*Multiple Prefabs*/
[RequireComponent(typeof(ARRaycastManager))]
public class WInterface_ARCamera3 : MonoBehaviour
{
    [SerializeField] private Button bookButton;
    [SerializeField] private Button tableButton;
    [SerializeField] private Button toiletButton;
    [SerializeField] private Camera arCamera;
    //[SerializeField] private Button dismissButton;

    //[SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject floor;
    [SerializeField] private Button toggleButton;
    [SerializeField] private TextMeshProUGUI log;

    [SerializeField] private TextMeshProUGUI selectionText;

    private GameObject placedPrefab;
    private GameObject spawnedObj;
    private ARRaycastManager arRaycastManager;

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        // set initial prefab
        ChangePrefabTo("Book");

        bookButton.onClick.AddListener(() => ChangePrefabTo("Book"));
        tableButton.onClick.AddListener(() => ChangePrefabTo("Table"));
        toiletButton.onClick.AddListener(() => ChangePrefabTo("Toilet"));
        //dismissButton.onClick.AddListener(Dismiss);
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
        //Block Raycast on UI
        var touchPos = touch.position;
        bool isOverUI = touchPos.IsPointOverUIObject();

        if (isOverUI)
        {
            log.text = $"{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")} blocked raycast";
        }

        // rayを照射
        var ray = arCamera.ScreenPointToRay(touch.position);
        //var isHitPlane = arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon);
        var isHitPlane = arRaycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon);
        if (isHitPlane && !isOverUI)
        {
            Pose pose = hits[0].pose;
            // 配置すべき座標
            Vector3 placePos = pose.position;
            Vector3 newPlacePos = new Vector3(placePos.x, floor.transform.position.y, placePos.z);

            spawnedObj = Instantiate(placedPrefab, newPlacePos, Quaternion.identity);
        }

    }

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
}
