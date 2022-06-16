using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the four directions: Left, Right, Up, and Down.
/// </summary>
public enum Direction
{
    Left,
    TopLeft,
    BotLeft,
    Right,
    TopRight,
    BotRight,
    Up,
    Down
}

/// <summary>
/// Represents political parties.
/// </summary>
public enum Party
{
    Life,
    Death,
    Neutral
}

/// <summary>
/// Represents a Square.
/// </summary>

public class Square : MonoBehaviour
{
    /// <summary>The District this Square is in.</summary>
    private District parentDistrict;

    /// <summary>The Map this Square is in.</summary>
    private Map parentMap;

    [SerializeField]
    ///<summary>The population of this Square.</summary>
    private int population;
    
    /// <summary>This Square's SpriteRenderer. </summary>
    private SpriteRenderer sRend;

    /// <summary>The Squares selected. </summary>
    protected static HashSet<Square> selectedSquares;

    [SerializeField]
    /// <summary>The X position of this Square.</summary>
    private int xPos;

    [SerializeField]
    /// <summary>The Y position of this Square.</summary>
    private int yPos;

    [SerializeField]
    /// <summary>The political party of this Square.</summary>
    private Party party;

    [SerializeField]
    /// <summary>The left connector of this Square.</summary>
    private SpriteRenderer leftConnector;

    [SerializeField]
    /// <summary>The right connector of this Square.</summary>
    private SpriteRenderer rightConnector;

    [SerializeField]
    /// <summary>The top connector of this Square.</summary>
    private SpriteRenderer topConnector;

    [SerializeField]
    /// <summary>The bot connector of this Square.</summary>
    private SpriteRenderer botConnector;

    [SerializeField]
    /// <summary>The top left connector of this Square.</summary>
    private SpriteRenderer topLeftConnector;

    [SerializeField]
    /// <summary>The top right connector of this Square.</summary>
    private SpriteRenderer topRightConnector;

    [SerializeField]
    /// <summary>The bot left connector of this Square.</summary>
    private SpriteRenderer botLeftConnector;

    [SerializeField]
    /// <summary>The bot right connector of this Square.</summary>
    private SpriteRenderer botRightConnector;

    [SerializeField]
    ///<summary>The SpriteRenderer that controls this Square's majority animation.</summary>
    private SpriteRenderer majorityRenderer;

    [SerializeField]
    ///<summary>All Sprites in this majority animation.</summary>
    private Sprite[] majorityAnimation;

    [SerializeField]
    ///<summary>The Animator for this Square.</summary>
    private Animator squareAnim;

    /// <summary>true if this Square is being darkened by its Animator.</summary>
    private bool darkened;

    /// <summary>true if user is hovering over this Square, false otherise.</summary>
    private bool hovering;

    /// <summary>This Square's starting x position.</summary>
    private int startX;

    /// <summary>This Square's starting y position.</summary>
    private int startY;

    /// <summary>The District this Square starts under.</summary>
    private District startParentDistrict;


    private void Start()
    {

        sRend = GetComponent<SpriteRenderer>();
        selectedSquares = new HashSet<Square>();
        startX = xPos;
        startY = yPos;


        FindParentDistrictAndMap();
        DisplayConnectors();
        ResetGridPosition();
        if (majorityAnimation != null && majorityRenderer != null) StartCoroutine(StartMajorityAnimation());

    }

    private void Update()
    {
        DisplayConnectors();
    }

    /// <summary>
    /// Tries to set this Square's parent Map and District to the Map and District it is a child of.
    /// If it can't, sets to null instead.
    /// </summary>
    private void FindParentDistrictAndMap()
    {
        District d = transform.parent.GetComponent<District>();
        parentDistrict = d;

        Map m = transform.parent.parent.GetComponent<Map>();
        parentMap = m;

        startParentDistrict = parentDistrict;
    }

