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

    /// <summary>true if this District is highlighted. </summary>
    private bool highlighted;

    private void Awake()
    {
        UpdateSquares();
    }

    private void Update()
    {
        if (!highlighted) HighlightAll();
        else UnHighlightAll();
    }


    /// <summary>
    /// Finds all Squares in this District.
    /// </summary>
    /// <returns>A HashSet of all found Squares.</returns>
    public void UpdateSquares()
    {
        HashSet<Square> childSquares = new HashSet<Square>();
        foreach(Transform t in transform)
        {
            Square s = t.GetComponent<Square>();
            if (s != null) childSquares.Add(s);
        }
        squares = childSquares;
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
        if (HoveringDistrict() && !highlighted)
        {
            foreach (Square s in squares)
            {
                s.Highlight();
            }
            highlighted = true;
        }

    }

    /// <summary>
    /// Highlights all Squares in this District.
    /// </summary>
    /// <returns>None</returns>
    public void UnHighlightAll()
    {
        if (!HoveringDistrict() && highlighted)
        {
            foreach (Square s in squares)
            {
                s.UnHighlight();
            }
            highlighted = false;
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
        int totalDeath = 0;
        foreach (Square s in squares)
        {
            if (s.PoliticalParty() == Party.Death) totalDeath++;

        }
        return totalDeath;
    }

}
