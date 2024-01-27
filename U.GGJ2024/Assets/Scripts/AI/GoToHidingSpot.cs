using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoToHidingSpot : MonoBehaviour
{
    [SerializeField] HidingSpot chosenHidingSpot;

    public AIMovement aiMovement => GetComponent<AIMovement>();
    public Hide hide => GetComponent<Hide>();

    private void Start()
    {
        RandomizeHidingSpot();
    }

    private void Update()
    {
        UpdateHidingSpot();
    }

    private void UpdateHidingSpot()
    {
        if (aiMovement.TargetIsAvailable && aiMovement.targetReached)
        {
            Debug.Log("False");
            chosenHidingSpot.hidingObject = gameObject;
            //hide.SetActive3D(false, 2);
            chosenHidingSpot.isHidingHere = true;
            hide.IsHiding = true;
        }

        if (!aiMovement.TargetIsAvailable)
        {
            RandomizeHidingSpot();
        }
    }

    public void RandomizeHidingSpot()
    {
        if (hide.IsHiding) { return; }
        aiMovement.SetDestination(HidingSpotManager.instance.GetTheRightHidingSpot());

        if (aiMovement.target != null)
        {
            chosenHidingSpot = aiMovement.target.GetComponent<HidingSpot>();
        }
    }
}
