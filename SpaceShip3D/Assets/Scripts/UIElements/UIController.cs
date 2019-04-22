using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{

    #region UIElements
    [SerializeField] protected GameObject PlayMenu;
    [SerializeField] protected GameObject MainMenu;
    [SerializeField] protected GameObject PlanetsMenu;
	[SerializeField] protected GameObject Coins;
    [SerializeField] protected GameObject Player;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        GameManager.ChangeModeEvent += ChangeMode;
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    #endregion


    protected void ChangeMode(GameManager.GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameManager.GameMode.Idle:
                StartCoroutine(IdleMenu());
                break;
            case GameManager.GameMode.Game:
                StartCoroutine(GameMenu());
                break;
            case GameManager.GameMode.ChangePlanet:
                break;
            case GameManager.GameMode.Death:
				Player.SetActive(false);
                StartCoroutine(RestartGame());
                break;
            default:
                break;
        }
    }

    private IEnumerator IdleMenu()
    {
        yield return null;
        PlayMenu.SetActive(false);
        PlanetsMenu.SetActive(false);
		Coins.SetActive(true);
		MainMenu.SetActive(true);
    }

    private IEnumerator GameMenu()
    {
        yield return null;
        MainMenu.SetActive(false);
        PlanetsMenu.SetActive(false);
		Coins.SetActive(false);
		PlayMenu.SetActive(true);
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        PlayMenu.SetActive(false);
        MainMenu.SetActive(true);
		Coins.SetActive(true);
		Player.SetActive(true);
        GameManager.Mode = GameManager.GameMode.Idle;
    }
}
