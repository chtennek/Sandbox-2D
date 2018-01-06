using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    Move,
    Fire,
    Suicide
}

[System.Serializable]
public struct Action
{
    public ActionType type;
    public Vector2 target;
    public float waitTime; // After current action is executed
}

[RequireComponent(typeof(ProjectileSpawner))]
public class TopdownAI : MonoBehaviour
{
    public Action[] actions;
    public float movementSpeed = 5f;
    public bool loopActions = true;

    private int currentCommandIndex;
    private float nextActionTime;
    private Vector2 currentDestination;

    private ProjectileSpawner spawner;

    private void Awake()
    {
        spawner = GetComponent<ProjectileSpawner>();
        nextActionTime = Time.time;
        currentDestination = transform.position;
    }

    private void FixedUpdate()
    {
        // Handle action execution
        if (Time.time >= nextActionTime && currentCommandIndex < actions.Length)
        {
            ExecuteAction(actions[currentCommandIndex]);
            nextActionTime = Time.time + actions[currentCommandIndex].waitTime;
            currentCommandIndex++;
            if (loopActions)
            {
                currentCommandIndex %= actions.Length;
            }
        }

        // Handle AI movement
        Vector2 toDestination = currentDestination - (Vector2)transform.position;
        if (toDestination.magnitude <= movementSpeed)
        {
            transform.position = currentDestination;
        }
        else
        {
            transform.position += movementSpeed * (Vector3)toDestination.normalized;
        }
    }

    private void ExecuteAction(Action action)
    {
        switch (action.type)
        {
            case ActionType.Move:
                currentDestination = action.target;
                break;
            case ActionType.Fire:
                spawner.Fire();
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
