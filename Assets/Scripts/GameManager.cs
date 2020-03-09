using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Witch witch;
    public Stake stake;
    public UIManager ui;

    void Start()
    {
        stake.onGameOver += GameOver;
    }

    public void GameOver()
    {
        stake.Deactivate();
        witch.SetInactive();

        var enemies = Physics2D.OverlapAreaAll(new Vector2(-20.0f, 10.0f), new Vector2(20.0f, -10.0f), LayerMask.GetMask("Enemies"));
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var point in spawnPoints)
        {
            point.GetComponent<SpawnPoint>().Deactivate();
        }

        ui.GameOver();
    }

    public void RestartGame()
    {
        ui.PlayUi();

        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var point in spawnPoints)
        {
            point.GetComponent<SpawnPoint>().Activate();
        }

        witch.SetActive();
        stake.Activate();
        stake.ResetDurability();
    }
}
