using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AnimationCar : MonoBehaviour
{
    [SerializeField] private GameObject placementPrefab;
    [SerializeField] private Camera arCamera;
    [SerializeField] private ARRaycastManager raycastManager;
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    [SerializeField] private GameObject SeatCollider;
    [SerializeField] private GameObject LightCollider;

    Animator anim;

    bool flg = false;

    void Awake()
    {
        anim = placementPrefab.GetComponent<Animator>();
    }

    private void Update()
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
                /*
                if (target.name.Contains("Car"))
                {
                    Destroy(target);
                    return;
                }*/

                if (target.name.Contains("SeatCollider"))
                {
                    anim.SetBool("SeatUp", true);
                    anim.SetBool("LightUp", false);
                }
                else if (target.name.Contains("LightCollider"))
                {
                    anim.SetBool("SeatUp", false);
                    anim.SetBool("LightUp", true);
                }


            }

            // Cubeを配置
            var isHitPlane = raycastManager.Raycast(ray, raycastHits, TrackableType.PlaneWithinPolygon);
            if (isHitPlane && !flg)
            {
                // 複数のPlaneがあった場合、最も近いPlaneが0番目に入っている
                Pose pose = raycastHits[0].pose;
                // 配置すべき座標
                Vector3 placePosition = pose.position;
                Instantiate(placementPrefab, placePosition, Quaternion.identity);

                flg = true;
            }
        }
    }
}

