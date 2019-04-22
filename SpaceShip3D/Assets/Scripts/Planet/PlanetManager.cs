using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
	public delegate void SetPlanetsHandler();
	public static SetPlanetsHandler SetPlanetsEvent;

	public void OnClickPlanetButton()
	{
        StartCoroutine(SettingPlanets()); 
	}

    IEnumerator SettingPlanets()
    {
        yield return null;
        SetPlanetsEvent?.Invoke();
    }
}
