using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RangeType
{
    Radius,
    Angle
}

public class EntityMultiSpawner : EntitySpawner
{
    [Header("Count")]
    public RangeType outerIteration;
    public int iterations = 1;
    public float iterationDelay = 0f;

    [Header("Radius")]
    public int radiusCount = 1;
    public float radiusRange = 0f;
    public float radiusTime = 0f;
    public float RadiusDelay { get { return (radiusCount <= 1) ? 0 : radiusTime / (radiusCount - 1); } }
    public float RadiusIncrement { get { return radiusRange / (radiusCount - 1); } }

    [Header("Angle")]
    public int angleCount = 1;
    public float angleRange = 0f;
    public float angleTime = 0f;
    public bool includeRangeEnd = true; // Set to false for easy 360-degree spawns
    public bool mirrorRange = false; // Include negative range to easily specify by center
    public float AngleInitial { get { return mirrorRange ? velocity.O - angleRange / 2 : velocity.O; } }
    public float AngleDelay { get { return (angleCount <= 1) ? 0 : angleTime / (angleCount - 1); } }
    public float AngleIncrement
    {
        get
        {
            float increment = includeRangeEnd ? angleRange / (angleCount - 1) : angleRange / angleCount;
            return mirrorRange ? increment * 2 : increment;
        }
    }

    public override void Spawn()
    {
        StartCoroutine(Coroutine_Spawn());
    }

    private IEnumerator Coroutine_Spawn()
    {
        for (int n = 0; n < iterations; n++)
        {
            switch (outerIteration)
            {
                case RangeType.Radius:
                    for (int i = 0; i < radiusCount; i++)
                    {
                        float radius = velocity.r + i * RadiusIncrement;

                        for (int j = 0; j < angleCount; j++)
                        {
                            float angle = AngleInitial + j * AngleIncrement;

                            Spawn(new Cylindrical3(radius, angle, velocity.z));

                            if (AngleDelay > 0)
                                yield return new WaitForSeconds(AngleDelay);
                        }

                        if (RadiusDelay > 0)
                            yield return new WaitForSeconds(RadiusDelay);
                    }
                    break;
                case RangeType.Angle:
                    for (int i = 0; i < angleCount; i++)
                    {
                        float angle = AngleInitial + i * AngleIncrement;

                        for (int j = 0; j < radiusCount; j++)
                        {
                            float radius = velocity.r + j * RadiusIncrement;

                            Spawn(new Cylindrical3(radius, angle, velocity.z));

                            if (Mathf.Approximately(RadiusDelay, 0) == false)
                                yield return new WaitForSeconds(RadiusDelay);
                        }

                        if (Mathf.Approximately(AngleDelay, 0) == false)
                            yield return new WaitForSeconds(AngleDelay);
                    }
                    break;
            }

            if (iterationDelay > 0)
                yield return new WaitForSeconds(iterationDelay);
        }
    }

    public void SpawnOverRadius()
    {
        StartCoroutine(Coroutine_SpawnOverRadius());
    }

    private IEnumerator Coroutine_SpawnOverRadius()
    {
        for (int i = 0; i < radiusCount; i++)
        {
            float radius = velocity.r + i * RadiusIncrement;
            Spawn(new Cylindrical3(radius, velocity.O, velocity.z));
            if (Mathf.Approximately(RadiusDelay, 0) == false)
                yield return new WaitForSeconds(RadiusDelay);
        }
    }

    public void SpawnOverAngle()
    {
        StartCoroutine(Coroutine_SpawnOverAngle());
    }

    private IEnumerator Coroutine_SpawnOverAngle()
    {
        for (int i = 0; i < angleCount; i++)
        {
            float angle = AngleInitial + i * AngleIncrement;
            Spawn(new Cylindrical3(velocity.r, angle, velocity.z));
            if (Mathf.Approximately(AngleDelay, 0) == false)
                yield return new WaitForSeconds(AngleDelay);
        }
    }
}
