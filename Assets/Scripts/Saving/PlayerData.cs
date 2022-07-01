using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Represents data about the player saved into JSON.
/// </summary>
public class PlayerData
{
    /// <summary>All factions.</summary>
    private readonly List<string> factionNames = new List<string>()
    {
        "Arnolica"
    };


    /// <summary>The player's name.</summary>
    public string playerName;

    /// <summary>The level the player is on. 0 corresponds to the tutorial.</summary>
    public int currentLevel;

    /// <summary>The faction the player is on. </summary>
    public string currentFaction;


    /// <summary>The current faction.</summary>
    private FactionProgress currentFactionProgress;


    [System.Serializable]
    public class FactionProgress
    {
        List<string> levelsUnlocked;
    }


    /// <summary>
    /// Sets the current faction.
    /// </summary>
    /// <param name="factionName">The faction to set.</param>
    public void SetCurrentFaction(string factionName)
    {
        if (!factionNames.Contains(factionName)) return;
        if (FactionByName(factionName) == null)
        {

        }
    }

  
    private FactionProgress FactionByName(string factionName)
    {
        switch (factionName)
        {
            case "Arnolica":
                return arnolica;
            default:
                return null;
        }
    }



    /// <summary>
    /// Increments <c>currentLevel</c> by one if possible.
    /// </summary>
    public void SetCurrrentLevel(int currLevel)
    {
        
    }



    /// <summary>
    /// Updates the highest level reached for a faction.
    /// </summary>
    /// <param name="factionName">The name of the faction.</param>
    /// <param name="newHighest">The new highest level.</param>
    private void UpdateHighestLevel(string factionName, int newHighest)
    {
        
    }

    /// <summary>
    /// Returns the highest level reached in a given faction.
    /// </summary>
    /// <param name="factionName">The faction to get the highest level reached.</param>
    /// <returns> the highest level reached in a given faction.</returns>
    public int HighestLevel(string factionName)
    {
        return default;
    }

  


}
