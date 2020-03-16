using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int value = 0;

    private TextMeshProUGUI uiScore;

    void Start()
    {
        value = 0;
        uiScore = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        uiScore.text = value.ToString();
    }
}
