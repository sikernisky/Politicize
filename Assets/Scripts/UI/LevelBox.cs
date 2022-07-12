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
    ///<summary>Faction this LevelBox represents.</summary>
    private string faction;

    [SerializeField]
    ///<summary>Button that changes scenes.</summary>
    private SceneChangeButton sceneChanger;

    [SerializeField]
    ///<summary>The Animator component for the seat of this LevelBox.</summary>
    private Animator seatAnimator;

    [SerializeField]
    ///<summary>Sprite to represent this seat when it has not been completed.</summary>
    private Sprite unfinishedSeat;

    [SerializeField]
    ///<summary>Sprite to represent this seat when it has been completed.</summary>
    private Sprite finishedSeat;

    [SerializeField]
    ///<summary>The sprite renderer for this seat.</summary>
    private Image seatRenderer;

    /// <summary>
    /// Performs an action when this LevelBox is clicked.
    /// </summary>
    public void ClickLevelBox()
    {
        if (SaveManager.data.Completed(faction)) return;
        if (!SaveManager.data.FactionUnlocked(faction)) return;
        int levelNumber = SaveManager.data.HighestLevel(faction);
        if(levelNumber == 0 && faction == "Arnolica")
        {
            sceneChanger.ChangeScene("Tutorial");
        }
        else sceneChanger.ChangeScene(faction + (levelNumber).ToString());
    }

    private void Update()
    {
        if(LevelSelect.FactionToPulse() == faction && !seatAnimator.GetBool("pulse")
            && !SaveManager.data.Completed(faction))
        {
            seatAnimator.SetBool("pulse", true);
        }
        if (SaveManager.data.Completed(faction)) seatRenderer.sprite = finishedSeat;
        if (!SaveManager.data.Completed(faction)) seatRenderer.sprite = unfinishedSeat;
    }
}
