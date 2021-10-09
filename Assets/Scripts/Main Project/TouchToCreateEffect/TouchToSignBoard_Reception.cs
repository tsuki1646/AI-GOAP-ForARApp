using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TouchToSignBoard_Reception : MonoBehaviour
{
    [SerializeField] private GameObject boardCanvas;
    void Awake()
    {
        boardCanvas.SetActive(false);
    }
    public void OnClick()
    {
        boardCanvas.SetActive(true);
    }
}
