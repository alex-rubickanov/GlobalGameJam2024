using UnityEngine;

public class BaseballBat : MeleeWeapon
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CharacterBones"))
        {
            NPlayerManager playerManager = other.gameObject.GetComponentInParent<NPlayerManager>();
            if (playerManager.PlayerCombat == owner) return;
            Rigidbody boneRigidbody = other.gameObject.GetComponent<Rigidbody>();

            playerManager.RagdollController.EnableRagdoll();
            Vector3 powerDirection = other.GetContact(0).normal;
            boneRigidbody.AddForce(powerDirection * hitMultiplier, ForceMode.Impulse);
            playerManager.RagdollController.DisableRagdollWithDelay(3.0f);
        }
    }
}