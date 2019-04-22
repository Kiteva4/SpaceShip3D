using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    /// <summary>
    /// Rocket prefab reference
    /// </summary>
    [SerializeField] protected GameObject rocketPrefab;
    /// <summary>
    /// Use while pooling
    /// </summary>
    [HideInInspector] protected GameObject _rocket;

    //public static Transform planetT;
    [SerializeField] protected float spawnRate;

    [Tooltip("Time before start come rockets, seconds")]
    [SerializeField] protected float timeBeforeRockets = 2.0f;

    [Tooltip("Maximum rockets can be in game in one moment")]
    [SerializeField] protected int maxRocketInGame = 10;
    [HideInInspector] public int rocketInGame;

	private float flyTime;

	public Queue<GameObject> pool;

	public static RocketSpawner Instatnce;

	#region MonoBehaviour
	private void Awake()
    {
		Instatnce = this;
		Random.InitState(System.DateTime.Now.Millisecond);
        GameManager.ChangeModeEvent += ChangeMode;
    }

    private void Start()
    {
        PoolInit();
    }
    #endregion

    protected void ChangeMode(GameManager.GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameManager.GameMode.Game:
                StartCoroutine(RocketSpawning());
                break;
            default:
                break;
        }
    }

    IEnumerator RocketSpawning()
    {
		yield return new WaitForSeconds(timeBeforeRockets);

        while (GameManager.Mode == GameManager.GameMode.Game)
        {			
			if (rocketInGame >= maxRocketInGame)
                yield return new WaitUntil(() => rocketInGame < (maxRocketInGame + (2 * (PlanetController.OccupedPlanetNum + 1))));
			if(GameManager.Mode == GameManager.GameMode.Game)
				_rocket = SpawnFromPool();
            yield return new WaitForSeconds(spawnRate);
        }
    }

	private GameObject SpawnFromPool()
    {
        if (pool.Count == 0)
        {
            GameObject obj = Instantiate(rocketPrefab);
            //obj.SetActive(false);
            pool.Enqueue(obj);
        }

        GameObject objectToSpawn = pool.Dequeue();

		//Set parent of rocket
		objectToSpawn.transform.parent = PlanetController.occupedPlanet;

        objectToSpawn.SetActive(true);

        rocketInGame++;
        return objectToSpawn;
    }

    private void PoolInit()
    {
        pool = new Queue<GameObject>();
    }
}
