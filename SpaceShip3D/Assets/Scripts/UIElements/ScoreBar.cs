using UnityEngine;
using TMPro;

public class ScoreBar : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI currentScore;
	[SerializeField] private TextMeshProUGUI bestScore;
	[SerializeField] private GameScores gameScores;

	private void Awake()
	{
		gameScores.OnCurrentScoreChanged += HandlerCurScrChanged;
		gameScores.OnBestScoreChanged += HandlerBestScrChanged;
	}
	private void Start()
	{
		bestScore.text = gameScores.GetBestScore().ToString();
	}

	private void HandlerCurScrChanged(int amount)
	{
		currentScore.text = amount.ToString();
	}

	private void HandlerBestScrChanged(int amount)
	{
		bestScore.text = amount.ToString();
	}
}
