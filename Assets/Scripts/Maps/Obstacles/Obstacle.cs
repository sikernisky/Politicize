using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle: MonoBehaviour
{
    [SerializeField]
    /// <summary>The X position of this Obstacle.</summary>
    private int xPos;

    [SerializeField]
    /// <summary>The Y position of this Obstacle.</summary>
    private int yPos;

    /// <summary>This Obstacle's SpriteRenderer. </summary>
    private SpriteRenderer sRend;

    /// <summary>The Map this Obstacle is in. </summary>
    private Map parentMap;

    [SerializeField]
    ///<summary>This Obstacle's persistent, constant animation, if it has one.</summary>
    private Sprite[] animationTrack;

    /// <summary>The time to wait between each frame of this Obstacle's animation. </summary>
    private static readonly float delayTick = .1f;

    /// <summary>true if this Obstacle should play its default animation. </summary>
    private bool playAnimation = true;

    private static HashSet<Obstacle> obstacles = new HashSet<Obstacle>();

    private void Start()
    {
        SetupComponents();
        SetObstaclePosition();
        StartCoroutine(StartObstacleAnimation());
        ApplyObstacleEffects();
    }

    /// <summary>
    /// Sets up this Obstacle's components and fields.
    /// </summary>
    private void SetupComponents()
    {
        sRend = GetComponent<SpriteRenderer>();
        parentMap = transform.parent.GetComponent<Map>();
        obstacles.Add(this);
    }

    /// <summary>
    /// Sets this Obstacle's position on the map.
    /// </summary>
    private void SetObstaclePosition()
    {
        Vector2 resetPos;
        if (ParentMap().Size() % 2 == 0) resetPos = new Vector2(ParentMap().SquareSize() * (xPos + .5f),
             ParentMap().SquareSize() * (yPos + .5f));
        else resetPos = new Vector2(ParentMap().SquareSize() * xPos,
             ParentMap().SquareSize() * yPos);

        transform.position = resetPos;
        transform.localScale = new Vector2(ParentMap().SquareSize(), ParentMap().SquareSize());

        sRend.sortingOrder = 2;
    }

    /// <summary>
    /// Returns this Obstacle's parent map.
    /// </summary>
    /// <returns>this Obstacle's parent map.</returns>
    protected Map ParentMap()
    {
        return parentMap;
    }

    /// <summary>
    /// Returns this Obstacle's map position.
    /// </summary>
    /// <returns>A vector2int representing this Obstacle's Map Position.</returns>
    public Vector2Int MapPosition()
    {
        return new Vector2Int(xPos, yPos);
    }

    private IEnumerator StartObstacleAnimation()
    {
        if(animationTrack != null && animationTrack.Length > 1)
        {
            while (playAnimation)
            {
                foreach(Sprite s in animationTrack)
                {
                    sRend.sprite = s;
                    yield return new WaitForSeconds(delayTick);
                }
                yield return null;
            }
        }
    }

    /// <summary>
    /// Applies this Obstacle's effect to neighboring Squares.
    /// </summary>
    public virtual void ApplyEffect()
    {
        return;
    }

    public static void ApplyObstacleEffects()
    {
        foreach(Obstacle o in obstacles)
        {
            o.ApplyEffect();
        }
    }


    

}
