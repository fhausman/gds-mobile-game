using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int value = 0;

    private Text uiScore;

    void Start()
    {
        value = 0;
        uiScore = GetComponent<Text>();
    }

    void Update()
    {
        uiScore.text = value.ToString();
    }
}
