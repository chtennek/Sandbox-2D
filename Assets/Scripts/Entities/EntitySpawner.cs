using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EntitySpawner : InputBehaviour
{
    [Header("Input")]
    public string buttonName = "Fire";

    [Header("Spawn")]
    public Transform prefab;
    public float spawnCooldown = 0f;
    public bool rapidFire = true;

    [Header("Transform")]
    public Vector3 offset;
    public bool fixRotation; // Use the default rotation for all entities, instead of using velocity direction

    [Header("Velocity")]
    public Cylindrical3 velocity;
    public Vector3 spread;

    private float nextAllowableSpawnTime = -Mathf.Infinity;

    public Vector3 ApplySpread(Vector3 velocity)
    {
        Vector3 currentSpread = new Vector3(Random.Range(-spread.x, spread.x), Random.Range(-spread.x, spread.x), Random.Range(-spread.x, spread.x));
        return velocity + currentSpread;
    }

    private void Update()
    {
        if (Time.time >= nextAllowableSpawnTime && (input.GetButtonDown(buttonName) || rapidFire && input.GetButton(buttonName)))
        {
            nextAllowableSpawnTime = Time.time + spawnCooldown;
            Spawn(ApplySpread(velocity));
        }
    }

    public virtual void Spawn() { Spawn(velocity); }
    public void Spawn(Vector3 velocity)
    {
        Vector3 position = transform.position + transform.rotation * (Vector3)offset;
        Vector3 rotation = Vector3.forward * ((Polar2)velocity).O;

        Transform entity = Instantiate(prefab);
        entity.position = position;
        if (fixRotation == false)
        {
            entity.Rotate(rotation);
            entity.Rotate(transform.eulerAngles);
        }

        MovementManager mover = entity.GetComponent<MovementManager>();
        if (mover != null)
            mover.Velocity = transform.rotation * velocity;
    }
}
