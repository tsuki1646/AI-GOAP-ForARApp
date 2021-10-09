using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class ARPlacementInteractableMany : ARBaseGestureInteractable
{
    [SerializeField] private GameObject floor;
    [Tooltip("A GameObject to place when a raycast from a user touch hits a plane.")]
    public GameObject placementPrefab;

    [SerializeField]
    [Tooltip("Callback event executed after object is placed.")]
    private ARManyObjectPlacementEvent onObjectPlaced;

    private GameObject placementObject;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private static GameObject trackablesObject;

    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        if (gesture.StartPosition.IsPointOverUIObject())
        {
            return false;
        }

        // Allow for test planes
        if (gesture.TargetObject == null || gesture.TargetObject.layer == 9)
            return true;

        return false;
    }

    public void DestroyPlacementObject()
    {
        Destroy(placementObject);
    }

    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.WasCancelled)
            return;

        // If gesture is targeting an existing object we are done.
        // Allow for test planes
        if (gesture.TargetObject != null && gesture.TargetObject.layer != 9)
            return;

        // Raycast against the location the player touched to search for planes.
        if (GestureTransformationUtility.Raycast(gesture.StartPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0];

            //For tracking on the floor of environment
            Pose pose = hits[0].pose;
            // 配置すべき座標
            Vector3 placePos = pose.position;
            Vector3 newPlacePos = new Vector3(placePos.x, floor.transform.position.y, placePos.z);

            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if (Vector3.Dot(Camera.main.transform.position - hit.pose.position, hit.pose.rotation * Vector3.up) < 0)
                return;

            if (placementObject == null)
            {

                if (placementPrefab.tag == "Cube")
                {
                    placementObject = Instantiate(placementPrefab, newPlacePos, hit.pose.rotation);
                }
                else
                {
                    placementObject = Instantiate(placementPrefab, hit.pose.position, hit.pose.rotation);
                }
                //placementObject = Instantiate(placementPrefab, hit.pose.position, hit.pose.rotation);

                // Create anchor to track reference point and set it as the parent of placementObject.
                // TODO: this should update with a reference point for better tracking.
                var anchorObject = new GameObject("PlacementAnchor");

                if (placementPrefab.tag == "Cube")
                {
                    anchorObject.transform.position = newPlacePos;
                }
                else
                {
                    anchorObject.transform.position = hit.pose.position;
                }
                //anchorObject.transform.position = hit.pose.position;
                anchorObject.transform.rotation = hit.pose.rotation;
                placementObject.transform.parent = anchorObject.transform;


                // Find trackables object in scene and use that as parent
                if (trackablesObject == null)
                    trackablesObject = GameObject.Find("Trackables");
                if (trackablesObject != null)
                    anchorObject.transform.parent = trackablesObject.transform;

                onObjectPlaced?.Invoke(this, placementObject);
            }
        }
    }
}
