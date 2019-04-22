using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetProgressBar : MonoBehaviour
{
	[SerializeField] private Slider progressSlider;
	[SerializeField] private GamePlanetProgress planetProgress;
	[SerializeField] private float updateSpeedSecond = 1.2f;
	private bool barIsIncreasing;

	private void Awake()
	{
		planetProgress.OnPlanetProgressChanged += HandlerPlanetProgressChanged;
		progressSlider.value = planetProgress.GetPlanetProgressPct(planetProgress.GetPlanetProgres());
	}

	private void HandlerPlanetProgressChanged(float scr)
	{
		//Debug.Log("StartCoroutine");
		StartCoroutine(ChangePlanetProgressBar(scr));
	}

	private IEnumerator ChangePlanetProgressBar(float scr)
	{
		yield return new WaitUntil(() => !barIsIncreasing);
		//Debug.Log("Joined");
		barIsIncreasing = true;
		float preChangePct = progressSlider.value;
		float needAdd = planetProgress.GetPlanetProgressPct(scr);
		if (needAdd > 1f)
			needAdd = 1f;

		float elapsed = 0;
		while(elapsed < updateSpeedSecond)
		{
			//Debug.Log("onWhile");
			elapsed += Time.deltaTime;
			progressSlider.value = Mathf.Lerp(preChangePct, needAdd, elapsed / updateSpeedSecond);
			if (progressSlider.value >= 1f)
			{
				planetProgress.ClampPlanetProgressValue();
				progressSlider.value = 0f;
				break;
			}
			yield return null;
		}
		barIsIncreasing = false;
		//Debug.Log("Out");

	}

	//private IEnumerator AddPlanetProgress()
	//{

	//    yield return new WaitForEndOfFrame();
	//    int _gameScore = CurrentScore;
	//    NextPlanetProgress += _gameScore;
	//    float _progressPlanetMustAdd = _gameScore / (CurveScaleCoef * planetOpenCurve.Evaluate(OpenPlanetCount + 1));

	//    float added = 0f;
	//    while (added < _progressPlanetMustAdd)
	//    {
	//        float d = Time.deltaTime * _progressPlanetMustAdd * 0.5f;
	//        added += d;
	//        planetProgressSlider.value += d;
	//        yield return new WaitForEndOfFrame();
	//    }

	//    StartCoroutine(_AddPlanetProgress());
	//}

	//private IEnumerator _AddPlanetProgress()
	//{

	//    float a = CurveScaleCoef * planetOpenCurve.Evaluate(OpenPlanetCount + 1);
	//    if (NextPlanetProgress > a)
	//    {
	//        NextPlanetProgress = NextPlanetProgress - a;
	//        planetProgressSlider.value = 0;
	//        OpenPlanetCount++;
	//        float _progressPlanetMustAdd = NextPlanetProgress / (CurveScaleCoef * planetOpenCurve.Evaluate(OpenPlanetCount + 1));

	//        float added = 0f;
	//        while (added < _progressPlanetMustAdd)
	//        {
	//            float d = Time.deltaTime * _progressPlanetMustAdd * 0.5f;
	//            added += d;
	//            planetProgressSlider.value += d;
	//            yield return new WaitForEndOfFrame();
	//        }

	//        yield return new WaitForEndOfFrame();
	//        //Реккурсия
	//        StartCoroutine(_AddPlanetProgress());
	//    }
	//}

}
