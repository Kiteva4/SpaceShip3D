using System;
using UnityEngine;

public class GamePlayerSkins : MonoBehaviour
{
	public event Action<int> OnSkinChanged = delegate { };

	private int selectedSkinIndex;
	private int SelectedSkinIndex
	{
		get { return selectedSkinIndex; }
		set
		{
			selectedSkinIndex = value;
			PlayerPrefs.SetInt("selectedSkinIndex", selectedSkinIndex);
		}
	}

	private void Awake()
	{
		if (!PlayerPrefs.HasKey("selectedSkinIndex"))
			PlayerPrefs.SetInt("selectedSkinIndex", 0);
	}

	private void Start()
	{
		ChangeSelectedPlayerSkin(PlayerPrefs.GetInt("selectedSkinIndex"));
	}

	public void ChangeSelectedPlayerSkin(int index)
	{
		SelectedSkinIndex = index;
		OnSkinChanged(SelectedSkinIndex);
	}

	public int GetSelectedSkinIndex()
	{
		return SelectedSkinIndex;
	}
}
