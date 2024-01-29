using System.Collections;
using UnityEngine;

public class Baloons : MonoBehaviour
{
    [Range(-9, 9)]
    [SerializeField] private int points;
    [SerializeField] private float flyTime;
    private NPlayerManager takenPlayer;
    private FixedJoint joint;
    private bool once = false;
    private Rigidbody rb;
    private void Start()
    {
        joint = GetComponentInChildren<FixedJoint>();
        rb = GetComponent<Rigidbody>();

        transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if(other.gameObject.layer == LayerMask.NameToLayer("CharacterBones"))
        {
            takenPlayer = other.gameObject.GetComponentInParent<NPlayerManager>();
            

            if (!once)
            {
                TakePlayerAndGoUp();
                UpdatePlayerPoints();
                once = true;
            }
        }
    }

    private void TakePlayerAndGoUp()
    {
        SFX();
        takenPlayer.RagdollController.EnableRagdoll();
        takenPlayer.RagdollController.AttachPelvisTo(joint);
        StartCoroutine(UpAndDestroy());
    }

    private IEnumerator UpAndDestroy()
    {
        rb.velocity = new Vector3(0, 2, 0);
        yield return new WaitForSeconds(flyTime);
        joint.connectedBody = null;
        takenPlayer.RagdollController.DisableRagdollWithDelay(3.0f);
        Destroy(gameObject);
    }

    private void UpdatePlayerPoints()
    {
        PointsGainUIManager.instance.ShowUIPoints(takenPlayer.playerPawn.transform, points);
    }
    
    //SFX
    void SFX()
    {
        AudioManager audioManager = AudioManager.instance;
        audioManager.PlayOneShotSfx(audioManager.BalloonSFx);
    }

}
