using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class MysteryBoxUI : MonoBehaviour
{
    [SerializeField] GameObject uiCanvas;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float sphereRad;
    // Update is called once per frame
    void Update()
    {
        EnableDisableUI();
    }

    private void EnableDisableUI()
    {
        if (Physics.CheckSphere(transform.position, sphereRad, playerLayer))
        {
            uiCanvas.SetActive(true);
        }
        else
        {
            uiCanvas.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRad);
    }
}
