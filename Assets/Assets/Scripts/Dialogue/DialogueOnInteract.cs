using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnInteract : InteractableBehaviour {
    public Dialogue dialogue;

    public override void OnInteract(Transform source) {
        // [TODO] Switch to event system so we can maybe use different DialogueBehaviours
        DialogueBehaviour.current.LoadDialogue(dialogue);
    }
}
