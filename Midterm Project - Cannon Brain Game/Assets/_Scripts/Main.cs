using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   // Enables the loading & reloading of scenes
using UnityEngine.UI; // For Legacy Text
using TMPro; // For TextMeshPro
using UnityEngine.UIElements; 

public class Main : MonoBehaviour
{
    static public Main S;                        // A private singleton for Main
    private BoundsCheck bndCheck;

    [Header("Inscribed")]
    public TextMeshProUGUI uitScore;
    public TextMeshProUGUI uitLevel;
    public TextMeshProUGUI uitHighScore;
    public TextMeshProUGUI uitSBullets;
    public TextMeshProUGUI uitDBullets;
    public TextMeshProUGUI uitFBullets;
    public TextMeshProUGUI uitCrates;
    public TextMeshProUGUI uitCountdown;
    public TextMeshProUGUI levelChangeText;
    public AudioClip levelUpClip;
    public AudioClip levelDownClip;
    public AudioClip onSuccessClip; // Audio to play on successful crate destruction
    public GameObject onDeathParticles; // Particle system prefabs to instantiate on crate destruction
    public GameObject[] prefabCrates;               // Array of Crate prefabs 
    public float crateSpawnPerSecond = 0.5f;  // # Crates spawned/second
    public float crateInsetDefault = 1.5f;    // Inset from the sides

    [Header("Dynamic")]
    public int currentScore;
    public int level;
    public int levelMax;
    public int numCurrSpawnedCrates;
    public int highScore;
    public int remainingCrates; // Number of crates left to destroy.
    public int numDestroyedCrates; // TOTAL crates destroyed in this level
    public int numCorrectlyDestroyedCrates; // For keeping track of the goal conditions
                                            // A crate is correctly destroyed if its crate value reaches exactly 0.
    public int pointsPerSBullet = 1; // Strength of bullets from input "S"
    public int pointsPerDBullet = 2;
    public int pointsPerFBullet = 3;
    private AudioSource audioSource;

    // Level Related Attributes
    public int[] maxCrates; // Max number of crates to spawn per level
    public int[] maxBulletsPerLevel;
    public int[] remainingSBullets; // Number of "S" shots remaining, per level
    public int[] remainingDBullets;
    public int[] remainingFBullets;
    public int[] goals; // Set the goals for each level.
                        // Goals are based on number of *correctly* destroyed crates.
    public string finalMessage; // Message yto display at end scene

    private float startTime; // For countdown.
    private int timeRemain; // For countdown.


    void Awake()
    {
        S = this; // Define the singleton

        level = 0;
        levelMax = 3;
        currentScore = 0;
        numCurrSpawnedCrates = 0;
        remainingCrates = maxCrates[level];
        numDestroyedCrates = 0;
        numCorrectlyDestroyedCrates = 0;
        startTime = Time.time;
        audioSource = GetComponent<AudioSource>();
        levelChangeText.enabled = false;

        maxBulletsPerLevel = new int[] { 50, 30, 20 };
        remainingSBullets = new int[] { maxBulletsPerLevel[0], maxBulletsPerLevel[1], maxBulletsPerLevel[2] };
        remainingDBullets = new int[] { maxBulletsPerLevel[0], maxBulletsPerLevel[1], maxBulletsPerLevel[2] };
        remainingFBullets = new int[] { maxBulletsPerLevel[0], maxBulletsPerLevel[1], maxBulletsPerLevel[2] };
        maxCrates = new int[] { 9, 12, 15 };
        goals = new int[] { 5, 7, 8 }; // Destroy more than half of the crates to reach this goal

        // Playerprefs for highscore
        if (PlayerPrefs.HasKey("HighScore"))
        {
            SCORE = PlayerPrefs.GetInt("HighScore");
        }
        // Assign the high score to HighScore
        PlayerPrefs.SetInt("HighScore", SCORE);


        // Set bndCheck to reference the BoundsCheck component on this GameObject
        bndCheck = GetComponent<BoundsCheck>();

        // Invoke SpawnCrate() once (in 2 seconds, based on default values)
        Invoke(nameof(SpawnCrate), 2f);                // a
    }

