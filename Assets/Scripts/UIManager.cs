using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameObject mainMenu;
    GameObject spellbook;
    GameObject credits;
    GameObject hud;

    AudioListener audioListener;

    void Start()
    {
        Time.timeScale = 0.0f;

        mainMenu = transform.Find("MainMenu").gameObject;
        spellbook = transform.Find("Spellbook").gameObject;
        credits = transform.Find("Credits").gameObject;
        hud = transform.Find("HUD").gameObject;

        audioListener = GameObject.Find("Main Camera").GetComponent<AudioListener>();
    }

    void DisableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void Play()
    {
        Time.timeScale = 1.0f;
        GameObject.Find("Witch").GetComponent<Witch>().enabled = true;
        mainMenu.SetActive(false);
        hud.SetActive(true);
    }

    public void Spellbook()
    {
        DisableAllChildren();
        spellbook.SetActive(true);
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

    public void OnSoundToggleChange(bool val)
    {
        audioListener.enabled = !audioListener.enabled;
    }

    public void OnTutorialToggleChange(bool val)
    {
        //enable tutorial
    }
}
