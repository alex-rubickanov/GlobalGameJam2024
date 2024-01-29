using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpotList : MonoBehaviour
{
    public List<GameObject> hidingSpots;
    public bool playerInside;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<HidingSpot>() != null)
        {
            hidingSpots.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hidingSpots.Contains(other.gameObject))
        {
            hidingSpots.Remove(other.gameObject);
        }

        if (other.tag == "Player")
        {
            playerInside = false;
        }
    }
}