    /// <summary>
    /// Sets the localPosition of this Square.
    /// 
    /// <br></br>The position is its X and Y positions multiplied by the Square size
    /// determined by its parent map.
    /// </summary>
    public void ResetGridPosition()
    {
        xPos = startX;
        yPos = startY;

        DisplayConnectors();

        transform.localPosition = new Vector2(parentMap.SquareSize() * startX,
            parentMap.SquareSize() * startY);
        sRend.sortingOrder = 2;

        transform.SetParent(startParentDistrict.transform);

    }

    private void OnMouseDown()
    {
        DetermineSelection();
    }

    private IEnumerator StartMajorityAnimation()
    {
        int counter = 0;

        while (true)
        {
            if (parentDistrict.WinConditionMet())
            {
                majorityRenderer.sprite = majorityAnimation[counter];
                yield return new WaitForSeconds(.2f);
                if (counter + 1 == majorityAnimation.Length) counter = 0;
                else counter++;
            }
            else majorityRenderer.sprite = null;
            yield return null;
        }
        
    }

    /// <summary>
    /// Performs some action based on what Squares are selected.
    /// </summary>
    private void DetermineSelection()
    {
        if (!LevelManager.playable) return;
        if (selectedSquares.Count == 0) Select();
        else if (selectedSquares.Contains(this)) DeSelect();
        else
        {
            Square selectedSquare = null;
            foreach (Square s in selectedSquares)
            {
                selectedSquare = s;
            }
            selectedSquare.DeSelect();
            Select();
        }
    }

    private void OnMouseEnter()
    {
        hovering = true;
    }

    private void OnMouseExit()
    {
        hovering = false;
    }

    /// <summary>
    /// Returns true if the user is hovering over this Square.
    /// </summary>
    /// <returns>true if the user is hovering over this Square.</returns>
    public bool Hovering()
    {
        return hovering;
    }

    /// <summary>
    /// Swaps the parentDistrict of this Square and other.
    /// </summary>
    /// <param name="other">A Square.</param>
    public void SwapDistricts(Square other)
    {
        District otherDistrict = other.parentDistrict;
        other.parentDistrict = parentDistrict;
        parentDistrict = otherDistrict;

        transform.SetParent(parentDistrict.transform);
        other.transform.SetParent(other.parentDistrict.transform);

        parentDistrict.UpdateSquares();
        other.parentDistrict.UpdateSquares();
    }

    /// <summary>
    /// Selects this Square if possible.
    /// </summary>
    protected virtual void Select()
    {
        selectedSquares.Add(this);
    }

    /// <summary>
    /// Deselects this Square if possible.
    /// </summary>
    protected virtual void DeSelect()
    {
        selectedSquares.Remove(this);
    }

    /// <summary>
    /// Highlights this Square a bright red.
    /// </summary>
    public void Highlight()
    {
        if (darkened) return;
        squareAnim.SetTrigger("darken");
    }

    /// <summary>
    /// Sets this Square's highlight animation to be darkened.
    /// </summary>
    public void SetDarkened()
    {
        if (darkened) return;
        darkened = true;
    }


    /// <summary>
    /// Sets this Square's highlight animation to be darkened.
    /// </summary>
    public void SetLightened()
    {
        if (!darkened) return;
        darkened = false;
    }


    /// <summary>
    /// Removes all color changes to this Square.
    /// </summary>
    public void UnHighlight()
    {
        if (!darkened) return;
        squareAnim.SetTrigger("lighten");
    }


    /// <summary>
    /// Sets this Square's Sprite to <c>s</c>.
    /// </summary>
    protected void SetSprite(Sprite s)
    {
        sRend.sprite = s;
    }

    /// <summary>
    /// Returns this Square's current Sprite.
    /// </summary>
    /// <returns>The Sprite currently occupying this Square's SpriteRenderer.</returns>
    protected Sprite CurrentSprite()
    {
        return sRend.sprite;
    }

