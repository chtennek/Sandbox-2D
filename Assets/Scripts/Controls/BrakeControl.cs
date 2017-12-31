using UnityEngine;
using System.Collections;

public class BrakeControl : MonoBehaviour
{
	public const string brakeButton = "Brake";
	
	public float brakeDrag = 20f;
	
	public AudioClip collisionClip;
	public AudioClip hitStunClip;

	private InputReceiver input;
	private Rigidbody2D rb;
	private AudioSource audio;

	private float defaultDrag;
	
	void Awake()
	{
		input = GetComponent<InputReceiver>();
		rb = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource>();

		defaultDrag = rb.drag;
	}
	
	void Update()
	{
		if (input.player.GetButtonDown(brakeButton)) {
			rb.drag = brakeDrag;
		}
		if (input.player.GetButtonUp(brakeButton)) {
			rb.drag = defaultDrag;
		}
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		InputReceiver controller = collision.gameObject.GetComponent<InputReceiver>();
		if (controller == null) {
			return;
		}
		else {
			if (audio != null) {
				audio.clip = collisionClip;
				audio.Play();
			}
		}
	}
}
