using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;

public class PlaceOnMesh : MonoBehaviour
{
    [SerializeField] private ARMeshManager meshManager = null;
    [SerializeField] private Camera arCamera = null;
    [SerializeField] private Material invisibleMaterial = null;
    [SerializeField] private Material visibleMaterial = null;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Slider prefabSize;
    [SerializeField] private TextMeshProUGUI prefabSizeLabel = null;
    [SerializeField] private TextMeshProUGUI visibilityStatus = null;
    [SerializeField] private LayerMask layersToInclude;
    //private GameObject mesh;

    private bool IsVisible { get; set; } = true;

    // Start is called before the first frame update
    void Awake()
    {
        prefabSize.onValueChanged.AddListener(PrefabSizeChanged);
    }

    public void PrefabSizeChanged(float newSize)
    {
        prefabSizeLabel.text = $"SIZE{newSize}";
    }

    public void ToggleMesh()
    {
        var meshes = meshManager.meshes;
        var meshPrefabRenderer = meshManager.meshPrefab.GetComponent<MeshRenderer>();

        if (IsVisible)
        {
            meshPrefabRenderer.material = invisibleMaterial;

            foreach(var mesh in meshes)
            {
                mesh.GetComponent<MeshRenderer>().material = invisibleMaterial;
            }

            IsVisible = false;
            visibilityStatus.text = "Mesh Visibility (ON)";
        }
        else
        {
            meshPrefabRenderer.material = visibleMaterial; 

            foreach(var mesh in meshes)
            {
                mesh.GetComponent<MeshRenderer>().material = visibleMaterial;
            }

            IsVisible = true;
            visibilityStatus.text = "Mesh Visibility (OFF)";
        }
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            var touchPhase = touch.phase;

            if(touchPhase == TouchPhase.Began || touchPhase == TouchPhase.Moved)
            {
                var ray = arCamera.ScreenPointToRay(touch.position);
                var hasHit = Physics.Raycast(ray, out var hit, float.PositiveInfinity, layersToInclude);

                if (hasHit && prefabs.Length > 0)
                {
                    //GameObject newObject = null;
                    Vector2 scale = Vector3.one * prefabSize.value;

                    Quaternion newObjectRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                    int selector = Random.Range(0, prefabs.Length);
                    GameObject newObject = GameObject.Instantiate(prefabs[selector], hit.point, newObjectRotation);

                    newObject.transform.localScale = scale;
                    newObject.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
            }
        }
    }
}
