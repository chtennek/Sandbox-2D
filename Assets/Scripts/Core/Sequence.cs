using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using DG.Tweening;

[System.Serializable]
public class SequenceEvent
{
    public bool uninterruptable;
    public float time;
    public UnityEvent action;
    public UnityEvent exit;
    public UnityEvent undo;
}

public class Sequence : MonoBehaviour
{
    public bool restartIfRunning = false;
    public bool finishOnStop = false;
    public SequenceEvent global;
    public SequenceEvent[] sequence;

    private IEnumerator current;
    private int index;

    public void Run()
    {
        if (restartIfRunning == false && current != null)
            return;
        Stop();
        current = RunSequence();
        StartCoroutine(current);
    }

    public void Stop()
    {
        if (current == null || (index < sequence.Length && sequence[index].uninterruptable == true))
            return;

        StopCoroutine(current);
        current = null;

        if (index < sequence.Length)
            sequence[index].exit.Invoke();
        global.exit.Invoke();

        // We probably don't need this functionality
        for (int i = Mathf.Min(index, sequence.Length - 1); i >= 0; i--)
            sequence[i].undo.Invoke();
        global.undo.Invoke();

        if (finishOnStop)
            for (int i = index + 1; i < sequence.Length; i++)
                sequence[i].action.Invoke();
    }

    private IEnumerator RunSequence()
    {
        global.action.Invoke();
        for (index = 0; index < sequence.Length; index++)
        {
            SequenceEvent e = sequence[index];
            e.action.Invoke();
            yield return new WaitForSeconds(e.time);
        }
        global.exit.Invoke();
        current = null;
    }
}
