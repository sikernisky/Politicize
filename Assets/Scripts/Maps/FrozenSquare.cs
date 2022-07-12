using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a Frozen Square.
/// </summary>
public class FrozenSquare : Square
{

    /// <summary>true if this FrozenSquare is currently frozen. </summary>
    private bool currentlyFrozen = true;

    [SerializeField]
    ///<summary>Sprite representing this FrozenSquare when it is unfrozen.</summary>
    private Sprite unfrozenSprite;

    [SerializeField]
    ///<summary>Sprite representing this FrozenSquare when it is frozen.</summary>
    private Sprite frozenSprite;

    [SerializeField]
    ///<summary>Sprite representing this FrozenSquare when it is unfrozen and locked.</summary>
    private Sprite lockedUnfrozenSprite;

    [SerializeField]
    ///<summary>Sprite representing this FrozenSquare when it is frozen and locked.</summary>
    private Sprite lockedFrozenSprite;

    /// <summary>State of this Square's frozen status as it moves. </summary>
    private Stack<bool> frozenStates;

    [SerializeField]
    ///<summary>Track for the frozen animation. </summary>
    private Sprite[] frozenAnimationTrack;

    [SerializeField]
    ///<summary>SpriteRenderer playing the frozenAnimation.</summary>
    private SpriteRenderer frozenAnimation;



    protected override void Update()
    {
        base.Update();
        OnAbsoluteMajority();
    }

    protected override void Start()
    {
        base.Start();
        if (HasAbsoluteMajority()) UnFreeze();
        StartCoroutine(PlayFrozenAnimation());
    }

    public override void LockSquare(Sprite customLockedSprite = null)
    {
        if (currentlyFrozen) base.LockSquare(lockedFrozenSprite);
        else base.LockSquare(lockedUnfrozenSprite);
    }

    public override void UnlockSquare(Sprite customUnlockedSprite = null)
    {
        if (currentlyFrozen) base.UnlockSquare(frozenSprite);
        else base.UnlockSquare(unfrozenSprite);
    }


    public override bool CanSwapWith()
    {
        return !currentlyFrozen;
    }

    /// <summary>
    /// Unfreezes this FrozenSquare.
    /// </summary>
    private void UnFreeze()
    {
        if (!currentlyFrozen) return;
        UpdateFreezeSprite(false);
        currentlyFrozen = false;
    }

    /// <summary>
    /// Freezes this FrozenSquare.
    /// </summary>
    private void Freeze()
    {
        if (currentlyFrozen) return;
        UpdateFreezeSprite();
        currentlyFrozen = true;
    }

    /// <summary>
    /// Sets this FrozenSquare's Sprite depending on if it is frozen or not.
    /// </summary>
    private void UpdateFreezeSprite(bool frozen = true)
    {
        if (frozen)
        {
            if (Locked()) SetSprite(lockedFrozenSprite);
            else SetSprite(frozenSprite);
            
        }
        else
        {
            if (Locked()) SetSprite(lockedUnfrozenSprite);
            else SetSprite(unfrozenSprite);
            frozenAnimation.enabled = false;
        }
    }

    private void OnAbsoluteMajority()
    {
        if (!HasAbsoluteMajority()) return;
        UnFreeze();
    }

    public override void OnReset()
    {
        base.OnReset();
        if(frozenStates != null) frozenStates.Clear();
        Freeze();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (frozenStates == null) frozenStates = new Stack<bool>();
        frozenStates.Push(currentlyFrozen);
    }

    public override void PrevState()
    {
        base.PrevState();
        if (frozenStates == null || frozenStates.Count == 0) return;
        currentlyFrozen = frozenStates.Pop();
        
    }

    IEnumerator PlayFrozenAnimation()
    {
        while (currentlyFrozen)
        {
            frozenAnimation.enabled = true;
            for (int i = 0; i < 5; i++)
            {
                frozenAnimation.sprite = frozenAnimationTrack[i];
                yield return new WaitForSeconds(.025f);

            }
            frozenAnimation.enabled = false;
            frozenAnimation.sprite = frozenAnimationTrack[0];
            yield return new WaitForSeconds(Random.Range(8, 22));
            yield return null;
        }
    }


}
