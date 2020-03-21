using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
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
    public bool soundEnabled = true;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI manaText;

    private int highScore = 0;
    private string saveFilePath { get => Application.persistentDataPath + "/witch_data_003.zdf"; }

    [System.Serializable]
    class SaveData
    {
        public bool tutorialEnabled = true;
        public bool soundEnabled = true;
        public int mana = 2000;
        public int highScore = 0;
        public List<int> buyCounts = new List<int>();
        public List<bool> activeSpells = new List<bool>();
    }

    public void Save()
    {
        var formatter = new BinaryFormatter();
        var file = new FileStream(saveFilePath, FileMode.Create);

        var saveData = new SaveData
        {
            tutorialEnabled = tutorialEnabled,
            soundEnabled = soundEnabled,
            mana = spellbook.mana,
            highScore = highScore,
            buyCounts = spellbook.spelldata.Select(x => x.buyCount).ToList(),
            activeSpells = spellbook.spelldata.Select(x => x.active).ToList()
        };

        formatter.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        SaveData saveData;
        if(File.Exists(saveFilePath))
        {
            var formatter = new BinaryFormatter();
            var file = new FileStream(saveFilePath, FileMode.Open);

            saveData = formatter.Deserialize(file) as SaveData;

            file.Close();
        }
        else
        {
            saveData = new SaveData();
        }

        soundEnabled = saveData.soundEnabled;
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
        stake.onGameOver += witch.Burn;
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
        DestroyProjectiles();
        ui.DisableAllChildren();

        if (!tutorialEnabled)
        {
            DestroyEnemies(false);

            witch.SetActive();
            stake.Activate();
            stake.ResetDurability();

            StartCoroutine(SpawnPointsActivation());
            acceptsPlayerInput = true;
        }
        else
        {
            tut.gameObject.SetActive(true);
            tut.Begin();
        }
    }

    private IEnumerator SpawnPointsActivation()
    {
        yield return new WaitForSeconds(2.0f);

        ui.PlayUi();
        ActivateSpawnPoints();
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

        highScoreText.text = highScore.ToString();
        scoreText.text = Score.value.ToString();
        manaText.text = spellbook.mana.ToString();

        Score.value = 0;
    }
}
