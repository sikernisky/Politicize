using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the Level Select.
/// </summary>
public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    ///<summary>The LevelBox for Arnolica.</summary>
    private LevelBox arnolicaBox;


    private void Update()
    {

    }


    /// <summary>
    /// Returns the faction that should be pulsing in level select.
    /// </summary>
    /// <returns>the string name of the faction.</returns>
    public static string FactionToPulse()
    {
        if (SaveManager.data.Completed("Xates"))
        {
            return "";
        }
        if (SaveManager.data.Completed("Arnolica"))
        {
            return "Xates";
        }
        return "Arnolica";

    }



    /// <summary>All factions, in order.</summary>
    private readonly List<string> allFactions = new List<string>()
    {
        "Arnolica",
        "Xates",
        "Thau"
    };

   

   
}
