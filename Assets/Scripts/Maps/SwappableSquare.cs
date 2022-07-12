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

    /// <summary>The duration that it takes for two Squares to swap.</summary>
    public static readonly float swapLerpDuration = .15f;

    /// <summary>Time elapsed for swap lerp.</summary>
    private float swapLerpElapsed;

    /// <summary>true if this SwappableSquare is currently lerping.</summary>
    private bool lerping;

    /// <summary>The target to lerp swap to. </summary>
    private Transform lerpTarget;

    /// <summary>The starting position for this SwappableSquare during the Lerp. </summary>
    private Vector3 thisStart;

    /// <summary>The end position for this SwappableSquare during the Lerp. </summary>
    private Vector3 thisEnd;

    /// <summary>The starting position for the target Square during the Lerp. </summary>
    private Vector3 otherStart;

    /// <summary>The end position for the target Square during the Lerp. </summary>
    private Vector3 otherEnd;

    [SerializeField]
    /// <summary>The curve for this lerp swap.</summary>
    private AnimationCurve lerpCurve;

    /// <summary>The next direction to move to after the current one. </summary>
    private List<Direction> queuedSwap = new List<Direction>();


    protected override void Update()
    {
        base.Update();
        TrySwap();
        LerpSwap();
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
        if (Resetting() || target.Resetting()) return;
        if (ParentMap().RemainingSwaps() == 0 && ParentMap().SwapLimitEnabled()) return;
        if (lerping)
        {
            if (queuedSwap.Count == 0) queuedSwap.Add(d);
            return;
        }

        ParentMap().UpdatePositions();

        StartCoroutine(StartLerpSwap(target));
        SwapPositions(target);
        SwapDistricts(target);

        swapsMade++;
        totalSwapsMade++;
    }

    private IEnumerator StartLerpSwap(Square other)
    {
        thisStart = transform.position;
        thisEnd = other.transform.position;

        otherStart = other.transform.position;
        otherEnd = transform.position;

        lerpTarget = other.transform;

        swapLerpElapsed = 0;

        lerping = true;

        yield return new WaitForSeconds(swapLerpDuration);

        AfterLerp(other);

    }

    /// <summary>
    /// Updates the game as necessary after a swap.
    /// </summary>
    private void AfterLerp(Square other)
    {

        lerping = false;
        ParentMap().TryResetMap();
        ParentDistrict().TryLockSquares();
        other.ParentDistrict().TryLockSquares();

        if (Locked()) transform.localPosition = LockedPosition();
        else transform.localPosition = UnlockedPosition();
        if (other.Locked()) other.transform.localPosition = other.LockedPosition();
        else other.transform.localPosition = other.UnlockedPosition();

        if (queuedSwap.Count == 1)
        {
            Swap(queuedSwap[0]);
            queuedSwap.RemoveAt(0);
        }
    }

    /// <summary>
    /// Lerp swaps to a target.
    /// </summary>
    private void LerpSwap()
    {
        if (lerping)
        {
            swapLerpElapsed += Time.deltaTime;
            float percentComplete = swapLerpElapsed / swapLerpDuration;
            transform.position = Vector3.Lerp(thisStart, thisEnd, lerpCurve.Evaluate(percentComplete));
            lerpTarget.transform.position = Vector3.Lerp(otherStart, otherEnd, lerpCurve.Evaluate(percentComplete));
        }
    }


    /// <summary>
    /// Swaps this Square if possible.
    /// </summary>
    private void TrySwap()
    {
        if (!LevelManager.playable) return;
        if (Input.GetKeyDown("up")) Swap(Direction.Up);
        if (Input.GetKeyDown("down")) Swap(Direction.Down);
        if (Input.GetKeyDown("left")) Swap(Direction.Left);
        if (Input.GetKeyDown("right")) Swap(Direction.Right);
        
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
    /// Adds to the swap count.
    /// </summary>
    /// <param name="amount">The number of swaps to add.</param>
    public static void AddToSwapCount(int amount)
    {
        totalSwapsMade -= amount;
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

    public override void OnReset()
    {
        if (queuedSwap.Count == 1) queuedSwap.RemoveAt(0);
        base.OnReset();
    }

}
