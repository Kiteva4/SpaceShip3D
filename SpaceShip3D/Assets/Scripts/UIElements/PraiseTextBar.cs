using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PraiseTextBar : MonoBehaviour
{
	[SerializeField] private GameCombo gameCombo;
	[SerializeField] private GameObject[] PraiseTexts;

	private void Awake()
	{
		gameCombo.OnComboChanged += HandlerChangeCombo;
	}

	private void HandlerChangeCombo(int c)
	{
		if (c > 5 && gameObject.activeInHierarchy)
			StartCoroutine(ShowPraiseMessages(c));
	}

	private IEnumerator ShowPraiseMessages(int c)
	{
		yield return new WaitForEndOfFrame();
		if (GameManager.Mode == GameManager.GameMode.Game)
		{
			switch (c)
			{
				case 6:
					StartCoroutine(EnableAndDisableGoAfter(PraiseTexts[0]));
					break;
				case 10:
					StartCoroutine(EnableAndDisableGoAfter(PraiseTexts[1]));
					break;
				case 25:
					StartCoroutine(EnableAndDisableGoAfter(PraiseTexts[2]));
					break;
				case 40:
					StartCoroutine(EnableAndDisableGoAfter(PraiseTexts[3]));
					break;
				case 55:
					StartCoroutine(EnableAndDisableGoAfter(PraiseTexts[4]));
					break;
				default:
					break;
			}
		}
	}

	private IEnumerator EnableAndDisableGoAfter(GameObject go, float t = 1.5f)
	{
		go.SetActive(true);
		yield return new WaitForSecondsRealtime(t);
		go.SetActive(false);
	}
}
