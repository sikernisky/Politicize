using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Represents the container for all Squares in a level.
/// 
/// There may only be one map per scene.
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

    /// <summary>All Squares that started on this Map. </summary>
    private HashSet<Square> originalSquares;

    /// <summary>All Squares that are currently on this Map. </summary>
    private HashSet<Square> activeSquares;

    /// <summary>All Obstacles in this map.</summary>
    private HashSet<Obstacle> obstacles;

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
    ///<summary>Level number of this map. </summary>
    private int levelNum;

    [SerializeField]
    ///<summary>true if this Map is the last level in this Faction.</summary>
    private bool lastLevel;

    [SerializeField]
    ///<summary>Animator for this Map. </summary>
    private Animator mapAnimator;

    [SerializeField]
    ///<summary>True if this Map should pop in at the start of the scene.</summary>
    private bool popInOnStart;

    [SerializeField]
    ///<summary>The label for this map.</summary>
    private GameObject label;

    [SerializeField]
    ///<summary>The text component for the label.</summary>
    private TMP_Text labelText;

    [SerializeField]
    ///<summary>The undo button.</summary>
    private GameObject undoButton;

    /// <summary>
    /// States of the map as the player swaps.
    /// </summary>
    private Stack<(Dictionary<Square, Vector2Int>, Dictionary<Square, Transform>)> mapStates;

    /// <summary>The positions part of the map state that was last added. </summary>
    private Dictionary<Square, Vector2Int> prevAddedPositions;

    /// <summary>The parents part of the map state that was last added. </summary>
    private Dictionary<Square, Transform> prevAddedParents;

    /// <summary>Number of swaps the player has left.</summary>
    private int swapsRemaining;

    /// <summary>This map's graveyard.</summary>
    private Transform graveyard;

    /// <summary>true if this map is resetting is tiles.</summary>
    private bool resetting;

    /// <summary>How many times the user has pressed the arrow keys. </summary>
    private static int arrowKeyCounter;

    [SerializeField]
    ///<summary>true if this Map should reset on start.</summary>
    private bool resetOnStart = true;

    /// <summary>true if this Map is shaking.</summary>
    private bool shaking;


    private void Awake()
    {
        GatherDistrictsAndSquares();
        CreateGraveyard();
    }

    private void Start()
    {
        SetCurrentLevel();
        FillMap();
        SetupSwapCounter();
        SetupMapName();
        SetSizes();
        PlayAnimations();
        EnablePowerups();
        if(resetOnStart) ResetMap(false);

        LevelManager.playable = true;
    }

    private void Update()
    {
        UpdateAllConnectors();
        UpdateAllDistrictSquares();
        UpdateLevelWon();
        UpdateSwapCounter();
        UpdateDistrictCounter();
        if (Won() && !levelOver) OnWin();

        //if (Input.GetKeyDown(KeyCode.Z)) EndLevel();
        if (Input.GetKeyDown(KeyCode.R)) ResetMap();
        if (Input.GetKeyDown(KeyCode.U)) TryUndo();

        if (shaking) Debug.Log("shaking");
    }



    /// <summary>
    /// Resets this map to how it started originally:
    /// 
    /// - Lerps all Squares to their starting positions
    /// - Resets all counters
    /// - (Re)enables all districts.
    /// </summary>
    public void ResetMap(bool playAudio = true)
    {
        if (Won()) return;
        foreach (Square s in allSquares)
        {
            if (s.Resetting()) return;
        }

        StartCoroutine(ResetMapCoro(playAudio));
    }

    /// <summary>
    /// Resets the Map without checking for disqualifying conditions.
    /// </summary>
    public void ForceResetMap()
    {
        StartCoroutine(ResetMapCoro());
    }

    private IEnumerator ResetMapCoro(bool playAudio = true)
    {

        levelOver = false;
        resetting = true;
        SwapManager.ResetSwapCount();

        AudioManager am = FindObjectOfType<AudioManager>();
        if (am != null && !am.Playing("Reset") && playAudio) am.Play("Reset");

        yield return new WaitForSeconds(SwappableSquare.swapLerpDuration);


        if (mapStates != null) mapStates.Clear();

        HashSet<Square> newAllSquares = new HashSet<Square>();
             
        foreach(District d in districts)
        {
            d.EnableDistrict();
        }

        foreach (Square s in allSquares)
        {
            if (!originalSquares.Contains(s))
            {
                Destroy(s.gameObject);
                activeSquares.Remove(s);
            }
            else
            {
                activeSquares.Add(s);
                newAllSquares.Add(s);
                s.gameObject.SetActive(transform);
                s.OnReset();
            }
        }
        allSquares = newAllSquares;
        resetting = false;

    }

    /// <summary>
    /// Tries to reset the Map. Needs swap limit enabled.
    /// </summary>
    public void TryResetMap()
    {
        if (swapLimitDisabled) return;
        if (Won()) return;
        if (TooManySwaps()) ResetMap();
    }


    /// <summary>
    /// Tries to undo the board.
    /// </summary>
    public void TryUndo()
    {
        if (mapStates == null || mapStates.Count == 0) return;
        if (Won()) return;
        if (resetting) return;
        if (Shaking()) return;


        foreach (Square s in allSquares)
        {
            if (s.Resetting()) return;
        }


        FindObjectOfType<AudioManager>().Play("Undo");

        (Dictionary<Square, Vector2Int>, Dictionary<Square, Transform>) state = mapStates.Pop();


        List<Square> allowedSquares = new List<Square>(state.Item1.Keys);
        UndoFilter(allowedSquares);

        foreach(KeyValuePair<Square, Vector2Int> pair in state.Item1)
        {
            Square s = pair.Key;
            if (!s.Resetting())
            {
                Vector2Int pos = pair.Value; //The position of the square
                Transform t = state.Item2[pair.Key]; //The transform of the square
                s.gameObject.SetActive(true);
                s.OnUndo(pos, t);
                activeSquares.Add(s);
                allSquares.Add(s);
            }
        }
        SwapManager.AddToSwapCount(1);

    }

    /// <summary>
    /// Filters Squares such that only previous state's squares can remain on the board.
    /// </summary>
    /// <param name="allowedSquares">The list of squares not to destroy.</param>
    private void UndoFilter(List<Square> allowedSquares)
    {
        foreach(District d in districts)
        {
            foreach(Square s in d.DistrictSquares())
            {

                if (!allowedSquares.Contains(s))
                {
                    activeSquares.Remove(s);
                    allSquares.Remove(s);
                    Destroy(s.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Sets the scale of all Squares in this map to <c>squareSize</c>.
    /// </summary>
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

        FindObjectOfType<AudioManager>().Play("WinLevel");

        if (lastLevel)
        {
            if(faction == "Xates") sceneChanger.ChangeScene("DemoEnd");
            else sceneChanger.ChangeScene("LevelSelect");
            SaveManager.data.CompleteFaction(faction);
            SaveManager.data.UnlockNextFaction(faction);
        }
        else
        {
            if (sceneChanger == null) sceneChanger = FindObjectOfType<SceneChangeButton>();
            sceneChanger.ChangeScene(SaveManager.data.currentFaction + (SaveManager.data.CurrentLevel() + 1).ToString());
        }

    }

    /// <summary>
    /// Finds and adds all Districts in this Map to districts.
    /// </summary>
    /// <returns>Nothing.</returns>
    private void GatherDistrictsAndSquares()
    {
        districts = new HashSet<District>();
        allSquares = new HashSet<Square>();
        originalSquares = new HashSet<Square>();
        activeSquares = new HashSet<Square>();
        obstacles = new HashSet<Obstacle>();
        foreach(Transform t in transform)
        {
            District d = t.GetComponent<District>();
            Obstacle o = t.GetComponent<Obstacle>();

            if (d != null) //Child is a District.
            {
                districts.Add(d);
                foreach(Transform tt in d.transform)
                {
                    Square s = tt.GetComponent<Square>();
                    if (s != null)
                    {
                        allSquares.Add(s);
                        originalSquares.Add(s);
                        activeSquares.Add(s);
                        s.FindParentDistrictAndMap();
                    }
                }
            }

            if(o != null) //Child is an Obstacle.
            {
                obstacles.Add(o);
            }
        }
    }

    /// <summary>
    /// Creates the graveyard.
    /// </summary>
    private void CreateGraveyard()
    {
        GameObject gy = new GameObject("Graveyard");
        gy.transform.SetParent(transform);
        gy.transform.localPosition = Vector3.zero;
        gy.transform.localScale = new Vector3(1, 1, 1);
        graveyard = gy.transform;
    }

    /// <summary>
    /// Sends a Square to the graveyard.
    /// </summary>
    /// <param name="s"></param>
    public void BanishSquare(Square s)
    {
        if (!activeSquares.Contains(s)) return;
        s.OnBanish();
        s.transform.SetParent(graveyard);
        activeSquares.Remove(s);
        s.gameObject.SetActive(false);
    }

    /// <summary>
    /// Returns true if a Square is banished.
    /// </summary>
    /// <returns></returns>
    public bool Banished(Square s)
    {
        foreach(Transform t in graveyard)
        {
            if (t.GetComponent<Square>() == s) return true;
        }
        return false;
    }

    /// <summary>
    /// Adds a Square to this map under a district.
    /// </summary>
    /// <param name="s">The Square to add.</param>
    /// <param name="d">The district to add it to.</param>
    public void AddSquare(Square s, District d, bool active = true)
    {
        allSquares.Add(s);
        if(active) activeSquares.Add(s);
        s.transform.SetParent(d.transform);
        s.FindParentDistrictAndMap();
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
        foreach (Square s in activeSquares)
        {
            if (squarePos == s.MapPosition()) return s;
        }
        return null;
    }

    /// <summary>
    /// Returns a HashSet of Squares that surround a position.
    /// </summary>
    /// <returns></returns>
    public HashSet<Square> SurroundingSquares(Vector2Int pos)
    {
        HashSet<Square> surrounding = new HashSet<Square>();
        int xPos = pos.x;
        int yPos = pos.y;

        Square left = SquareByPos(new Vector2Int(xPos - 1, yPos));
        if (left != null) surrounding.Add(left);
        Square right = SquareByPos(new Vector2Int(xPos + 1, yPos));
        if (right != null) surrounding.Add(right);
        Square up = SquareByPos(new Vector2Int(xPos, yPos + 1));
        if (up != null) surrounding.Add(up);
        Square down = SquareByPos(new Vector2Int(xPos, yPos - 1));
        if (down != null) surrounding.Add(down);
        Square topLeft = SquareByPos(new Vector2Int(xPos - 1, yPos + 1));
        if (topLeft != null) surrounding.Add(topLeft);
        Square topRight = SquareByPos(new Vector2Int(xPos + 1, yPos + 1));
        if (topRight != null) surrounding.Add(topRight);
        Square botLeft = SquareByPos(new Vector2Int(xPos - 1, yPos - 1));
        if (botLeft != null) surrounding.Add(botLeft);
        Square botRight = SquareByPos(new Vector2Int(xPos + 1, yPos - 1));
        if (botRight != null) surrounding.Add(botRight);

        return surrounding;
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
        int activeDistricts = 0;
        foreach(District d in districts)
        {
            if (d.WinConditionMet() && d.Active()) wonDistricts++;
            if (d.Active()) activeDistricts++;
        }

        float ratio = (float)wonDistricts / activeDistricts;

        if (ratio <= .5) mapWon = false;
        else if (SwapManager.CurrentSwaps() > swapLimit && SwapLimitEnabled()) mapWon = false;
        else mapWon = true;
    }

    /// <summary>
    /// Returns true if the player has gone over the swap limit.
    /// </summary>
    /// <returns>true if the player has gone over the swap limit.</returns>
    public bool TooManySwaps()
    {
        return SwapManager.CurrentSwaps() >= swapLimit;
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
        rend.sortingOrder = 0;
    }

    /// <summary>
    /// Returns the faction of this map.
    /// </summary>
    /// <returns>the faction.</returns>
    public string Faction()
    {
        return faction;
    }

    /// <summary>
    /// Updates all Squares to display the correct connectors.
    /// </summary>
    public void UpdateAllConnectors()
    {
        foreach(Square s in allSquares)
        {
            if(s.gameObject.activeInHierarchy) s.DisplayConnectors();
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
    /// Returns true if the swap limit is enabled.
    /// </summary>
    /// <returns>true if the swap limit is enabled.s</returns>
    public bool SwapLimitEnabled()
    {
        return !swapLimitDisabled;
    }

    /// <summary>
    /// Updates the Swap counter to display the remaining number of swaps.
    /// </summary>
    private void UpdateSwapCounter()
    {
        swapsRemaining = swapLimit - SwapManager.CurrentSwaps();
        if (swapLimitDisabled) return;
        if (swapCounterText == null) return;
        swapCounterText.text = swapsRemaining.ToString();
        if (swapsRemaining <= 3) swapCounterText.color = new Color32(255, 0, 0, 255);
        else swapCounterText.color = new Color32(0, 0, 0, 255);
    }

    /// <summary>
    /// Returns the number of swaps left.
    /// </summary>
    /// <returns>the int number of swaps left.</returns>
    public int RemainingSwaps()
    {
        return swapsRemaining;
    }

    /// <summary>
    /// Updates the district counter to display total number of districts completed.
    /// </summary>
    private void UpdateDistrictCounter()
    {
        if (districtsNeededText == null) return;
        if (districtCounterText == null) return;
        int numWon = 0;
        int numActive = 0;
        foreach(District d in districts)
        {
            if (d.Active())
            {
                numActive++;
                if (d.WinConditionMet()) numWon++;
            }
        }
        districtCounterText.text = (numWon.ToString()) + "/" + (numActive.ToString());
        districtsNeededText.text = "NEED TO WIN: " + NeededDistricts().ToString(); 
    }

    /// <summary>
    /// Returns the number of districts needed to win this Map.
    /// </summary>
    /// <returns>The int number of districts needed to win this Map.</returns>
    private int NeededDistricts()
    {
        HashSet<District> activeDistricts = new HashSet<District>();
        foreach(District d in districts) { if (d.Active()) activeDistricts.Add(d); }

        for (int i = 1; i <= activeDistricts.Count; i++)
        {
            if ((float)i / (float)activeDistricts.Count > .5f) return i;
        }
        return activeDistricts.Count;
    }

    /// <summary>
    /// Sets up the Swap Counter so that it displays the initial swap limit.
    /// </summary>
    private void SetupSwapCounter()
    {
        if (swapCounterText == null) return;
        SwapManager.ResetSwapCount();
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
        SaveManager.data.SetCurrentFaction(faction);
        SaveManager.data.SetCurrrentLevel(levelNum);
    }

    /// <summary>
    /// Plays any animations for this map.
    /// </summary>
    private void PlayAnimations()
    {
        if (popInOnStart) mapAnimator.SetTrigger("pop");
    }

    /// <summary>
    /// Shows the label at a Square's position.
    /// </summary>
    /// <param name="s">The Square to show the label at.</param>
    public void ShowLabel(Square s)
    {
        if (label == null || labelText == null) return;
        StartCoroutine(ShowLabelDelay(s));
    }

    
    IEnumerator ShowLabelDelay(Square s)
    {
        yield return new WaitForSeconds(1f);
        if (s == null) yield return null;
        else if (s.Hovering())
        {
            label.transform.position = s.transform.position;
            label.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            labelText.text = "Party: " + s.PoliticalParty().ToString() + "\nPopulation: " + s.Pop().ToString();
        }
    }

    /// <summary>
    /// Hides the label. </summary>
    public void HideLabel()
    {
        if (label == null || labelText == null) return;
        label.transform.localScale = Vector3.zero;
    }


    /// <summary>
    /// Updates all positions of Squares.
    /// </summary>
    public void UpdatePositions()
    {
        if (mapStates == null) mapStates = new Stack<(Dictionary<Square, Vector2Int>, Dictionary<Square, Transform>)>();
        Dictionary<Square, Vector2Int> prevPositions = new Dictionary<Square, Vector2Int>();
        Dictionary<Square, Transform> prevParents = new Dictionary<Square, Transform>();
        foreach (Square s in activeSquares)
        {
            prevPositions.Add(s, s.MapPosition());
            prevParents.Add(s, s.ParentDistrict().transform);
            s.UpdateState();
        }

        if(!prevPositions.Equals(prevAddedPositions) && !prevParents.Equals(prevAddedParents))
        {
            mapStates.Push((prevPositions, prevParents));
            prevAddedParents = prevParents;
            prevAddedPositions = prevPositions;
        }
    }

    /// <summary>
    /// Removes the previously added map state.
    /// </summary>
    public void RemoveLastMapState()
    {
        if (mapStates == null) return;
        if (mapStates.Count == 0) return;
        mapStates.Pop();
    }

    /// <summary>
    /// Checks enhancements the player has and activates them.
    /// </summary>
    public void EnablePowerups()
    {
        if (SaveManager.data.moreSwaps) swapLimit = swapLimit + (int) Mathf.Round(swapLimit * .1f);

        if(undoButton != null) undoButton.SetActive(true);

    }

    /// <summary>
    /// Returns true if this map is the last level in the faction.
    /// </summary>
    /// <returns>true if this Map is the last level in the faction, false otherwise.</returns>
    public bool FinalLevel()
    {
        return lastLevel;
    }

    /// <summary>
    /// Tries to lock all squares.
    /// </summary>
    public void TryLockAllSquares()
    {
        foreach(District d in districts)
        {
            d.TryLockSquares();
        }
    }

    /// <summary>
    /// Shakes this Map. 
    /// </summary>
    public void Shake()
    {
        shaking = true;
        mapAnimator.SetTrigger("shake");
    }

    /// <summary>
    /// Stops shaking this map.
    /// </summary>
    public void StopShaking()
    {
        shaking = false;
    }

    /// <summary>
    /// Returns true if this Map is shaking, false otherwise.
    /// </summary>
    /// <returns>true if this Map is shaking, false otherwise.</returns>
    public bool Shaking()
    {
        return shaking;
    }

    /// <summary>
    /// Returns true if this Map is resetting.
    /// </summary>
    /// <returns>true if this Map is resetting.</returns>
    public bool MapResetting()
    {
        return resetting;
    }


}
