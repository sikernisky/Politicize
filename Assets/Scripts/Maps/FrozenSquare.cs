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


    protected override void Update()
    {
        base.Update();
        OnAbsoluteMajority();
    }

    protected override void Start()
    {
        base.Start();
        if (HasAbsoluteMajority()) UnFreeze();
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
        Freeze();
    }


}
