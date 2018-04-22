using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDirector : MonoBehaviour
{
    public NoteChart chart;
    public NoteSpawner spawner;
    public NoteSpawner spawnerBlue;
    public NoteSpawner spawnerYellow;

    public NoteSpawner spawnerMove;
    public NoteSpawner spawnerShot;

    public float travelTime = 4f;

    public float inputWindow = 0.2f;
    public InputReceiver input;
    public Trigger beatTrigger;
    public Trigger offbeatTrigger;

    public AudioSource audio;

    private float startTime;
    private float lastInput;

    private float nextBeat;
    private float nextMove;
    private float nextShot;
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
        //beatTrigger.Active = inputWindow > BeatsFrom(currentBeat, 0) * BeatToSeconds;
        //offbeatTrigger.Active = inputWindow > BeatsFrom(currentBeat, 0.5f) * BeatToSeconds;

        // Move
        float nextNoteBeat = nextMove - (inputWindow + travelTime) * SecondsToBeat;
        if (currentBeat >= nextNoteBeat)
        {
            spawnerMove.Spawn();
            nextMove++;
        }

        // Shot
        nextNoteBeat = nextShot - (inputWindow + travelTime) * SecondsToBeat;
        if (currentBeat >= nextNoteBeat)
        {
            spawnerShot.Spawn();
            nextShot++;
        }

        // Constant notes
        nextNoteBeat = nextBeat - travelTime * SecondsToBeat;
        if (currentBeat >= nextNoteBeat)
        {
            Spawn();
            nextBeat += chart.beatIncrement;
        }

        // Custom notes
        if (beats.Count == 0)
            return;
        nextNoteBeat = beats.Peek() - travelTime * SecondsToBeat;
        if (currentBeat >= nextNoteBeat)
        {
            Spawn();
            beats.Dequeue();
        }
    }

    private void Spawn()
    {
        if (nextBeat % 1 == 0)
            spawner.Spawn();
        else if (nextBeat % 1 == 0.5f)
            spawnerBlue.Spawn();
        else
            spawnerYellow.Spawn();
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
        nextBeat = chart.startBeat;
        nextMove = chart.startMove;
        nextShot = chart.startMove + 0.5f;
    }

    private IEnumerator Coroutine_PlayAudio(float delay)
    {
        yield return new WaitForSeconds(delay);
        audio.Play();
    }
}
