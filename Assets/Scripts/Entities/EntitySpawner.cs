using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EntitySpawner : InputBehaviour
{
    [Header("Input")]
    public string buttonName = "Fire";

    [Header("Parameters")]
    public Transform prefab;
    public float spawnCooldown = 0f;
    public bool rapidFire = true;
    public Vector3 offset;

    public float angleWhenUpright; // Set angle at which the projectile should be upright
    public bool fixRotation; // Use the same rotation for all sub-projectiles, instead of factoring in velocity direction

    #region extend
    public Vector3[] velocities = new Vector3[1];
    public Cylindrical3[] polars = new Cylindrical3[1];
    public Vector3 spread;

    private float nextAllowableSpawnTime = -Mathf.Infinity;

    public Vector3 ApplySpread(Vector3 velocity)
    {
        Vector3 currentSpread = new Vector3(Random.Range(-spread.x, spread.x), Random.Range(-spread.x, spread.x), Random.Range(-spread.x, spread.x));
        return velocity + currentSpread;
    }
    #endregion

    private void Update()
    {
        if (Time.time >= nextAllowableSpawnTime && (input.GetButtonDown(buttonName) || rapidFire && input.GetButton(buttonName)))
        {
            nextAllowableSpawnTime = Time.time + spawnCooldown;
            SpawnAll();
        }
    }

    public void SpawnAll()
    {
        foreach (Vector3 velocity in velocities)
            Spawn(ApplySpread(velocity));
        foreach (Vector3 velocity in polars)
            Spawn(ApplySpread(velocity));
    }

    public void Spawn() { Spawn(Vector3.zero); }
    public void Spawn(Vector3 velocity)
    {
        Vector3 position = transform.position + transform.rotation * (Vector3)offset;
        float rotationFromProjectileVelocity = fixRotation ? 0 : velocity.y;
        Quaternion rotation = Quaternion.Euler(0, 0, rotationFromProjectileVelocity - angleWhenUpright);

        Transform entity = Instantiate(prefab, position, transform.rotation);

        MovementManager mover = entity.GetComponent<MovementManager>();
        if (mover != null)
            mover.Velocity = velocity;
    }
}
