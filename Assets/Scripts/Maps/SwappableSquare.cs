using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Represents a Square that swaps when selected and some keys
/// are pressed.
/// </summary>
public class SwappableSquare : Square
{
    /// <summary>Number of swaps made by all SwappableSquares.</summary>
    private static int totalSwapsMade;

    /// <summary>Number of arrow key presses that have swapped SwappableSquares.</summary>
    private static int currentSwapCount;

    [SerializeField]
    ///<summary>The number of swaps this SwappableSquare is allowed this level.
    /// A value of -1 represents an infinite amount.
    /// </summary>
    private int swapsLeft = -1;

    /// <summary>How many swaps this SwappableSquare had left to swap with at the start.</summary>
    private int startingSwapsLeft;

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

    /// <summary>true if this SwappableSquare has been swapped.</summary>
    private bool swapped;

    [SerializeField]
    ///<summary>Text component displaying how many swaps this SwappableSquare has left. </summary>
    private TMP_Text swapText;

    [SerializeField]
    ///<summary>The background for the swaps left text component. </summary>
    private SpriteRenderer swapTextPlaque;

    /// <summary>How many swaps  </summary>
    private Stack<int> swapCounts;


    protected override void Start()
    {
        base.Start();
        SetupIndividualSwapCounter();
    }

    protected override void Update()
    {
        base.Update();
        LerpSwap();
        UpdateIndividualSwapCounter();
    }

    /// <summary>
    /// Returns true and swaps this SwappableSquare if possible, otherwise returns false.
    /// </summary>
    /// <param name="d">The direction to swap.</param>
    /// <returns>true if this SwappableSquare swapped, false otherwise..</returns>
    public bool Swap(Direction d)
    {
        Square target = Neighbor(d);
        if (target == null || !target.CanSwapWith() || !CanSwap()) return false;
        if (Resetting() || target.Resetting()) return false;
        if (ParentMap().RemainingSwaps() == 0 && ParentMap().SwapLimitEnabled()) return false;
        if (lerping) return false;
        if (!Selected()) return false;
        if (ParentMap().Banished(this)) return false;
        if (!isActiveAndEnabled) return false;
       
        ParentDistrict().UnlockAllSquares();
        target.ParentDistrict().UnlockAllSquares();

        target.OnSwap();
        StartCoroutine(StartLerpSwap(target));
        SwapDistricts(target);
        SwapPositions(target);

        swapsMade++;
        DecrementIndividualSwapCounter();
        SwappableSquare ssTarget = target as SwappableSquare;
        if (ssTarget != null) ssTarget.DecrementIndividualSwapCounter();
        
      
        return true;
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

        FindObjectOfType<AudioManager>().Play("Swap");

        yield return new WaitForSeconds(swapLerpDuration);
        AfterLerp(other);
    }

    /// <summary>
    /// Updates the game as necessary after a swap.
    /// </summary>
    private void AfterLerp(Square other)
    {
        lerping = false;
        other.AfterSwap();

        ParentMap().TryResetMap();
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

    private void OnMouseDown()
    {
        Select();
    }



    /// <summary>
    /// Returns true if this SwappableSquare is lerping.
    /// </summary>
    /// <returns>true if this SwappableSquare is lerping, false otherwise.</returns>
    public bool Lerping()
    {
        return lerping;
    }

    /// <summary>
    /// Returns the number of Swaps this SwappableSquare has made.
    /// </summary>
    /// <returns>the number of Swaps this SwappableSquare has made.</returns>
    public int NumSwaps()
    {
        return swapsMade;
    }

    /// <summary>
    /// Updates this SwappableSquare's swap counter.
    /// </summary>
    private void UpdateIndividualSwapCounter()
    {
        if (swapsLeft != -1) swapText.text = swapsLeft.ToString();
    }

    /// <summary>
    /// Sets up this SwappableSquare's swap counter.
    /// </summary>
    private void SetupIndividualSwapCounter()
    {
        startingSwapsLeft = swapsLeft;
        if (swapsLeft < 0)
        {
            swapText.enabled = false;
            swapTextPlaque.enabled = false;
        }
        else
        {
            swapText.text = swapsLeft.ToString();
        }
    }

    /// <summary>
    /// Adds to this SwappableSquare's swap counter.
    /// </summary>
    /// <param name="amount">The amount to add, > 0. </param>
    private void IncrementIndividualSwapCounter(int amount = 1)
    {
        if (amount < 0 || swapsLeft < 0) return;
        swapsLeft += amount;
    }


    /// <summary>
    /// Adds to this SwappableSquare's swap counter.
    /// </summary>
    /// <param name="amount">The amount to subtract, > 0. Must not make the number
    /// of remaining swaps to be less than zero. </param>
    private void DecrementIndividualSwapCounter(int amount = 1)
    {
        if (amount < 0 || swapsLeft < 0) return;
        swapsLeft = Mathf.Max(0, swapsLeft - amount);
        if (swapsLeft == 0) FindObjectOfType<AudioManager>().Play("OutSwaps");
    }

    /// <summary>
    /// Sets this SwappableSquare's swap limit to what it was originally.
    /// </summary>
    public void ResetIndividualSwapCounter()
    {
        swapsLeft = startingSwapsLeft;
    }

    /// <summary>
    /// Returns the number of swaps this SwappableSquare has left.
    /// </summary>
    /// <returns>the integer number of swaps this SwappableSquare has left.</returns>
    public int IndividualSwapsLeft()
    {
        return swapsLeft;
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

    public override void DeSelect()
    {
        return;
    }

    public override void Select()
    {
        
        base.Select();
    }

    public override void OnReset(bool lerp = true)
    {
        base.OnReset();
        lerping = false;
        ResetIndividualSwapCounter();
        if (swapCounts != null) swapCounts.Clear();
    }

    public override void OnUndo(Vector2Int position, Transform newParent, bool lerp = true)
    {
        base.OnUndo(position, newParent, lerp);
        lerping = false;
    }

    /// <summary>
    /// Returns true if this SwappableSquare can swap with something.
    /// </summary>
    /// <returns>true if this SwappableSquare can swap with something, false otherwise.</returns>
    private bool CanSwap()
    {
        return swapsLeft != 0;
    }

    public override void PrevState()
    {
        base.PrevState();
        if (swapCounts == null || swapCounts.Count == 0) return;
        int prevCount = swapCounts.Pop();
        IncrementIndividualSwapCounter(prevCount - IndividualSwapsLeft());
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (swapCounts == null) swapCounts = new Stack<int>();
        swapCounts.Push(IndividualSwapsLeft());
    }

    public override void OnBanish()
    {
        lerping = false;
        base.OnBanish();
    }

    public override GameObject ConvertParty(bool playSound = true, GameObject customConversion = null)
    {
        SwappableSquare convertedSwap = base.ConvertParty(playSound, customConversion).GetComponent<SwappableSquare>();
        convertedSwap.Select();
        return convertedSwap.gameObject;

    }



}
