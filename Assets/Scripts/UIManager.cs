using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameObject mainMenu;
    GameObject spellbook;
    GameObject credits;
    GameObject hud;
    GameObject gameOver;
    GameObject tutorialMessages;
    Toggle tutorialToggle;
    Toggle soundToggle;

    Spellbook spellbookManager;
    GameManager gameManager;

    void Start()
    {
        mainMenu = transform.Find("MainMenu").gameObject;
        spellbook = transform.Find("Spellbook").gameObject;
        credits = transform.Find("Credits").gameObject;
        hud = transform.Find("HUD").gameObject;
        gameOver = transform.Find("GameOverMenu").gameObject;
        tutorialMessages = transform.Find("TutorialMessages").gameObject;

        spellbookManager = GameObject.Find("SpellManager").GetComponent<Spellbook>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        tutorialToggle = mainMenu.transform.Find("Tutorial").GetComponent<Toggle>();
        soundToggle = mainMenu.transform.Find("Sound").GetComponent<Toggle>();

        gameManager.Load();

        UpdateToggles();
    }

    public void UpdateToggles()
    {
        soundToggle.isOn = gameManager.soundEnabled;
        tutorialToggle.isOn = gameManager.tutorialEnabled;
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
        UpdateToggles();
        gameManager.Save();
    }

    public void GameOver()
    {
        DisableAllChildren();
        gameOver.SetActive(true);
    }

    public void ShowTutorial()
    {
        DisableAllChildren();
        tutorialMessages.SetActive(true);
    }

    public void SetTutorialText(string text)
    {
        var message = tutorialMessages.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        message.text = text;
    }

    public void OnSoundToggleChange(bool val)
    {
        gameManager.soundEnabled = val;
        AudioListener.volume = val ? 1 : 0;
    }

    public void OnTutorialToggleChange(bool val)
    {
        gameManager.tutorialEnabled = val;
    }
}
