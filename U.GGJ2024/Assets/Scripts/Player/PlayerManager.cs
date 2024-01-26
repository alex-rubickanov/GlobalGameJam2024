using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private GameObject playerPawn;
    [SerializeField] private GameObject playerModel;
    public Transform spawnPoint;

    
    public PlayerInput PlayerInput => GetComponent<PlayerInput>();
    public InputHandler InputHandler => GetComponent<InputHandler>();
    public PlayerMovement PlayerMovement => playerPawn.GetComponent<PlayerMovement>();
    public PlayerGrabbing PlayerGrabbing => playerPawn.GetComponent<PlayerGrabbing>();
    public PlayerHealth PlayerHealth => playerPawn.GetComponent<PlayerHealth>();
    public MeleeCombat PlayerMelee => playerPawn.GetComponent<MeleeCombat>();
    public PlayerAnimator PlayerAnimator => playerPawn.GetComponentInChildren<PlayerAnimator>();
    
    //private bool isReady = false;
    
    private void Awake()
    {
        playerPawn.SetActive(false);
        
        SpawnPlayer();
    }

    public void KillPlayer()
    {
        PlayerInput.DeactivateInput();
        playerModel.SetActive(false);
        StartCoroutine(RespawnCoroutine());
    }

    private void SpawnPlayer()
    {
        playerPawn.transform.position = spawnPoint.position;
        playerPawn.transform.rotation = spawnPoint.rotation;
        
        playerPawn.SetActive(true);
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        SpawnPlayer();
        playerModel.SetActive(true);
        PlayerInput.ActivateInput();
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }

    public bool CanAttack()
    {
        return !PlayerGrabbing.isGrabbed && !PlayerGrabbing.isGrabbing;
    }
    
    public void SetPlayerReady()
    {
        //isReady = true;
    }
}