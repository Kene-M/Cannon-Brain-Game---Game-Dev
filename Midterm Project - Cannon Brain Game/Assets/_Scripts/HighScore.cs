using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    private TextMeshProUGUI uitHighScore;

    // Start is called before the first frame update
    void Start()
    {
        uitHighScore = GetComponent<TextMeshProUGUI>();
        uitHighScore.text = "High Score: " + Main.S.highScore.ToString();
    }
}
