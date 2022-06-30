using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField]
    ///<summary>Maximum number of swaps the player can make this Map.</summary>
    private int swapLimit;

    [SerializeField]
    /// <summary>true if there should be no swap limit to win this Map.</summary>
    private bool swapLimitDisabled;

    [SerializeField]
    ///<summary>The swap counter text for this Map.</summary>
    private TMP_Text swapCounterText;

    [SerializeField]
    ///<summary>The district counter text for this Map.</summary>
    private TMP_Text districtCounterText;

    [SerializeField]
    ///<summary>The text displaying how many districts are needed to win.</summary>
    private TMP_Text districtsNeededText;

    [SerializeField]
    ///<summary>Name of this map.</summary>
    private string mapName;

    [SerializeField]
    ///<summary>Text component displaying this Map's name.</summary>
    private TMP_Text mapNameText;

    [SerializeField]
    ///<summary>Faction this map is in.</summary>
    private string faction;

    [SerializeField]
    ///<summary>Level number of this map.</summary>
    private int levelNum;




    private void Awake()
    {
        GatherDistrictsAndSquares();
    }

    private void Start()
    {
        SetCurrentLevel();
        FillMap();
        SetupSwapCounter();
        SetupMapName();
        SetSizes();
        LevelManager.playable = true;
    }

    private void Update()
    {
        UpdateAllConnectors();
        UpdateAllDistrictSquares();
        UpdateLevelWon();
        UpdateSwapCounter();
        UpdateDistrictCounter();
        TryResetMap();
        if (Won() && !levelOver) OnWin();
    }

    /// <summary>
    /// Resets this map to how it started originally.
    /// </summary>
    public void ResetMap()
    {
        foreach(Square s in allSquares)
        {
            s.OnReset();
        }
        levelOver = false;
        SwappableSquare.ResetSwapCount();
    }

    /// <summary>
    /// Tries to reset the Map. Needs swap limit enabled.
    /// </summary>
    private void TryResetMap()
    {
        if (swapLimitDisabled) return;
        if (Won()) return;
        if (TooManySwaps()) ResetMap(0);
    }

    /// <summary>
    /// Resets this map to how it started originally after some time.
    /// </summary>
    /// <param name="delay">How long to wait before resetting.</param>
    public void ResetMap(float delay)
    {
        SwappableSquare.ResetSwapCount();
        StartCoroutine(ResetMapWithDelay(delay));
    }

    private IEnumerator ResetMapWithDelay(float delay)
    {
        LevelManager.playable = false;
        yield return new WaitForSeconds(delay);
        ResetMap();
        LevelManager.playable = true;
    }

    private void SetSizes()
    {
        foreach(Square s in allSquares)
        {
            s.transform.localScale = new Vector2(squareSize, squareSize);
        }
    }

    /// <summary>
    /// Performs actions when the Player beats this Map.
    /// </summary>
    private void OnWin()
    {
        if (dialogue == null) EndLevel();
        else Debug.Log("Dialogue is to take care of ending this level.");
    }

    /// <summary>
    /// Ends this level and returns to level selection.
    /// </summary>
    /// <param name="win">true if this level should be counted as a win.</param>
    public void EndLevel(bool win = true)
    {
        LevelManager.playable = false;
        levelOver = true;
        SaveManager.data.IncrementLevel();
        sceneChanger.ChangeScene(SaveManager.data.currentFaction + SaveManager.data.currentLevel.ToString());
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
    /// Returns the size of this Map.
    /// </summary>
    /// <returns>the integer size of this Map.</returns>
    public int Size()
    {
        return mapSize;
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


        //Checking for disqualifying conditions.

        if (ratio <= .5) mapWon = false;
        else if (SwappableSquare.TotalSwapsPerformed() > swapLimit && !swapLimitDisabled) mapWon = false;
        else mapWon = true;
    }

    /// <summary>
    /// Returns true if the player has gone over the swap limit.
    /// </summary>
    /// <returns>true if the player has gone over the swap limit.</returns>
    public bool TooManySwaps()
    {
        return SwappableSquare.TotalSwapsPerformed() >= swapLimit;
    }

    /// <summary>
    /// Fills all empty Squares in this map with placeholder Sprites.
    /// </summary>
    private void FillMap()
    {
        if(mapSize % 2 == 0)
        {
            for(int x = mapSize / -2; x <= (mapSize / 2) - 1; x++)
            {
                for(int y = mapSize / -2; y <= (mapSize / 2) - 1; y++)
                {
                    Vector2 pos = new Vector2(x + .5f, y + .5f);
                    if (SquareByPos(pos) == null) SpawnPlaceholderTile(pos);
                }
            }
        }
        else
        {
            for (int x = mapSize / -2; x <= mapSize / 2; x++)
            {
                for (int y = mapSize / -2; y <= mapSize / 2; y++)
                {
                    Debug.Log(new Vector2(x, y));
                    Vector2 pos = new Vector2(x, y);
                    if (SquareByPos(pos) == null) SpawnPlaceholderTile(pos);
                }
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

    /// <summary>
    /// Updates all Squares to display the correct connectors.
    /// </summary>
    public void UpdateAllConnectors()
    {
        foreach(Square s in allSquares)
        {
            s.DisplayConnectors();
        }
    }

    /// <summary>
    /// Updates all District's squares.
    /// </summary>
    public void UpdateAllDistrictSquares()
    {
        foreach(District d in districts)
        {
            d.UpdateSquares();
        }
    }


    /// <summary>
    /// Adds to the SwapLimit.
    /// </summary>
    /// <param name="amount">The amount to add.</param>
    public void AddToSwapLimit(int amount)
    {
        swapLimit += amount;
    }

    /// <summary>
    /// Enables the SwapLimit for this map.
    /// </summary>
    public void EnableSwapLimit()
    {
        swapLimitDisabled = false;
    }

    /// <summary>
    /// Disables the SwapLimit for this map.
    /// </summary>
    public void DisableSwapLimit()
    {
        swapLimitDisabled = true;
    }

    /// <summary>
    /// Updates the Swap counter to display the remaining number of swaps.
    /// </summary>
    private void UpdateSwapCounter()
    {
        if (swapLimitDisabled) return;
        if (swapCounterText == null) return;
        swapCounterText.text = (swapLimit - SwappableSquare.TotalSwapsPerformed()).ToString();
        if (swapLimit - SwappableSquare.TotalSwapsPerformed() <= 3) swapCounterText.color = new Color32(255, 0, 0, 255);
        else swapCounterText.color = new Color32(0, 0, 0, 255);
    }

    /// <summary>
    /// Updates the district counter to display total number of districts completed.
    /// </summary>
    private void UpdateDistrictCounter()
    {
        if (districtsNeededText == null) return;
        if (districtCounterText == null) return;
        int numWon = 0;
        foreach(District d in districts)
        {
            if (d.WinConditionMet()) numWon++;
        }
        districtCounterText.text = (numWon.ToString()) + "/" + (districts.Count.ToString());
        districtsNeededText.text = "NEED TO WIN: " + NeededDistricts().ToString(); 
    }

    /// <summary>
    /// Returns the number of districts needed to win this Map.
    /// </summary>
    /// <returns>The int number of districts needed to win this Map.</returns>
    private int NeededDistricts()
    {
        for (int i = 1; i <= districts.Count; i++)
        {
            if ((float)i / (float)districts.Count > .5f) return i;
        }
        return districts.Count;
    }

    /// <summary>
    /// Sets up the Swap Counter so that it displays the initial swap limit.
    /// </summary>
    private void SetupSwapCounter()
    {
        if (swapCounterText == null) return;
        SwappableSquare.ResetSwapCount();
        swapCounterText.text = swapLimit.ToString();
    }

    /// <summary>
    /// Sets up the display for this Map's name.
    /// </summary>
    private void SetupMapName()
    {
        if (mapName == default) mapName = "INTERIM PROPOSAL";
        mapNameText.text = "PROPOSAL " + mapName;
    }

    /// <summary>
    /// Sets the current level number and faction.
    /// </summary>
    private void SetCurrentLevel()
    {
        SaveManager.data.currentFaction = faction;
        SaveManager.data.currentLevel = levelNum;
    }

}
