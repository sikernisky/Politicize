using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArnolicaFinalDialogue : DialogueManager
{
  

    public override void Start()
    {
        startQuotes = new string[] {
            "Our last map in Arnolica.",
            "Correct this one, and we've gained our first national senate seat.",
            "With the seat, we control factional law, funding, and executive power.",
            "The new Arnolica will be an example of our greatness. The rest of the country will admire it.",
            "Finish strong."
        };
        base.Start();
    }

 
}
