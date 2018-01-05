using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputReceiver))]
public class FireProjectile : MonoBehaviour
{
    public Transform projectilePrefab;

    public Vector2 spawnOffset; // projectile spawn relative to the player's location
    public Vector2 projectileVelocity;
    //public Vector2 velocitySpread; // Randomly modify velocity within spread range
    public float fireCooldown;
    public bool rapidFire; // Continue firing when input is held
    public int projectileLimit = -1; // Set to negative value for no limit

    private float lastFiredTimestamp = -Mathf.Infinity;
    private List<Transform> projectilesFired = new List<Transform>();

    private InputReceiver input;
    private SidescrollerControlManager manager;

    private void Awake()
    {
        input = GetComponent<InputReceiver>();
        manager = GetComponent<SidescrollerControlManager>();
    }

    private void FixedUpdate()
    {
        if (input.player.GetButtonDown("Fire") || (rapidFire && input.player.GetButton("Fire")))
        {
            if (Time.time - lastFiredTimestamp >= fireCooldown && (projectileLimit < 0 || projectilesFired.Count < projectileLimit))
            {
                Vector3 spawnLocation = transform.position + transform.rotation * (Vector3)spawnOffset;
                Transform projectile = Instantiate(projectilePrefab, spawnLocation, Quaternion.identity);
                projectilesFired.Add(projectile);
                lastFiredTimestamp = Time.time;

                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                ProjectileDriver pd = projectile.GetComponent<ProjectileDriver>();
                if (rb != null)
                {
                    rb.velocity = transform.rotation * (Vector3)projectileVelocity;
                }
                if (pd != null)
                {
                    pd.sourceObject = transform;
                }
            }
        }
    }

    public void OnProjectileDeletion(Transform projectile)
    {
        projectilesFired.Remove(projectile);
    }
}
