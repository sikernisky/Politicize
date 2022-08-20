using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Represents boxes in the level selection area.
/// </summary>
public class LevelBox : MonoBehaviour
{

    [SerializeField]
    ///<summary>Sprite to represent this seat when it has not been completed.</summary>
    private Sprite unfinishedSeat;

    [SerializeField]
    ///<summary>Sprite to represent this seat when it has been completed.</summary>
    private Sprite finishedSeat;

    [SerializeField]
    ///<summary>Sprite to represent this seat when no level for it exists.</summary>
    private Sprite nonexistentSeat;

    [SerializeField]
    ///<summary>Sprite to represent this seat when it is the next level to beat.</summary>
    private Sprite nextUpSeat;

    [SerializeField]
    ///<summary>Text component to display this Box's level number.</summary>
    private TMP_Text levelNumberText;

    [SerializeField]
    ///<summary>The sprite renderer for this seat.</summary>
    private Image seatImage;

    [SerializeField]
    ///<summary>The level this LevelBox takes the player to.</summary>
    private int levelNumber;

    /// <summary>The faction this LevelBox represents. </summary>
    private string faction;


    /// <summary>
    /// Sets the faction of this LevelBox.
    /// </summary>
    /// <param name="faction">The faction to set it to.</param>
    public void SetFaction(string faction)
    {
        if (!LevelSelect.factions.Contains(faction)) return;
        this.faction = faction;
    }


    /// <summary>
    /// Sets this LevelBox's image to green or purple depending on if the player
    /// has completed its level or not.
    /// </summary>
    public void SetBoxImage()
    {
        if (!SaveManager.data.FactionUnlocked(faction))
        {
            seatImage.sprite = unfinishedSeat;
            levelNumberText.text = levelNumber.ToString();
            GetComponent<Button>().interactable = false;
        }
        else if (levelNumber > LevelSelect.maxLevels[faction])
        {
            seatImage.sprite = nonexistentSeat;
            levelNumberText.text = "";
            GetComponent<Button>().interactable = false;
        }
        else if (SaveManager.data.HighestLevel(faction) + 1 > levelNumber)
        {
            seatImage.sprite = finishedSeat;
            levelNumberText.text = levelNumber.ToString();
            GetComponent<Button>().interactable = true;
        }
        else if (SaveManager.data.HighestLevel(faction) + 1 == levelNumber)
        {
            if (SaveManager.data.Completed(faction)) seatImage.sprite = finishedSeat;
            else seatImage.sprite = nextUpSeat;
            levelNumberText.text = levelNumber.ToString();
            GetComponent<Button>().interactable = true;
        }
        else
        {
            seatImage.sprite = unfinishedSeat;
            levelNumberText.text = levelNumber.ToString();
            GetComponent<Button>().interactable = false;
        }
    }

    /// <summary>
    /// Loads the scene when this LevelBox is clicked.
    /// </summary>
    public void ClickLevelBox()
    {
        if (!SaveManager.data.FactionUnlocked(faction)) return;
        if (faction == "Arnolica" && levelNumber == 1) FindObjectOfType<SceneChangeButton>().ChangeScene("Tutorial", true);
        else FindObjectOfType<SceneChangeButton>().ChangeScene(faction + (levelNumber -1).ToString(), true);
    }


}
