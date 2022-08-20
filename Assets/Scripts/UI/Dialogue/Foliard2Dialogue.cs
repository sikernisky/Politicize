using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foliard2Dialogue : DialogueManager
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

        startQuotes = new string[] {
            SaveManager.data.playerName + ", you continue to breach normality.",
            "The Life Party has passed more legislation in the Senate to slow your evil work.",
            "We've implemented swap limits on individual Compass Blocs.",
            "Also, we initated procedures to freeze Life Party blocs. Try getting around that.",
            "Well, you'll have to 'get around that', because you surely won't be able to move them.",
            "Keep it up, and we will do more. Maybe that will discourage you.",
        };

        base.Start();
    }


    public override void NextQuote()
    {

        string nextQuote = NextQuoteText();

        if(nextQuote == SaveManager.data.playerName + ", you continue to breach normality.")
        {
            UpdateDialogueImage(kaitlynPanel);
            UpdateDialogueArrow(lifeArrow);
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
}
