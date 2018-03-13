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
    [Header("Multi")]
    public RangeType type;
    public float range = 10f;
    public int count = 3;
    public float overTime = 0f;

    public override void Spawn()
    {
        switch (type)
        {
            case RangeType.Radius:
                SpawnOverRadius();
                break;
            case RangeType.Angle:
                SpawnOverAngle();
                break;
        }
    }

    public void SpawnOverRadius()
    {
        StartCoroutine(Coroutine_SpawnOverRadius(velocity, range, count, overTime));
    }

    public IEnumerator Coroutine_SpawnOverRadius(Cylindrical3 velocity, float range, int count, float overTime)
    {
        float increment = range / count;
        float delay = (count <= 1) ? 0 : overTime / (count - 1);
        for (int i = 0; i < count; i++)
        {
            float radius = velocity.r + i * increment;
            Spawn(new Cylindrical3(radius, velocity.O, velocity.z));
            if (Mathf.Approximately(delay, 0) == false)
                yield return new WaitForSeconds(delay);
        }
        yield return null;
    }

    public void SpawnOverAngle()
    {
        StartCoroutine(Coroutine_SpawnOverAngle(velocity, range, count, overTime));
    }

    public IEnumerator Coroutine_SpawnOverAngle(Cylindrical3 velocity, float range, int count, float overTime)
    {
        float increment = range / count;
        float delay = (count <= 1) ? 0 : overTime / (count - 1);
        for (int i = 0; i < count; i++)
        {
            float angle = velocity.O + i * increment;
            Spawn(new Cylindrical3(velocity.r, angle, velocity.z));
            if (Mathf.Approximately(delay, 0) == false)
                yield return new WaitForSeconds(delay);
        }
        yield return null;
    }
}
