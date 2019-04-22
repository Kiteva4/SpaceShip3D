using UnityEngine;
using TMPro;

public class CoinBar : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI coins;
	[SerializeField] private GameCoins gameCoins;

	private void Awake()
	{
		gameCoins.OnCoinsCountChanged += HandlerOnCoinsChanged;
	}

	private void HandlerOnCoinsChanged(int amount)
	{
		coins.text = amount.ToString();
	}
}
