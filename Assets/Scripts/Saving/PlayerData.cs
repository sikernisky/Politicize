using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represents data about the player saved into JSON.
/// </summary>
public class PlayerData
{
    /// <summary>The player's name.</summary>
    public string playerName;

    /// <summary>The level the player is on. 0 corresponds to the tutorial.</summary>
    public int currentLevel;

    /// <summary>The faction the player is on. </summary>
    public string currentFaction;

    /// <summary>The most advanced level the player has unlocked. 0 corresponds
    /// to tutorial.</summary>
    public int highestLevel;

    /// <summary>True if the player has gone through the tutorial dialogue.</summary>
    public bool tutorialDialogueCompleted;

    /// <summary>True if the player has gone through the Arnolica1 dialogue.</summary>
    public bool arnolica1DialogueCompleted;


    /// <summary>
    /// Increments <c>currentLevel</c> by one if possible.
    /// </summary>
    public void IncrementLevel()
    {
        if (currentLevel >= highestLevel) currentLevel++;
        else return;
    }
}
