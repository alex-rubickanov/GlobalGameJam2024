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
            chosenHidingSpot.hidingObject = gameObject;
            hide.SetHide(true);
            chosenHidingSpot.isHidingHere = true;
            hide.IsHiding = true;
        }
        else if (!aiMovement.targetReached)
        {
            hide.SetHide(false);
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
