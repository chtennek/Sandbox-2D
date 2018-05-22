using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueResponder : MenuPopulator
{
    public void PopulateMenu(List<DialogueBranch> branches)
    {
        List<string> selections = new List<string>();
        foreach (DialogueBranch branch in branches)
            selections.Add(branch.selection);
        PopulateMenu(selections);
    }
}
