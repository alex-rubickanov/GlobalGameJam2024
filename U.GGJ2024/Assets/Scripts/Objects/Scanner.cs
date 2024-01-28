using System.Collections;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [Range(-9, 9)][SerializeField] private int pointsIfFace;
    [Range(-9, 9)][SerializeField] private int pointsIfButt;
    private NPlayerManager scannedPlayer;
    [SerializeField] private float sleepTime = 2.0f;
    private bool isCooldown = false;

    [SerializeField] private Transform facePhoto;
    [SerializeField] private Transform buttPhoto;
    [SerializeField] private Transform photoSpawnPoint;
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponentInParent<NPlayerManager>() == null) return;

        scannedPlayer = other.transform.GetComponentInParent<NPlayerManager>();

        Debug.Log(scannedPlayer.GrabbablePlayer.wasThrown);
        if (scannedPlayer && !isCooldown && scannedPlayer.GrabbablePlayer.wasThrown)
        {
            ScanPlayer(scannedPlayer);
        }
    }

    private void ScanPlayer(NPlayerManager nPlayerManager)
    {
        SFX();
        isCooldown = true;
        StartCoroutine(Cooldown());

        int random = Random.Range(0, 101);
        if (random <= 20)
        {
            Transform buttPhotoObj = Instantiate(buttPhoto, photoSpawnPoint.position, photoSpawnPoint.rotation);
            //UpdatePlayerPointsFace(nPlayerManager);
            Destroy(buttPhotoObj.gameObject, 2.0f);
        }
        else
        {
            //UpdatePlayerPointsButt(nPlayerManager);
            Transform facePhotoObj = Instantiate(facePhoto, photoSpawnPoint.position, photoSpawnPoint.rotation);
            Destroy(facePhotoObj.gameObject, 2.0f);
        }
    }

    private void UpdatePlayerPointsFace(NPlayerManager scannedPlayer)
    {
        PointsGainUIManager.instance.ShowUIPoints(
            scannedPlayer.GrabbablePlayer.lastGrabbedByPlayer.playerManager.playerPawn, pointsIfFace);
    }

    private void UpdatePlayerPointsButt(NPlayerManager scannedPlayer)
    {
        PointsGainUIManager.instance.ShowUIPoints(
            scannedPlayer.GrabbablePlayer.lastGrabbedByPlayer.playerManager.playerPawn, pointsIfButt);
    }

    protected IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(sleepTime);
        isCooldown = false;
    }

    //SFX
    void SFX()
    {
        AudioManager audioManager = AudioManager.instance;
        audioManager.PlayOneShotSfx(audioManager.scanSFX);
    }
}
