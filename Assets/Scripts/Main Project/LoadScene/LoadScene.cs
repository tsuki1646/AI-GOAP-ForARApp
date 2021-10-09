using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private GameObject arCamera;
    [SerializeField] private GameObject effectObj;
    [SerializeField] private VisualEffect vfxEffectObj;
    [SerializeField] int sceneNum;
    public bool IsPlaying = true;

    RaycastHit hit;
    void Start()
    {
        vfxEffectObj = GetComponent<VisualEffect>();
        effectObj.SetActive(false);
    }
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit))
            {
                if (hit.transform.tag == "Cube")
                {
                    Instantiate(effectObj, hit.transform.position, hit.transform.rotation);
                    effectObj.SetActive(true);
                    if (!IsPlaying)
                    {
                        IsPlaying = true;
                        vfxEffectObj.Play();
                        Invoke("StopEffect", 0.5f);
                    }
                    Destroy(effectObj, 0.2f);
                    Invoke("ChangeScene", 0.2f);
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
