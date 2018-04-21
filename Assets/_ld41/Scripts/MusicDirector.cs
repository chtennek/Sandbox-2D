using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDirector : MonoBehaviour
{
    public NoteChart chart;
    public NoteSpawner spawner;
    public float travelTime = 4f;

    public float inputWindow = 0.05f;
    public InputReceiver input;
    public Trigger beatTrigger;
    public Trigger offbeatTrigger;

    public AudioSource audio;

    private float startTime;
    private float lastInput;

    private int currentNote;
    private Queue<float> beats;

    private float BeatToSeconds { get { return 1 / (chart.bpm / 60); } }
    private float SecondsToBeat { get { return (chart.bpm / 60); } }

    private void Reset()
    {
        audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Play();
    }

    private void Update()
    {
        if (input != null && input.GetAnyButtonDown())
            lastInput = Time.time;

        float currentBeat = GetBeat(Time.time);
        beatTrigger.Active = inputWindow > BeatsFrom(currentBeat, 0) * BeatToSeconds;
        offbeatTrigger.Active = inputWindow > BeatsFrom(currentBeat, 0.5f) * BeatToSeconds;


        if (beats.Count == 0)
            return;
        float nextNoteBeat = beats.Peek() - travelTime * SecondsToBeat;
        if (currentBeat >= nextNoteBeat)
        {
            beats.Dequeue();
            spawner.Spawn();
        }
    }

    private float BeatsFrom(float beat, float other)
    {
        return Mathf.Abs((beat + 0.5f + other) % 1 - 0.5f);
    }

    private float GetBeat(float time)
    {
        return (time - startTime - chart.offset) * SecondsToBeat;
    }

    public void Play()
    {
        beats = new Queue<float>();
        foreach (float beat in chart.beats)
            beats.Enqueue(beat);

        audio.clip = chart.clip;
        audio.Play();
        startTime = Time.time;
        currentNote = 0;
    }

    private IEnumerator Coroutine_PlayAudio(float delay)
    {
        yield return new WaitForSeconds(delay);
        audio.Play();
    }
}
