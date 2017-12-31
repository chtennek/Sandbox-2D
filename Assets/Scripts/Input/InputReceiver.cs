using UnityEngine;
using System.Collections;

using Rewired;

public class InputReceiver : MonoBehaviour
{
	public int playerId = 0;
	public Player player;
	
	void Awake()
	{
		player = ReInput.players.GetPlayer(playerId);
	}

	public Vector3 GetNormalizedMovementVector()
	{
		return GetMovementVector().normalized;
	}

	public Vector3 GetMovementVector()
	{
		float horizontal = player.GetAxisRaw("Move Horizontal");
		float vertical = player.GetAxisRaw("Move Vertical");
		return new Vector3(horizontal, vertical);
	}
}
