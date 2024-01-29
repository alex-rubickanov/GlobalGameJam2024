using UnityEngine;

public class BoneRagdollTrigger : MonoBehaviour
{
    private RagdollController ragdollController;
    private float throwHitPowerMultiplier = 50f;

    private void Start()
    {
        ragdollController = GetComponentInParent<RagdollController>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CharacterBones") && ragdollController.isRagdoll)
        {
            NPlayerManager playerManager = other.gameObject.GetComponentInParent<NPlayerManager>();

            if (ragdollController.ragdollBonesList.Contains(other.transform.GetComponent<Rigidbody>())) return;
            if(ragdollController.playerManager.GrabbablePlayer.isGrabbed|| ragdollController.pelvisRigidbody.velocity.magnitude < 20) return;
            Rigidbody boneRigidbody = other.gameObject.GetComponent<Rigidbody>();
            playerManager.RagdollController.EnableRagdoll();
            Vector3 powerDirection = other.GetContact(0).normal;
            boneRigidbody.AddForce(
                powerDirection + ragdollController.playerRigidbody.velocity * throwHitPowerMultiplier,
                ForceMode.Impulse);
            playerManager.RagdollController.DisableRagdollWithDelay(3.0f);
        }
    }
}