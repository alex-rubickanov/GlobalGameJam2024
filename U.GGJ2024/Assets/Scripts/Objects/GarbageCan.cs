using UnityEngine;

public class GarbageCan : StunObject
{
    [Range(-9, 9)]
    [SerializeField] private int points;
    [SerializeField] GameObject throwUI;

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponentInParent<NPlayerManager>() == null) return;

        playerInside = other.transform.GetComponentInParent<NPlayerManager>();

        Debug.Log(playerInside.GrabbablePlayer.wasThrown);
        if (playerInside && !isOccupied && playerInside.GrabbablePlayer.wasThrown)
        {
            SFX();
            throwUI.SetActive(false);
            TrapPlayer(playerInside);
            UpdatePlayerPoints(playerInside);
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

    private void UpdatePlayerPoints(NPlayerManager trappedPlayer)
    {
        //PointsGainUIManager.instance.ShowUIPoints(
            //trappedPlayer.GrabbablePlayer.lastGrabbedByPlayer.playerManager.playerPawn, points);
    }
    
    //SFX
    void SFX()
    {
        AudioManager audioManager = AudioManager.instance;
        audioManager.PlayOneShotSfx(audioManager.playerTrappedSfx);
    }
}