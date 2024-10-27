using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalMessage : MonoBehaviour
{
    private TextMeshProUGUI uitFinalMessage;

    // Start is called before the first frame update
    void Start()
    {
        uitFinalMessage = GetComponent<TextMeshProUGUI>();
        uitFinalMessage.text = Main.S.finalMessage;
    }
}