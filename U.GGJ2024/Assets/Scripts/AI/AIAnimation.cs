using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimation : MonoBehaviour
{
    [SerializeField] string currentAnim;
    [Header("Animations")]
    public string AI_IDLE = "IDLE";
    public string AI_RUN = "RUN";
    public string AI_HIT = "HIT";
    public string AI_DEAD = "DEAD";
    public string AI_HIDE = "HIDE";
    public string AI_ROLL = "ROLL";
    public Animator animator => GetComponent<Animator>();

    public void SetAnim(string animationName)
    {
        if (currentAnim != animationName)
        {
            currentAnim = animationName;
            animator.Play(currentAnim);
        }
    }
}
