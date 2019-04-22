using System.Collections;
using UnityEngine;
using TMPro;

public class ComboText : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI comboText;
	[SerializeField] private Animation an;
	[SerializeField] private GameCombo gameCombo;

	private void Awake()
	{
		gameCombo.OnComboChanged += HandlerOnComboChanged;
	}

	private void HandlerOnComboChanged(int c)
	{
		if (gameObject.activeInHierarchy)
			StartCoroutine(ComboTextChange(c));
	}

	private IEnumerator ComboTextChange(int c)
	{
		yield return new WaitForEndOfFrame();
		comboText.text = 'x' + c.ToString();
		if (c > 5)
			an.Play();
		else if (an.isPlaying)
			an.Stop();
	}
}
