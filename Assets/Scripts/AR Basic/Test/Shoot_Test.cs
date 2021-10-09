using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Test : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private Camera arCamera;

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
                    Destroy(target);
                    Instantiate(explosion, hit.transform.position, hit.transform.rotation);
                    Destroy(explosion, 2f);  // nothing gets left behind
                    return;
                }
                /*
                if (target.name.Contains("Cube"))
                {
                    //anim.SetBool("SeatUp", true);
                    //anim.SetBool("LightUp", false);
                }*/

                /*
                if (hit.transform.tag == "Cube")
                {
                    Destroy(hit.transform.gameObject);
                    Instantiate(explosion, hit.transform.position, hit.transform.rotation);
                    Destroy(explosion, 2f);  // nothing gets left behind
                }*/

            }

            /*
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
            }*/
        }
    }
}
