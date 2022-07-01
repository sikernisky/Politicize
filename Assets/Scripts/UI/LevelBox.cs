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

    /// <summary>
    /// Performs an action when this LevelBox is clicked.
    /// </summary>
    public void ClickLevelBox()
    {
        int levelNumber = SaveManager.data.HighestLevel(faction);
        if(levelNumber == 0)
        {
            sceneChanger.ChangeScene("Tutorial");
        }
        else sceneChanger.ChangeScene(faction + (levelNumber).ToString());
    }
}
