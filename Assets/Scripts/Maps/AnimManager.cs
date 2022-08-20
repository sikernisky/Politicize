using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimManager : MonoBehaviour
{
    [SerializeField]
    ///<summary>The animations to play. </summary>
    private AnimObject[] animations;


    /// <summary>
    /// Plays an animation.
    /// </summary>
    /// <param name="animationName">The name of the animation to play.</param>
    public void Play(string animationName, Vector3 position)
    {
        AnimObject aob = Array.Find(animations, anim => anim.Name() == animationName);
        if (aob == null) 
        {
            Debug.Log("Could not find animation with name: " + animationName + ".");
            return;
        }
        StartCoroutine(PlayAtLocation(aob, position));
    }

    /// <summary>
    /// Creates a GameObject with a SpriteRenderer and plays an Animation.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayAtLocation(AnimObject anim, Vector3 pos)
    {
        GameObject ob = new GameObject(anim.Name());
        ob.transform.position = pos;
        ob.transform.localScale = new Vector2(anim.Scale(), anim.Scale());
        ob.transform.SetParent(FindObjectOfType<Map>().transform);
        SpriteRenderer rendToAnim = ob.AddComponent<SpriteRenderer>();
        rendToAnim.sortingOrder = 999;

        Sprite[] track = anim.AnimTrack();
        bool loop = true;

        while (loop)
        {
            foreach (Sprite spr in track)
            {
                rendToAnim.sprite = spr;
                yield return new WaitForSeconds(anim.Delay());
            }
            if (!anim.Repeats()) loop = false;
            yield return null;
        }
        Destroy(ob);
    }
}
