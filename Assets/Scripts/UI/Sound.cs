using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField]
    ///<summary>This sound's audio clip.</summary>
    private AudioClip audioClip;

    [SerializeField]
    ///<summary>The name of this audio clip.</summary>
    private string audioName;

    [SerializeField]
    ///<summary>Volume of this clip.</summary>
    private float startVol = 1f;

    [SerializeReference]
    ///<summary>What time of this clip to start playing it at.</summary>
    private float startSkip;

    ///<summary>Audio Source for this Sound. </summary>
    private AudioSource audioSource;

    public virtual void SetupSound(AudioSource aSource)
    {
        audioSource = aSource;
        audioSource.clip = audioClip;
        audioSource.time = startSkip;
        audioSource.volume = startVol;
    }

    /// <summary>
    /// Changes this Sound's volume to be <c>percentage</c> of its starting volume.
    /// </summary>
    /// <param name="percentage">The percentage of the current volume.</param>
    public void AdjustVolume(float percentage)
    {
        audioSource.volume = Mathf.Clamp01(startVol * percentage);
    }

    /// <summary>
    /// Changes this Sound's volume to be <c>newVol</c>.
    /// </summary>
    /// <param name="newVol">The new volume.</param>
    public void ReplaceVolume(float newVol)
    {
        audioSource.volume = Mathf.Clamp01(newVol);
    }

    /// <summary>
    /// Returns this Sound's AudioSource.
    /// </summary>
    /// <returns>The AudioSource attached to this Sound.</returns>
    public AudioSource Source()
    {
        return audioSource;
    }

    /// <summary>
    /// Returns the name of this Sound.
    /// </summary>
    /// <returns>The string name of this Sound.</returns>
    public string SoundName()
    {
        return audioName;
    }

    /// <summary>
    /// Returns the length, in seconds, of this Sound.
    /// </summary>
    /// <returns>the float length (seconds) of this Sound.</returns>
    public float Length()
    {
        return Source().clip.length;
    }

    /// <summary>
    /// Returns the volume of this Sound.
    /// </summary>
    /// <returns>the float volume of this sound.</returns>
    public float Vol()
    {
        return audioSource.volume;
    }
}
