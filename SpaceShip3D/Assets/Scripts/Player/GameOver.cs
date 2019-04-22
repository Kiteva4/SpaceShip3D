using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Mode = GameManager.GameMode.Death;
    }
}
