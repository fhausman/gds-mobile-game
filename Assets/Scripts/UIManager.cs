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
        tutorialMessages = transform.Find("TutorialMessages").gameObject;

        spellbookManager = GameObject.Find("SpellManager").GetComponent<Spellbook>();
        audioListener = GameObject.Find("Main Camera").GetComponent<AudioListener>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        var tutorialToggle = mainMenu.transform.Find("Tutorial").GetComponent<Toggle>();
        tutorialToggle.isOn = gameManager.tutorialEnabled;
        gameManager.tutorialEnabled = tutorialToggle.isOn; //workaround
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
        audioListener.enabled = !audioListener.enabled;
    }

    public void OnTutorialToggleChange(bool val)
    {
        Debug.Log("hrhrhr");
        gameManager.tutorialEnabled = !gameManager.tutorialEnabled;
    }
}
