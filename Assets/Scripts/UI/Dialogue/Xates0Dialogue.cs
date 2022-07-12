using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Xates0Dialogue : DialogueManager
{

    [SerializeField]
    ///<summary>Animator for the shop.</summary>
    private Animator shopAnimator;

    


    public override void Start()
    {
        startQuotes = new string[] {
            "Welcome to Xates, " + SaveManager.data.playerName + ".",
            "Pronounced 'zay - tees'.",
            "It's larger than Arnolica. But you can already see that on this map.",
            "This faction's lawmakers require us to document population while redistricting.",
            "That means that some voter blocs are worth twice or three times as much as much as others.",
            "Also, it looks like Kaitlyn started freezing her own blocs too.",
            "Unfortunate. That's not fixable.",
            "But Arnolica, in the meantime, is molding to our vision.",
            "Now fix these maps quickly - I hate this place."
        };
        base.Start();
        SaveManager.data.undoActive = true;
        SaveManager.data.moreSwaps = false;
    }

}
