using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialDialogue : DialogueManager
{
    [SerializeField]
    ///<summary>The Animator for the swappable square highlighted in the tutorial.</summary>
    private Animator swapSquareAnimator;

    [SerializeField]
    ///<summary>The Animator for the name box.</summary>
    private Animator nameBoxAnimator;

    [SerializeField]
    ///<summary>The input field for the name box.</summary>
    private TMP_InputField nameBoxInput;

    [SerializeField]
    ///<summary>The SwappableSquare to unhighlight when its animation is over.</summary>
    private SwappableSquare swapSquare;


    public override void Start()
    {
        startQuotes = new string[] {
            "Finally, you're here.",
            "Late, too. That's unacceptable: I'm a busy cat. Please do be on time in the future.",
            "Your tardiness aside, my name is Regry, and I want to welcome you to Arnolica.",
            "It's a beautiful faction. My favorite of the seven.",
            "But its people... they thwart my work.",
            "In their fear of change, they vote the party of restriction and oppression.",
            "The Life Party.",
            "I, however, am President of the Death Party.",
            "Its name is intense, yes. But we chose it for that exact reason.",
            "It scares the 'life' out of those power-drunk Life Party politicians.",
            "Heh.",
            "As President, I am responsible for the progress of our cause.",
            "TRUE freedom. Few laws. When a nation's people are unchained, they are unstoppable.",
            "I am hopeful. Not only do I have you but also redistricting power for this upcoming election.",
            "That means we can make some \"tweaks\" to Arnolica's voter maps.",
            "Here's what I need you to do. Listen carefully.",
            "Identify our voter blocs. We are represented by dark green.",
            "Find swappable voter blocs. You can move those against other blocs in the four primary directions.",
            "Districts are groups of blocs connected by beams.",
            "This map has two.",
            "Push our voters into districts such that a majority of blocs in a district are dark green.",
            "Your job is done when a majority of this map's districts vote for the Death Party.",
            "Good luck; I will contact you when you're finished.",
            "And put on some mosturizer, please. Your current image is a disservice to our party."
        };

        afterWinQuotes = new string[] {
            "Would you look at that.",
            "Do you see what you just did?",
            "This map used to split its voters. Half for the Life Party, half for us.",
            "Look at it now, though. A majority of its districts vote for the Death Party.",
            "That means we've gained absolute control over this piece of Arnolica, thanks to you.",
            "Say, what do you call yourself?",
            "...",
            "Well, not bad today, " + SaveManager.data.playerName + ".",
            "But there is more work to be done, and there are more maps to fix.",
            "Indeed, this was just one map of one faction. So you best get going.",
            "We'll be in touch."
        };



        base.Start();

    }

    public override void NextQuote()
    {

        string nextQuote = NextQuoteText();
        
        switch (nextQuote)
        {
            case "Find swappable voter blocs. You can move those against other blocs in the four primary directions.":
                swapSquareAnimator.SetBool("pulse", true);
                break;
            case "Say, what do you call yourself?":
                StartCoroutine(NameBoxDelay());
                break;
            case "...":
                if (nameBoxInput.text.Length == 0) return;
                nameBoxAnimator.SetTrigger("pop");
                SaveManager.data.playerName = nameBoxInput.text.Trim();

                currentQuotes = new string[] {
                    "Would you look at that.",
                    "Do you see what you just did?",
                    "This map used to split its voters. Half for the Life Party, half for us.",
                    "Look at it now, though. A majority of its districts vote for the Death Party.",
                    "That means we've gained absolute control over this piece of Arnolica, thanks to you.",
                    "Say, what do you call yourself?",
                    "...",
                    "Well, not bad today, " + SaveManager.data.playerName + ".",
                    "But there is more work to be done, and there are more maps to fix.",
                    "Indeed, this was just one district of one faction. So you best get going.",
                    "We'll be in touch."
                };

                break;
            default:
                swapSquareAnimator.SetBool("pulse", false);
                swapSquare.UnHighlight();
                break;
        }


        base.NextQuote();


    }

    private IEnumerator NameBoxDelay()
    {
        yield return new WaitForSeconds(.5f);
        nameBoxAnimator.SetTrigger("pop");
    }

    
}
