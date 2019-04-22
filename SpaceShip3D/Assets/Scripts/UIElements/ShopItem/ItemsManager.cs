using System;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
	public event Action SkinRotator = delegate { };
	public event Action<int> OnChangedSkin = delegate { };


	private void Update()
	{
		SkinRotator();
	}
}
