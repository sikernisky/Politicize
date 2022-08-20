using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimObject
{
    [SerializeField]
    /// <summary>This AnimObject's animation track.</summary>
    private Sprite[] track;

    [SerializeField]
    /// <summary>The delay between each frame of this Animation.</summary>
    private float tickDelay;

    [SerializeField]
    ///<summary>true if this AnimObject should repeat its animation. false if it should play once. </summary>
    private bool repeat;

    [SerializeField]
    ///<summary>The name of this Animation.</summary>
    private string animName;

    [SerializeField]
    ///<summary>The scale of this Animation.</summary>
    private float scale;


    /// <summary>
    /// Returns the name of this AnimObject.
    /// </summary>
    /// <returns>The name of this AnimObject.</returns>
    public string Name()
    {
        return animName;
    }

    /// <summary>
    /// Returns this AnimObject's animation track. 
    /// </summary>
    /// <returns>the Sprite array of this AnimObject's anim track.</returns>
    public Sprite[] AnimTrack()
    {
        return track;
    }

    /// <summary>
    /// Returns the delay between each frame of this Animation.
    /// </summary>
    /// <returns>the float delay.</returns>
    public float Delay()
    {
        return tickDelay;
    }

    /// <summary>
    /// Returns true if this AnimObject should repeat, false otherwise.
    /// </summary>
    /// <returns>true if this AnimObject should repeat, false otherwise.</returns>
    public bool Repeats()
    {
        return repeat;
    }

    /// <summary>
    /// Returns the scale of this animation.
    /// </summary>
    /// <returns>the float scale of this animation.</returns>
    public float Scale()
    {
        return scale;
    }
}
