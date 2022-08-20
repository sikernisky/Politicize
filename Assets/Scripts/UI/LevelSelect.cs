using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the Level Select.
/// </summary>
public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    ///<summary>All LevelBoxes. </summary>
    private HashSet<LevelBox> boxes;

    [SerializeField]
    ///<summary>The background image for LevelSelect. </summary>
    private Image backgroundImage;

    [SerializeField]
    ///<summary>The title image for LevelSelect. </summary>
    private Image titleImage;

    [SerializeField]
    ///<summary>The background sprite for Arnolica.</summary>
    private Sprite arnolicaBackground;

    [SerializeField]
    ///<summary>The title sprite for Arnolica.</summary>
    private Sprite arnolicaTitle;

    [SerializeField]
    ///<summary>The background sprite for Foliard.</summary>
    private Sprite foliardBackground;

    [SerializeField]
    ///<summary>The title sprite for Foliard.</summary>
    private Sprite foliardTitle;

    [SerializeField]
    ///<summary>The background sprite for Xates.</summary>
    private Sprite xatesBackground;

    [SerializeField]
    ///<summary>The title sprite for Xates.</summary>
    private Sprite xatesTitle;

    /*    [SerializeField]
        ///<summary>The background sprite for Gorneo.</summary>
        private Sprite gorneoBackground;

        [SerializeField]
        ///<summary>The title sprite for Gorneo.</summary>
        private Sprite gorneoTitle;*/

    [SerializeField]
    ///<summary>Button to load the previous faction.</summary>
    private Button prevArrow;

    [SerializeField]
    ///<summary>Button to load the next faction.</summary>
    private Button nextArrow;

    /// <summary>Index of the current LevelSelect page.</summary>
    private int pageIndex;

    [SerializeField]
    ///<summary>The scene changer.</summary>
    private SceneChangeButton sceneChanger;

    /// <summary>The options menu.</summary>
    private GameObject optionsMenu;


    /// <summary>All factions.</summary>
    public static readonly List<string> factions = new List<string>()
    {
        "Arnolica",
        "Foliard",
        "Xates"
    };


    /// <summary>Each faction and its maximum number of levels.</summary>
    public static readonly Dictionary<string, int> maxLevels = new Dictionary<string, int>()
    {
        {"Arnolica", 14},
        {"Foliard", 14 },
        {"Xates", 8 }
    };

    ///<summary>The faction the current page of the level select represents.</summary>
    private string factionPage;


    private void Awake()
    {
        GatherLevelBoxes();
    }

    private void Start()
    {
        RefreshPage();
        SaveManager.data.currentFaction = "MainMenu";
    }


    /// <summary>
    /// Finds all LevelBoxes.
    /// </summary>
    private void GatherLevelBoxes()
    {
        boxes = new HashSet<LevelBox>(FindObjectsOfType<LevelBox>());
    }

    /// <summary>
    /// Updates the current LevelSelect page to reflect the current faction.
    /// </summary>
    private void RefreshPage()
    {
        pageIndex = SaveManager.data.levelSelectPage;
        factionPage = factions[pageIndex];
        foreach (LevelBox lb in boxes)
        {
            lb.SetFaction(factionPage);
            lb.SetBoxImage();
        }
        SetBackgroundImage();
        SetTitle();
        SetArrows();
    }

    /// <summary>
    /// Increments the LevelSelect page.
    /// </summary>
    public void NextPage()
    {
        if (pageIndex == factions.Count - 1) return;
        if (!SaveManager.data.FactionUnlocked(factions[pageIndex + 1])) return;
        pageIndex++;
        SaveManager.data.levelSelectPage = pageIndex;
        RefreshPage();
    }

    /// <summary>
    /// Decrements the LevelSelect page.
    /// </summary>
    public void PrevPage()
    {
        if (pageIndex == 0) return;
        if (!SaveManager.data.FactionUnlocked(factions[pageIndex -1])) return;
        pageIndex--;
        SaveManager.data.levelSelectPage = pageIndex;
        RefreshPage();
    }


    /// <summary>
    /// Returns the faction the LevelSelect is currently on.
    /// </summary>
    /// <returns>The string faction the LevelSelect is currently on.</returns>
    public string Faction()
    {
        return factionPage;
    }

    /// <summary>
    /// Sets the background image of LevelSelect depending on the current faction.
    /// </summary>
    private void SetBackgroundImage()
    {
        if (factionPage == "Arnolica") backgroundImage.sprite = arnolicaBackground;
        if (factionPage == "Foliard") backgroundImage.sprite = foliardBackground;
        if (factionPage == "Xates") backgroundImage.sprite = xatesBackground;
        //if (factionPage == "Gorneo") backgroundImage.sprite = gorneoBackground;
    }


    /// <summary>
    /// Sets the Title image of LevelSelect depending on the current faction.
    /// </summary>
    private void SetTitle()
    {
        if (factionPage == "Arnolica") titleImage.sprite = arnolicaTitle;
        if (factionPage == "Foliard") titleImage.sprite = foliardTitle;
        if (factionPage == "Xates") titleImage.sprite = xatesTitle;
        //if (factionPage == "Gorneo") titleImage.sprite = gorneoTitle;
    }

    /// <summary>
    /// Sets the Arrows' interactability depending on the current faction.
    /// </summary>
    private void SetArrows()
    {
        nextArrow.interactable = true;
        prevArrow.interactable = true;
        if (pageIndex == 0 || !SaveManager.data.FactionUnlocked(factions[pageIndex - 1]))
        {
            prevArrow.interactable = false;
        }
        if (pageIndex == factions.Count - 1 || !SaveManager.data.FactionUnlocked(factions[pageIndex + 1]))
        {
            nextArrow.interactable = false;
        }


    }
}
