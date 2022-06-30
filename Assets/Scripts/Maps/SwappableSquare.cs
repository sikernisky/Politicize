using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represents a Square that swaps when selected and some keys
/// are pressed.
/// </summary>
public class SwappableSquare : Square
{

    /// <summary>Number of swaps made by all SwappableSquares.</summary>
    private static int totalSwapsMade;

    /// <summary>Number of swaps made by this SwappableSquare.</summary>
    private int swapsMade;

    [SerializeField]
    ///<summary>Sprite representing this Swappable when locked and selected.</summary>
    private Sprite lockedSelected;


    protected override void Update()
    {
        base.Update();
        TrySwap();
    }

    /// <summary>
    /// Swaps this SwappableSquare.
    /// </summary>
    /// <param name="d">A Direction.</param>
    /// <returns>Nothing.</returns>
    private void Swap(Direction d)
    {
        Square target = Neighbor(d);
        if (target == null || !target.CanSwapWith()) return;

        SwapPositions(target);
        SwapDistricts(target);

        if (Locked()) transform.localPosition = LockedPosition();
        else transform.localPosition = UnlockedPosition();
        if (target.Locked()) target.transform.localPosition = target.LockedPosition();
        else target.transform.localPosition = target.UnlockedPosition();

        //ParentDistrict().UnHighlightAll();
        //target.ParentDistrict().UnHighlightAll();

        swapsMade++;
        totalSwapsMade++;
    }


    /// <summary>
    /// Swaps this Square if possible.
    /// </summary>
    private void TrySwap()
    {
        if (!LevelManager.playable) return;
        if(selectedSquares.Contains(this) && selectedSquares.Count == 1)
        {
            if (Input.GetKeyDown("up")) Swap(Direction.Up);
            if (Input.GetKeyDown("down")) Swap(Direction.Down);
            if (Input.GetKeyDown("left")) Swap(Direction.Left);
            if (Input.GetKeyDown("right")) Swap(Direction.Right);
        }
    }

    /// <summary>
    /// Returns the number of swaps made by all SwappableSquares across all Maps.
    /// </summary>
    /// <returns>the number of swaps made by all SwappableSquares across all Maps.</returns>
    public static int TotalSwapsPerformed()
    {
        return totalSwapsMade;
    }

    /// <summary>
    /// Changes the SwappableSquare swap count to zero.
    /// 
    /// Does NOT affect individual Squares' swap count.
    /// </summary>
    public static void ResetSwapCount()
    {
        totalSwapsMade = 0;
    }

    /// <summary>
    /// Returns the number of Swaps this SwappableSquare has made.
    /// </summary>
    /// <returns>the number of Swaps this SwappableSquare has made.</returns>
    public int NumSwaps()
    {
        return swapsMade;
    }

    public override void LockSquare(Sprite customLockedSprite = null)
    {
        if (Selected()) base.LockSquare(lockedSelected);
        else base.LockSquare();
    }

    public override void UnlockSquare(Sprite customUnlockedSprite = null)
    {
        if (Selected()) base.UnlockSquare(selectedSprite);
        else base.UnlockSquare();
    }

}
