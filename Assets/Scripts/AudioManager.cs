using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to handle sounds.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    ///<summary>The AudioSource for this AudioManager.</summary>
    private AudioSource audioSource;

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
