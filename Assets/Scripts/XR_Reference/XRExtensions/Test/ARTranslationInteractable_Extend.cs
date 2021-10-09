using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

[RequireComponent(typeof(ARSelectionInteractable_Extend))]
public class ARTranslationInteractable_Extend : ARBaseGestureInteractable
{
    private GameObject floor;
    [SerializeField]
    [Tooltip("Controls whether the object will be constrained vertically, horizontally, or free to move in all axis.")]
    GestureTransformationUtility.GestureTranslationMode m_ObjectGestureTranslationMode;
    /// <summary>
    /// The translation mode of this object.
    /// </summary>
    public GestureTransformationUtility.GestureTranslationMode objectGestureTranslationMode { get { return m_ObjectGestureTranslationMode; } set { m_ObjectGestureTranslationMode = value; } }

    [SerializeField]
    [Tooltip("The maximum translation distance of this object.")]
    float m_MaxTranslationDistance = 10.0f;
    /// <summary>
    /// The maximum translation distance of this object.
    /// </summary>
    public float maxTranslationDistance { get { return m_MaxTranslationDistance; } set { m_MaxTranslationDistance = value; } }

    const float k_PositionSpeed = 12.0f;
    const float k_DiffThreshold = 0.0001f;

    bool m_IsActive = false;

    Vector3 m_DesiredLocalPosition;
    float m_GroundingPlaneHeight;
    float m_GroundingPlaneHeight_floor;
    Vector3 m_DesiredAnchorPosition;
    Quaternion m_DesiredRotation;
    GestureTransformationUtility.Placement m_LastPlacement;

    /// <summary>
    /// The Unity's Start method.
    /// </summary>
    protected void Start()
    {
        m_DesiredLocalPosition = new Vector3(0, 0, 0);
        floor = GameObject.Find("Environment");
    }

    /// <summary>
    /// The Unity's Update method.
    /// </summary>
    void Update()
    {
        UpdatePosition();
    }

    /// <summary>
    /// Returns true if the manipulation can be started for the given gesture.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    /// <returns>True if the manipulation can be started.</returns>
    protected override bool CanStartManipulationForGesture(DragGesture gesture)
    {
        if (gesture.TargetObject == null)
        {
            return false;
        }

        // If the gesture isn't targeting this item, don't start manipulating.
        if (gesture.TargetObject != gameObject)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Function called when the manipulation is started.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    protected override void OnStartManipulation(DragGesture gesture)
    {
        m_GroundingPlaneHeight = transform.parent.position.y;
    }

    /// <summary>
    /// Continues the translation.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    protected override void OnContinueManipulation(DragGesture gesture)
    {
        Debug.Assert(transform.parent != null, "Translate interactable needs a parent object.");
        m_IsActive = true;

        GestureTransformationUtility.Placement desiredPlacement =
            GestureTransformationUtility.GetBestPlacementPosition(
                transform.parent.position, gesture.Position, m_GroundingPlaneHeight, 0.03f,
                maxTranslationDistance, objectGestureTranslationMode);

        if (desiredPlacement.HasHoveringPosition && desiredPlacement.HasPlacementPosition)
        {
            // If desired position is lower than current position, don't drop it until it's finished.
            m_DesiredLocalPosition = transform.parent.InverseTransformPoint(desiredPlacement.HoveringPosition);
            m_DesiredAnchorPosition = desiredPlacement.PlacementPosition;

            m_GroundingPlaneHeight = desiredPlacement.UpdatedGroundingPlaneHeight;

            // Rotate if the plane direction has changed.
            if (((desiredPlacement.PlacementRotation * Vector3.up) - transform.up).magnitude > k_DiffThreshold)
                m_DesiredRotation = desiredPlacement.PlacementRotation;
            else
                m_DesiredRotation = transform.rotation;

            if (desiredPlacement.HasPlane)
                m_LastPlacement = desiredPlacement;
            
        }
    }

    /// <summary>
    /// Finishes the translation.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    protected override void OnEndManipulation(DragGesture gesture)
    {
        if (!m_LastPlacement.HasPlacementPosition)
            return;

        GameObject oldAnchor = transform.parent.gameObject;

        Pose desiredPose = new Pose(m_DesiredAnchorPosition, m_LastPlacement.PlacementRotation);
        
        Vector3 desiredLocalPosition = transform.parent.InverseTransformPoint(desiredPose.position);

        if (desiredLocalPosition.magnitude > maxTranslationDistance)
            desiredLocalPosition = desiredLocalPosition.normalized * maxTranslationDistance;
        desiredPose.position = transform.parent.TransformPoint(desiredLocalPosition);

        //Anchor newAnchor = m_LastPlacement.Trackable.CreateAnchor(desiredPose);
        var anchorGO = new GameObject("PlacementAnchor");

        Vector3 placePos = m_LastPlacement.PlacementPosition;
        Vector3 newPlacePos = new Vector3(placePos.x, floor.transform.position.y, placePos.z);

        if (this.gameObject.tag == "Cube" || this.gameObject.tag == "Table" || this.gameObject.tag == "Toilet")
        {
            anchorGO.transform.position = newPlacePos;
        }
        else
        {
            anchorGO.transform.position = m_LastPlacement.PlacementPosition;
        }

        //anchorGO.transform.position = m_LastPlacement.PlacementPosition;
        anchorGO.transform.rotation = m_LastPlacement.PlacementRotation;
        transform.parent = anchorGO.transform;

        Destroy(oldAnchor);

        m_DesiredLocalPosition = Vector3.zero;

        // Rotate if the plane direction has changed.
        if (((desiredPose.rotation * Vector3.up) - transform.up).magnitude > k_DiffThreshold)
            m_DesiredRotation = desiredPose.rotation;
        else
            m_DesiredRotation = transform.rotation;

        // Make sure position is updated one last time.
        m_IsActive = true;
    }

    void UpdatePosition()
    {
        if (!m_IsActive)
            return;

        // Lerp position.
        Vector3 oldLocalPosition = transform.localPosition;        
        Vector3 newLocalPosition = Vector3.Lerp(
            oldLocalPosition, m_DesiredLocalPosition, Time.deltaTime * k_PositionSpeed);

        float diffLenght = (m_DesiredLocalPosition - newLocalPosition).magnitude;

        if (diffLenght < k_DiffThreshold)
        {            
            newLocalPosition = m_DesiredLocalPosition;                       
            m_IsActive = false;
        }

        Vector3 placePos2 = newLocalPosition;
        Vector3 newPlacePos2 = new Vector3(placePos2.x, floor.transform.position.y, placePos2.z);

        if (this.gameObject.tag == "Cube" || this.gameObject.tag == "Table" || this.gameObject.tag == "Toilet")
        {
            transform.localPosition = newPlacePos2;
        }
        else
        {
            transform.localPosition = newLocalPosition;
        }        

        // Lerp rotation.
        Quaternion oldRotation = transform.rotation;
        Quaternion newRotation =
            Quaternion.Lerp(oldRotation, m_DesiredRotation, Time.deltaTime * k_PositionSpeed);
        transform.rotation = newRotation;
    }
}
