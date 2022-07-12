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
    ///<summary>The Animator for the ArrowKeys ui box.</summary>
    private Animator arrowKeysAnimator;

    /// <summary>true if the arrow keys have popped up.</summary>
    private bool arrowKeysUp;

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
            "My name is Regry, and I want to welcome you to Arnolica.",
            "It's a beautiful faction. My favorite of the seven.",
            "But its people... they thwart my work. They vote for my political opponents.",
            "I am President of the Death Party. The antithesis to the ruling Life Party.",
            "And I am hopeful. Not only do I have you but also redistricting power for this upcoming election.",
            "That means we can make some \"tweaks\" to Arnolica's voter maps.",
            "Here's what I need you to do. Listen carefully.",
            "Find the Compass bloc and move it around the map.",
            "Use it to push our voters, dark green blocs, into districts.",
            "Districts are groups of blocs connected by beams.",
            "We've won a district when more than half of its blocs are dark green.",
            "And we've won the map when more than half of the districts vote for us.",
            "Good luck; I will contact you when you're finished.",
            "And put on some mosturizer, please. Your current image is a disservice to our party."
        };

        afterWinQuotes = new string[] {
            "Would you look at that.",
            "This map used to vote for the Life Party.",
            "Now, a majority of its districts vote for the Death Party.",
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

    protected override void Update()
    {
        base.Update();
        if (arrowKeysUp && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            arrowKeysAnimator.SetTrigger("pop");
        }
        
    }

    public override void NextQuote()
    {
        if (OutOfCurrentQuotes())
        {
            arrowKeysAnimator.SetTrigger("pop");
            arrowKeysUp = true;
        }


        string nextQuote = NextQuoteText();
        
        switch (nextQuote)
        {
            case "Find the Compass bloc and move it around the map.":
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
                    "This map used to vote for the Life Party.",
                    "Now, a majority of its districts vote for the Death Party.",
                    "That means we've gained absolute control over this piece of Arnolica, thanks to you.",
                    "Say, what do you call yourself?",
                    "...",
                    "Well, not bad today, " + SaveManager.data.playerName + ".",
                    "But there is more work to be done, and there are more maps to fix.",
                    "Indeed, this was just one map of one faction. So you best get going.",
                    "We'll be in touch."
                };

                break;
            case "We'll be in touch.":
                LevelManager.playable = true;
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
