using UnityEngine;

public class RocketTrail : MonoBehaviour
{
    [SerializeField] protected ParticleSystem ps;
    protected Transform simulationSpace;

    #region MonoBehaviour
    //private void Awake()
    //{
    //    InitTrailToPlanet();
    //}
    #endregion
    public void InitTrailToPlanet()
    {
		simulationSpace = PlanetController.occupedPlanet;
		ParticleSystem.MainModule main = ps.main;
		ps.Clear();
		ps.Play();
        main.customSimulationSpace = simulationSpace;
	}

	public void ClearTrail()
	{
		ps.Clear();
	}
}

