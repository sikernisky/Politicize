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


    public override void Start()
    {
        startQuotes = new string[] {
            "Hello again.",
            "Let's see what you've got here.",
            "Ah, this map is larger than the last. And it has three districts.",
            "Remember, we only need a majority of districts, not all of them, to win this map.",
            "That means you only need to fix two districts.",
            "And something else.",
            "I haven't told you this, but the law in this country puts limits on the redistricting process.",
            "The Life Party watches us redraw these maps. The more you move a shaky voter bloc, the more suspicious they get.",
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
            "Why would you do evils for him? You have education from a top university. Don't you know bet-",
            "Hey there, Kaitlyn.",
            "Please, Mr. Darmen, refer to me as Madame President.",
            "What your... client? Intern? Trainee? What they did here today was completely illegal.",
            "Yes, you have redistricting power. But you still must follow the law.",
            "The LAW! Hah.",
            "The rules that you passed without the input of even one Death Party representative?",
            "Yes, Mr. Darmen, those laws, supported by democratically elected Life Party candidates.",
            "Rectify this atrocity, or I will sue.",
            "I will be back soon to ensure you two have made the necessary changes.",
            "...",
            "I told you, " + SaveManager.data.playerName + ", she's no fun.",
            "Look, I'm going to provide you with a swap limit.",
            "If you stay under that many swaps, Madame President has no real court case.",
            "Try this again."
        };

        base.Start();
    }

    public override void NextQuote()
    {

        string nextQuote = NextQuoteText();

        switch (nextQuote)
        {
            case "You there!":
                UpdateDialogueImage(kaitlynPanel);
                break;
            case "Hey there, Kaitlyn.":
                UpdateDialogueImage(regryPanel);
                break;
            case "Please, Mr. Darmen, refer to me as Madame President.":
                UpdateDialogueImage(kaitlynPanel);
                break;
            case "The LAW! Hah.":
                UpdateDialogueImage(regryPanel);
                break;
            case "Yes, Mr. Darmen, those laws, supported by democratically elected Life Party candidates.":
                UpdateDialogueImage(kaitlynPanel);
                break;
            case "...":
                UpdateDialogueImage(regryPanel);
                break;
            default:
                break;
        }


        base.NextQuote();
    }
}
