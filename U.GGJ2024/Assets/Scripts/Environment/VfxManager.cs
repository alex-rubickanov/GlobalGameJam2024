using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public GameObject[] vfxList;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnVFX(vfxList[0], 5f);
        }
    }
    public void PlayVFX(GameObject vfx)
    {
        vfx.SetActive(true);
    }

    public void SpawnVFX(GameObject vfx, float destroyTime)
    {
        GameObject fx =Instantiate(vfx,transform.position, Quaternion.identity);
        fx.SetActive(true);
        Destroy(fx,destroyTime);
    }
}
