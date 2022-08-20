using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Script to manage all SwappableSquares and swap events.
/// </summary>
public class SwapManager : MonoBehaviour
{
    /// <summary>Number of swaps made by all SwappableSquares.</summary>
    private static int totalSwapsMade;

    /// <summary>Number of arrow key presses that have swapped SwappableSquares.</summary>
    private static int currentSwapCount;

    /// <summary>All SwappableSquares.</summary>
    private static List<SwappableSquare> allSwappables = new List<SwappableSquare>();

    /// <summary>All ExplosiveSquares. </summary>
    private static HashSet<ExplosiveSquare> allExplosives = new HashSet<ExplosiveSquare>();

    /// <summary>All FrozenSquares. </summary>
    private static HashSet<FrozenSquare> allFrozen = new HashSet<FrozenSquare>();

    /// <summary>Queued direction to swap. </summary>
    private static Direction queuedDirection = Direction.Null;

    /// <summary>The instance of this SwapManager.</summary>
    private static SwapManager instance;

    /// <summary>true if the Coroutine for the queued Swap is running.</summary>
    private bool queueCoroRunning;

    /// <summary>The current Map. </summary>
    private Map map;


    private void Start()
    {
        instance = this;
        CollectAllSwappables();
        CollectAllExplosives();
        CollectAllFrozen();
        instance.map = FindObjectOfType<Map>();
    }

    private void Update()
    {
        UpdateTotalSwaps();
        TrySwapSquares();
        //CheckToReset();
    }

    /// <summary>
    /// Selects all SwappableSquares.
    /// </summary>
    private static void SelectAllSwappables()
    {
        foreach(SwappableSquare ss in allSwappables)
        {
            ss.Select();
        }
    }

