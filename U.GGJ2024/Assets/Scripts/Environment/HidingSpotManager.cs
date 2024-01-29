using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HidingSpotManager : MonoBehaviour
{
    public static HidingSpotManager instance;
    [SerializeField] HidingSpotList[] hidingSpotList;
    [SerializeField] Transform target;

    [Header("Debugging")]
    [SerializeField] GameObject prefab;
    [SerializeField] Transform spawnPoint;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        SpawnVirus();
    }

    /// <summary>
    /// For Debug
    /// </summary>
    private void SpawnVirus()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
    }

    //public Transform GetTheRightHidingSpot()
    //{
    //    for (int i = 0; i < furnitureList.Length; i++)
    //    {
    //        if (furnitureList[i].playerInside)
    //        {
    //            continue;
    //        }
    //        else
    //        {
    //            if (furnitureList[i].hidingSpots.Count > 0)
    //            {
    //                int randomHidingSpotIndex = Random.Range(0, furnitureList[i].hidingSpots.Count);

    //                //// Avoid selecting the same hiding spot as the current target
    //                while (furnitureList[i].hidingSpots[randomHidingSpotIndex].GetComponent<HidingSpot>().bug != null)
    //                {
    //                    randomHidingSpotIndex = Random.Range(0, furnitureList[i].hidingSpots.Count);
    //                }

    //                Transform hidingSpot = furnitureList[i].hidingSpots[randomHidingSpotIndex].transform;
    //                return hidingSpot;
    //            }
    //        }
    //    }
    //    return null;
    //}

    /// <summary>
    /// Returns A Hiding spot that is suitable to hide that no players are inside that room
    /// </summary>
    public Transform GetTheRightHidingSpot()
    {
        int randomRoom = Random.Range(0, hidingSpotList.Length);

        while (hidingSpotList[randomRoom].playerInside)
        {
            randomRoom = Random.Range(0, hidingSpotList.Length);
        }

        Transform hidingSpot = SelectRandomHidingSpot(hidingSpotList[randomRoom].hidingSpots);

        if (hidingSpot != null)
        {
            return hidingSpot;
        }
        return null;

        //foreach (var furniture in hidingSpotList)
        //{
        //    if (furniture.playerInside)
        //    {
        //        continue;
        //    }

        //    if (furniture.hidingSpots.Count == 0)
        //    {
        //        continue;
        //    }

        //    Transform hidingSpot = SelectRandomHidingSpot(furniture.hidingSpots);

        //    if (hidingSpot != null)
        //    {
        //        return hidingSpot;
        //    }
        //}
        //return null;
    }
    private Transform SelectRandomHidingSpot(List<GameObject> hidingSpots)
    {
        if (hidingSpots.All(spot => spot.GetComponent<HidingSpot>().hidingObject != null))
        {
            return null;
        }

        int randomHidingSpotIndex;
        do
        {
            randomHidingSpotIndex = Random.Range(0, hidingSpots.Count);
        } while (hidingSpots[randomHidingSpotIndex].GetComponent<HidingSpot>().hidingObject != null);

        return hidingSpots[randomHidingSpotIndex].transform;
    }
}
