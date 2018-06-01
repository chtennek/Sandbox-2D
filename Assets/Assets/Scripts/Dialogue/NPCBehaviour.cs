using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;

    public void OnInteractBy(Transform source)
    {
        if (DialogueBehaviour.main == null)
            return;

        DialogueBehaviour.main.LoadDialogue(dialogue);

        if (MenuNavigator.main == null || DialogueBehaviour.main.menu == null)
            return;

        MenuNavigator.main.MenuOpen(DialogueBehaviour.main.menu);
    }
}
