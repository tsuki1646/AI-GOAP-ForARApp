using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap_2 : MonoBehaviour
{
    public Transform player;
    [SerializeField] private Camera m_MiniMapCam;
    float sliderValue = 6;

    private Slider scaleSlider;
    float minOrthoSize = 5;
    float maxOrthoSize = 14;


    void Start()
    {
        m_MiniMapCam = GetComponent<Camera>();
        scaleSlider = GameObject.Find("ZoomSlider").GetComponent<Slider>();
        scaleSlider.minValue = minOrthoSize;
        scaleSlider.maxValue = maxOrthoSize;

        scaleSlider.onValueChanged.AddListener(ScaleSliderUpdate);
    }

    void OnGUI()
    {
        sliderValue = GUILayout.VerticalSlider(sliderValue, minOrthoSize, maxOrthoSize, GUILayout.Height(400));
    }

    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }

    void ScaleSliderUpdate(float sliderValue)
    {
        sliderValue = Mathf.Clamp(sliderValue - Input.GetAxisRaw("Mouse ScrollWheel") * 5f, minOrthoSize, maxOrthoSize);
        m_MiniMapCam.orthographicSize = sliderValue;
    }
}