    /// <summary>
    /// Returns the Square to the Direction <c>d</c> of this Square.
    /// </summary>
    /// <param name="d">The direction of which to return the neighbor.</param>
    /// <returns>The neighbor in Direction <c>d</c>, or null if that neighbor
    /// doesn't exist.</returns>
    public Square Neighbor(Direction d)
    {

        switch (d)
        {
            case Direction.Left:
                return parentMap.SquareByPos(new Vector2Int(xPos - 1, yPos));
            case Direction.Right:
                return parentMap.SquareByPos(new Vector2Int(xPos + 1, yPos));
            case Direction.Up:
                return parentMap.SquareByPos(new Vector2Int(xPos, yPos + 1));
            case Direction.Down:
                return parentMap.SquareByPos(new Vector2Int(xPos, yPos - 1));
            case Direction.TopLeft:
                return parentMap.SquareByPos(new Vector2Int(xPos - 1, yPos + 1));
            case Direction.TopRight:
                return parentMap.SquareByPos(new Vector2Int(xPos + 1, yPos + 1));
            case Direction.BotLeft:
                return parentMap.SquareByPos(new Vector2Int(xPos - 1, yPos - 1));
            case Direction.BotRight:
                return parentMap.SquareByPos(new Vector2Int(xPos + 1, yPos - 1));
            default:
                return null;
        }
    }

    /// <summary>
    /// Returns true if a SwappableSquare can swap with this Square.
    /// </summary>
    /// <returns>true if a SwappableSquare can swap with this Square,
    /// false otherwise.</returns>
    public virtual bool CanSwapWith()
    {
        return true;
    }

    /// <summary>
    /// Returns the (x, y) position of this Square.
    /// </summary>
    /// <returns>The Vector2Int of this Square's (x, y) position.</returns>
    public Vector2Int MapPosition()
    {
        return new Vector2Int(xPos, yPos);
    }

    /// <summary>
    /// Swaps the (x, y) positions of this Square and <c>other</c>.
    /// </summary>
    /// <returns>Nothing.</returns>
    protected void SwapPositions(Square other)
    {
        int otherX = other.xPos;
        int otherY = other.yPos;

        other.xPos = xPos;
        other.yPos = yPos;
        xPos = otherX;
        yPos = otherY;
    }

    /// <summary>
    /// Returns the Party of this Square.
    /// </summary>
    /// <returns>The Party enum of this Square.</returns>
    public Party PoliticalParty()
    {
        return party;
    }


    /// <summary>
    /// Displays certain connectors depending on this Square's neighbors.
    /// </summary>
    public void DisplayConnectors()
    {

        Dictionary<Direction, SpriteRenderer> neighbors = new Dictionary<Direction, SpriteRenderer>();

        neighbors.Add(Direction.Left, leftConnector);
        neighbors.Add(Direction.Right, rightConnector);
        neighbors.Add(Direction.Up, topConnector);
        neighbors.Add(Direction.Down, botConnector);

        neighbors.Add(Direction.TopLeft, topLeftConnector);
        neighbors.Add(Direction.TopRight, topRightConnector);
        neighbors.Add(Direction.BotLeft, botLeftConnector);
        neighbors.Add(Direction.BotRight, botRightConnector);

        foreach (var item in neighbors)
        {
            TryEnableConnector(item.Key, item.Value);
        } 
    }

    /// <summary>
    /// Tries to enable <c>connectorRenderer</c> if <c>other</c> exists in its position.
    /// </summary>
    /// <param name="other">The other Square.</param>
    /// <param name="connectorRenderer">The connector to try to enable.</param>
    private void TryEnableConnector(Direction other, SpriteRenderer connectorRenderer)
    {
        Square s = Neighbor(other);
        if (s != null && parentDistrict == s.parentDistrict) connectorRenderer.enabled = true;
        else connectorRenderer.enabled = false;
    }



}
