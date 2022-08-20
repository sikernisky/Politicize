using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xates7Dialogue : DialogueManager
{
    public override void Start()
    {
        startQuotes = new string[] {
            "Chained Voter Blocs are victims of the Life Party's cruel, for-profit prison system.",
            "They're incarcerated and cannot vote until we free them.",
            "To do that, swap them with a Compass Bloc until they're out of jail.",
            "Then, they'll count towards our party.",
            "As for Chained Life Party blocs, you might want to hide your morality and leave them be.",
            "Remember our goal."
        };
        base.Start();
    }

}
