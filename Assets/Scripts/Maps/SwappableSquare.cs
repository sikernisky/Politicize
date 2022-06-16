using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represents a Square that swaps when selected and some keys
/// are pressed.
/// </summary>
public class SwappableSquare : Square
{
    [SerializeField]
    /// <summary>The Sprite representing this SwappableSquare when it is selected.</summary>
    private Sprite selectedSprite;

    /// <summary>The Sprite representing this SwappableSquare when it is deselected. </summary>
    private Sprite deselectedSprite;

    /// <summary>
    /// Swaps this SwappableSquare.
    /// </summary>
    /// <param name="d">A Direction.</param>
    /// <returns>Nothing.</returns>
    private void Swap(Direction d)
    {

        Square target = Neighbor(d);
        if (target == null || !target.CanSwapWith()) return;


        SwapPositions(target);
        SwapDistricts(target);
        

        Vector3 pos1 = target.transform.localPosition;
        Vector3 pos2 = transform.localPosition;
        target.transform.localPosition = pos2;
        transform.localPosition = pos1;

        DisplayConnectors();
        target.DisplayConnectors();
    }


    /// <summary>
    /// Swaps this Square if possible.
    /// </summary>
    private void TrySwap()
    {
        if (!LevelManager.playable) return;
        if(selectedSquares.Contains(this) && selectedSquares.Count == 1)
        {
            if (Input.GetKeyDown("up")) Swap(Direction.Up);
            if (Input.GetKeyDown("down")) Swap(Direction.Down);
            if (Input.GetKeyDown("left")) Swap(Direction.Left);
            if (Input.GetKeyDown("right")) Swap(Direction.Right);
        }
    }

    protected override void Select()
    {
        base.Select();
        deselectedSprite = CurrentSprite();
        SetSprite(selectedSprite);
    }

    protected override void DeSelect()
    {
        base.DeSelect();
        SetSprite(deselectedSprite);
    }

    private void Update()
    {
        DisplayConnectors();
        TrySwap();
    }

}
