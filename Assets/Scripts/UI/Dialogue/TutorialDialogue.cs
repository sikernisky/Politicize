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
            "Late, too. That's unacceptable: I'm a busy cat. Please do be on time in the future.",
            "Your tardiness aside, my name is Regry, and I want to welcome you to Arnolica.",
            "It's a beautiful faction. My favorite of the seven.",
            "But its people... they thwart my work.",
            "In their fear of change, they vote the party of restriction and oppression.",
            "The Life Party.",
            "I, however, am President of the Death Party.",
            "And I am hopeful. Not only do I have you but also redistricting power for this upcoming election.",
            "That means we can make some \"tweaks\" to Arnolica's voter maps.",
            "Here's what I need you to do. Listen carefully.",
            "Identify our dark green voter blocs on the map.",
            "Find Compass blocs. You can move those against other blocs in the four primary directions.",
            "Districts are groups of blocs connected by beams.",
            "This map has three.",
            "Push our voters into districts until a majority of blocs in a district are dark green.",
            "Your job is done when a majority -- two -- of this map's districts vote for the Death Party.",
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


        SaveManager.data.currentLevel = 0; //DELETE ME WHEN DEMO IS OVER!
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
            case "Find Compass blocs. You can move those against other blocs in the four primary directions.":
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
            default:
                swapSquareAnimator.SetBool("pulse", false);
                swapSquare.UnHighlight();
                break;
        }


        base.NextQuote();


    }

/*    public virtual void EnterPressed()
    {
        if (Input.GetKeyDown(KeyCode.Return) && lastDialogue && OutOfCurrentQuotes())
        {
            NextQuote();
            map.EndLevel();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return) && started) NextQuote();
    }*/

    private IEnumerator NameBoxDelay()
    {
        yield return new WaitForSeconds(.5f);
        nameBoxAnimator.SetTrigger("pop");
    }

    
}
