using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class DeleteObject_Test : MonoBehaviour
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

    [SerializeField] private Camera arCamera;
    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }
    public int TapCount { get; private set; }

    private float initialDistance;
    private Vector3 initialScale;

    //int TapCount;
    public float MaxDoubleTapTime;
    float NewTime;

    bool flg = false;

    void Awake()
    {
        TapCount = 0;
        m_RaycastManager = GetComponent<ARRaycastManager>();
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
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            //TapCount += 1;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                spawnedObject.transform.position = hitPose.position;
            }

            //DeleteObj();
            DeleteByMultiTouch();

        }

        if (Input.touchCount == 2)
        {
            FingerMove();
        }
        

    }

    /*
    void DeleteObj()
    {
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
                return;
            }
        }
    }*/

    
    void DeleteByMultiTouch()
    {
        var touch = Input.GetTouch(0);
        //Double Tap - Start
        
        if (touch.phase == TouchPhase.Ended)
        {
            TapCount += 1;
        }

        if (TapCount == 1)
        {
            NewTime = Time.time + MaxDoubleTapTime;
        }
        else if (TapCount == 2 && Time.time <= NewTime)
        {
            //Whatever you want after a dubble tap    
            print("Double tap");
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
                    return;
                }
            }

            TapCount = 0;
        }

        if (Time.time > NewTime)
        {
            TapCount = 0;
        }
        //Double- End
    }

    void FingerMove()
    {
        //scale using pinch involves 2 touches
        //we need to count both the touches, store it somewhere, measure the distance between pinch
        //and scale gameobject depending on the pinch distance
        //we also need to ignore if the pinch distance is small(cases where 2 touches are registered accidently)

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

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}
