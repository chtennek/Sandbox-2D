using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReceiver))]
public class Walk2DControl : MonoBehaviour
{
	public float walkSpeed = 0f;
	public float maxWalkableSpeed = 15f;
	
	private InputReceiver input;
	private Rigidbody2D rb;

	void Awake()
	{
		input = GetComponent<InputReceiver>();
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (walkSpeed > 0 && rb.velocity.magnitude <= maxWalkableSpeed) {
			rb.velocity = walkSpeed * input.GetNormalizedMovementVector();
		}
	}
}
