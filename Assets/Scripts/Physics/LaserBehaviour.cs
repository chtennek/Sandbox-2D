using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class LaserBehaviour : MonoBehaviour
{
    public const float MAX_LENGTH = 10000;

    public bool displayFullLength = true; // In editor
    public float length = 10f; // Don't set to infinity
    public float minSpeed = Mathf.Infinity;

    [SerializeField] private bool m_active = true;

    [Header("Collision")]
    public LayerMask refractMask;
    public LayerMask absorbMask;
    public LayerMask reflectMask = ~0;
    public int maxReflects = 0;

    //public UnityEvent onRefract;
    //public UnityEvent onAbsorb;
    //public UnityEvent onReflect;

    private float currentLength;
    private Vector3[] positions;

    private LineRenderer[] lines;
    private RigidbodyWrapper rb;
    private EdgeCollider2D coll2D;

    private void Awake()
    {
        currentLength = 0;
    }

    private void Start()
    {
        rb = GetComponent<RigidbodyWrapper>();
        lines = GetComponentsInChildren<LineRenderer>();
        coll2D = GetComponent<EdgeCollider2D>();

        RecalculatePositions();
    }

    private void OnValidate()
    {
        length = Mathf.Clamp(length, -MAX_LENGTH, MAX_LENGTH);
        if (displayFullLength)
            currentLength = length;
        Start();
    }

    private void FixedUpdate()
    {
        if (m_active == false)
            return;

        if (rb == null)
            currentLength = Mathf.Min(length, currentLength + minSpeed * Time.deltaTime);
        else
        {
            float lastLength = currentLength;
            float speed = Mathf.Max(minSpeed, rb.Velocity.magnitude);
            currentLength = Mathf.Min(length, currentLength + speed * Time.deltaTime);

            // Backtrack on distance traveled, as our laser extends
            transform.position -= rb.Velocity.normalized * (currentLength - lastLength);
        }

        if (Application.isPlaying == true || transform.hasChanged)
            RecalculatePositions();
    }

    public void Activate()
    {
        m_active = true;
    }

    public void Deactivate()
    {
        m_active = false;
        currentLength = 0;
        RecalculatePositions();
    }

    private void RecalculatePositions()
    {
        float workingLength = currentLength;
        Vector3 currentPosition = transform.position;
        Vector3 currentDirection = transform.rotation * Vector3.right;
        int i = 0;

        if (positions == null || positions.Length != 4 + maxReflects)
            positions = new Vector3[4 + maxReflects];

        // Reflect laser against walls
        if (maxReflects > 0)
        {

            RaycastHit hit;
            for (i = 0; i < maxReflects; i++)
            {
                Vector3 point, normal;
                float distance;
                Transform other;

                // Find next reflection/absorption point
                RaycastHit2D hit2D = Physics2D.Raycast(currentPosition, currentDirection, workingLength, absorbMask | reflectMask);
                if (true || hit2D.collider == null)
                {
                    if (Physics.Raycast(currentPosition, currentDirection, out hit, workingLength, absorbMask | reflectMask) == false)
                    {
                        currentPosition += currentDirection * workingLength;
                        break;
                    }
                    point = hit.point;
                    normal = hit.normal;
                    distance = hit.distance;
                    other = hit.transform;
                }
                else
                {
                    point = hit2D.point;
                    normal = hit2D.normal;
                    distance = hit2D.distance;
                    other = hit2D.transform;
                }

                // Send collision events
                foreach (RaycastHit h in Physics.RaycastAll(currentPosition, currentDirection, distance, refractMask))
                {
                    ProcessCollision(h.transform, h.point, h.normal, h.distance);
                }
                foreach (RaycastHit2D h in Physics2D.RaycastAll(currentPosition, currentDirection, distance, refractMask))
                {
                    ProcessCollision(h.transform, h.point, h.normal, h.distance);
                }
                ProcessCollision(other, point, normal, distance);

                // Update values for next raycast
                currentPosition = point;
                currentDirection = Vector3.Reflect(currentDirection, normal);
                workingLength -= distance;

                if (absorbMask.Contains(other.gameObject.layer))
                    break;

                // Update LineRenderer points
                positions[i + 2] = transform.InverseTransformPoint(currentPosition);
            }
        }
        else
            currentPosition += currentDirection * workingLength;

        // Populate remaining LineRenderer points
        for (int j = i + 2; j < positions.Length; j++)
        {
            positions[j] = transform.InverseTransformPoint(currentPosition);
        }

        SetPositions(positions);
    }

    protected virtual void ProcessCollision(Transform other, Vector3 point, Vector3 normal, float distance)
    {
        return;
    }

    public void SetPositions(Vector3[] positions)
    {
        foreach (LineRenderer line in lines)
        {
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }
        if (coll2D != null)
        {
            coll2D.points = System.Array.ConvertAll<Vector3, Vector2>(positions, x => x);
        }
    }
}
