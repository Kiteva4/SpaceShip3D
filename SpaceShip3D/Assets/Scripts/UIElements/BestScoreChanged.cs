using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestScoreChanged : MonoBehaviour
{
	[SerializeField] private GameObject newBestScore;
	[SerializeField] private GameScores gameScores;

	private void Awake()
	{
		GameManager.ChangeModeEvent += ChangeMode;
	}

	private void ChangeMode(GameManager.GameMode gameMode)
	{
		switch (gameMode)
		{
			case GameManager.GameMode.Game:
				gameScores.OnBestScoreChanged += HandlerChangeBestScore;
				break;
			default:
				break;
		}
	}

	private void HandlerChangeBestScore(int c)
	{
		if(gameObject.activeInHierarchy)
			StartCoroutine(BestScoreHasChanged(c));
	}

	private IEnumerator BestScoreHasChanged(int c)
	{
		gameScores.OnBestScoreChanged -= HandlerChangeBestScore;
		yield return new WaitForSecondsRealtime(1f);
		if(GameManager.Mode == GameManager.GameMode.Game)
			StartCoroutine(EnableAndDisableGoAfter(newBestScore, 1f));
	}

	private IEnumerator EnableAndDisableGoAfter(GameObject go, float t = 0f)
	{
		go.SetActive(true);
		yield return new WaitForSecondsRealtime(t);
		go.SetActive(false);
	}
}
