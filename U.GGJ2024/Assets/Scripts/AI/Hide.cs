using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Hide : MonoBehaviour
{
    public GameObject bugModel;
    public bool isHiding;
    public bool IsHiding { get => isHiding; set => isHiding = value; }

    public void SetActive3D(bool value, float delay)
    {
        StartCoroutine(Set3D(value, delay));
    }

    public void SetActive3D(bool value)
    {
        bugModel.SetActive(value);
    }


    IEnumerator Set3D(bool value, float delay)
    {
        yield return new WaitForSeconds(delay);
        bugModel.SetActive(value);
    }
}
