using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class ARSelectionInteractable_Extend : ARBaseGestureInteractable
{
    ResourceData foData;
    public bool m_GestureSelected;

    private float timePressed = 0.0f;
    private float timeLastPress = 0.0f;
    public float timeDelayThreshold = 1.0f;


    /// <summary>
    /// The visualization game object that will become active when the object is selected.
    /// </summary>        
    [SerializeField, Tooltip("The GameObject that will become active when the object is selected.")]
    GameObject m_SelectionVisualization;
    public GameObject selectionVisualization { get { return m_SelectionVisualization; } set { m_SelectionVisualization = value; } }


    /// <summary>
    /// The Unity Update() method.
    /// </summary>
    void Update()
    {
    }

    /// <summary>
    /// Determines if this interactable can be selected by a given interactor.
    /// </summary>
    /// <param name="interactor">Interactor to check for a valid selection with.</param>
    /// <returns>True if selection is valid this frame, False if not.</returns>
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        if (!(interactor is ARGestureInteractor))
            return false;

        return m_GestureSelected;
    }

    /// <summary>
    /// Returns true if the manipulation can be started for the given gesture.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    /// <returns>True if the manipulation can be started.</returns>
    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        return true;
    }

    /// <summary>
    /// Function called when the manipulation is ended.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.WasCancelled)
            return;
        if (gestureInteractor == null)
            return;

        if (gesture.TargetObject == gameObject)
        {
            // Toggle selection
            m_GestureSelected = !m_GestureSelected;
        }
        else
            m_GestureSelected = false;
    }

    /// <summary>This method is called by the interaction manager 
    /// when the interactor first initiates selection of an interactable.</summary>
    /// <param name="interactor">Interactor that is initiating the selection.</param>
    protected override void OnSelectEnter(XRBaseInteractor interactor)
    {
        base.OnSelectEnter(interactor);

        if (m_SelectionVisualization != null)
            m_SelectionVisualization.SetActive(true);

        timePressed = Time.time - timeLastPress;
        

    }

    /// <summary>This method is called by the interaction manager 
    /// when the interactor ends selection of an interactable.</summary>
    /// <param name="interactor">Interactor that is ending the selection.</param>
    protected override void OnSelectExit(XRBaseInteractor interactor)
    {
        base.OnSelectExit(interactor);

        if (m_SelectionVisualization != null)
            m_SelectionVisualization.SetActive(false);

        if (timePressed > timeDelayThreshold)
        {
            // Is the time pressed greater than our time delay threshold?
            //Do whatever you want
            /*
            if (this.gameObject.tag == "Toilet" || this.gameObject.tag == "Table")
            {
                GWorld.Instance.GetQueue(foData.resourceQueue).RemoveResource(this.gameObject);
                GWorld.Instance.GetWorld().ModifyState(foData.resourceState, -1);
            }*/
            Destroy(this.gameObject);            
        }

    }
}
