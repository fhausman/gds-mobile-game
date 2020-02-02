using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour
{
    public static int livesLeft = 3;

    private Text livesUi;

    void Start()
    {
        livesLeft = 3;
        livesUi = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (livesLeft < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        livesUi.text = "Lives left: " + livesLeft.ToString();
    }
}
