using UnityEngine;
using System.Collections;

using Rewired;

public class GameStateManager : MonoBehaviour
{
	public float winningScore = 25;
	public Color[] teams;
	private float[] teamScores;
	private int[] teamAssignments;

	void Start()
	{
		teamScores = new float[teams.Length];
		teamAssignments = new int[ReInput.players.playerCount];
		for (int i = 0; i < teamAssignments.Length; i++) {
			teamAssignments[i] = i % teams.Length;
		}
	}

	public void ResetGame()
	{
		teamScores = new float[teams.Length];
	}

	public int GetNextTeamId(int teamId, bool includeInactiveId)
	{
		if (teamId < teams.Length - 1)
			return teamId + 1;
		return includeInactiveId ? -1 : 0;
	}

	public int GetPreviousTeamId(int teamId, bool includeInactiveId)
	{
		if ((teamId > -1 && includeInactiveId) || (teamId > 0 && !includeInactiveId))
			return teamId - 1;
		return teams.Length - 1;
	}

	public void ChangePlayerTeam(int playerId, int teamId)
	{
		if (playerId < 0 || teamId < -1 || playerId >= teamAssignments.Length || teamId >= teams.Length) {
			return;
		}
		teamAssignments[playerId] = teamId;
	}
	
	public int GetPlayerTeamId(int playerId)
	{
		return teamAssignments[playerId];
	}

	public Color GetPlayerColor(int playerId)
	{
		if (teamAssignments[playerId] == -1) {
			return Color.black;
		}
		return teams[teamAssignments[playerId]];
	}

	public void AddScoreForTeam(int teamId, float score)
	{
		if (teamId < 0 || teamId >= teamScores.Length)
			return;

		teamScores[teamId] += score;
	}

	public void AddScoreForPlayer(int playerId, float score)
	{
		AddScoreForTeam(GetPlayerTeamId(playerId), score);
	}

	public float GetScoreForTeam(int teamId)
	{
		return teamScores[teamId];
	}

	public float GetScoreForPlayer(int playerId)
	{
		return teamScores[GetPlayerTeamId(playerId)];
	}
}