    public void SpawnCrate()
    {
        // Pick a random Crate prefab to instantiate
        int ndx = Random.Range(0, prefabCrates.Length);                     // b
        GameObject go = Instantiate<GameObject>(prefabCrates[ndx]);     // c

        // Position the Crate above the screen with a random x position
        float crateInset = crateInsetDefault;                                // d
        if (go.GetComponent<BoundsCheck>() != null)
        {                        // e
            crateInset = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        // Set the initial position for the spawned Crate                    // f
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + crateInset;
        float xMax = bndCheck.camWidth - crateInset;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + crateInset;
        go.transform.position = pos;

        numCurrSpawnedCrates++; // Count spawned crate

        // Invoke SpawnCrate() again
        if (numCurrSpawnedCrates < maxCrates[level])
        {
            // Invoke SpawnCrate() in specified time
            Invoke(nameof(SpawnCrate), 1f / crateSpawnPerSecond);                // g
        }
    }

    // For showing the data in the GUITexts
    void UpdateGUI()
    {
        uitScore.text = "Score: " + currentScore.ToString();
        uitLevel.text = "Level: " + (level+1).ToString() + " of " + levelMax.ToString();
        uitSBullets.text = "S (1): " + remainingSBullets[level].ToString();
        uitDBullets.text = "D (2): " + remainingDBullets[level].ToString();
        uitFBullets.text = "F (3): " + remainingFBullets[level].ToString();
        uitCrates.text = "Crates Left: " + remainingCrates.ToString();

        // Countdown logic
        int timeElapsed = (int)(Time.time - startTime);
        //print(timeElapsed);
        timeRemain = 180 - timeElapsed; // 3 minute countdown

        if (timeRemain > 0)
        {
            uitCountdown.text = "Time Remaining: " + timeRemain.ToString();
        }
        else
        {
            uitCountdown.text = "Countdown has finished";

            // Other conditions to end game: Timer runs out.
            finalMessage = "You ran out of time. Try again!";
            SceneManager.LoadScene("GameOverScreen");
        }

        Color c = uitCountdown.color;
        c.a = timeRemain / 180f;
        uitCountdown.color = c;
    }

    void Update()
    {
        UpdateGUI();

        // Check for level completion
        if (numDestroyedCrates == maxCrates[level])
        {
            // Check for level up
            if ((numCorrectlyDestroyedCrates >= goals[level])) 
            {
                if ((level + 1) < levelMax)
                {
                    level++;
                    numDestroyedCrates = 0;
                    numCorrectlyDestroyedCrates = 0;
                    numCurrSpawnedCrates = 0;
                    remainingCrates = maxCrates[level];
                    remainingSBullets[level] = maxBulletsPerLevel[level];
                    remainingDBullets[level] = maxBulletsPerLevel[level];
                    remainingFBullets[level] = maxBulletsPerLevel[level];

                    // Level up pop up sound and graphics
                    audioSource.PlayOneShot(levelUpClip);
                    levelChangeText.enabled = true;
                    levelChangeText.text = "Level Up!\nReached Level " + (level+1).ToString() + "!";
                    Invoke(nameof(HideLevelChangeGraphic), 2f); // Hide after 2 seconds

                    // Invoke SpawnCrate() in specified time
                    Invoke(nameof(SpawnCrate), 1f / crateSpawnPerSecond);
                }
                // Beat the game.
                else
                {
                    // call gameover screen 
                    finalMessage = "Congrats, you beat the game!\n" + timeRemain.ToString() + " leftover secs have been added your final score.";
                    currentScore += timeRemain;
                    TRY_TO_SET_HIGH_SCORE(currentScore);
                    SceneManager.LoadScene("GameOverScreen");
                }
            }

            // Check for level down [other conditions include time runs out]
            else
            {
                if (level > 0)
                {
                    level--;
                    numDestroyedCrates = 0;
                    numCorrectlyDestroyedCrates = 0;
                    numCurrSpawnedCrates = 0;
                    remainingCrates = maxCrates[level];
                    remainingSBullets[level] = maxBulletsPerLevel[level];
                    remainingDBullets[level] = maxBulletsPerLevel[level];
                    remainingFBullets[level] = maxBulletsPerLevel[level];

                    // Level up pop up sound and graphics
                    audioSource.PlayOneShot(levelDownClip);
                    levelChangeText.enabled = true;
                    levelChangeText.text = "You lost a level!\nNow Level " + (level + 1).ToString() + "!";
                    Invoke(nameof(HideLevelChangeGraphic), 2f); // Hide after 2 seconds

                    // Invoke SpawnCrate() in specified time
                    Invoke(nameof(SpawnCrate), 1f / crateSpawnPerSecond);
                }
                // Level too low, game over.
                else
                {
                    // call gameover screen
                    finalMessage = "You missed the goals for too many crates. Try again!";
                    SceneManager.LoadScene("GameOverScreen");
                }

            }
        }
    }

    public void SHOT_FIRED(int bulletToRemove)
    {
        // Reduce the count of the given bullet type
        if (bulletToRemove == pointsPerSBullet && remainingSBullets[level] > 0)
        {
            remainingSBullets[level]--;
        }
        else if (bulletToRemove == pointsPerDBullet && remainingDBullets[level] > 0)
        {
            remainingDBullets[level]--;
        }
        else if (bulletToRemove == pointsPerFBullet && remainingFBullets[level] > 0)
        {
            remainingFBullets[level]--;
        }
    }

    // Amount of points to add or deduct
    public void ON_DESTROY(int scoreChange)
    {
        currentScore += scoreChange;

        if (currentScore < 0) 
            currentScore = 0;

        TRY_TO_SET_HIGH_SCORE(currentScore);
    }

    void HideLevelChangeGraphic()
    {
        levelChangeText.enabled = false;
    }

    public void TRY_TO_SET_HIGH_SCORE(int scoreToTry)
    {
        if (scoreToTry <= SCORE) return;
        SCORE = scoreToTry;
    }

    public int SCORE
    { 
        get { return highScore; } 
        private set 
        { 
            highScore = value;
            PlayerPrefs.SetInt("HighScore", value);
            if (uitHighScore != null)
            {
                uitHighScore.text = "High Score: " + value.ToString();            
            }
        }
    }
}