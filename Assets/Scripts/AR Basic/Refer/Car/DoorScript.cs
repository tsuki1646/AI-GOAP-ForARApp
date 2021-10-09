using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    Animation anim;
    bool doorStatus = false;

    void Start()
    {
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        if (doorStatus == false)
        {
            anim["DoorAnim"].speed = 1;
            anim.Play();

            doorStatus = true;
        }
        else
        {
            anim["DoorAnim"].speed = -1;
            anim["DoorAnim"].time = anim["DoorAnim"].length;
            anim.Play();

            doorStatus = false;
        }
    }
}
