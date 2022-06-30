using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Controls character dialogue.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    ///<summary>The quotes that the Dialogue in this scene will say, in order, during the level.</summary>
    protected string[] startQuotes;

    [SerializeField]
    ///<summary>The quotes that the Dialogue in this scene will say, in order, after winning.</summary>
    protected string[] afterWinQuotes;

    /// <summary>The quotes this Dialogue is currently working with.</summary>
    protected string[] currentQuotes;

    [SerializeField]
    ///<summary>The text box attached to this dialogue.</summary>
    private TMP_Text textBox;

    /// <summary>The coroutine that is revealing text. </summary>
    private IEnumerator revealCoroutine;

    [SerializeField]
    /// <summary>The Animator for this dialgoue box.</summary>
    private Animator dialogueAnimator;

    /// <summary>The index of the current quote being displayed. </summary>
    private int currentQuoteNum = -1;

    /// <summary>true if this dialogue is not on the screen.</summary>
    private bool hidden;

    [SerializeField]
    ///<summary>How long to wait before starting dialogue as the scene begins. Negative numbers mean
    ///that this dialogue should not go through this process at all.</summary>
    private float speakOnStartDelay;

    [SerializeField]
    ///<summary>How long to wait before starting dialogue after the player wins. Negative numbers mean
    ///that this dialogue should not go through this process at all</summary>
    private float speakAfterWinDelay;

    /// <summary>true if this dialogue has started.</summary>
    protected bool started;

    [SerializeField]
    ///<summary>This level's map.</summary>
    protected Map map;

    /// <summary>true if the current dialogue is the last of this Dialogue.</summary>
    private bool lastDialogue;

    [SerializeField]
    ///<summary>The Image for this Dialogue.</summary>
    private Image dialogueImage;

    [SerializeField]
    ///<summary>The Image for this Dialogue's arrow.</summary>
    private Image dialogueArrowImage;

    /// <summary>True if this Dialogue has asked the Map to end the level.</summary>
    private bool ended;

    [SerializeField]
    ///<summary>Audioclip that plays whenever a text is revealed. </summary>
    private AudioClip textAudio;

    [SerializeField]
    ///<summary>This DialogueManager's AudioManager.</summary>
    private AudioManager audioManager;

    public virtual void Start()
    {
        ResetDialogue();
        StartCoroutine(StartDelay(startQuotes, speakOnStartDelay));
    }


    protected virtual void Update()
    {
        EnterPressed();
        TryStartWinDialogue();
    }

    /// <summary>
    /// Does something when the user presses the enter/return key/
    /// </summary>
    protected virtual void EnterPressed()
    {
        if (Input.GetKeyDown(KeyCode.Return)) ClickDialogueButton();
    }

    public virtual void ClickDialogueButton()
    {
        if(lastDialogue && OutOfCurrentQuotes() && !ended)
        {
            map.EndLevel();
            ended = true;
            return;
        }
        if (started) NextQuote();
    }

    private IEnumerator StartDelay(string[] quotes, float delay)
    {
        if(quotes != null && quotes.Length > 0)
        {
            LevelManager.playable = false;
            if (delay >= 0.0) yield return new WaitForSeconds(speakOnStartDelay);
            else yield return new WaitForSeconds(0);
            StartDialogue(quotes);
        }
        yield return null;
    }


    /// <summary>
    /// Starts this DialogueManager's immediate dialogue process.
    /// </summary>
    public virtual void StartDialogue(string[] quotes)
    {
        if (started) return;

        hidden = true;
        started = true;

        ShowDialogueBox();
        currentQuotes = quotes;
        NextQuote();
    }

    /// <summary>
    /// Starts this DialogueManager's win dialogue process.
    /// </summary>
    private void StartWinDialogue()
    {
        if (started) return;
        lastDialogue = true;
        StartCoroutine(StartDelay(afterWinQuotes, speakAfterWinDelay));
    }

    private void TryStartWinDialogue()
    {
        if (map.Won() && !lastDialogue && !started && hidden && afterWinQuotes != null)
        {
            StartWinDialogue();
        }
    }


    /// <summary>
    /// Displays the next quote in <c>orderedQuotes</c>.
    /// </summary>
    public virtual void NextQuote()
    {
        if (!started) return;
        if (hidden) return;

        //No more quotes.
        if (currentQuoteNum == currentQuotes.Length - 1 || currentQuotes == null)
        {
            ResetDialogue();
            return;
        }
        //audioManager.PlaySound(textAudio);

        if (revealCoroutine != null) StopCoroutine(revealCoroutine);
        revealCoroutine = RevealText(currentQuotes);
        StartCoroutine(revealCoroutine);
    }

    /// <summary>
    /// Resets this Dialogue and prepares it for another set of quotes.
    /// </summary>
    private void ResetDialogue()
    {
        HideDialogueBox();
        currentQuotes = null;
        started = false;
        currentQuoteNum = -1;
        LevelManager.playable = true;
    }

    /// <summary>
    /// Reveals the text in <c>textBox</c> overtime.
    /// </summary>
    /// <returns>The IENumerator of this Coroutine.</returns>
    private IEnumerator RevealText(string[] quotes)
    {
        currentQuoteNum++;
        textBox.maxVisibleCharacters = 0;
        textBox.text = quotes[currentQuoteNum];
        foreach(char c in textBox.text)
        {
            textBox.maxVisibleCharacters++;
            yield return new WaitForSeconds(.02f);
        }
    }

    /// <summary>
    /// Causes the dialogue box to appear on the screen.
    /// </summary>
    private void ShowDialogueBox()
    {
        if (!hidden) return;
        dialogueAnimator.SetBool("show", true);
        hidden = false;
    }


    /// <summary>
    /// Causes the dialogue box to dissapear from the screen.
    /// </summary>
    private void HideDialogueBox()
    {
        if (hidden) return;
        dialogueAnimator.SetBool("show", false);
        hidden = true;
    }


    /// <summary>
    /// Returns the next quote in this Dialogue's quote order.
    /// </summary>
    /// <returns>The next quote this Dialogue will say.</returns>
    protected string NextQuoteText()
    {
        if (currentQuoteNum + 1 < 0 || currentQuoteNum + 1>= currentQuotes.Length) return "";
        return currentQuotes[currentQuoteNum + 1];
    }

    /// <summary>
    /// Returns true if this Dialogue should speak after winning the level.
    /// </summary>
    /// <returns>true if this Dialogue should speak after winning, false otherwise.</returns>
    public bool SpeakAfterWin()
    {
        return afterWinQuotes != null && afterWinQuotes.Length > 0;
    }

    /// <summary>
    /// Sets the Dialogue sprite. S cannot be null.
    /// </summary>
    /// <param name="s">The sprite to set the Dialogue to.</param>
    protected void UpdateDialogueImage(Sprite s)
    {
        if (s == null) return;
        dialogueImage.sprite = s;
    }

    /// <summary>
    /// Sets the Dialogue arrow sprite. S cannot be null.
    /// </summary>
    /// <param name="s">The sprite to set the Dialogue arrow to.</param>
    protected void UpdateDialogueArrow(Sprite s)
    {
        if (s == null) return;
        dialogueArrowImage.sprite = s;
    }

    /// <summary>
    /// Returns true if there are no more current quotes.
    /// </summary>
    /// <returns>true if there are no more current quotes, false otherwise.</returns>
    protected bool OutOfCurrentQuotes()
    {
        return currentQuotes == null || currentQuoteNum >= currentQuotes.Length - 1;
    }
}


