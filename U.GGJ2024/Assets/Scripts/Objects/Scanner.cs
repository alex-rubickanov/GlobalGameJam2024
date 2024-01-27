using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    private NPlayerManager scannedPlayer;
    [SerializeField] private float sleepTime = 2.0f;
    private bool isCooldown = false;

    [SerializeField] private Transform facePhoto;
    [SerializeField] private Transform buttPhoto;
    [SerializeField] private Transform photoSpawnPoint;
    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.GetComponentInParent<NPlayerManager>() == null) return;
        
        scannedPlayer = other.transform.GetComponentInParent<NPlayerManager>();
        
        Debug.Log(scannedPlayer.GrabbablePlayer.wasThrown);
        if (scannedPlayer && !isCooldown && scannedPlayer.GrabbablePlayer.wasThrown)
        {
            ScanPlayer(scannedPlayer);
        }
    }

    private void ScanPlayer(NPlayerManager nPlayerManager)
    {
        isCooldown = true;
        StartCoroutine(Cooldown());
        
        int random = Random.Range(0, 101);
        if (random <= 20)
        {
            Transform buttPhotoObj = Instantiate(buttPhoto, photoSpawnPoint.position, photoSpawnPoint.rotation);
            Destroy(buttPhotoObj.gameObject, 2.0f);
        }
        else
        {
            Transform facePhotoObj = Instantiate(facePhoto, photoSpawnPoint.position, photoSpawnPoint.rotation);
            Destroy(facePhotoObj.gameObject, 2.0f);
        }
    }
    
    protected IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(sleepTime);
        isCooldown = false;
    }
}
