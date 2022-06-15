using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the Level Select.
/// </summary>
public class LevelSelect : MonoBehaviour
{
    /// <summary>The LevelBoxes under this LevelSelect.</summary>
    private HashSet<LevelBox> boxes;

    [SerializeField]
    /// <summary>The Button that "turns" the page to the new levels.</summary>
    private Button nextPageButton;

    /// <summary>The page that the level boxes are on.</summary>
    private int currentPage;

    /// <summary>All factions, in order.</summary>
    private readonly List<string> allFactions = new List<string>()
    {
        "Arnolica",
        "Xates",
        "Thau"
    };

    private void Start()
    {
        GatherBoxes();
    }

    /// <summary>
    /// Obtains all LevelBoxes under this LevelSelect.
    /// </summary>
    private void GatherBoxes()
    {
        boxes = new HashSet<LevelBox>();
        foreach(Transform t in transform)
        {
            LevelBox lb = t.GetComponent<LevelBox>();
            if (lb != null) boxes.Add(lb);
        }
    }

    /// <summary>
    /// Performs some action when the button to go the next page of levels
    /// is pressed.
    /// </summary>
    public void ClickNextPage()
    {
        int increment = boxes.Count;
        currentPage++;
        foreach(LevelBox lb in boxes) { lb.IncrementNumber(increment); }
    }

    /// <summary>
    /// Returns the name of the current faction this page represents.
    /// </summary>
    /// <returns>The string name of the current faction this page represents.</returns>
    public string CurrentFaction()
    {
        if (currentPage > allFactions.Count - 1) return "";
        return allFactions[currentPage];
    }
}
