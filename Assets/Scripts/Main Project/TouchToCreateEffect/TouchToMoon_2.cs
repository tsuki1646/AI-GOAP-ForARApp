using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class TouchToMoon_2 : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
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

                if (target.name.Contains("Moon"))
                {
                    effectObj.SetActive(true);
                    IsPlaying = !IsPlaying;
                }
                if (IsPlaying)
                {
                    GetComponent<VisualEffect>().Play();
                }
                else
                {
                    GetComponent<VisualEffect>().Stop();
                }

            }
        }
    }
}
