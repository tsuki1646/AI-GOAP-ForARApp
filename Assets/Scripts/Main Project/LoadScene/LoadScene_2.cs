using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class LoadScene_2 : MonoBehaviour
{
    //[SerializeField] private GameObject arCamera;
    [SerializeField] private Camera arCamera;
    [SerializeField] private GameObject effectObj;
    [SerializeField] private VisualEffect vfxEffectObj;
    //[SerializeField] private ARRaycastManager raycastManager;
    //private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    [SerializeField] int sceneNum;
    public bool IsPlaying = false;

    RaycastHit hit;
    void Start()
    {
        vfxEffectObj = GetComponent<VisualEffect>();
    }
    void Update()
    {
        if (Input.touchCount <= 0) return;

        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
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
                    Instantiate(effectObj, hit.transform.position, hit.transform.rotation);
                    effectObj.SetActive(true);
                    if (!IsPlaying)
                    {
                        IsPlaying = true;
                        vfxEffectObj.Play();
                        Invoke("StopEffect", 1f);
                    }
                    Destroy(effectObj, 0.5f);
                    Invoke("ChangeScene", 1.5f);
                    //return;
                }
            }
        }
    }

    public void StopEffect()
    {
        vfxEffectObj.Stop();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneNum);
        
    }
}
