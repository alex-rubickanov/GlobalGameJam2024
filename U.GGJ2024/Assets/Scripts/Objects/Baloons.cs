using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Baloons : MonoBehaviour
{
    [SerializeField] private float flyTime;
    private NPlayerManager takenPlayer;
    private FixedJoint joint;
    private bool once = false;
    private Rigidbody rb;
    private void Start()
    {
        joint = GetComponentInChildren<FixedJoint>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if(other.gameObject.layer == LayerMask.NameToLayer("CharacterBones"))
        {
            takenPlayer = other.gameObject.GetComponentInParent<NPlayerManager>();
            

            if (!once)
            {
                TakePlayerAndGoUp();
                once = true;
            }
        }
    }

    private void TakePlayerAndGoUp()
    {
        takenPlayer.RagdollController.EnableRagdoll();
        takenPlayer.RagdollController.AttachPelvisTo(joint);
        StartCoroutine(UpAndDestroy());
    }

    private IEnumerator UpAndDestroy()
    {
        rb.velocity = new Vector3(0, 3, 0);
        yield return new WaitForSeconds(flyTime);
        joint.connectedBody = null;
        takenPlayer.RagdollController.DisableRagdollWithDelay(3.0f);
        Destroy(gameObject);
    }
}
