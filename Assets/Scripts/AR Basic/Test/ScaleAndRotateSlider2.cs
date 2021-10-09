using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleAndRotateSlider2 : MonoBehaviour
{
    private PlacementObject placementObject;
    private bool initialized = false;
    // we need two sliders
    // we need min and max values of each

    private Slider scaleSlider;
    private Slider rotateSlider;
    public float scaleMinValue;
    public float scaleMaxValue;
    public float rotMinValue;
    public float rotMaxValue;

    void Awake()
    {
        SetupSelection();
        scaleSlider.enabled = false;
        rotateSlider.enabled = false;
    }

    void SetupSelection ()
    {
        placementObject = GetComponent<PlacementObject>();
        initialized = true;
    }

    void Start()
    {
        // find the sliders by name
        //initialize the max and min value when starting
        // Add a listener to the slider when value is changed
        
        scaleSlider = GameObject.Find("ScaleSlider").GetComponent<Slider>();
        scaleSlider.minValue = scaleMinValue;
        scaleSlider.maxValue = scaleMaxValue;

        scaleSlider.onValueChanged.AddListener(ScaleSliderUpdate);


        rotateSlider = GameObject.Find("RotateSlider").GetComponent<Slider>();
        rotateSlider.minValue = rotMinValue;
        rotateSlider.maxValue = rotMaxValue;

        rotateSlider.onValueChanged.AddListener(RotateSliderUpdate);

    }

    void Update()
    {
        if (this.gameObject == placementObject.Selected)
        {
            
            CanSelectable(placementObject.Selected);
        }        
    }

    void CanSelectable(bool isActive)
    {
        //ScaleSliderUpdate();

        scaleSlider.enabled = true;
        rotateSlider.enabled = true;

        
    }

    void ScaleSliderUpdate(float value)
    {
        transform.localScale = new Vector3(value, value, value);
    }

    void RotateSliderUpdate(float value)
    {
        transform.localEulerAngles = new Vector3(transform.rotation.x, value, transform.rotation.z);
    }


}
