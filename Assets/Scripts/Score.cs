using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int score = 0;

    private Text uiScore;

    void Start()
    {
        score = 0;
        uiScore = GetComponent<Text>();
    }

    void Update()
    {
        uiScore.text = score.ToString();
    }
}
