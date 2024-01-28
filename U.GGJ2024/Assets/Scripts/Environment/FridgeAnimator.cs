using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeAnimator : MonoBehaviour
{
    public string TryingToEscape;
    public string PlayerEntered;
    public string currentAnim;
    public Animator anim => GetComponent<Animator>();

    public void PlayAnim(string anim)
    {
        if (anim != currentAnim)
        {
            currentAnim = anim;
            this.anim.Play(anim);
        }
    }
}
