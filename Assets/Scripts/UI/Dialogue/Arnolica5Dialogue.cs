using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arnolica5Dialogue : DialogueManager
{
    [SerializeField]
    ///<summary>Image for Regry dialogue.</summary>
    private Sprite regryPanel;

    [SerializeField]
    ///<summary>Image for Kaitlyn dialogue.</summary>
    private Sprite kaitlynPanel;

    [SerializeField]
    ///<summary>Arrow for the Death Party's dialogue.</summary>
    private Sprite deathArrow;

    [SerializeField]
    ///<summary>Arrow for the Life Party's dialogue.</summary>
    private Sprite lifeArrow;


    public override void Start()
    {
        afterWinQuotes = new string[] {
            "Good work. Good work.",
            "Take your eyes off the map for a minute. I've got something more important to show you.",
            "On my orders, the Death Party employed the beautiful act of espionage.",
            "In other words, we spied on the Life Party.",
            "Are you scoffing?",
            "Call it wrong all you want. But to do good, we've sometimes got to do a little bad.",
            "Just listen.",
            "Good morning team. Let's get started.",
            "We need to discuss the redistricting process. Urgently.",
            "As you know, President Regry Darmen has recruited the best political scientist in the nation.",
            "Their name is " + SaveManager.data.playerName + ".",
            "Together, the two are redrawing maps. Masterfully, in fact.",
            "At this pace, they will soon flip the Senate seat of Arnolica.",
            "That would be the first time the Death Party has ever held one of the seven seats.",
            "It would be a nightmare. We must not grant Darmen an ounce of power.",
            "So, while we maintain the senate majority, we must pass laws limiting redistricting power.",
            "My preliminary ideas stem from freezing bl...",
            "Hear that, " + SaveManager.data.playerName + "?",
            "It speaks for itself. We must keep going. And quickly too - before she passes those laws.",
            "Onwards."

        };
        base.Start();
    }


    public override void NextQuote()
    {

        string nextQuote = NextQuoteText();

        string constantQuote = "Hear that, " + SaveManager.data.playerName + "?";

        if (nextQuote == constantQuote)
        {
            UpdateDialogueImage(regryPanel);
            UpdateDialogueArrow(deathArrow);
        }

        switch (nextQuote)
        {
            case "Good morning team. Let's get started.":
                UpdateDialogueImage(kaitlynPanel);
                UpdateDialogueArrow(lifeArrow);
                break;
            default:
                break;
        }
        base.NextQuote();
    }
}
