using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionController : MonoBehaviour
{
	protected Transform target;
	[Range(0.1f, 1.5f)]
	[SerializeField] protected float distanceToEnableSlowMotioon;

	protected RocketController rc;

	public delegate void CkeckHandler();
	public CkeckHandler CkeckEvent;

	#region MonoBehaviour
	private void Awake()
	{
		rc = GetComponent<RocketController>();
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}

	private void OnEnable()
	{
		InitRangeCheckers();
	}

	private void InitRangeCheckers()
	{
		if (CheckDistance())
		{
			CkeckEvent = ControllOutRange;
		}
		else
		{
			CkeckEvent = ControllJoinRange;
		}
	}

	private void Update()
	{
		if(rc.rocketStage == RocketController.RocketStage.moved)
		{
			CkeckEvent?.Invoke();
		}
	}
	#endregion
	void ControllJoinRange()
	{
		if (CheckDistance())
		{
			CameraController.checkJoinedCounter++;

			CkeckEvent = ControllOutRange;
		}
	}

	public void ControllOutRange()
	{
		if (!CheckDistance())
		{
            CameraController.checkJoinedCounter--;

			CkeckEvent = ControllJoinRange;
		}
	}
	public bool CheckDistance()
	{
		return (Vector3.Distance(transform.position, target.position) < distanceToEnableSlowMotioon);
	}
}
