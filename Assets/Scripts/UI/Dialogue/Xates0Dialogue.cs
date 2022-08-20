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
            "I told you before. It's a desert, and I'm not sure why anyone would live here.",
            "Just like Foliard, Kaitlyn is worried. She will apply new strategies to slow us down.",
            "That means you must keep pushing forward."
        };


        base.Start();
        SaveManager.data.undoActive = true;
        SaveManager.data.moreSwaps = false;
    }

}
