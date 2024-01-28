using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxAndVfx : MonoBehaviour
{
    public AudioClip [] sfx;

    public void PlaySFX(AudioClip clip)
    {
        AudioManager.instance.PlayOneShotSfx(clip);
    }
}
