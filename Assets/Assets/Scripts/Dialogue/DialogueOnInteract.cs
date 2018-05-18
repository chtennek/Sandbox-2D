using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnInteract : InteractableBehaviour
{
    public Dialogue dialogue;

    public override void OnInteract(Transform source)
    {
        if (DialogueBehaviour.main != null)
            DialogueBehaviour.main.LoadDialogue(dialogue);
    }
}
