using UnityEngine;


public class PlanetController : MonoBehaviour
{
	public static PlanetController Instance;
    public static Transform occupedPlanet;

    protected Transform playerT;

    private int _occupedPlanetNum;
	public static int OccupedPlanetNum
	{
		get 
        {
            return Instance._occupedPlanetNum;
        }

		set
		{
			Instance._occupedPlanetNum = value;
			PlayerPrefs.SetInt("occupedPlanetNum", OccupedPlanetNum);
			occupedPlanet = Instance.planetsT[OccupedPlanetNum];
			InputController.SetPlanet(occupedPlanet);
		}
	}

	public Transform[] planetsT;


	private void Awake()
	{
		if(Instance==null)
			Instance = this;

        playerT = GameObject.FindWithTag("Player").transform;

        if (!PlayerPrefs.HasKey("occupedPlanetNum"))
            PlayerPrefs.SetInt("occupedPlanetNum", 0);

        OccupedPlanetNum = PlayerPrefs.GetInt("occupedPlanetNum");

        UnitStartPosition();
    }

	void UnitStartPosition()
    {
        playerT.position = occupedPlanet.position + Vector3.back * 1.1f;
        Camera.main.transform.position = new Vector3(playerT.position.x, playerT.position.y, -5.0f);
    }
}
