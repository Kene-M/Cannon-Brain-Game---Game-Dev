using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSound : MonoBehaviour
{
    private Sprite soundOnImage;
    public Sprite soundOffImage;
    public Button button;
    public static bool isMusicOn = true;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        soundOnImage = button.image.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        if (isMusicOn)
        {
            button.image.sprite = soundOffImage;
            isMusicOn = false;
            audioSource.mute = true;
        }
        else
        {
            button.image.sprite = soundOnImage;
            isMusicOn = true;
            audioSource.mute = false;
        }
    }
}
