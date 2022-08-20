using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChainedSquare : Square
{
    [SerializeField]
    ///<summary>Number of swaps this ChainedSquare requires to unlock.</summary>
    private int startLockedSwaps;

    /// <summary>Swaps until thi </summary>
    private int swapsLeft;

    [SerializeField]
    ///<summary>Sprite that represents this ChainedSquare when it is chained.</summary>
    private Sprite chainedSprite;

    [SerializeField]
    ///<summary>Sprite that represents this ChainedSquare when it is chained.</summary>
    private Sprite lockedChainedSprite;

    [SerializeField]
    ///<summary>Sprite that represents this ChainedSquare when it is unchained.</summary>
    private Sprite unchainedSprite;

    [SerializeField]
    ///<summary>Sprite that represents this ChainedSquare when it is unchained.</summary>
    private Sprite lockedUnchainedSprite;

    /// <summary>State of this ChainedSquare's chained status as it swaps.</summary>
    private Stack<int> chainCountStates;

    /// <summary>true if this Square is chained. </summary>
    private bool chained;

    [SerializeField]
    /// <summary>The party of this ChainedSquare when it is unchained. </summary>
    private Party unchainedParty;

    [SerializeField]
    /// <summary>Text component representing how many swaps this ChainedSquare has left until it unchains.</summary>
    private TMP_Text swapCountText;



    protected override void Start()
    {
        base.Start();
        Chain();
        if(ChainCount() == 0) UnChain(false);
        UpdateChainText();
    }


    /// <summary>
    /// Chains this ChainedSquare.
    /// </summary>
    private void Chain()
    {
        if (Chained()) return;
        chained = true;
        SetParty(Party.Neutral);
        SetSprite(chainedSprite);
    }

    /// <summary>
    /// Unchains this ChainedSquare.
    /// </summary>
    private void UnChain(bool playAudio = true)
    {
        if (!Chained()) return;
        chained = false;
        SetParty(unchainedParty);
        SetSprite(unchainedSprite);
        if(playAudio) FindObjectOfType<AudioManager>().Play("Chain");
    }


    /// <summary>
    /// Returns true if this ChainedSquare is chained, false otherwise.
    /// </summary>
    /// <returns>true if this ChainedSquare is chained, false otherwise.</returns>
    public bool Chained()
    {
        return chained;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (chainCountStates == null) chainCountStates = new Stack<int>();
        chainCountStates.Push(swapsLeft);
        UpdateChainText();
    }

    public override void PrevState()
    {
        base.PrevState();
        if (chainCountStates == null || chainCountStates.Count == 0) return;
        int chainCount = chainCountStates.Pop();
        if (chainCount > 0) Chain();
        swapsLeft = chainCount;
        UpdateChainText();
    }

    public override void OnReset(bool lerp = true)
    {
        base.OnReset();
        if (chainCountStates != null) chainCountStates.Clear();
        ResetChainCount();
        Chain();
        UpdateChainText();
    }

    public override void OnSwap()
    {
        base.OnSwap();
        DecrementChainCount();
        if (ChainCount() > 0) Chain();
        else UnChain();
        UpdateChainText();
    }

    /// <summary>
    /// Decrements the swap count for this ChainedSquare by one.
    /// </summary>
    private void DecrementChainCount()
    {
        if (swapsLeft - 1 < 0) return;
        else swapsLeft--;
    }

    /// <summary>
    /// Increments the swap count for this ChainedSquare by one.
    /// </summary>
    private void IncrementChainCount()
    {
        if (swapsLeft + 1 > startLockedSwaps) return;
        else swapsLeft++;
    }

    /// <summary>
    /// Sets up this ChainedSquare's swap limit.
    /// </summary>
    private void ResetChainCount()
    {
        swapsLeft = startLockedSwaps;
        Chain();
        Debug.Log("Here");
    }


    /// <summary>
    /// Returns how many swaps this ChainedSquare has left before it becomes unchained.
    /// </summary>
    /// <returns>the int amount of swaps this ChainedSquare has left before it unchains.</returns>
    public int ChainCount()
    {
        return swapsLeft;
    }

    /// <summary>
    /// Updates the text component that displays this ChainedSquare's swap count to display the correct
    /// number of remaining swaps.
    /// </summary>
    private void UpdateChainText()
    {
        swapCountText.text = ChainCount().ToString();
    }

    public override void LockSquare(Sprite customLockedSprite = null)
    {
        if (Chained()) base.LockSquare(lockedChainedSprite);
        else base.LockSquare(lockedUnchainedSprite);
    }

    public override void UnlockSquare(Sprite customUnlockedSprite = null)
    {
        if (Chained()) base.UnlockSquare(chainedSprite);
        else base.UnlockSquare(unchainedSprite);
    }

    public override GameObject ConvertParty(bool playSound = true, GameObject customConversion = null)
    {
        ChainedSquare cs = base.ConvertParty(playSound, customConversion).GetComponent<ChainedSquare>();
        cs.swapsLeft = swapsLeft;
        return cs.gameObject;
    }

    public override Party PoliticalParty()
    {
        if (chained) return Party.Neutral;
        else return unchainedParty;
    }


}
