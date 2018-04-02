using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class CameraCollider2D : MonoBehaviour
{
    private Camera cam;
    private EdgeCollider2D coll;

    private void Start()
    {
        cam = GetComponent<Camera>();
        coll = GetComponent<EdgeCollider2D>();

        SetColliderBoundaries();
    }

    private void SetColliderBoundaries()
    {
        Vector2[] points = new Vector2[5];
        points[0] = cam.ViewportToWorldPoint(Vector2.zero);
        points[1] = cam.ViewportToWorldPoint(Vector2.right);
        points[2] = cam.ViewportToWorldPoint(Vector2.one);
        points[3] = cam.ViewportToWorldPoint(Vector2.up);
        points[4] = points[0];
        coll.points = points;
    }
}
