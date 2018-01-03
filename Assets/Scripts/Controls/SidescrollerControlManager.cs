using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SidescrollerControlManager : MonoBehaviour
{
    public float defaultGravityScale = 2f;
    [SerializeField]

    public float playerHeight = 0.22f;
    public float collisionCheckWidth = 0.05f;
    public LayerMask groundMask;

    public bool IsGrounded() { return IsGrounded(Vector2.down, groundMask); }
    public bool IsGrounded(Vector2 direction) { return IsGrounded(direction, groundMask); }
    public bool IsGrounded(Vector2 direction, LayerMask colliderMask)
    {
        bool isGrounded = Physics2D.Raycast(transform.position, direction, 0.5f * playerHeight * transform.lossyScale.y + collisionCheckWidth, colliderMask);
        return isGrounded;
    }
}
