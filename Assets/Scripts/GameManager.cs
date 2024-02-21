using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip backgroundMusic1;

    [SerializeField] 
    private AudioClip backgroundMusic2;

    [SerializeField] 
    private AudioClip playSFX;

    [SerializeField] 
    private AudioSource musicSource;

    [SerializeField] 
    private AudioSource sfxSource;

    [SerializeField]
    GameObject ball, startButton, scoreText, quitButton, restartButton,livesText;

    int score;
    int lives;

    [SerializeField]
    Rigidbody2D left, right;


    [SerializeField]
    Vector3 startPos;

    public int multiplier;

    bool canPlay;

    public static GameManager instance;

    private GameObject currentBallInstance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1;
        score = 0;
        lives = 3;
        multiplier = 1;
        canPlay = false;
        UpdateLivesText();
    }

    private void Update()
    {
        if (!canPlay) return;
        if(Input.GetKey(KeyCode.A))
        {
            left.AddTorque(25f);
        }
        else
        {
            left.AddTorque(-20f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            right.AddTorque(-25f);
        }
        else
        {
            right.AddTorque(20f);
        }

    }

    public void UpdateScore(int point, int mullIncrease)
    {
        multiplier += mullIncrease;
        score += point * multiplier;
        Text scoreTextComponent = scoreText.GetComponent<Text>();
        if (scoreTextComponent != null)
        {
            scoreTextComponent.text = "Score: " + score;
        }
        else
        {
            Debug.LogError("Text component on scoreText GameObject is missing or scoreText GameObject is not assigned.");
        }
    }

    public void LoseLife()
    {
        lives--;  
        UpdateLivesText(); 
        if (lives <= 0)
        {
            GameEnd();  
        }
        else
        {
            ResetBall();  
        }
    }
    private void UpdateLivesText()
    {
        Text livesTextComponent = livesText.GetComponent<Text>();
        if (livesTextComponent != null)
        {
            livesTextComponent.text = "Lives " + lives;
        }
    }

    private void ResetBall()
    {
        if (ball != null)
        {

            if (currentBallInstance != null)
                Destroy(currentBallInstance);
            currentBallInstance = Instantiate(ball, startPos, Quaternion.identity);
        }
    }

    public void GameEnd()
    {
        musicSource.clip = backgroundMusic2;
        musicSource.Play();
        Time.timeScale = 0;
        quitButton.SetActive(true);
        restartButton.SetActive(true);
    }

   public void GameStart()
    {
        sfxSource.PlayOneShot(playSFX);
        musicSource.clip = backgroundMusic1;
        musicSource.Play();
        startButton.SetActive(false);
        scoreText.SetActive(true);
        livesText.SetActive(true);
        currentBallInstance = Instantiate(ball, startPos, Quaternion.identity);
        canPlay = true;
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void GameRestart()
    {
        sfxSource.PlayOneShot(playSFX);
        musicSource.clip = backgroundMusic1;
        musicSource.Play();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
