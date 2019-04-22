using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public enum GameMode
	{
		Idle,

		Game,
		ChangePlanet,
		Death,
		Finish
	}
	[Tooltip("Base rotation speed")]
	/// <summary>
	/// Base rotatiopn speed
	/// </summary>
	public static float gameBaseSpeed = 70;

	private static GameManager Instance;

	public delegate void ChangeModeEventHandler(GameMode gameMode);
	public static event ChangeModeEventHandler ChangeModeEvent;

	private static GameMode mode = GameMode.Finish;

	public static GameMode Mode
	{
		get
		{
			return mode;
		}

		set
		{
			Instance.StartCoroutine(ChangingMode(value));
		}
	}

	protected static IEnumerator ChangingMode(GameMode _mode)
	{
		yield return new WaitForEndOfFrame();
		if (mode != _mode)
		{
			mode = _mode;
			ChangeModeEvent?.Invoke(mode);
		}
	}

	#region MonoBehaviour

	private void Awake()
	{
		if (Instance == null)
		{ // Экземпляр менеджера был найден
			Instance = this; // Задаем ссылку на экземпляр объекта
		}
		else if (Instance == this)
		{ // Экземпляр объекта уже существует на сцене
			Destroy(gameObject); // Удаляем объект
		}
	}

	private void Start()
	{
		Mode = GameMode.Idle;
	}

	#endregion

	public void GameStart()
	{
        StartCoroutine(_GameStart());
	}

    IEnumerator _GameStart()
    {
        yield return new WaitUntil(() => Mode == GameMode.Idle);
        Mode = GameMode.Game;
    }
}
