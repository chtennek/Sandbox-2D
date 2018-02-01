using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScreenDialogueBehaviour : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    public Text actorText;
    public Text lineText;
    public Dialogue dialogue;

    [Header("Properties")]
    public float textSpeed; // characters per second

    private string currentLine;
    private int nextLineIndex;

    private void Start()
    {
        actorText.text = "";
        lineText.text = "";
        nextLineIndex = 0;
        NextLine();
    }

    public void LoadDialogue(Dialogue d)
    {
        dialogue = d;
        nextLineIndex = 0;
        NextLine();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (lineText.text == currentLine)
        {
            NextLine();
        }
        else
        {
            StopCoroutine("DisplayLine");
            lineText.text = currentLine;
        }
    }

    private void NextLine()
    {
        actorText.text = dialogue.script[nextLineIndex].actor.actorName;
        lineText.text = "";
        currentLine = dialogue.script[nextLineIndex].line;
        nextLineIndex++;
        StartCoroutine("DisplayLine");
    }

    private IEnumerator DisplayLine()
    {
        if (textSpeed <= 0)
        {
            lineText.text = currentLine;
            yield return null;
        }
        else
        {
            for (int i = 0; i < currentLine.Length; i++)
            {
                lineText.text += currentLine[i];
                yield return new WaitForSeconds(1 / textSpeed);
            }
        }
    }
}
