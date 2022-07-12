using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xates7Dialogue : DialogueManager
{
    public override void Start()
    {
        startQuotes = new string[] {
            "Oh, look. It just got real.",
            "This map contains an undecided voter bloc. They're on the fence.",
            "Or, as I call them, stupid.",
            "If you surround an undecided voter bloc with Death Party blocs, they'll turn green.",
            "That means you should circle them. All eight sides: diagonals too!",
            "But the same goes for Life Party blocs. So be careful.",
            "Get to it. This faction is too hot!"
        };
        base.Start();
    }

}
