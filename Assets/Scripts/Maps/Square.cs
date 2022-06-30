using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Represents the eight directions.
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
    ///<summary>The Sprite for the locked left connector.</summary>
    private Sprite lockedLeftConnector;

    [SerializeField]
    ///<summary>The Sprite for the unlocked left connector.</summary>
    private Sprite unlockedLeftConnector;

    [SerializeField]
    /// <summary>The right connector of this Square.</summary>
    private SpriteRenderer rightConnector;

    [SerializeField]
    ///<summary>The Sprite for the locked right connector.</summary>
    private Sprite lockedRightConnector;

    [SerializeField]
    ///<summary>The Sprite for the locked right connector.</summary>
    private Sprite unlockedRightConnector;

    [SerializeField]
    /// <summary>The top connector of this Square.</summary>
    private SpriteRenderer topConnector;

    [SerializeField]
    ///<summary>The Sprite for the locked top connector.</summary>
    private Sprite lockedTopConnector;

    [SerializeField]
    ///<summary>The Sprite for the locked top connector.</summary>
    private Sprite unlockedTopConnector;

    [SerializeField]
    /// <summary>The bot connector of this Square.</summary>
    private SpriteRenderer botConnector;

    [SerializeField]
    ///<summary>The Sprite for the locked bot connector.</summary>
    private Sprite lockedBotConnector;

    [SerializeField]
    ///<summary>The Sprite for the locked bot connector.</summary>
    private Sprite unlockedBotConnector;

    [SerializeField]
    /// <summary>The top left connector of this Square.</summary>
    private SpriteRenderer topLeftConnector;

    [SerializeField]
    /// <summary>The Sprite for the locked top left connector.</summary>
    private Sprite lockedTopLeftConnector;

    [SerializeField]
    /// <summary>The Sprite for the unlocked top left connector.</summary>
    private Sprite unlockedTopLeftConnector;

    [SerializeField]
    /// <summary>The top right connector of this Square.</summary>
    private SpriteRenderer topRightConnector;

    [SerializeField]
    /// <summary>The Sprite for the locked top right connector.</summary>
    private Sprite lockedTopRightConnector;

    [SerializeField]
    /// <summary>The Sprite for the unlocked top right connector.</summary>
    private Sprite unlockedTopRightConnector;

    [SerializeField]
    /// <summary>The bot left connector of this Square.</summary>
    private SpriteRenderer botLeftConnector;

    [SerializeField]
    /// <summary>The Sprite for the locked bot left connector.</summary>
    private Sprite lockedBotLeftConnector;

    [SerializeField]
    /// <summary>The Sprite for the unlocked bot left connector.</summary>
    private Sprite unlockedBotLeftConnector;

    [SerializeField]
    /// <summary>The bot right connector of this Square.</summary>
    private SpriteRenderer botRightConnector;

    [SerializeField]
    /// <summary>The Sprite for the locked bot right connector.</summary>
    private Sprite lockedBotRightConnector;

    [SerializeField]
    /// <summary>The Sprite for the unlocked bot right connector.</summary>
    private Sprite unlockedBotRightConnector;

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

    [SerializeField]
    ///<summary>Text component displaying this Square's population</summary>
    private TMP_Text populationText;

    [SerializeField]
    ///<summary>Sprite representing this Square when it is locked.</summary>
    private Sprite lockedSprite;

    [SerializeField]
    /// <summary>The Sprite representing this SwappableSquare when it is selected.</summary>
    protected Sprite selectedSprite;

    [SerializeField]
    ///<summary>The Sprite representing this Square when it is unlocked.</summary>
    private Sprite unlockedSprite;

    /// <summary>The Sprite representing this SwappableSquare when it is deselected. </summary>
    private Sprite deselectedSprite;

    /// <summary>This Square's sprite when it first spawned.</summary>
    private Sprite startSprite;

    /// <summary>true if this Square is locked.</summary>
    private bool locked;

    [SerializeField]
    ///<summary>true if this square starts selected.</summary>
    private bool startSelected;


    protected virtual void Start()
    {
        SetupSprites();
        selectedSquares = new HashSet<Square>();
        startX = xPos;
        startY = yPos;
        if(!Selected()) deselectedSprite = CurrentSprite();

        FindParentDistrictAndMap();
        ResetGridPosition();
    }

    protected virtual void Update()
    {
        TrySelectOnStart();
        UpdateDistrict();
        DisplayPopulation();
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
    /// Sets up this Square's SpriteRenderer and Sprites.
    /// </summary>
    private void SetupSprites()
    {
        sRend = GetComponent<SpriteRenderer>();
        startSprite = sRend.sprite;
        if (unlockedSprite == null) unlockedSprite = deselectedSprite;
    }

    /// <summary>
    /// Sets the localPosition of this Square.
    /// 
    /// <br></br>The position is its X and Y positions multiplied by the Square size
    /// determined by its parent map.
    /// </summary>
    private void ResetGridPosition()
    {
        xPos = startX;
        yPos = startY;

        Vector2 resetPos;
        if (parentMap.Size() % 2 == 0) resetPos = new Vector2(ParentMap().SquareSize() * (startX + .5f),
             ParentMap().SquareSize() * (startY + .5f));
        else resetPos = new Vector2(ParentMap().SquareSize() * startX,
             ParentMap().SquareSize() * startY);

        transform.localPosition = resetPos;

        if (sRend == null) Debug.Log(name);
        sRend.sortingOrder = 2;

        transform.SetParent(startParentDistrict.transform);

        UnlockSquare();
    }

    /// <summary>
    /// Does something when this map is reset.
    /// </summary>
    public virtual void OnReset()
    {
        ResetGridPosition();
    }


    private void OnMouseDown()
    {
        DetermineSelection();
    }


    /// <summary>
    /// Performs some action based on what Squares are selected.
    /// </summary>
    private void DetermineSelection()
    {
        if (!LevelManager.playable) return;
        if (selectedSquares.Count == 0) Select();
        else if (Selected()) DeSelect();
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

    /// <summary>
    /// Returns true if this Square is selected.
    /// </summary>
    /// <returns>true if this Square is selected, false otherwise.</returns>
    protected bool Selected()
    {
        return selectedSquares.Contains(this);
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
    }

    /// <summary>
    /// Selects this Square if possible.
    /// </summary>
    protected virtual void Select()
    {
        if (selectedSprite == null) selectedSprite = CurrentSprite();
        selectedSquares.Add(this);
        SetSprite(selectedSprite);
    }

    /// <summary>
    /// Deselects this Square if possible.
    /// </summary>
    protected virtual void DeSelect()
    {
        selectedSquares.Remove(this);
        SetSprite(deselectedSprite);
    }

    /// <summary>
    /// Highlights this Square a DARK color ("a dark highlight")
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
        FindMap();
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
    protected void SwapPositions(Square other)
    {
        Vector3 pos1 = other.transform.position;
        Vector3 pos2 = transform.position;
        other.transform.position = pos2;
        transform.position = pos1;

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
    /// Updates this Square's parent district.
    /// </summary>
    private void UpdateDistrict()
    {
        District parent = transform.parent.GetComponent<District>();
        if (parent != null) parentDistrict = parent;
    }

    /// <summary>
    /// Updates this Square's parent map.
    /// </summary>
    private void FindMap()
    {
        Map map = transform.parent.parent.GetComponent<Map>();
        if (map != null) parentMap = map;
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

        topLeftConnector.enabled = false;
        topRightConnector.enabled = false;
        botLeftConnector.enabled = false;
        botRightConnector.enabled = false;

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

        if(other == Direction.TopLeft || other == Direction.TopRight || 
            other == Direction.BotLeft || other == Direction.BotRight)
        {
            Square top = Neighbor(Direction.Up);
            Square right = Neighbor(Direction.Right);
            Square bot = Neighbor(Direction.Down);
            Square left = Neighbor(Direction.Left);

            if(other == Direction.BotRight)
            {
                if((bot != null && parentDistrict == bot.parentDistrict) || right != null && parentDistrict == right.parentDistrict)
                {
                    connectorRenderer.enabled = false;
                    return;
                }
            }

            if (other == Direction.BotLeft)
            {
                if ((bot != null && parentDistrict == bot.parentDistrict) || left != null && parentDistrict == left.parentDistrict)
                {
                    connectorRenderer.enabled = false;
                    return;
                }
            }

            if (other == Direction.TopRight)
            {
                if (top != null && parentDistrict == top.parentDistrict || right != null && parentDistrict == right.parentDistrict)
                {
                    connectorRenderer.enabled = false;
                    return;
                }
            }

            if (other == Direction.TopLeft)
            {
                if (top != null && parentDistrict == top.parentDistrict || left != null && parentDistrict == left.parentDistrict)
                {
                    connectorRenderer.enabled = false;
                    return;
                }
            }

        }

        if (s != null && parentDistrict == s.parentDistrict) connectorRenderer.enabled = true;
        else connectorRenderer.enabled = false;
           
    }

    /// <summary>
    /// Returns this Square's parent map.
    /// </summary>
    /// <returns>this Square's parent map.</returns>
    protected Map ParentMap()
    {
        return parentMap;
    }

    /// <summary>
    /// Returns this Square's parent district.
    /// </summary>
    /// <returns>this Square's parent district.</returns>
    public District ParentDistrict()
    {
        return parentDistrict;
    }

    /// <summary>
    /// Displays this Square's population.
    /// </summary>
    private void DisplayPopulation()
    {
        if (population == 0) population = 1;
        if (population == 1) populationText.text = "";
        else populationText.text = population.ToString();
    }

    /// <summary>
    /// Locks this Square because it is in a district where the Death Party has a majority.
    /// </summary>
    public virtual void LockSquare(Sprite customLockedSprite = null)
    {
        if (locked) return;

        locked = true;

        if (customLockedSprite != null) SetSprite(customLockedSprite);
        else SetSprite(lockedSprite);
        LockConnectors();

        transform.localPosition = LockedPosition();
    }

    /// <summary>
    /// Lock this Square's connectors.
    /// </summary>
    private void LockConnectors()
    {
        leftConnector.sprite = lockedLeftConnector;
        rightConnector.sprite = lockedRightConnector;
        topConnector.sprite = lockedTopConnector;
        botConnector.sprite = lockedBotConnector;
        topLeftConnector.sprite = lockedTopLeftConnector;
        topRightConnector.sprite = lockedTopRightConnector;
        botLeftConnector.sprite = lockedBotLeftConnector;
        botRightConnector.sprite = lockedBotRightConnector;
    }

    /// <summary>
    /// Unlocks this Square.
    /// </summary>
    public virtual void UnlockSquare(Sprite customUnlockedSprite = null)
    {
        if (!locked) return;

        locked = false;

        if (customUnlockedSprite != null) SetSprite(customUnlockedSprite);
        else if (unlockedSprite == null) SetSprite(deselectedSprite);
        else SetSprite(unlockedSprite);
        UnlockConnectors();

        transform.localPosition = UnlockedPosition();
    }

    /// <summary>
    /// Unlock this Square's connectors.
    /// </summary>
    private void UnlockConnectors()
    {
        leftConnector.sprite = unlockedLeftConnector;
        rightConnector.sprite = unlockedRightConnector;
        topConnector.sprite = unlockedTopConnector;
        botConnector.sprite = unlockedBotConnector;

        topLeftConnector.sprite = unlockedTopLeftConnector;
        topRightConnector.sprite = unlockedTopRightConnector;
        botLeftConnector.sprite = unlockedBotLeftConnector;
        botRightConnector.sprite = unlockedBotRightConnector;
    }

    /// <summary>
    /// Returns true if this Square is locked.
    /// </summary>
    /// <returns>True if this Square is locked, false otherwise.</returns>
    public bool Locked()
    {
        return locked;
    }

    /// <summary>
    /// Returns the position of this Square when locked.
    /// </summary>
    /// <returns>The Vector2 position that represents this Square when locked.</returns>
    public Vector2 LockedPosition()
    {
        float sub = parentMap.SquareSize() * .0625f;

        float newYPos;
        if(parentMap.Size() % 2 == 0) newYPos = (yPos + .5f) * parentMap.SquareSize() - sub;
        else newYPos = yPos * parentMap.SquareSize() - sub;
        return new Vector2(transform.localPosition.x, newYPos);
    }


    /// <summary>
    /// Returns the position of this Square when unlocked.
    /// </summary>
    /// <returns>The Vector2 position that represents this Square when unlocked.</returns>
    public Vector2 UnlockedPosition()
    {
        if(parentMap.Size() % 2 == 0) return new Vector2(transform.localPosition.x, (yPos + .5f) * parentMap.SquareSize());
        return new Vector2(transform.localPosition.x, yPos * parentMap.SquareSize());
    }

    /// <summary>
    /// Tries to select this Square on start.
    /// </summary>
    private void TrySelectOnStart()
    {
        if (startSelected)
        {
            Select();
            startSelected = false;
        }
    }

    /// <summary>
    /// Returns true if this Square is in a district with an absolute majority of its Party type.
    /// </summary>
    /// <returns>true or false depending on if it is in an absolute majority.</returns>
    protected bool HasAbsoluteMajority()
    {
        return parentDistrict.AbsoluteMajority();
    }

    /// <summary>
    /// Returns this Square's SpriteRenderer component.
    /// </summary>
    /// <returns>This Square's SpriteRenderer componenet.</returns>
    protected SpriteRenderer SquareRenderer()
    {
        return sRend;
    }

    /// <summary>
    /// Returns the Sprite this Square started with.
    /// </summary>
    /// <returns>the Sprite this Square started with, or null if it didn't start with one.</returns>
    protected Sprite StarterSprite()
    {
        return startSprite;
    }

    /// <summary>
    /// Returns the Sprite this Square adopts when it is locked.
    /// </summary>
    /// <returns>the Sprite this Square is when locked.</returns>
    protected Sprite LockedSprite()
    {
        return lockedSprite;
    }

    /// <summary>
    /// Sets this Square's unlocked Sprite.
    /// </summary>
    protected void SetLockedSprite(Sprite s)
    {
        if (s == null) return;
        lockedSprite = s;
    }


    /// <summary>
    /// Sets this Square's locked Sprite.
    /// </summary>
    protected void SetUnlockedSprite(Sprite s)
    {
        if (s == null) return;
        unlockedSprite = s;
    }



}