    /// <summary>
    /// Tries to swap SwappableSquares on this Map.
    /// </summary>
    private void TrySwapSquares()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) SwapAll(Direction.Down);
        if (Input.GetKeyDown(KeyCode.UpArrow)) SwapAll(Direction.Up);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) SwapAll(Direction.Left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) SwapAll(Direction.Right);

    }

    /// <summary>
    /// Collects all SwappableSquares.
    /// </summary>
    /// <param name="select">Whether to select these SwappableSquares too.</param>
    private static void CollectAllSwappables(bool select = true)
    {
        allSwappables = new List<SwappableSquare>(FindObjectsOfType<SwappableSquare>());
        if(select) SelectAllSwappables();
    }


    /// <summary>
    /// Collects all ExplosiveSquares. 
    /// </summary>
    private static void CollectAllExplosives()
    {
        allExplosives = new HashSet<ExplosiveSquare>(FindObjectsOfType<ExplosiveSquare>());
    }

    /// <summary>
    /// Collects all ExplosiveSquares. 
    /// </summary>
    private static void CollectAllFrozen()
    {
        allFrozen = new HashSet<FrozenSquare>(FindObjectsOfType<FrozenSquare>());
    }



    /// <summary>
    /// Attemps to Swap all SwappableSquares in direction d.
    /// </summary>
    /// <param name="d">The direction to swap.</param>
    public static void SwapAll(Direction d)
    {

        if (!LevelManager.playable) return;
        bool successfulSwap = false;

        if (instance.map.MapResetting()) return;
        if (instance.map.Shaking()) return;

        if (instance.AnyLerping())
        {
            queuedDirection = d;
            instance.StartCoroutine(instance.SwapQueueDelay(d));
            return;
        }
        else
        {
            CollectAllSwappables();
            CollectAllExplosives();
            CollectAllFrozen();
            //instance.StartCoroutine(instance.AdjustMapForSwap());
            List<SwappableSquare> filteredSquares = SquaresToSwap(allSwappables, d);
            if (filteredSquares.Count > 0) instance.map.UpdatePositions();
            instance.StartCoroutine(instance.AfterSwap());
            foreach (SwappableSquare ss in filteredSquares)
            {
                if (ss.Swap(d)) successfulSwap = true;              
            }
            if (successfulSwap)
            {
                currentSwapCount++;
            }
            else instance.map.RemoveLastMapState();
        }
    }

    /// <summary>
    /// Returns true if a SwappableSquare can swap in a certain direction.
    /// </summary>
    /// <returns></returns>
    private static List<SwappableSquare> SquaresToSwap(List<SwappableSquare> squares, Direction d)
    {
        List<SwappableSquare> toSwap = new List<SwappableSquare>();

        foreach(SwappableSquare ss in squares)
        {
            if(d == Direction.Left)
            {
                if (ss.Neighbor(Direction.Right) as SwappableSquare == null ||
                    (ss.Neighbor(Direction.Right) as SwappableSquare).IndividualSwapsLeft() == 0) toSwap.Add(ss);
            }
            else if (d == Direction.Right)
            {
                if (ss.Neighbor(Direction.Left) as SwappableSquare == null ||
                    (ss.Neighbor(Direction.Left) as SwappableSquare).IndividualSwapsLeft() == 0) toSwap.Add(ss);
            }
            else if (d == Direction.Down)
            {
                if (ss.Neighbor(Direction.Up) as SwappableSquare == null ||
                    (ss.Neighbor(Direction.Up) as SwappableSquare).IndividualSwapsLeft() == 0) toSwap.Add(ss);
            }
            else if (d == Direction.Up)
            {
                if (ss.Neighbor(Direction.Down) as SwappableSquare == null ||
                    (ss.Neighbor(Direction.Down) as SwappableSquare).IndividualSwapsLeft() == 0) toSwap.Add(ss);
            }

        }
        return toSwap;
       
    }

    /// <summary>
    /// Returns true if any SwappableSquare is lerping.
    /// </summary>
    /// <returns>true if any SwappableSquare is lerping, false otherwise. </returns>
    private bool AnyLerping()
    {
        CollectAllSwappables();
        foreach (SwappableSquare ss in allSwappables)
        {
            if (ss.Lerping()) return true;
        }
        return false;
    }

    private IEnumerator SwapQueueDelay(Direction d)
    {
        if (queuedDirection != Direction.Null && !queueCoroRunning)
        {
            queueCoroRunning = true;
            while (AnyLerping())
            {
                yield return null;
            }
            SwapAll(d);
            queuedDirection = Direction.Null;
            queueCoroRunning = false;
        }
        yield return null;
    }

    IEnumerator AfterSwap()
    {
        yield return new WaitForSeconds(SwappableSquare.swapLerpDuration);
        Obstacle.ApplyObstacleEffects();
        foreach(ExplosiveSquare es in allExplosives)
        {
            es.CheckExplode();
        }
        foreach(FrozenSquare fs in allFrozen)
        {
            fs.CheckToFreeze();
        }
        Map m = FindObjectOfType<Map>();
        if (m != null)
        {
            m.TryLockAllSquares();
            while (m.Shaking())
            {
                yield return null;
            }
            Debug.Log(3);
            m.TryResetMap();
        }

    }


    /// <summary>
    /// Updates the total number of swaps.
    /// </summary>
    private static void UpdateTotalSwaps()
    {
        SwappableSquare[] obs = FindObjectsOfType<SwappableSquare>();
        int counter = 0;
        foreach (SwappableSquare ss in obs)
        {
            counter += ss.NumSwaps();
        }
        totalSwapsMade = counter;
    }

    /// <summary>
    /// Returns the number of swaps made by all SwappableSquares across all Maps.
    /// </summary>
    /// <returns>the number of swaps made by all SwappableSquares across all Maps.</returns>
    public static int TotalSwapsPerformed()
    {
        return totalSwapsMade;
    }

    /// <summary>
    /// Returns the number of ArrowKey presses that have swapped a Square.
    /// </summary>
    /// <returns>the number of ArrowKey presses that have swapped a Square.</returns>
    public static int CurrentSwaps()
    {
        return currentSwapCount;
    }

    /// <summary>
    /// Changes the SwappableSquare swap count to zero.
    /// 
    /// Does NOT affect individual Squares' swap count.
    /// </summary>
    public static void ResetSwapCount()
    {
        currentSwapCount = 0;
    }

    /// <summary>
    /// 'Adds' to the swap count.
    /// </summary>
    /// <param name="amount">The number of swaps to add.</param>
    public static void AddToSwapCount(int amount)
    {
        currentSwapCount -= amount;
    }

    /// <summary>
    /// Returns the total number of SwappableSquares.
    /// </summary>
    /// <returns>the integer total number of SwappableSquares.</returns>
    public static int NumSwappables()
    {
        return allSwappables.Count;
    }

    /// <summary>
    /// Returns true if no SwappableSquares can swap, and resets the map.
    /// </summary>
    /// <returns>true if this reset the map, false otherwise.</returns>
    private bool CheckToReset()
    {

        Map m = FindObjectOfType<Map>();
        if (m.Won()) return false;
        if (allSwappables.Count == 0) return false;
        foreach(SwappableSquare ss in allSwappables)
        {
            if (ss.IndividualSwapsLeft() != 0) return false;
        }
        m.ResetMap();
        return true;
    }

    
}
