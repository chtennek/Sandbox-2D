using UnityEngine;
using System.Collections;

public class LightBasedAimingIndicator : MonoBehaviour
{
	private InputReceiver input;
	private Light light;

	void Awake()
	{
		light = GetComponent<Light>();
	}

	void Start()
	{
		input = transform.root.GetComponent<InputReceiver>();
		light.cullingMask = 1 << transform.parent.gameObject.layer;
	}

	void Update()
	{
		// [TODO] indicate hitstun
		transform.position = transform.parent.position + (input.GetMovementVector() + Vector3.back).normalized;
	}

	public void OnHitStun()
	{
		light.enabled = false;
	}

	public void OnHitStunEnd()
	{
		light.enabled = true;
	}
}
