using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Represents data about the player saved into JSON.
/// </summary>
public class PlayerData
{
 
    /// <summary>The player's name.</summary>
    public string playerName;

    /// <summary>The level the player is on. 0 corresponds to the tutorial.</summary>
    private int currentLevel;

    /// <summary>The faction the player is on. </summary>
    public string currentFaction;

    /// <summary>True if the player chose to buy more swaps.</summary>
    public bool moreSwaps;

    /// <summary>True if the player has access to the undo button. </summary>
    public bool undoActive = true;

    /// <summary>Factions and their highest level unlocked.</summary>
    public Dictionary<string, int> factions = new Dictionary<string, int>()
    {
        {"Arnolica", 0},
        {"Xates", 0 },
        {"Ryndalma", 0}
    };

    /// <summary>Factions and whether they have been completed or not. </summary>
    public Dictionary<string, bool> factionsCompleted = new Dictionary<string, bool>()
    {
        {"Arnolica", false},
        {"Xates", false},
        {"Ryndalma", false}
    };

    /// <summary>Factions and whether they have been unlocked or not.</summary>
    public Dictionary<string, bool> factionsUnlocked = new Dictionary<string, bool>()
    {
        {"Arnolica", true },
        {"Xates", false },
        {"Ryndalma", false}
    };


    /// <summary>
    /// Sets the current faction.
    /// </summary>
    /// <param name="factionName">The faction to set.</param>
    public void SetCurrentFaction(string factionName)
    {
        if (!factions.ContainsKey(factionName)) return;
        currentFaction = factionName;
    }



    /// <summary>
    /// Sets the current level.
    /// </summary>
    public void SetCurrrentLevel(int currLevel)
    {
        if (currLevel < 0) return;
        currentLevel = currLevel;
        UpdateHighestLevel(CurrentFaction(), CurrentLevel());
    }

    /// <summary>
    /// Returns the current level.
    /// </summary>
    /// <returns>the current level.</returns>
    public int CurrentLevel()
    {
        return currentLevel;
    }


    /// <summary>
    /// Returns the current faction.
    /// </summary>
    /// <returns>the current faction.</returns>
    public string CurrentFaction()
    {
        return currentFaction;
    }


    /// <summary>
    /// Updates the highest level reached for a faction.
    /// </summary>
    /// <param name="factionName">The name of the faction.</param>
    /// <param name="newHighest">The new highest level.</param>
    private void UpdateHighestLevel(string factionName, int newHighest)
    {
        if (!factions.ContainsKey(factionName)) return;
        if (newHighest > factions[factionName]) factions[factionName] = newHighest;
    }

    /// <summary>
    /// Returns the highest level reached in a given faction.
    /// </summary>
    /// <param name="factionName">The faction to get the highest level reached.</param>
    /// <returns> the highest level reached in a given faction.</returns>
    public int HighestLevel(string factionName)
    {
        if (!factions.ContainsKey(factionName)) return 0;
        return factions[factionName];
    }

    /// <summary>
    /// Completes a faction.
    /// </summary>
    /// <param name="factionName">The faction to complete.</param>
    public void CompleteFaction(string factionName)
    {
        if (!factionsCompleted.ContainsKey(factionName)) return;
        factionsCompleted[factionName] = true;
    }

    /// <summary>
    /// Returns true if a faction is completed.
    /// </summary>
    /// <param name="factionName">the faction to check if completed.</param>
    /// <returns>true if factionName is completed.</returns>
    public bool Completed(string factionName)
    {
        if (!factionsCompleted.ContainsKey(factionName)) return false;
        return factionsCompleted[factionName];
    }

    /// <summary>
    /// Unlocks the next faction.
    /// </summary>
    /// <param name="factionName">the faction to before the one to unlock.</param>
    public void UnlockFaction(string factionName)
    {
        string nextFac = "";
        switch (factionName)
        {
            case "Arnolica":
                nextFac = "Xates";
                break;
            case "Xates":
                nextFac = "Ryndalma";
                break;
            default:
                nextFac = "Arnolica";
                break;
        }
        if (!factionsUnlocked.ContainsKey(nextFac)) return;
        factionsUnlocked[nextFac] = true;
    }

    /// <summary>
    /// Returns true if a faction has been unlocked. 
    /// </summary>
    /// <param name="factionName">The faction to check if unlocked or not.</param>
    /// <returns>True or false depending on if the faction is unlocked.</returns>
    public bool FactionUnlocked(string factionName)
    {
        if (!factionsUnlocked.ContainsKey(factionName)) return false;
        return factionsUnlocked[factionName];
    }


  


}
