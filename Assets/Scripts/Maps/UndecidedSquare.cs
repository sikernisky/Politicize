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

    public override void OnReset()
    {
        if(instantiatedCopy != null) transform.position = instantiatedCopy.transform.position;
        base.OnReset();
    }

    /// <summary>
    /// Converts this Square to a Life Party or Death Party bloc.
    /// </summary>
    /// <param name="newBloc">Which bloc to convert to.</param>
    private void Convert(GameObject newBloc)
    {
        GameObject instantiated = Instantiate(newBloc);
        ParentMap().AddSquare(instantiated.GetComponent<Square>(), transform.parent.GetComponent<District>());
        instantiated.transform.position = transform.position;
        instantiated.GetComponent<Square>().SetMapPosition(MapPosition().x, MapPosition().y);
        instantiated.GetComponent<Square>().SetPopulation(Pop());
        instantiated.transform.localScale = transform.localScale;
        instantiated.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        instantiated.GetComponent<Animator>().SetTrigger("fadeIn");
        instantiatedCopy = instantiated;
        ParentMap().BanishSquare(this);
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
        if (allDeath) Convert(deathPartyBloc);
        if (allLife) Convert(lifePartyBloc);
    }


}
