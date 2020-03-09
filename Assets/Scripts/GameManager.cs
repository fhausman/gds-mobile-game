using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Witch witch;

    public void GameOver()
    {
        witch.SetInactive();

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var point in spawnPoints)
        {
            point.GetComponent<SpawnPoint>().Deactivate();
        }
    }

    public void RestartGame()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var point in spawnPoints)
        {
            point.GetComponent<SpawnPoint>().Activate();
        }

        witch.SetActive();
    }
}
