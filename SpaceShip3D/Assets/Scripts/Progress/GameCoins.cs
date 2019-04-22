using System;
using UnityEngine;
using TMPro;

public class GameCoins : MonoBehaviour
{
	public event Action<int> OnCoinsCountChanged = delegate { };	

	private int coins;
	private int Coins
	{
		get { return coins; }
		set
		{
			coins = value;
			PlayerPrefs.SetInt("coins", coins);
		}
	}

	public void Add(int amount)
	{
		Coins += amount;
		OnCoinsCountChanged(Coins);
	}
	public bool Payed(int price)
	{
		if (Coins >= price)
		{
			Coins -= price;
			OnCoinsCountChanged(Coins);
			return true;
		}
		else return false;
	}

	public void ChangeCoins(int amount)
	{
		Coins = amount;
		OnCoinsCountChanged(Coins);
	}
	public int GetCoinsCount()
	{
		return Coins;
	}
	#region MonoBehaviour
	private void Awake()
	{
		PlayerPrefsInit();
	}

	private void Start()
	{
		ChangeCoins(PlayerPrefs.GetInt("coins"));
	}
	#endregion

	private void PlayerPrefsInit()
	{
		if (!PlayerPrefs.HasKey("coins"))
			PlayerPrefs.SetInt("coins", 250);
	}
}
