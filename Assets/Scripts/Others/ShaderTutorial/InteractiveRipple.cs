using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveRipple : MonoBehaviour
{
    private Material material;
    private Color previousColor;

    private struct ShaderPropertyIDs
    {
        public int _BaseColor;
        public int _RippleColor;
        public int _RippleCenter;
        public int _RippleStartTime;
    }

    private ShaderPropertyIDs shaderProps;

    // Start is called before the first frame update
    private void Start()
    {
        //Duplicate the material so changes made at runtime are not remembered
        var renderer = GetComponent<MeshRenderer>();
        material = Instantiate(renderer.sharedMaterial);
        renderer.material = material;
        //material = GetComponent<MeshRenderer>().sharedMaterial;

        //Cache property IDs
        shaderProps = new ShaderPropertyIDs()
        {
            _BaseColor = Shader.PropertyToID("_BaseColor"),
            _RippleColor = Shader.PropertyToID("_RippleColor"),
            _RippleCenter = Shader.PropertyToID("_RippleCenter"),
            _RippleStartTime = Shader.PropertyToID("_RippleStartTime"),
        };

        //Set the base & ripple colors are equal so the ripple will not flash when the game look
        previousColor = material.GetColor("_BaseColor");
        material.SetColor("_RippleColor", previousColor);
    }

    private void OnDestroy()
    {
        if (material != null)
        {
            Destroy(material);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastClickRay();
        }
    }

    private void CastClickRay()
    {
        var camera = Camera.main;
        var mousePosition = Input.mousePosition;
        //The XY coordinates are in screen space, while the Z coordinate is in view space
        var ray = camera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, camera.nearClipPlane));
        //If our ray hits a collider, and that collider is attached to this game object
        if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject == gameObject)
        {
            StartRipple(hit.point);
        }
    }

    private void StartRipple(Vector3 center)
    {
        //Choose a random color
        Color rippleColor = Color.HSVToRGB(Random.value, 1, 1);

        material.SetVector(shaderProps._RippleCenter, center);
        //The Time.timeSceneLevelLoad value is the same as the Time node in shader graph
        material.SetFloat(shaderProps._RippleStartTime, Time.time);

        material.SetColor(shaderProps._BaseColor, previousColor);
        material.SetColor(shaderProps._RippleColor, rippleColor);

        //Store the current ripple color so we can set the base color to it next time
        previousColor = rippleColor;
    }
}
