using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{
    private TextMeshProUGUI uitScore;

    // Start is called before the first frame update
    void Start()
    {
        uitScore = GetComponent<TextMeshProUGUI>();
        uitScore.text = "Final Score: " + Main.S.currentScore.ToString();
    }
}
