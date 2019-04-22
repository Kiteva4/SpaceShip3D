using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
	[SerializeField] private GameObject[] shipSkins;
	//private Color[] shipSkinColors;
	private int currentSkinIndex;
	private GamePlayerSkins gamePlayerSkins;

	private void Awake()
	{
		gamePlayerSkins = FindObjectOfType<GamePlayerSkins>();
		gamePlayerSkins.OnSkinChanged += HandlerSkinChange;
	}

	public void HandlerSkinChange(int index)
	{
		shipSkins[currentSkinIndex].SetActive(false);
		shipSkins[index].SetActive(true);
		currentSkinIndex = index;
	}
}
