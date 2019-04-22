using System.Collections;
using UnityEngine;

public class ChangePlanet : MonoBehaviour
{
    #region Rotate around planet variables
    protected Vector3 playerToTarget;
    protected Vector3 planetToPlayer;
    protected Vector3 targetCourse;
    protected float targetAngle;
    #endregion

    protected delegate void StartMoveToSelectedPlanetHandler();
    protected StartMoveToSelectedPlanetHandler StartMoveToSelectedPlanet;

    protected delegate void StartMoveAroundHandler();
    protected StartMoveAroundHandler StartMoveAround;

    /// <summary>
    /// Change planet move speed
    /// </summary>
    protected float speed = 1f;
    /// <summary>
    /// Player transform
    /// </summary>
    protected Transform t;
    //protected TrailController tc;
    static public Transform targetPlanetT;

    private int _targetPlanetNum;
    protected int TargetPlanetNum
    {
        get { return _targetPlanetNum; }
        set
        {
            _targetPlanetNum = value;
            targetPlanetT = PlanetController.Instance.planetsT[TargetPlanetNum];
            //tc.SetSimulationSpace();
            t.up = targetPlanetT.position - t.position;
            speed = SetPlanetChangeSpeed(PlanetController.occupedPlanet.position, targetPlanetT.position);
            StartMoveToSelectedPlanet = _StartMoveToSelectedPlanet;
        }
    }

    #region MonoBehaviour
    private void Awake()
    {
        //tc = GetComponent<TrailController>();
        t = GetComponent<Transform>();
    }

    private void Start()
    {
        targetPlanetT = PlanetController.occupedPlanet;
    }
    private void Update()
    {
        StartMoveToSelectedPlanet?.Invoke();
        StartMoveAround?.Invoke();
    }
    #endregion

    void _StartMoveToSelectedPlanet()
    {
        t.position = Vector3.MoveTowards(t.position, (targetPlanetT.position + (Vector3.forward * 1.1f)), 1.5f * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, (targetPlanetT.position + (Vector3.forward * 1.1f))) < 0.001f)
        {
            StartMoveToSelectedPlanet = null;

            t.rotation = Quaternion.Euler(0.0f, 0.0f, t.rotation.eulerAngles.z);
			PlanetController.OccupedPlanetNum = TargetPlanetNum;

			//tc.SetTrailToPlanet();
            StartMoveAround = _StartMoveAround;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (Application.isPlaying)
    //    {
    //        if (targetPlanetT != null)
    //            Gizmos.DrawSphere(playerToTarget, 0.1f);

    //        Gizmos.color = Color.green;
    //        Gizmos.DrawRay(t.position, t.up);
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawRay(t.position, t.forward);
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawRay(t.position, t.right);
    //        Gizmos.color = Color.white;
    //        Gizmos.DrawRay(t.position, playerToTarget);
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawRay(t.position, targetCourse);
    //    }
    //}

    void _StartMoveAround()
    {
        t.RotateAround(targetPlanetT.position, -t.right, (GameManager.gameBaseSpeed * 1.6f) * Time.deltaTime);

        if (Vector3.Distance(t.position, (targetPlanetT.position - (Vector3.forward * 1.1f))) < 0.02f)
        {
            StartMoveAround = null;
           // PlanetController.OccupedPlanetNum = TargetPlanetNum;
            t.up = t.up;
            GameManager.Mode = GameManager.GameMode.Idle;
        }
    }

    public void OnClickChangePlanet(int selected)
    {
        if (PlanetController.OccupedPlanetNum == selected)
        {
            //Debug.Log("OccupedPlanetNum == selected");
            //Continius play
        }

        else if (PlayerPrefs.GetInt("openPlanetCount") < selected)
        {
            //Debug.Log("Planet is closed");
        }

        else
        {
            StartCoroutine(ChangingPlanetTraget(selected));
        }
    }

    IEnumerator ChangingPlanetTraget( int target)
    {
        GameManager.Mode = GameManager.GameMode.ChangePlanet;
        yield return new WaitForEndOfFrame();
        TargetPlanetNum = target;
    }

    /// <summary>
    /// Set planets the change speed.
    /// </summary>
    /// <returns>The change speed.</returns>
    /// <param name="currentPlanet">Current planet.</param>
    /// <param name="targetPlanet">Target planet.</param>
    /// <param name="time">Время, Moving time between two planets, default 1 second.</param>
    float SetPlanetChangeSpeed(Vector3 currentPlanet, Vector3 targetPlanet, float time = 1f)
    {
        return (Vector3.Distance(currentPlanet, targetPlanet) / time);
    }
}