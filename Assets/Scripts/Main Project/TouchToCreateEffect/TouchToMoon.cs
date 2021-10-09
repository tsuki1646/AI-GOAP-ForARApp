using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class TouchToMoon : MonoBehaviour
{
    [SerializeField] private GameObject arCamera;
    [SerializeField] private GameObject effectObj;
    //[SerializeField] private GameObject characterGroup;
    [SerializeField] private VisualEffect vfxEffectObj;
    public bool IsPlaying = true;
    public static readonly string SPAWN_RATE_NAME = "spawn rate";

    RaycastHit hit;
    void Start()
    {
        effectObj.SetActive(false);
        vfxEffectObj = GetComponent<VisualEffect>();
    }
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit))
            {
                if (hit.transform.tag == "Moon")
                {
                    IsPlaying = !IsPlaying;
                    
                }
                if (!IsPlaying) //Start
                {
                    IsPlaying = true;
                    effectObj.SetActive(true);
                    //Instantiate(effectObj, hit.transform.position, hit.transform.rotation);                   
                    //vfxEffectObj.Play();
                    //vfxEffectObj.SetInt(SPAWN_RATE_NAME, 100);
                }
                else //Stop
                {
                    IsPlaying = false;
                    effectObj.SetActive(false);
                    //vfxEffectObj.SetInt(SPAWN_RATE_NAME, 0);
                    //vfxEffectObj.Stop();
                    //Destroy(vfxEffectObj, 0.2f);
                }

            }
        }
    }
}
