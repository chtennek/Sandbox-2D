using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDriver : MonoBehaviour
{
    public float lifetime = Mathf.Infinity;
    public Transform sourceObject;
    public int playerId = -1;

    private float spawnTimestamp;

    private void Awake()
    {
        spawnTimestamp = Time.time;
    }

    private void FixedUpdate()
    {
        if (Time.time - spawnTimestamp > lifetime)
        {
            TearDown();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == sourceObject) // Don't trigger on ourselves
        {
            return;
        }
        TearDown();
    }

    private void TearDown()
    {
        sourceObject.SendMessage("OnProjectileDeletion", transform);
        Destroy(gameObject);
    }
}
