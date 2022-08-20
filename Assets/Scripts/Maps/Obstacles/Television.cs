using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Television : Obstacle
{

    [SerializeField]
    /// <summary>The Party this Television represents.</summary>
    private Party party;

    /// <summary>
    /// Applies the Television Obstacle effect:
    /// 
    /// Converts all surrounding squares to the Life Party.
    /// </summary>
    public override void ApplyEffect()
    {
        HashSet<Square> surrounding = ParentMap().SurroundingSquares(MapPosition());
        foreach(Square s in surrounding)
        {
            BrainwashSquare(s);
        }
    }

    /// <summary>
    /// Converts a Square to a party.
    /// </summary>
    /// <param name="s">The Square to brainwash.</param>
    /// <param name="p">The Party to brainwash the square to.</param>
    private void BrainwashSquare(Square s)
    {
        if (s as ChainedSquare != null) Debug.Log(s.PoliticalParty());
        if (s.PoliticalParty() == party) return;

        AnimManager am = FindObjectOfType<AnimManager>();
        int animNum = Random.Range(0, 2);
        if (animNum == 0) am.Play("CrimeRates", transform.position);
        else am.Play("Inflation", transform.position);

        FindObjectOfType<AudioManager>().Play("Television");
        if (s as UndecidedSquare != null) (s as UndecidedSquare).ConvertParty(party, false);
        else s.ConvertParty(false);
    }
}
