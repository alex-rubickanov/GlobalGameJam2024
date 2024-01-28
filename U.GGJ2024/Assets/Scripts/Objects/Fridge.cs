using System.Collections;
using UnityEngine;

public class Fridge : StunObject
{
    [Range(-9, 9)]
    [SerializeField] private int points;
    [SerializeField] GameObject throwUI;
    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.GetComponentInParent<NPlayerManager>() == null) return;
        
        playerInside = other.transform.GetComponentInParent<NPlayerManager>();
        
        Debug.Log(playerInside.GrabbablePlayer.wasThrown);
        if (playerInside && !isOccupied && playerInside.GrabbablePlayer.wasThrown)
        {
            TrapPlayer(playerInside);
            UpdatePlayerPoints(playerInside);
            anim.PlayAnim(anim.PlayerEntered);
        }
    }
    private void Update()
    {
        if (isOccupied) 
        {
            throwUI.SetActive(false);
        }
        else
        {
            throwUI.SetActive(true);
        }
    }


    protected override void TrapPlayer(NPlayerManager playerInside)
    {
        TrappedSFX();
        isOccupied = true;
        playerInside.Stun(this);
        playerInside.playerModel.gameObject.SetActive(false);
        
        playerInside.playerPawn.transform.position = playerPawnHolder.position;
        playerInside.playerPawn.transform.rotation = playerPawnHolder.rotation;
    }
    
    public override void UnTrapPlayer()
    {
        playerInside.playerModel.gameObject.SetActive(true);
        playerInside = null;
        StartCoroutine(EnableCanWithDelay());
    }
    
    private void UpdatePlayerPoints(NPlayerManager trappedPlayer)
    {
        PointsGainUIManager.instance.ShowUIPoints(trappedPlayer.GrabbablePlayer.lastGrabbedByPlayer.playerManager.playerPawn, points);
    }

    public FridgeAnimator anim => GetComponent<FridgeAnimator>();


    //SFX
    void TrappedSFX()
    {
        AudioManager audioManager = AudioManager.instance;
        audioManager.PlayOneShotSfx(audioManager.playerTrappedSfx);
    }
}
