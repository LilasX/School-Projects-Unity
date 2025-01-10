using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null; //Singleton

    private int score = 0; //score in numeric version
    public string preTextScore = "SCORE: "; //text to display before score

    const int pointsPerBounce = 100; //points per bond
    public Text txtScore;
    const float timeScaleLimit = 3.0f; //Maximum Game Speed

    private float oldTime = 0f; //Old time (0 by default) which allows us to toggle (switch) between a pause and the time scale of the game

    public GameObject smoke; //Effect of smoke

    private bool gameOver = false; //Game over or not?
    public GameObject btnExit; //Reference for button Exit
    public GameObject btnRetry; //Reference for button Retry?
    public string sceneToReload = "SampleScene"; // Load this screen when retrying game

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance !=this)
        {
            Destroy(this);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        txtScore.text = preTextScore + score.ToString("D8"); //SCORE: 00000000
        btnExit.SetActive(false); //Hide button Exit
        btnRetry.SetActive(false); //Hide button Retry?
        Cursor.lockState = CursorLockMode.Locked; //Freeze the cursor (mouse) in the center of the screen
        Cursor.visible = false; //Hide cursor
    }

    public void Quit()
    {
        Application.Quit(); //Terminate App
    }

    public void Retry()
    {
        SceneManager.LoadScene(sceneToReload); //Load the scene SampleScene
    }

    public void GameOver(GameObject ball)
    {
        smoke.transform.position = ball.transform.position;
        smoke.GetComponent<AudioSource>().Play(); //Play the sound of smoke
        Destroy(ball); //Erase ball from game
        Time.timeScale = 1.0f; //Put the time back to normal
        smoke.GetComponent<ParticleSystem>().Play(); //Play smoke effect
        if (!gameOver)
        {
            gameOver = true;
            btnExit.SetActive(true); //Button Exit appears
            btnRetry.SetActive(true); //Button Retry? appears
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void IncreaseDifficulty()
    {
        if (Time.timeScale >= timeScaleLimit) //If we reach speed limit
        {
            return; //Stop running the method
        }

        float timeScaler = Time.timeScale + 0.03f; //Increase speed by 0.03
        Time.timeScale = timeScaler; //Apply new speed

        Debug.Log("timeScale: " + Time.timeScale);
    }

    public void AddScore()
    {
        score += pointsPerBounce;
        txtScore.text = preTextScore + score.ToString("D8"); //SCORE: 00000000
        IncreaseDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        //PAUSE
        if (Input.GetButtonDown("Cancel") && !gameOver) //if I press on "Escape", the game will pause/play
        {
            float prevTime = oldTime; //Hide the saved timeScaled
            oldTime = Time.timeScale; //Permute (alter) the timeScale so we can come back to it
            Time.timeScale = prevTime; //Change timeScale for the hidden value

            Cursor.lockState = (Time.timeScale > 0) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = (Time.timeScale > 0) ? false : true;
        }
    }
}
