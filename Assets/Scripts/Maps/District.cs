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

/*    /// <summary>true if this District is highlighted. </summary>
    private bool highlighted;*/


    private void Awake()
    {
        UpdateSquares();
    }


    private void Update()
    {
        TryLockSquares();
        HighlightAll();
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
        return PercentageDeath() > .5f;
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
    /// Returns the percentage of Squares in this District that represent
    /// the death party.
    /// </summary>
    /// <returns>The percentage of Squares in this District in the Death Party.</returns>
    private float PercentageDeath()
    {
        return ((float) NumDeath() / squares.Count);
    }

    /// <summary>
    /// Returns the number of Squares in this District that represent 
    /// the Death Party.
    /// </summary>
    /// <returns>The number of Squares in this District that represent
    /// the Death Party.</returns>
    public int NumDeath()
    {
        squares = FindSquares();
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
    /// Locks all Squares in this district if it has a majority.
    /// </summary>
    private void TryLockSquares()
    {
        foreach(Square s in squares)
        {
            if (WinConditionMet()) s.LockSquare();
            else s.UnlockSquare();
        }
    }

    

}
