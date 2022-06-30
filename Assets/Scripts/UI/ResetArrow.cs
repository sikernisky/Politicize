using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an Arrow to reset the entire map or undo a move.
/// </summary>
public class ResetArrow : MonoBehaviour
{
    [SerializeField]
    ///<summary>True if this Arrow is responsible for resetting the map.</summary>
    private bool isReset;

    [SerializeField]
    ///<summary>This Arrow's map.</summary>
    private Map map;

    [SerializeField]
    ///<summary>True if this Arrow is responsible for undoing one move.</summary>
    private bool isUndo;

    private void OnMouseDown()
    {
        TryReset();
        TryUndo();
    }

    /// <summary>
    /// Tries to reset the map.
    /// </summary>
    private void TryReset()
    {
        if (!isReset) return;
        map.ResetMap();
    }

    /// <summary>
    /// Tries to undo the last move.
    /// </summary>
    private void TryUndo()
    {
        if (!isUndo) return;
        //undo code, add here later.
    }
}
