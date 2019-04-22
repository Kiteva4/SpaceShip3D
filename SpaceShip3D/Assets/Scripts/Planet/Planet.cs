using System.Collections;
using UnityEngine;

public class Planet : MonoBehaviour, IPlanet
{

	protected delegate void PlanetIdleRotateHandler();
	protected PlanetIdleRotateHandler PlanetRotator;

	protected Transform t;

	[SerializeField] protected int ID;
	[SerializeField] protected float idleRotateSpeed = 10f;
	protected bool isOpen = false;
	protected Transform tOpenedPlanet;
	protected Transform tClosedPlanet;

	GameObject OpenedPlanetObj;
	GameObject LockedPlanetObj;

	[SerializeField] private GameObject UI_OpenedPlanetObj;
	[SerializeField] private GameObject UI_LockedPlanetObj;

	#region MonoBehaviour
	private void Awake()
	{
		GameManager.ChangeModeEvent += ChangeMode;
		PlanetManager.SetPlanetsEvent += InitState;
		t = GetComponent<Transform>();
		OpenedPlanetObj = transform.GetChild(0).gameObject;
		LockedPlanetObj = transform.GetChild(1).gameObject;
		tOpenedPlanet = transform.GetChild(0).GetChild(1).GetComponent<Transform>();
		tClosedPlanet = transform.GetChild(1).GetChild(1).GetComponent<Transform>();
	}

	private void Start()
	{
		InitState();
	}

	private void Update()
	{
		PlanetRotator?.Invoke();
	}
	#endregion

	public void InitState()
	{
		if ((!PlayerPrefs.HasKey("occupedPlanetNum") && ID == 0) || PlayerPrefs.HasKey("openPlanetCount") && PlayerPrefs.GetInt("openPlanetCount") >= ID)
		{
			isOpen = true;
			LockedPlanetObj.SetActive(false);
			UI_LockedPlanetObj.SetActive(false);
			OpenedPlanetObj.SetActive(true);
			UI_OpenedPlanetObj.SetActive(true);
		}

		else
		{
			isOpen = false;
			OpenedPlanetObj.SetActive(false);
			UI_OpenedPlanetObj.SetActive(false);
			LockedPlanetObj.SetActive(true);
			UI_LockedPlanetObj.SetActive(true);
		}

		if (PlanetController.OccupedPlanetNum != ID)
			StartPlanetIdleRotation();
		else PlanetRotator = null;
	}

	public void StartPlanetIdleRotation()
	{
		if (isOpen)
			PlanetRotator = _OpenedPlanetIdleRotation;
		else
			PlanetRotator = _ClosedPlanetIdleRotation;
	}

	protected void _OpenedPlanetIdleRotation()
	{
		tOpenedPlanet.Rotate(0.0f, 0.0f, idleRotateSpeed * Time.deltaTime);
	}

	protected void _ClosedPlanetIdleRotation()
	{
		tClosedPlanet.Rotate(0.0f, 0.0f, idleRotateSpeed * Time.deltaTime);
	}

	protected void ChangeMode(GameManager.GameMode gameMode)
	{
		switch (gameMode)
		{
			case GameManager.GameMode.Idle:
				if (PlanetController.OccupedPlanetNum != ID)
					StartPlanetIdleRotation();
				else PlanetRotator = null;
				break;
			case GameManager.GameMode.Game:
				StartCoroutine(SetPlanetOnGame());
				break;
			case GameManager.GameMode.ChangePlanet:
				StartPlanetIdleRotation();
				break;
			default:
				break;
		}
	}

	IEnumerator SetPlanetOnGame()
	{
		yield return null;
		PlanetRotator = null;
		if (PlanetController.OccupedPlanetNum != ID)
		{
			OpenedPlanetObj.SetActive(false);
			UI_OpenedPlanetObj.SetActive(false);
			LockedPlanetObj.SetActive(false);
			UI_LockedPlanetObj.SetActive(false);
		}
	}
}
