using UnityEngine;
using System.Collections;

public class RocketController : MonoBehaviour
{
    public enum RocketStage
    {
        moved,
        destroyed
    }

    public RocketStage rocketStage;

    protected delegate void RocketMoveHandler();
    protected RocketMoveHandler rocketMove;

    protected RocketSpawner rocketSpawner;
    protected RocketTrail rocketTrail;
    protected Transform playerT;
    protected SlowMotionController slowMotionController;
    protected Rigidbody rb;

    [SerializeField] protected GameObject RocketView;
    [SerializeField] protected GameObject RocketTrail;
    [SerializeField] protected float rotationDamping;
    [SerializeField] protected float radiusOfSpawn = 1.1f;
    [SerializeField] protected ParticleSystem boomEffect;
	[SerializeField] protected ProgressManager progressManager;
    protected Transform rocketT;
    private Vector3 targetCourse;

    /// <summary>
    /// Vector from rocket to player
    /// </summary>
    private Vector3 rocketToPlayer;

    /// <summary>
    /// Vector from planet to rocket
    /// </summary>
    private Vector3 planetToRocket;
    private float targetAngle;

    #region MonoBehaviour

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rocketTrail = GetComponent<RocketTrail>();
        slowMotionController = GetComponent<SlowMotionController>();
		progressManager = FindObjectOfType<ProgressManager>();
        GameManager.ChangeModeEvent += ChangeMode;

        rocketT = GetComponent<Transform>();
        playerT = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //Выстраивание рокеты в мировом пространстве
        rocketSpawner = RocketSpawner.Instatnce;
    }

    private void RocketPositionNormalized()
    {
        rocketStage = RocketStage.moved;
        rocketTrail.InitTrailToPlanet();
        rb.detectCollisions = true;
		RocketView.SetActive(true);
        //Перемещение в область генерации
        rocketT.position = PlanetController.occupedPlanet.position + SpawnCoord();
        // Получение вектора направления к игроку (к playerT)
        rocketToPlayer = playerT.position - rocketT.position;
        //Направление forward к игроку
        rocketT.forward = rocketToPlayer;
        // Получение вектора направления к от центра земли (от planetT)
        planetToRocket = rocketT.position - PlanetController.occupedPlanet.position;
        //Выпрямление ракеты по вертикали от земли
        rocketT.up = planetToRocket;
    }

    private void Update()
    {
        rocketMove?.Invoke();
    }

    private void OnEnable()
    {
        RocketPositionNormalized();
		rocketMove = RocketMove;
    }

    private void OnTriggerEnter()
    {
        rb.detectCollisions = false;
        rocketStage = RocketStage.destroyed;
        StartCoroutine(Boom());
    }

    #endregion

    protected void ChangeMode(GameManager.GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameManager.GameMode.Game:
                rocketMove = RocketMove;
                break;
            case GameManager.GameMode.Death:
				slowMotionController.CkeckEvent = null;
				if (rocketStage != RocketStage.destroyed)
                    StartCoroutine(Boom());
                break;
            default:
                break;
        }
    }

    //Explosion effects
    private IEnumerator Boom()
    {
		rocketStage = RocketStage.destroyed;
		if (slowMotionController.CheckDistance())
		{
			CameraController.checkJoinedCounter--;
		}
		progressManager.OnRocketExplosion();
		rocketMove = null;
        RocketView.SetActive(false);
        boomEffect.Play();
        yield return new WaitUntil(() => !boomEffect.isPlaying);
		yield return new WaitForEndOfFrame();
		gameObject.SetActive(false);
        rocketSpawner.pool.Enqueue(gameObject);
        rocketSpawner.rocketInGame--;
    }

    #region Move around planet logic
    //Move around planet
    private void RocketMove()
    {
        //Get rocket to player Vector
        rocketToPlayer = playerT.position - rocketT.position;
        //Get planet to rocket Vector
        planetToRocket = rocketT.position - PlanetController.occupedPlanet.position;

        targetCourse = Vector3.ProjectOnPlane(rocketToPlayer, rocketT.up);
        targetAngle = Vector3.SignedAngle(rocketT.forward, targetCourse, rocketT.up);
        rocketT.RotateAround(rocketT.position, planetToRocket, Mathf.LerpAngle(0f, targetAngle, Time.deltaTime * rotationDamping));
        rocketT.RotateAround(PlanetController.occupedPlanet.position, rocketT.right, (GameManager.gameBaseSpeed * 1.11f) * Time.deltaTime);
    }
    #endregion

    private Vector3 SpawnCoord()
    {
        /*Shpere shape R = (X^2 Y^2 Z^2)^(1/2)*/
        float posX = Random.Range(-0.5f * radiusOfSpawn, 0.5f * radiusOfSpawn);
        float posY = Random.Range(-0.5f * radiusOfSpawn, 0.5f * radiusOfSpawn);
        float posZ = Mathf.Sqrt(radiusOfSpawn * radiusOfSpawn - posX * posX - posY * posY);
        return new Vector3(posX, posY, posZ);
    }
}
