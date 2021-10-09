using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class DeleteObject_Test2 : MonoBehaviour
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

    private float initialDistance;
    private Vector3 initialScale;

    bool OneFingerTap;
    float TimerOne;
    float timerInterval = 0.1f; //the more you decrease it the more accurate it gets

    void Awake()
    {
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

        // get the first one
        Touch firstTouch = Input.GetTouch(0);
        if (OneFingerTap && (TimerOne + timerInterval <= Time.time))
        {
            HandleOneTapMovement();

            OneFingerTap = false;
        }

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
            Touch secondTouch = Input.GetTouch(1);
            if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
            {
                HandleTwoFingerMovement();
                OneFingerTap = false;
            }
            
        }//if Only one touch detected
        else if (firstTouch.phase == TouchPhase.Began)
        {
            TimerOne = Time.time;
            OneFingerTap = true;
        }

    }

    void HandleOneTapMovement()
    {
        //one tap function
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
    }

    //Pinch To Scale
    void HandleTwoFingerMovement()
    {
        //two tap function
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
