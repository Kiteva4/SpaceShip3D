using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Range(0, int.MaxValue)]
	public static int checkJoinedCounter;

	[Range(0.1f, 0.5f)]
	[SerializeField] protected float timeScalerMultipler;
	[SerializeField] protected float minZoomCamSize = 1.8f;
	[SerializeField] protected float maxZoomCamSize = 8.33f;
	[SerializeField] protected float normalZoomCamSize = 1.8f;
	[SerializeField] protected float midlePlanetPos_x = 2.36f;

	#region Camera scroll clamp y pos
	protected float cemeraMaxPos_Y = 28.0f;
	protected float cemeraMinPos_Y = 0.0f;
	#endregion
	protected Camera mCamera;
	protected Transform t;
	protected delegate void CameraSetterHandler();
	protected static CameraSetterHandler CameraMove;

	/// <summary>
	/// Open and close planet menu camera zooming speed
	/// </summary>
	private float ZoomNormilizedSpeed => 0.22f * (maxZoomCamSize - normalZoomCamSize);

	protected bool mouseIsHold;
	protected float startInputPos_Y;
	protected float currInputPos_Y;
	protected float verticalScrollingDelta;

	#region MonoBehavoiur

	private void Awake()
	{
		mCamera = GetComponent<Camera>();
		t = GetComponent<Transform>();
		InitCameraZoomerCheckers();
	}

	private void Update()
	{
		CameraMove?.Invoke();
		if (checkJoinedCounter > 0)
		{
			if (Time.timeScale != timeScalerMultipler)
				Time.timeScale = timeScalerMultipler;
		}

		else
		{
			if (Time.timeScale != 1.0f)
				Time.timeScale = 1.0f;
		}
	}

	#endregion

	private void InitCameraZoomerCheckers()
	{
		if (checkJoinedCounter == 0)
		{
			CameraMove = CameraViewIncrease;
		}
		else
		{
			CameraMove = CameraViewDecrease;
		}
	}

	void CameraViewIncrease()
	{
		if ((mCamera != null) && (checkJoinedCounter == 0 && mCamera.orthographicSize < normalZoomCamSize))
		{
			mCamera.orthographicSize = Mathf.Lerp(mCamera.orthographicSize, normalZoomCamSize + 0.1f, 4 * Time.unscaledDeltaTime);
		}
		else
		{
			CameraMove = CameraViewDecrease;
		}
	}

	void CameraViewDecrease()
	{
		if ((mCamera != null) && (checkJoinedCounter != 0 && mCamera.orthographicSize > minZoomCamSize))
		{
			mCamera.orthographicSize = Mathf.Lerp(mCamera.orthographicSize, minZoomCamSize - 0.1f, 4 * Time.unscaledDeltaTime);
		}
		else
		{
			CameraMove = CameraViewIncrease;
		}
	}

	public void OnClickPlanetsButton()
	{
		CameraMove = _CameraOpenPlanetsMenuMoving;
	}

	public void OnClickClosePlanetsMenu()
	{
		CameraMove = _CameraClosePlanetsMenuMoving;
	}

	protected void _CameraOpenPlanetsMenuMoving()
	{
		t.position = new Vector3(Mathf.Lerp(t.position.x, midlePlanetPos_x, 8f * Time.deltaTime), t.position.y, t.position.z);
        if (mCamera != null)
        {
            mCamera.orthographicSize = Mathf.Lerp(mCamera.orthographicSize, maxZoomCamSize + 0.5f, 1.4f * ZoomNormilizedSpeed * Time.deltaTime);
        }
        if (mCamera.orthographicSize >= maxZoomCamSize)
		{
			StartCoroutine(ScrollPlanets());
			CameraMove = _CameraVerticalScrolling;
		}
	}

	protected void _CameraClosePlanetsMenuMoving()
	{
		t.position = new Vector3(
								Mathf.Lerp(t.position.x, ChangePlanet.targetPlanetT.position.x, 8f * Time.deltaTime),
								Mathf.Lerp(t.position.y, ChangePlanet.targetPlanetT.position.y, 8f * Time.deltaTime),
								t.position.z);
        if (mCamera != null)
        {
            mCamera.orthographicSize = Mathf.Lerp(mCamera.orthographicSize, normalZoomCamSize - 0.5f, 1.4f * ZoomNormilizedSpeed * Time.deltaTime);
        }
        if (mCamera.orthographicSize <= normalZoomCamSize)
		{
			//StopCoroutine(ScrollPlanets());
			StopAllCoroutines();
			InitCameraZoomerCheckers();
		}
	}

	protected void _CameraVerticalScrolling()
	{
		verticalScrollingDelta = 0.99f * verticalScrollingDelta;
		t.Translate(0.0f, -0.4f * verticalScrollingDelta * Time.deltaTime, 0.0f);
		if (t.position.y > cemeraMaxPos_Y)
			t.position = new Vector3(t.position.x, cemeraMaxPos_Y, t.position.z);
		else if (t.position.y < cemeraMinPos_Y)
			t.position = new Vector3(t.position.x, cemeraMinPos_Y, t.position.z);
	}

//Scroll input
    protected IEnumerator ScrollPlanets()
	{
		yield return null;
		while (true)
		{
			yield return null;
#if UNITY_EDITOR
			#region mouse controll
			if (Input.GetMouseButtonDown(0))
			{
				startInputPos_Y = Input.mousePosition.y;
				mouseIsHold = true;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				mouseIsHold = false;
			}

			if (mouseIsHold)
			{
				currInputPos_Y = Input.mousePosition.y;
				verticalScrollingDelta = currInputPos_Y - startInputPos_Y;
				startInputPos_Y = currInputPos_Y;
			}

			#endregion
#else
			#region touch controll
            if (Input.touchCount == 0)
            {
                //deltaTouchMove = Vector2.zero;
                continue;
            }

            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    startInputPos_Y = Input.GetTouch(0).position.y;
					verticalScrollingDelta = 0.0f;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    currInputPos_Y = Input.GetTouch(0).position.y;
					verticalScrollingDelta = currInputPos_Y - startInputPos_Y;
					startInputPos_Y = currInputPos_Y;
                }
            }
			#endregion
#endif
		}

	}
}
