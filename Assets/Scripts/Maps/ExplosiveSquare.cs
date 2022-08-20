using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveSquare : Square {

    /// <summary>The number of Squares this ExplosiveSquare has blown up. </summary>
    private int squaresBlown;

    /// <summary>true if this ExplosiveSquare has exploded.</summary>
    private bool exploded;

    /// <summary>All ExplosiveSquares. </summary>
    private static HashSet<ExplosiveSquare> explosives;

    /// <summary>true if this ExplosiveSquare is unable to explode.</summary>
    private bool defused;


    protected override void Start()
    {
        base.Start();
        CheckExplode();
    }

    private void OnEnable()
    {
        if(ParentDistrict() != null) ParentDistrict().EnableDistrict();
        exploded = false;
    }

    protected override void Update()
    {
        base.Update();
    }

  

    /// <summary>
    /// Explodes this ExplosiveSquare, destroying its parent District and all Squares in that District.
    /// </summary>
    private void Explode()
    {
        if (exploded || defused) return;
        if (PoliticalParty() == Party.Life) return;

        exploded = true;
        FindObjectOfType<AudioManager>().Play("Explode");
        ParentMap().Shake();
        foreach(Square s in ParentDistrict().DistrictSquares())
        {
            squaresBlown++;
            FindObjectOfType<AnimManager>().Play("Explode", s.transform.position);
            ParentMap().BanishSquare(s);
        }
        ParentDistrict().DisableDistrict();
    }

    /// <summary>
    /// Checks all ExplosiveSquares and explodes them if necessary.
    /// </summary>
    public void CheckExplode()
    {
        if (!HasSimpleMajority()) Explode();
    }

    /// <summary>
    /// Defuses this Square.
    /// </summary>
    public void Defuse()
    {
        defused = true;
    }

    public override GameObject ConvertParty(bool playSound = true, GameObject customConversion = null)
    {
        if (exploded) return null;
        Defuse();
        return base.ConvertParty(playSound, customConversion);
    }


}
