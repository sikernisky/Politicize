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
        Debug.Log(Application.persistentDataPath);
        if (Input.GetKeyDown(KeyCode.A)) SaveManager.data.Foo();
        if (Input.GetKeyDown(KeyCode.B)) Debug.Log(SaveManager.data.IsNull());
    }



    /// <summary>All factions, in order.</summary>
    private readonly List<string> allFactions = new List<string>()
    {
        "Arnolica",
        "Xates",
        "Thau"
    };

   

   
}
