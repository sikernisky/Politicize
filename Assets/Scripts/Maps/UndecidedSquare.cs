using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndecidedSquare : Square
{

    [SerializeField]
    /// <summary>The Death Party bloc to convert to.</summary>
    private GameObject deathPartyBloc;

    [SerializeField]
    /// <summary>The Life Party bloc to convert to.</summary>
    private GameObject lifePartyBloc;

    /// <summary>The instantiated bloc.</summary>
    private GameObject instantiatedCopy;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        CheckToConvert();
    }

    public override void OnReset(bool lerp = true)
    {
        if(instantiatedCopy != null) transform.position = instantiatedCopy.transform.position;
        base.OnReset();
    }



    /// <summary>
    /// Checks if this Square should be converted.
    /// </summary>
    private void CheckToConvert()
    {
        List<Square> neighbors = new List<Square>()
        {
            Neighbor(Direction.Up),
            Neighbor(Direction.TopRight),
            Neighbor(Direction.Right),
            Neighbor(Direction.BotRight),
            Neighbor(Direction.Down),
            Neighbor(Direction.BotLeft),
            Neighbor(Direction.Left),
            Neighbor(Direction.TopLeft),
        };

        bool allDeath = true;
        bool allLife = true;
        foreach(Square s in neighbors)
        {
            if (s == null) return;
            if (s.PoliticalParty() == Party.Death) allLife = false;
            if (s.PoliticalParty() == Party.Life) allDeath = false;
        }
        if (allDeath) base.ConvertParty(true, deathPartyBloc);
        if (allLife) base.ConvertParty(true, lifePartyBloc);
    }


    /// <summary>
    /// Converts this UndecidedSquare into a Life Party or Death Party bloc.
    /// </summary>
    /// <param name="playSound">whether to play a sound or not.</param>
    /// <param name="party">the party to convert to.</param>
    /// <returns></returns>
    public GameObject ConvertParty(Party party, bool playSound = true)
    {
        if (party == Party.Death) return base.ConvertParty(playSound, deathPartyBloc);
        else return base.ConvertParty(playSound, lifePartyBloc);
        
    }


}
