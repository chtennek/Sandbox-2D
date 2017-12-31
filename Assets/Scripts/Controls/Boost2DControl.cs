using UnityEngine;
using System.Collections;

using Rewired;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReceiver))]
public class Boost2DControl : MonoBehaviour
{
	public const string boostButton = "Dash";

	public float boostSpeed = 40f;
	public float boostCooldown = 1f;
	public bool isBoostBufferingEnabled = true;
	public float maxBoostAllowedSpeed = 40f;

	public AudioClip boostClip;
	
	private InputReceiver input;
	private Rigidbody2D rb;
	private AudioSource audio;

	private float lastBoostTimestamp;
	private bool isBoostBufferable = true;
	
	void Awake()
	{
		input = GetComponent<InputReceiver>();
		rb = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource>();
	}
	
	void Update()
	{
		if (input.player.GetButtonDown(boostButton)) {
			isBoostBufferable = true;
		}
		
		if (input.player.GetButtonDown(boostButton) || (isBoostBufferingEnabled && isBoostBufferable && input.player.GetButton(boostButton))) {
			if (rb.velocity.magnitude <= maxBoostAllowedSpeed && Time.time > lastBoostTimestamp + boostCooldown) {
				Boost(boostSpeed * input.GetNormalizedMovementVector());
			}
		}
	}
	
	public void Boost(Vector3 velocity)
	{
		lastBoostTimestamp = Time.time;
		isBoostBufferable = false;
		rb.velocity = velocity;

		if (audio != null) {
			audio.clip = boostClip;
			audio.Play();
		}
	}
}
