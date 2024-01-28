using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class NPlayerManager : MonoBehaviour
{
    [HideInInspector] public NInputHandler InputHandler;
    [HideInInspector] public NPlayerMovement PlayerMovement;
    [HideInInspector] public NPlayerGrabbing PlayerGrabbing;
    [HideInInspector] public GrabbablePlayer GrabbablePlayer;
    [HideInInspector] public NPlayerAnimator PlayerAnimator;
    [HideInInspector] public RagdollController RagdollController;

    public Transform playerPawn;
    public Transform playerModel;
    [SerializeField] private int pressToUnStun = 3;
    [SerializeField] private Transform bucketModel;
    [SerializeField] private float bucketTime = 10f;
    [SerializeField] private float drunkTime = 10f;
    
    public bool isStun = false;
    private int pressCount = 0;
    
    private StunObject stunObject;
    
    private Transform spawnPoint;
    
    
    private void Awake()
    {
        InputHandler = GetComponent<NInputHandler>();
        PlayerMovement = GetComponentInChildren<NPlayerMovement>();
        PlayerGrabbing = GetComponentInChildren<NPlayerGrabbing>();
        PlayerAnimator = GetComponentInChildren<NPlayerAnimator>();
        RagdollController = GetComponentInChildren<RagdollController>();
        GrabbablePlayer = GetComponentInChildren<GrabbablePlayer>();
        
        DespawnPlayer();
    }

    private void Start()
    {
        if (NCoopManager.Instance.currentSceneType == SceneType.Gameplay)
        {
            InputHandler.OnUnStun += UnStan_pressed;
        }
        
        SpawnPlayer();
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        InputHandler.SubscribeToInputs(NCoopManager.Instance.currentSceneType);
        PlayerAnimator.SetAnimator(NCoopManager.Instance.currentSceneType);
    }

    private void UnStan_pressed()
    {
        if (!isStun) return;
        
        pressCount++;
        if (pressCount >= pressToUnStun)
        {
            UnStan(stunObject.playerPawnHolder.forward * 10f);
            pressCount = 0;
        }
    }

    public void Stun(StunObject _stunObject)
    {
        stunObject = _stunObject;
        isStun = true;
        
        RagdollController.DisableWithoutAnimation();
        
        RagdollController.playerCollider.enabled = false;
        RagdollController.playerRigidbody.isKinematic = true;
        
        PlayerMovement.canMove = false;
    }

    public void UnStan(Vector3 force)
    {
        isStun = false;
        stunObject.UnTrapPlayer();
        RagdollController.EnableRagdoll();
        RagdollController.pelvisRigidbody.AddForce(force, ForceMode.Impulse);
        RagdollController.DisableRagdollWithDelay(3f);
    }

    public void BucketOn()
    {
        PlayerMovement.isBucket = true;
        bucketModel.gameObject.SetActive(true);
        StartCoroutine(BucketOff());
    }
    
    private IEnumerator BucketOff()
    {
        yield return new WaitForSeconds(bucketTime);
        PlayerMovement.isBucket = false;
        bucketModel.gameObject.SetActive(false);
    }

    public void DrunkOn()
    {
        PlayerMovement.isDrunk = true;
        StartCoroutine(DrunkOff());
    }
    
    public void RestartDrunk()
    {
        StopCoroutine(DrunkOff());
        StartCoroutine(DrunkOff());
    }
    
    private IEnumerator DrunkOff()
    {
        yield return new WaitForSeconds(drunkTime);
        PlayerMovement.isDrunk = false;
    }

    public void FartOn()
    {
        PlayerMovement.isFart = true;
        StartCoroutine(FartOff());
    }
    
    private IEnumerator FartOff()
    {
        yield return new WaitForSeconds(3.0f);
        RagdollController.EnableRagdoll();
        yield return new WaitForSeconds(2.0f);
        RagdollController.DisableRagdoll();
        yield return new WaitForSeconds(3.0f);
        RagdollController.EnableRagdoll();
        yield return new WaitForSeconds(2.0f);
        RagdollController.DisableRagdoll();
        yield return new WaitForSeconds(3.0f);
        RagdollController.EnableRagdoll();
        yield return new WaitForSeconds(2.0f);
        RagdollController.DisableRagdoll();

        PlayerMovement.isFart = false;
    }

    public void DespawnPlayer()
    {
        playerPawn.gameObject.SetActive(false);
    }
    
    public void SpawnPlayer()
    {
        playerPawn.position = spawnPoint.position;
        playerPawn.rotation = spawnPoint.rotation;
        
        playerPawn.gameObject.SetActive(true);
    }
    
    public void SetSpawnPoint(Transform _spawnPoint)
    {
        spawnPoint = _spawnPoint;
    }
}
