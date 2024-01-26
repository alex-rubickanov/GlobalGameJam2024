using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Hide : MonoBehaviour
{
    [SerializeField] GameObject bugModel;
    public bool isHiding;
    public bool IsHiding { get => isHiding; set => isHiding = value; }

    public void SetHide(bool value)
    {
       StartCoroutine(Hide3DModel(value));
    }

    IEnumerator Hide3DModel(bool value)
    {
        yield return new WaitForSeconds(2);
        bugModel.SetActive(!value);
    }
}
