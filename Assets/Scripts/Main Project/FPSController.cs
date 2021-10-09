using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public GameObject rabbit;
    Animator anim;

    public AudioClip sayHello;
    public AudioSource audioSource;

    //bool isPlaying;

    void Start()
    {
        anim = rabbit.GetComponent<Animator>();
        audioSource = rabbit.GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Door")
        {
            //do things
            /*
            if (!isPlaying)
            {
                isPlaying = true;
                anim.SetTrigger("Say");
                audioSource.Play();
                //sayHello
                //audioSource.PlayOneShot(sayHello, 1f);
            }*/
            audioSource.Play();
        }
    }
}
