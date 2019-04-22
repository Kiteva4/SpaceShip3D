using System;
using UnityEngine;

public class GameScores : MonoBehaviour
{
	public event Action<int> OnCurrentScoreChanged = delegate { };
	public event Action <int> OnBestScoreChanged = delegate { };

	protected int CurrentScore { get; set; }

	private int bestScore;
	private int BestScore
	{
		get { return bestScore; }
		set
		{
			bestScore = value;
			PlayerPrefs.SetInt("bestScore", bestScore);
		}
	}

	#region Current score methods
	public void ChangeCurrentScore(int amount)
	{
		CurrentScore = amount;
		OnCurrentScoreChanged(CurrentScore);

		if (CurrentScore > BestScore)
			ChangeBestScore(CurrentScore);
	}

	public void AddToCurrentScore(int amount)
	{
		CurrentScore += amount;
		OnCurrentScoreChanged(CurrentScore);

		if (CurrentScore > BestScore)
			ChangeBestScore(CurrentScore);
	}

	public int GetCurrentScore()
	{
		return CurrentScore;
	}
	#endregion

	#region Best score methods
	public void ChangeBestScore(int amount)
	{
		BestScore = amount;
		OnBestScoreChanged(BestScore);
	}

	public void AddToBestScore(int amount)
	{
		BestScore += amount;
		OnBestScoreChanged(BestScore);
	}

	public int GetBestScore()
	{
		return BestScore;
	}
	#endregion

	#region MonoBehaviour
	private void Awake()
	{
		PlayerPrefsInit();
	}

	private void Start()
	{
		ChangeBestScore(PlayerPrefs.GetInt("bestScore"));
	}
	#endregion

	private void PlayerPrefsInit()
	{
		if (!PlayerPrefs.HasKey("bestScore"))
			PlayerPrefs.SetInt("bestScore", 0);
	}
}
