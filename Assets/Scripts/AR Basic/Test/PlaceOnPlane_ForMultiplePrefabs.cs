using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane_ForMultiplePrefabs : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    private float initialDistance;
    private Vector3 initialScale;

    [SerializeField] private Button Option1;
    [SerializeField] private Button Option2;
    [SerializeField] private Button Option3;

    [SerializeField] private TextMeshProUGUI selectionText;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();

        Option1.onClick.AddListener(() => ChangePrefabTo("Book"));
        Option2.onClick.AddListener(() => ChangePrefabTo("Table"));
        Option3.onClick.AddListener(() => ChangePrefabTo("Toilet"));
    }

    void ChangePrefabTo(string prefabName)
    {
        m_PlacedPrefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");

        if (placedPrefab == null)
        {
            Debug.LogError($"Prefab with name {prefabName} could not be loaded, " +
                $"make sure you check the naming of your prefabs...");
        }

        switch (prefabName)
        {
            case "Book":
                selectionText.text = $"Selected: <color='blue'>{prefabName}</color>";
            break;
            case "Table":
                selectionText.text = $"Selected: <color='blue'>{prefabName}</color>";
            break;
            case "Toilet":
                selectionText.text = $"Selected: <color='blue'>{prefabName}</color>";
            break;
        }
        
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
        if (placedPrefab == null)
        {
            return;
        }

        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
            }


        }

        //scale using pinch involves 2 touches
        //we need to count both the touches, store it somewhere, measure the distance between pinch
        //and scale gameobject depending on the pinch distance
        //we also need to ignore if the pinch distance is small(cases where 2 touches are registered accidently)

        if (Input.touchCount == 2)
        {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            //if any one of touchZero or touchOne is cancelled or maybe ended then do nothing
            if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled || touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
            {
                return; //basic do nothing
            }
            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                initialScale = spawnedObject.transform.localScale;
                Debug.Log("Initial Distance" + initialDistance + "Game Object:" + placedPrefab.name);
            }
            else // if touch is moved
            {
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
                //if accidentlly touched or pinch movement is very very small
                if (Mathf.Approximately(initialDistance, 0))
                {
                    return; //do nothing if it can be ignored where initial distance is very close to zero
                }

                var factor = currentDistance / initialDistance;
                spawnedObject.transform.localScale = initialScale * factor;
            }
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}
