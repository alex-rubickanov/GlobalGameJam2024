using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Virus")
        {
            AIDamageManager aiDamageManager = other.GetComponent<AIDamageManager>();

            if(aiDamageManager != null)
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
