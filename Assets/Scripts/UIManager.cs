using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameObject mainMenu;
    GameObject spellbook;
    GameObject credits;
    GameObject hud;
    GameObject gameOver;

    Spellbook spellbookManager;
    AudioListener audioListener;
    GameManager gameManager;

    void Start()
    {
        mainMenu = transform.Find("MainMenu").gameObject;
        spellbook = transform.Find("Spellbook").gameObject;
        credits = transform.Find("Credits").gameObject;
        hud = transform.Find("HUD").gameObject;
        gameOver = transform.Find("GameOverMenu").gameObject;

        spellbookManager = GameObject.Find("SpellManager").GetComponent<Spellbook>();
        audioListener = GameObject.Find("Main Camera").GetComponent<AudioListener>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        mainMenu.transform.Find("Tutorial").GetComponent<Toggle>().isOn = gameManager.tutorialEnabled;
    }

    public void DisableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void Play()
    {
        gameManager.RestartGame();
        PlayUi();
    }

    public void PlayUi()
    {
        DisableAllChildren();
        hud.SetActive(true);
        spellbookManager.UpdateActiveSpells();
    }

    public void Spellbook()
    {
        DisableAllChildren();
        spellbook.SetActive(true);
        spellbookManager.UpdateShop();
    }
    public void Credits()
    {
        DisableAllChildren();
        credits.SetActive(true);
    }

    public void Back()
    {
        DisableAllChildren();
        mainMenu.SetActive(true);
    }

    public void GameOver()
    {
        DisableAllChildren();
        gameOver.SetActive(true);
    }

    public void OnSoundToggleChange(bool val)
    {
        audioListener.enabled = !audioListener.enabled;
    }

    public void OnTutorialToggleChange(bool val)
    {
        gameManager.tutorialEnabled = !gameManager.tutorialEnabled;
    }
}
