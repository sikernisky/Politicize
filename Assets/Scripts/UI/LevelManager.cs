using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Responsible for all 'management' actions, such as:
///     - pressing escape to pause
///     - keeping track of general data
///     - loading things in
///     - && more.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    /// <summary>The options menu.</summary>
    private Image optionsMenu;

    [SerializeField]
    ///<summary>The main menu button under the options menu.</summary>
    private Image mainMenuButton;

    [SerializeField]
    ///<summary>The text field for the district's name.</summary>
    private TMP_Text districtNameText;

    [SerializeField]
    ///<summary>The text field for the district's progress.</summary>
    private TMP_Text districtProgressText;

    [SerializeField]
    ///<summary>The text field for the district's captured.</summary>
    private TMP_Text districtCapturedText;

    /// <summary>The Level Manager of this level. </summary>
    public static LevelManager levelManager;

    /// <summary>The number that represents this level. Zero corresponds to tutorial.</summary>
    private static int levelNumber;

    /// <summary>true if the player can interact with the game, false otherwise.</summary>
    public static bool playable = true;


    private void Start()
    {
        levelManager = this;
    }

    void Update()
    {
        CheckEscape();
    }

    /// <summary>
    /// Updates the District "inventory" display to represent the information
    /// of district <c>d</c>.
    /// </summary>
    /// <param name="d">The district to display.</param>
    public void UpdateDistrictDisplay(District d)
    {
        if (districtNameText != null) districtNameText.text = d.Name();
        if (districtCapturedText != null)
        {
            districtCapturedText.text = d.NumDeath().ToString();
        }
        if (districtProgressText != null)
        {
            districtProgressText.text = d.NumDeath().ToString() + " / " + d.Size().ToString();
        }
    }

    /// <summary>
    /// Pops up the options menu if it isn't already.
    /// Pops down the options menu if it is already up.
    /// </summary>
    private void OptionsPopup()
    {
        //TODO: Add an animation.
        if (optionsMenu.enabled)
        {
            optionsMenu.enabled = false;
            mainMenuButton.enabled = false;
        }
        else
        {
            optionsMenu.enabled = true;
            mainMenuButton.enabled = true;
        }
    }

    /// <summary>
    /// Pauses the game, performing necessary actions.
    /// </summary>
    private void PauseGame()
    {
        if (optionsMenu.enabled) Debug.Log("Game paused.");
        else Debug.Log("Game resumed.");
    }

    /// <summary>
    /// Performs some action if the user presses the escape key.
    /// </summary>
    private void CheckEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionsPopup();
            PauseGame();
        }
    }
}

