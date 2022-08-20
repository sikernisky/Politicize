using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arnolica1Dialogue : DialogueManager
{

    [SerializeField]
    ///<summary>Image for Regry dialogue.</summary>
    private Sprite regryPanel;

    [SerializeField]
    ///<summary>Image for Kaitlyn dialogue.</summary>
    private Sprite kaitlynPanel;

    /// <summary>true if the player is trying this map again.</summary>
    private bool onRetry;

    [SerializeField]
    ///<summary>The swap counter animator for this level.</summary>
    private Animator swapCounter;

    [SerializeField]
    ///<summary>Arrow for the Death Party's dialogue.</summary>
    private Sprite deathArrow;

    [SerializeField]
    ///<summary>Arrow for the Life Party's dialogue.</summary>
    private Sprite lifeArrow;

    /// <summary>true if the delay for the actual win is over.</summary>
    private bool delayOver;


    public override void Start()
    {

        startQuotes = new string[] {
            "Hello again.",
            "Let's see what you've got here.",
            "Ah, this map is larger than the last. It should be fine for you, though.",
            "Also.",
            "I haven't told you this, but the law in this country puts limits on the redistricting process.",
            "The Life Party watches us redraw these maps. The more you move a voter bloc, the more suspicious they get.",
            "If you move too many times, they can take your map to court to invalidate it.",
            "Plus, you'll have to deal with the Life Party's president, Kaitlyn. ",
            "She's not fun.",
            "Long story short, use as few moves as possible.",
            "Alright. Back to work."
        };

        afterWinQuotes = new string[] {
            "You there!",
            "Cease what you're doing immediately.",
            "Unbelievable. Regry Darmen must have recruited you.",
            "This is a closed meeting, Kaitlyn.",
            "Please, President Darmen, refer to me as Madame President.",
            "And what your client did here today was completely illegal. You know this.",
            "That's " + SaveManager.data.playerName + " to you.",
            "President Darmen.",
            "Yes, you have redistricting power. But you still must follow the law.",
            "The laws that you passed without the input of even one Death Party representative?",
            "Yes, President Darmen, those laws, supported by democratically elected Life Party candidates.",
            "Rectify this atrocity, or I will sue.",
            "...",
            "I told you, " + SaveManager.data.playerName + ", she's no fun.",
            "Look, I'm going to provide you with a swap limit.",
            "If you stay under that many swaps, Madame President has no real court case.",
            "Try this again."
        };
        base.Start();
    }

    protected override void Update()
    {
        if (onRetry && map.Won() && delayOver)
        {
            Debug.Log("Here");
            onRetry = false;
            map.EndLevel();
        }
        else if (onRetry && map.TooManySwaps()) map.ResetMap();
        else base.Update();
    }

    public override void NextQuote()
    {

        string nextQuote = NextQuoteText();

        string constantQuote = "That's " + SaveManager.data.playerName + " to you.";

        if (nextQuote == constantQuote)
        {
            UpdateDialogueImage(regryPanel);
            UpdateDialogueArrow(deathArrow);
        }

        switch (nextQuote)
        {
            case "You there!":
                UpdateDialogueImage(kaitlynPanel);
                UpdateDialogueArrow(lifeArrow);
                break;
            case "This is a closed meeting, Kaitlyn.":
                UpdateDialogueImage(regryPanel);
                UpdateDialogueArrow(deathArrow);
                break;
            case "Please, President Darmen, refer to me as Madame President.":
                UpdateDialogueImage(kaitlynPanel);
                UpdateDialogueArrow(lifeArrow);
                break;
            case "President Darmen.":
                UpdateDialogueImage(kaitlynPanel);
                UpdateDialogueArrow(lifeArrow);
                break;
            case "The laws that you passed without the input of even one Death Party representative?":
                UpdateDialogueImage(regryPanel);
                UpdateDialogueArrow(deathArrow);
                break;
            case "Yes, President Darmen, those laws, supported by democratically elected Life Party candidates.":
                UpdateDialogueImage(kaitlynPanel);
                UpdateDialogueArrow(lifeArrow);
                break;
            case "...":
                UpdateDialogueImage(regryPanel);
                UpdateDialogueArrow(deathArrow);
                break;
            case "Look, I'm going to provide you with a swap limit.":
                swapCounter.SetTrigger("fadeCounterIn");
                break;
            case "Try this again.":
                StartCoroutine(Delay());
                map.ForceResetMap();
                map.EnableSwapLimit();
                onRetry = true;
                break;
            default:
                break;
        }
        base.NextQuote();
    }

    protected override void EnterPressed()
    {
        if (Input.GetKeyDown(KeyCode.Return) && started) NextQuote();
    }

    public override void ClickDialogueButton()
    {
        if (started) NextQuote();
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        delayOver = true;
    }
}
