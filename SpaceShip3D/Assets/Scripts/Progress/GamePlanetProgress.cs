using System;
using UnityEngine;

public class GamePlanetProgress : MonoBehaviour
{
	public event Action<float> OnPlanetProgressPctChanged = delegate { };
	public event Action<float> OnPlanetProgressChanged = delegate { };
	public event Action<float> OnOpenedPlanetCountChanged = delegate { };

	[SerializeField] private AnimationCurve planetProgressCurve;
	public static float CurveScaleCoef = 200f;

	private float nextPlanetProgress;
	protected float NextPlanetProgress
	{
		get { return nextPlanetProgress; }
		set
		{
			nextPlanetProgress = value;
			PlayerPrefs.SetFloat("nextPlanetProgress", nextPlanetProgress);
		}
	}

	private int openPlanetCount;
	protected int OpenPlanetCount
	{
		get { return openPlanetCount; }
		set
		{
			openPlanetCount = value;
			PlayerPrefs.SetInt("openPlanetCount", openPlanetCount);
		}
	}

	public void AddToNextPlanetProgress(float amount)
	{
		NextPlanetProgress += amount;

		OnPlanetProgressPctChanged(GetPlanetProgressPct(NextPlanetProgress));
		OnPlanetProgressChanged(NextPlanetProgress);
	}

	public void ChangeNextPlanetProgress(float amount)
	{
		NextPlanetProgress = amount;

		OnPlanetProgressPctChanged(GetPlanetProgressPct(NextPlanetProgress));
		OnPlanetProgressChanged(NextPlanetProgress);
	}

	public float GetPlanetProgres()
	{
		return NextPlanetProgress;
	}

	public void OpenNewOnePlanet()
	{
		OpenPlanetCount += 1;
		OnOpenedPlanetCountChanged(OpenPlanetCount);
	}
	public void ChangeOpenPlanetCount(int amount)
	{
		OpenPlanetCount = amount;
		OnOpenedPlanetCountChanged(OpenPlanetCount);
	}
	public int GetOpenedPlanetCount()
	{
		return openPlanetCount;
	}

	#region MonoBehaviour
	private void Awake()
	{
		PlayerPrefsInit();
	}
	#endregion

	private void PlayerPrefsInit()
	{
		if (!PlayerPrefs.HasKey("nextPlanetProgress"))
			PlayerPrefs.SetFloat("nextPlanetProgress", 0);
		ChangeNextPlanetProgress(PlayerPrefs.GetFloat("nextPlanetProgress"));

		if (!PlayerPrefs.HasKey("openPlanetCount"))
			PlayerPrefs.SetInt("openPlanetCount", 0);
		ChangeOpenPlanetCount(PlayerPrefs.GetInt("openPlanetCount"));
	}

	public float GetPlanetProgressPct(float p)
	{
		return (p / (CurveScaleCoef * planetProgressCurve.Evaluate(OpenPlanetCount + 1)));
	}

	public void ClampPlanetProgressValue()
	{
		if (GetPlanetProgres() > (CurveScaleCoef * planetProgressCurve.Evaluate(OpenPlanetCount + 1)))
		{
			ChangeNextPlanetProgress(GetPlanetProgres() - (CurveScaleCoef * planetProgressCurve.Evaluate(OpenPlanetCount + 1)));
			OpenNewOnePlanet();
		}
		GetPlanetProgres();
	}
}
