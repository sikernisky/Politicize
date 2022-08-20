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
    /// <summary>The options menu.</summary>
    private OptionsMenu optionsMenu;

    /// <summary>The Level Manager of this level. </summary>
    public static LevelManager levelManager;

    /// <summary>true if the player can interact with the game, false otherwise.</summary>
    public static bool playable = true;

    /// <summary>true if the player is playing a level. </summary>
    private static bool inGame;


    private void Awake()
    {
        SpawnAudioManager();
        SpawnOptionsMenu();
        SpawnAnimManager();
        inGame = PlayingLevel();
        SpawnSwapManager();
    }

    private void Start()
    {
        levelManager = this;
        CapMap();
    }

    void Update()
    {
        CheckEscape();
    }

    /// <summary>
    /// Instantiates the AudioManager if it isn't already in the scene.
    /// </summary>
    private void SpawnAudioManager()
    {
        if (FindObjectOfType<AudioManager>() == null)
        {
            Instantiate(Resources.Load<GameObject>("UI/AudioManager"));
        }
    }

    /// <summary>
    /// Instantiates the OptionsMenu if it isn't already in the scene.
    /// </summary>
    private void SpawnOptionsMenu()
    {
        if (FindObjectOfType<OptionsMenu>() == null)
        {
            Instantiate(Resources.Load<GameObject>("UI/OptionsMenu"), FindObjectOfType<Canvas>().transform);
            optionsMenu = FindObjectOfType<OptionsMenu>();
        }
    }


    /// <summary>
    /// Instantiates the Animation Manager if it isn't already in the scene.
    /// </summary>
    private void SpawnAnimManager()
    {
        if (FindObjectOfType<AnimManager>() == null)
        {
            Instantiate(Resources.Load<GameObject>("UI/AnimManager"));
        }
    }

    /// <summary>
    /// Instantiates the Swap Manager if it isn't already in the scene.
    /// </summary>
    private void SpawnSwapManager()
    {
        SwapManager swapManager = new GameObject("SwappableManager").AddComponent<SwapManager>();
    }

    /// <summary>
    /// Returns true if the player is playing a level, false otherwise.
    /// </summary>
    /// <returns>true if the player is playing a level, false otherwise.</returns>
    private bool PlayingLevel()
    {
        return FindObjectOfType<Map>() != null;
    }


    /// <summary>
    /// Pops up the options menu if it isn't already.
    /// Pops down the options menu if it is already up.
    /// </summary>
    private void OptionsPopup()
    {
        if (optionsMenu.gameObject.activeInHierarchy) optionsMenu.HideOptionsMenu();
        else optionsMenu.ShowOptionsMenu();
    }

    /// <summary>
    /// Performs some action if the user presses the escape key.
    /// </summary>
    private void CheckEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) OptionsPopup();
    }

    /// <summary>
    /// Caps the number of Maps per scene. 
    /// </summary>
    private void CapMap()
    {
        Map[] maps = FindObjectsOfType<Map>();
        if (maps.Length <= 1) return;
        Debug.Log("Cannot have more than one map per scene.");
        Debug.Break();
    }


}

