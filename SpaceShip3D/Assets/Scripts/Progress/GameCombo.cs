using System;
using System.Collections;
using UnityEngine;

public class GameCombo : MonoBehaviour
{
	public event Action<int> OnComboChanged = delegate { };
	protected int ComboCount { get; set; }

	/// <summary>
	/// Add one point
	/// </summary>
	public void Increase()
	{
		ComboCount += 1;
		OnComboChanged(ComboCount);
	}
	/// <summary>
	/// Add some amount
	/// </summary>
	/// <param name="amount"></param>
	public void Add(int amount)
	{
		ComboCount += amount;
		OnComboChanged(ComboCount);
	}
	/// <summary>
	/// Set in to amount
	/// </summary>
	/// <param name="amount"></param>
	public void Change(int amount)
	{
		ComboCount = amount;
		OnComboChanged(ComboCount);
	}
	/// <summary>
	/// Return value of combo 
	/// </summary>
	/// <returns></returns>
	public int GetComboCount()
	{
		return ComboCount;
	}
}
