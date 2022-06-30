using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arnolica8Dialogue : DialogueManager
{
    // Start is called before the first frame update

    /// <summary>true if this level is ending.</summary>
    private bool ending;

    protected override void Update()
    {
        base.Update();
        if (map.Won() && !ending)
        {
            ending = true;
            map.EndLevel();
        }
    }

    public override void Start()
    {
        startQuotes = new string[] {
            SaveManager.data.playerName + ", stop for a minute. Bad news.",
            "Kaitlyn just championed some disgusting legislation in the Senate. Predictably.",
            "The law went into effect immediately. You can already see her wrongdoings on this map.",
            "She FROZE voter blocs.",
            "But I, as always, have worked out a solution.",
            "To defrost our Death Party blocs, secure an absolute majority in its district.",
            "But until then, they are immovable, unbreakable, unchangeable.",
            "Your job just got harder - you know who to thank!"
        };
        base.Start();
    }
}
