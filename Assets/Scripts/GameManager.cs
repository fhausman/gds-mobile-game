using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool acceptsPlayerInput = true;

    public Witch witch;
    public Stake stake;
    public Spellbook spellbook;
    public UIManager ui;
    public Tutorial tut;
    public bool tutorialEnabled = true;

    private int highScore = 0;
    private string saveFilePath { get => Application.persistentDataPath + "/witch_data.zdf"; }

    [System.Serializable]
    class SaveData
    {
        public bool tutorialEnabled = true;
        public int mana = 0;
        public int highScore = 0;
        public List<int> buyCounts = new List<int>();
        public List<bool> activeSpells = new List<bool>();
    }

    void Save()
    {
        var formatter = new BinaryFormatter();
        var file = new FileStream(saveFilePath, FileMode.Create);

        var saveData = new SaveData();

        saveData.tutorialEnabled = tutorialEnabled;
        saveData.mana = spellbook.mana;
        saveData.highScore = highScore;
        foreach(var spelldata in spellbook.spelldata)
        {
            saveData.buyCounts.Add(spelldata.buyCount);
            saveData.activeSpells.Add(spelldata.active);
        }

        formatter.Serialize(file, saveData);
        file.Close();
    }

    void Load()
    {
        SaveData saveData;
        if(File.Exists(saveFilePath))
        {
            var formatter = new BinaryFormatter();
            var file = new FileStream(saveFilePath, FileMode.Open);

            saveData = formatter.Deserialize(file) as SaveData;
        }
        else
        {
            saveData = new SaveData();
        }

        tutorialEnabled = saveData.tutorialEnabled;
        highScore = saveData.highScore;
        spellbook.mana = saveData.mana;
        for(int i = 0; i < saveData.buyCounts.Count; i++)
        {
            spellbook.spelldata[i].active = saveData.activeSpells[i];
            spellbook.spelldata[i].buyCount = saveData.buyCounts[i];
        }
    }

    void Start()
    {
        stake.onGameOver += GameOver;
        Load();
    }

    public void GameOver()
    {
        stake.Deactivate();
        witch.SetInactive();

        DestroyEnemies(true);
        DeactivateSpawnPoints();
        DestroyProjectiles();
        ManageScore();

        ui.GameOver();
        Save();
    }

    public void RestartGame()
    {
        ui.PlayUi();

        if (!tutorialEnabled)
        {
            DestroyEnemies(false);
            ActivateSpawnPoints();

            witch.SetActive();
            stake.Activate();
            stake.ResetDurability();
        }
        else
        {
            tut.gameObject.SetActive(true);
        }
    }

    private void DestroyProjectiles()
    {
        var projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }
    }

    private void DeactivateSpawnPoints()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var point in spawnPoints)
        {
            point.GetComponent<SpawnPoint>().Deactivate();
        }
    }

    private void ActivateSpawnPoints()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (var point in spawnPoints)
        {
            point.GetComponent<SpawnPoint>().Activate();
        }
    }

    private void DestroyEnemies(bool exceptTorch)
    {
        var enemies = Physics2D.OverlapAreaAll(new Vector2(-20.0f, 10.0f), new Vector2(20.0f, -10.0f), LayerMask.GetMask("Enemies"));
        foreach (var enemy in enemies)
        {
            if (enemy.tag == "Torch" && exceptTorch)
                continue;

            Destroy(enemy.gameObject);
        }
    }

    private void ManageScore()
    {
        if (highScore < Score.value)
        {
            highScore = Score.value;
        }

        spellbook.mana += Score.value / 5;
        Score.value = 0;
    }
}
