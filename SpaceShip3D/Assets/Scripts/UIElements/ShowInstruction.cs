using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInstruction : MonoBehaviour
{
	private Animation anim;

	private void Awake()
	{
		anim = GetComponent<Animation>();
		StartCoroutine(EndShowing());
	}

	private IEnumerator EndShowing()
	{
		yield return new WaitUntil(() => !anim.isPlaying);
		this.gameObject.SetActive(false);
	}
}
