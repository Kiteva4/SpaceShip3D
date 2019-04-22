using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
	[SerializeField] private Slider comboSlider;
	[SerializeField] private GameCombo gameCombo;

	private bool barIsDicreasing;

	private void Awake()
	{
		gameCombo.OnComboChanged += HandlerComboChanged;
	}

	private void OnEnable()
	{
		gameCombo.Change(0);
		barIsDicreasing = false;
		comboSlider.value = 0.0f;

	}

	private void HandlerComboChanged(int c)
	{
		if(c!=1)
		{
			comboSlider.value = 1.0f;

			if (!barIsDicreasing)
			{
				StartCoroutine(ChangeCombo());
			}
		}
	}

	private IEnumerator ChangeCombo()
	{
		barIsDicreasing = true;
		yield return new WaitForEndOfFrame();
		while (comboSlider.value > 0)
		{
			comboSlider.value -= Time.deltaTime * 0.2f;
			yield return null;
		}
		gameCombo.Change(1);
		barIsDicreasing = false;
	}
}
