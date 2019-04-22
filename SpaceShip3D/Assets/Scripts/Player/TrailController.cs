using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    [SerializeField] protected ParticleSystem worldPs;
    [SerializeField] protected ParticleSystem planetPs;
    #region Emissions
    protected ParticleSystem.EmissionModule emWorldPs;
    protected ParticleSystem.EmissionModule empPlanetPs;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        GameManager.ChangeModeEvent += ChangeMode;
        emWorldPs = worldPs.emission;
        empPlanetPs = planetPs.emission;
    }
	#endregion

	protected void ChangeMode(GameManager.GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameManager.GameMode.Idle:
				SetTrailToPlanet();
				break;
            case GameManager.GameMode.ChangePlanet:
                SetTrailToWorld();
                break;
            case GameManager.GameMode.Death:
                break;
            default:
                break;
        }
    }

    public void SetTrailToPlanet()
    {
		#region Actions with world particle
		emWorldPs.enabled = false;
		worldPs.Clear();
		#endregion

		#region Actions with planet particle
		empPlanetPs.enabled = true;

        var main = planetPs.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Custom;
        main.customSimulationSpace = PlanetController.occupedPlanet;

        planetPs.Clear();

        if (!planetPs.isPlaying)
            planetPs.Play();
        #endregion
    }

    protected void SetTrailToWorld()
    {
        #region Actions with planet particle
        empPlanetPs.enabled = false;
		planetPs.Clear();
		#endregion

		#region Actions with world particle
		emWorldPs.enabled = true;
        if (!worldPs.isPlaying)
            worldPs.Play();
        #endregion
    }
}
