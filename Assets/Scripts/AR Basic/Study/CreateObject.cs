using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class CreateObject : MonoBehaviour
{
    //生成するプレハブを追加
    [SerializeField] GameObject obj;

    //AR用にRayを発射するためのコンポーネント
    ARRaycastManager arRaycastManager;
    //ARRaycastManagerから発射されたRayにぶつかったオブジェクト情報を取得
    List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
    // Start is called before the first frame update

    void Start()
    {
        //arRayCastManagerを初期化 
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //もし画面がタップされたら
        if (Input.GetMouseButtonDown(0))
        {
            //画面をタップした場所からRayを発射、ぶつかったオブジェクトの情報をhitResultsに格納
            if (arRaycastManager.Raycast(Input.GetTouch(0).position, hitResults))
            {
                //ヒットした場所の座標情報を元にobjを生成
                Instantiate(obj, hitResults[0].pose.position, Quaternion.identity);
            }
        }
    }
}


