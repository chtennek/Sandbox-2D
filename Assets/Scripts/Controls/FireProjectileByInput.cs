using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputReceiver))]
[RequireComponent(typeof(ProjectileSpawner))]
public class FireProjectileByInput : MonoBehaviour
{
    public float fireCooldown;
    public bool rapidFire; // Continue firing when input is held
    public int projectileLimit = -1; // Set to negative value for no limit

    private float lastFiredTimestamp = -Mathf.Infinity;
    private List<Transform> projectilesFired = new List<Transform>();

    private InputReceiver input;
    private ProjectileSpawner firer;

    private void Awake()
    {
        input = GetComponent<InputReceiver>();
        firer = GetComponent<ProjectileSpawner>();
    }

    private void FixedUpdate()
    {
        if (input.player.GetButtonDown("Fire") || (rapidFire && input.player.GetButton("Fire")))
        {
            if (Time.time - lastFiredTimestamp >= fireCooldown && (projectileLimit < 0 || projectilesFired.Count < projectileLimit))
            {
                lastFiredTimestamp = Time.time;
                firer.Fire();
            }
        }
    }
}
