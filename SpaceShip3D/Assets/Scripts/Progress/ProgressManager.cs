using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
	GameScores gameScores;
	GameCombo gameCombo;
	GameCoins gameCoins;
	GamePlanetProgress gamePlanetProgress;

	private void Awake()
	{
		gameScores = GetComponent<GameScores>();
		gameCombo = GetComponent<GameCombo>();
		gameCoins = GetComponent<GameCoins>();
		gamePlanetProgress = GetComponent<GamePlanetProgress>();
		GameManager.ChangeModeEvent += ChangeMode;
	}

	private void ChangeMode(GameManager.GameMode gameMode)
	{
		switch (gameMode)
		{
			case GameManager.GameMode.Game:
				gameScores.ChangeCurrentScore(0);
				break;
			case GameManager.GameMode.Death:
				gameCoins.Add(gameScores.GetCurrentScore());
				gamePlanetProgress.AddToNextPlanetProgress(gameScores.GetCurrentScore());
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Increase Combo and score count
	/// </summary>
	public void OnRocketExplosion()
	{
		IncreaseCombo();
		IncreaseScore();
	}

	private void IncreaseCombo()
	{
		StartCoroutine(AddCombo());
	}

	private void IncreaseScore()
	{
		StartCoroutine(AddScore());
	}

	private IEnumerator AddScore()
	{
		yield return new WaitForEndOfFrame();
		gameScores.AddToCurrentScore((PlanetController.OccupedPlanetNum + 1) * gameCombo.GetComboCount());
	}

	private IEnumerator AddCombo()
	{
		yield return new WaitForEndOfFrame();
		gameCombo.Increase();
	}
}
