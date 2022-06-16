using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the container for all Squares in a level.
/// </summary>
public class Map : MonoBehaviour
{

    [SerializeField]
    ///<summary>The size of each Square in this Map.</summary>
    private float squareSize;

    [SerializeField]
    ///<summary>The height and width, in Squares, of this map.</summary>
    private int mapSize;

    /// <summary>All Districts in this Map. </summary>
    private HashSet<District> districts;

    /// <summary>All Squares in this Map. </summary>
    private HashSet<Square> allSquares;

    /// <summary>true if the player has won this Map.</summary>
    private bool mapWon;

    /// <summary>true if the level is over and the game should stop working as normal.</summary>
    private bool levelOver;

    [SerializeField]
    ///<summary>The manager that changes scenes in this scene.</summary>
    private SceneChangeButton sceneChanger;

    [SerializeField]
    ///<summary>This level's dialogue manager.</summary>
    private DialogueManager dialogue;

    [SerializeField]
    ///<summary>The background (usually a scroll) of this map.</summary>
    private SpriteRenderer mapBackground;

    [SerializeField]
    ///<summary>Sprite to represent placeholder tiles.</summary>
    private Sprite placeholderSprite;

    [SerializeField]
    ///<summary>Parent GameObject holding placeholder tiles.</summary>
    private GameObject placeholderParent;

    private void Awake()
    {
        GatherDistrictsAndSquares();
    }

    private void Start()
    {
        FillMap();
    }

    private void Update()
    {
        UpdateLevelWon();
        if (Won() && !levelOver) OnWin();
        if (Input.GetKeyDown(KeyCode.R)) ResetMap();
    }

    /// <summary>
    /// Resets this map to how it started originally.
    /// </summary>
    public void ResetMap()
    {
        foreach(Square s in allSquares)
        {
            s.ResetGridPosition();
        }
        levelOver = false;
        mapWon = false;
    }

    /// <summary>
    /// Performs actions when the Player beats this Map.
    /// </summary>
    private void OnWin()
    {
        if (dialogue != null && dialogue.SpeakAfterWin())
        {
            dialogue.StartWinDialogue();
        }
        else EndLevel();
    }

    /// <summary>
    /// Ends this level and returns to level selection.
    /// </summary>
    /// <param name="win">true if this level should be counted as a win.</param>
    public void EndLevel(bool win = true)
    {
        LevelManager.playable = false;
        levelOver = true;
        sceneChanger.ChangeScene("LevelSelect");
        SaveManager.data.IncrementLevel();
    }

    /// <summary>
    /// Finds and adds all Districts in this Map to districts.
    /// </summary>
    /// <returns>Nothing.</returns>
    private void GatherDistrictsAndSquares()
    {
        districts = new HashSet<District>();
        allSquares = new HashSet<Square>();
        foreach(Transform t in transform)
        {
            District d = t.GetComponent<District>();
            if (d != null)
            {
                districts.Add(d);
                foreach(Transform tt in d.transform)
                {
                    Square s = tt.GetComponent<Square>();
                    if (s != null) allSquares.Add(s);
                }
            }
        }
    }

    /// <summary>
    /// Returns true if the player has won the level.
    /// </summary>
    /// <returns>true if the player has won this Map; false otherwise.</returns>
    public bool Won()
    {
        return mapWon;
    }

    /// <summary>
    /// Returns the Square with position <c>squarePos.</c>
    /// </summary>
    /// <returns>The square with position <c>squarePos</c>, or null if 
    /// that Square doesn't exist.</returns>
    public Square SquareByPos(Vector2 squarePos)
    {

        foreach (Square s in allSquares)
        {
            if (squarePos == s.MapPosition()) return s;
        }
        return null;
    }

    /// <summary>
    /// Returns the size of all Squares in this Map.
    /// </summary>
    /// <returns>The float size of each Square in this Map.</returns>
    public float SquareSize()
    {
        return squareSize;
    }

    /// <summary>
    /// Sets mapWon to true if all Districts have their win conditions satisfied.
    /// </summary>
    private void UpdateLevelWon()
    {
        int wonDistricts = 0;

        foreach(District d in districts)
        {
            if (d.WinConditionMet()) wonDistricts++;
        }

        float ratio = (float)wonDistricts / districts.Count;
        if (ratio > .5) mapWon = true;
        else mapWon = false;
    }

    /// <summary>
    /// Fills all empty Squares in this map with placeholder Sprites.
    /// </summary>
    private void FillMap()
    {
        for (int x = (int)mapSize / -2; x <= (int)mapSize / 2; x++)
        {
            for (int y = (int)mapSize / -2; y <= (int)mapSize / 2; y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (SquareByPos(pos) == null) SpawnPlaceholderTile(pos);
            }
        }
    }

    /// <summary>
    /// Spawns a placeholder tile at <c>position</c>.
    /// </summary>
    /// <param name="position">The position to spawn the placeholder tile.</param>
    private void SpawnPlaceholderTile(Vector2 position)
    {
        GameObject placeHolder = new GameObject("PlaceHolder");
        SpriteRenderer rend = placeHolder.AddComponent<SpriteRenderer>();
        
        placeHolder.transform.SetParent(placeholderParent.transform);
        placeHolder.transform.localScale = new Vector2(squareSize, squareSize);
        placeHolder.transform.localPosition = position * squareSize;

        rend.sprite = placeholderSprite;
        rend.sortingOrder = 1;
    }

}
