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
    ///<summary>The number of this level.</summary>
    private int levelNumber;

    [SerializeField]
    ///<summary>The text on this LevelBox.</summary>
    private TMP_Text boxText;

    [SerializeField]
    ///<summary>This LevelBox's SpriteRenderer.</summary>
    private Image levelBoxImage;

    [SerializeField]
    ///<summary>The Sprite that represents a completed LevelBox.</summary>
    private Sprite completedSprite;

    [SerializeField]
    ///<summary>The Sprite that represents an uncompleted LevelBox.</summary>
    private Sprite uncompletedSprite;

    /// <summary>The level select manager.</summary>
    private LevelSelect levelSelect;

    [SerializeField]
    /// <summary>The scene changer.</summary>
    private SceneChangeButton sceneChanger;

    private void Start()
    {
        boxText.text = levelNumber.ToString();
        SetBoxSprite();
        FindLevelSelect();
    }

    public void ClickBox()
    {
        sceneChanger.ChangeScene(levelSelect.CurrentFaction() + levelNumber.ToString());
    }

    /// <summary>
    /// Increments this LevelBox's level number by <c>newNumber</c>.
    /// </summary>
    /// <param name="newNumber">The number to increment by, >= 1.</param>
    public void IncrementNumber(int newNumber)
    {
        levelNumber += newNumber;
        boxText.text = levelNumber.ToString();
        SetBoxSprite();
    }

    /// <summary>
    /// Sets this LevelBox's Sprite to completed if the player has completed its level.
    /// Otherwise, sets it to uncompleted.
    /// </summary>
    private void SetBoxSprite()
    {
        if (SaveManager.data.highestLevel > levelNumber) levelBoxImage.sprite = completedSprite;
        else levelBoxImage.sprite = uncompletedSprite;
    }

    /// <summary>
    /// Finds the LevelSelect script and sets it to this LevelBox.
    /// </summary>
    private void FindLevelSelect()
    {
        levelSelect = transform.parent.GetComponent<LevelSelect>();
    }
}
