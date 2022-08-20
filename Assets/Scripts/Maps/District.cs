using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represents a set of Squares in a Map.
/// </summary>
public class District : MonoBehaviour
{
    /// <summary>All Squares in this District. </summary>
    private HashSet<Square> squares;

    [SerializeField]
    ///<summary>The name of this District.</summary>
    private string districtName;

    /// <summary>true if this District is actively counting towards a win condition. </summary>
    private bool active = true;


    private void Awake()
    {
        UpdateSquares();
    }

    private void Start()
    {
        TryLockSquares();
        transform.localPosition = Vector3.zero;
    }


    private void Update()
    {
        HighlightAll();
        if (Input.GetKeyDown(KeyCode.L)) TryLockSquares();
    }

    /// <summary>
    /// Finds all Squares in this District.
    /// </summary>
    /// <returns>A HashSet of all found Squares.</returns>
    private HashSet<Square> FindSquares()
    {
        HashSet<Square> childSquares = new HashSet<Square>();
        foreach (Transform t in transform)
        {
            Square s = t.GetComponent<Square>();
            if (s != null) childSquares.Add(s);
        }
        return childSquares;
    }


    /// <summary>
    /// Updates this District's squares.
    /// </summary>
    public void UpdateSquares()
    {
        squares = FindSquares();
    }


    /// <summary>
    /// Returns true if any part of this district is being hovered over.
    /// </summary>
    /// <returns>true if any part of this district is being hovered over.</returns>
    private bool HoveringDistrict()
    {
        foreach(Square s in squares)
        {
            if (s.Hovering()) return true;
        }
        return false;
    }

    /// <summary>
    /// Highlights all Squares in this District.
    /// </summary>
    /// <returns>None</returns>
    public void HighlightAll()
    {
        if (HoveringDistrict())
        {
            foreach (Square s in squares)
            {
                s.Highlight();
            }
        }
        else UnHighlightAll();

    }

    /// <summary>
    /// Highlights all Squares in this District.
    /// </summary>
    /// <returns>None</returns>
    public void UnHighlightAll()
    {
        foreach (Square s in squares)
        {
            s.UnHighlight();
        }        
    }


    /// <summary>
    /// Returns this District's name.
    /// </summary>
    /// <returns>The name of this District.</returns>
    public string Name()
    {
        if (districtName == default) return name;
        else return districtName;
    }

    /// <summary>
    /// Returns the number of Squares in this District.
    /// </summary>
    /// <returns>The number of Squares in this District.</returns>
    public int Size()
    {
        return squares.Count;
    }

    /// <summary>
    /// Returns true if this District's win condition has been met.
    /// </summary>
    /// <returns>true if this win condition is met, false otherwise.</returns>
    public bool WinConditionMet()
    {
        return PercentageDeathPopulation() > .5f;
    }


    /// <summary>
    /// Returns true if every Square in this District is a Death Party tile.
    /// </summary>
    /// <returns>true if every Square in this District is a Death Party tile, false otherwise.</returns>
    public bool AbsoluteMajority()
    {
        return PercentageDeath() >= 1f;
    }

    /// <summary>
    /// Returns true at least 50% of Squares in this District are Death Party tiles.
    /// </summary>
    /// <returns>true if >= 50% of Squares in this District are Death Party tiles, false otherwise.</returns>
    public bool SimpleMajority()
    {
        return PercentageDeathPopulation() > .5f;
    }

    /// <summary>
    /// Returns the percentage of Squares in this District that represent
    /// the death party.
    /// </summary>
    /// <returns>The percentage of Squares in this District in the Death Party.</returns>
    private float PercentageDeath()
    {
        return ((float) NumDeath() / squares.Count);
    }

    /// <summary>
    /// Returns the percentage of Squares in this District that represent the
    /// Death Party, with population taken into account.
    /// </summary>
    /// <returns>The percentage of population that represents the Death Party.</returns>
    private float PercentageDeathPopulation()
    {
        int totalDeath = 0;
        int totalAll = 0;

        foreach(Square s in squares)
        {
            if(s.PoliticalParty() == Party.Death)
            {
                totalDeath += s.Pop();
            }
            totalAll += s.Pop();
        }
        return (float)totalDeath / (float)totalAll;
    }

    /// <summary>
    /// Returns the number of Squares in this District that represent 
    /// the Death Party.
    /// </summary>
    /// <returns>The number of Squares in this District that represent
    /// the Death Party.</returns>
    public int NumDeath()
    {
        int totalDeath = 0;

        foreach (Square s in squares)
        {
            if (s.PoliticalParty() == Party.Death) totalDeath++;

        }
        return totalDeath;
    }

    public override string ToString()
    {
        string result = name + ":";

        foreach(Square s in squares)
        {
            result += " " + s.name + " " + s.PoliticalParty().ToString() + "\n";
        }

        return result;

    }

    /// <summary>
    /// Locks all Squares in this district if it has a majority, or unlocks them if it doesn't.
    /// </summary>
    public void TryLockSquares()
    {
        squares = FindSquares();

        foreach (Square s in squares)
        {
            if (WinConditionMet()) s.LockSquare();
            else s.UnlockSquare();
        }
    }

    /// <summary>
    /// Unlocks all Squares in this district.
    /// </summary>
    public void UnlockAllSquares()
    {
        foreach (Square s in squares)
        {
            s.UnlockSquare();
        }
    }

    /// <summary>
    /// Returns this District's Squares.
    /// </summary>
    /// <returns>Hashset of this District's Squares.</returns>
    public HashSet<Square> DistrictSquares()
    {
        return squares;
    }

    /// <summary>
    /// Adds a Square to this district.
    /// </summary>
    /// <param name="s">The Square to add.</param>
    public void AddSquare(Square s)
    {
        if (s != null)
        {
            squares.Add(s);
            s.transform.SetParent(transform);
        }
    }

    /// <summary>
    /// Removes a Square to this district.
    /// </summary>
    /// <param name="s">The Square to remove.</param>
    public void RemoveSquare(Square s)
    {
        if (squares.Contains(s)) squares.Remove(s);
    }

    /// <summary>
    /// Removes this District from its Map's consideration when counting for a win.
    /// </summary>
    public void DisableDistrict()
    {
        if (!active) return;
        active = false;
    }

    /// <summary>
    /// Adds this District to its Map's consideration when counting for a win.
    /// </summary>
    public void EnableDistrict()
    {
        if (active) return;
        active = true;
    }

    /// <summary>
    /// Returns true if this District is active in its parent map, false otherwise. 
    /// </summary>
    /// <returns>true if this District is active in its parent map, false otherwise</returns>
    public bool Active()
    {
        return active;
    }



}
