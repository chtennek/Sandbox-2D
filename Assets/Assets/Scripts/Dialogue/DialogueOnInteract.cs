using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnInteract : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;

    public void OnInteract(Transform source)
    {
        if (DialogueBehaviour.main == null)
            return;

        DialogueBehaviour.main.LoadDialogue(dialogue);

        if (MenuNavigator.main == null || DialogueBehaviour.main.menu == null)
            return;

        MenuNavigator.main.MenuOpen(DialogueBehaviour.main.menu);
    }
}
