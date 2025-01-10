using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //Singleton;

    [SerializeField] private GameObject btnStart; //Reference for button Start Game
    [SerializeField] private Camera gameCamera; //Reference for the camera used when playing the game
    [SerializeField] private GameObject cmfl; //Reference to the CM Free Look object
    [SerializeField] private Camera beginningCam; //Reference for the camera used before playing the game
    [SerializeField] private Text gameplay; //Reference for the text explaining the gameplay at the beginning
    public Camera mapCamera; //Reference for the map visual

    [SerializeField] private int lives = 3; //number of lives before game over
    private const string prelives = "Lives = "; //pretext before showing the number of lives
    [SerializeField] private Text txtlives; //text for lives
    [SerializeField] private GameObject character; //Reference to our character

    [SerializeField] private GameObject btnGo; //Reference for button Go
    [SerializeField] private GameObject btnReplay; //Reference for button Play Again
    [SerializeField] private GameObject btnQuit; //Reference for button Quit
    [SerializeField] private GameObject btnQuitP; //Reference for button Quit when pausing
    [SerializeField] private Text pause; //Reference for the text to tell player how to continue playing after pausing
    [SerializeField] private Text title; //Reference for the title
    [SerializeField] private Text gameOver; //Reference for the game over txt when dead
    [SerializeField] private GameObject btnback1; //Reference for button back1
    [SerializeField] private GameObject btnnext1; //Reference for button next1
    [SerializeField] private GameObject btnback2; //Reference for button back1
    [SerializeField] private Text gameplay2; //Reference for the text explaining the gameplay at the beginning part 2
    private string sceneToReload = "SampleScene"; // Load this screen when retrying game
    [SerializeField] private Text congratulations; //Reference to the text when winning game
    [SerializeField] private Text diedOnce; //Reference for the message when player died once and gets respawned
    private bool gameCompleted = false; //Game over or not?
    private float oldTime = 0f; //Old time (0 by default) which allows us to toggle (switch) between a pause and the time scale of the game


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public void StartGameCamera() //after clicking Start Game button
    {
        beginningCam.enabled = false; //disable the camera in the beginning
        btnStart.SetActive(false); //disable the button Start Game
        btnback2.SetActive(false); //inactive until btnnext1 clicked
        gameplay2.enabled = false; //disable the text with the gameplay/rules
        gameCamera.enabled = true; //enable the camera used for playing the game
        cmfl.SetActive(true); //active with game camera
        mapCamera.GetComponent<Camera>().enabled = false; //enable the camera used to see map
        txtlives.enabled = true; //activate text
        txtlives.text = prelives + lives.ToString("D1"); //show lives
        Cursor.lockState = CursorLockMode.Locked; //Freeze the cursor (mouse) in the center of the screen 
        Cursor.visible = false; //hide cursor
        gameCompleted = false;
    }

    public void TitlePage()
    {
        btnGo.SetActive(true); //the button Go is there to begin the explanation of the gameplay
        btnQuit.SetActive(true); //the button Quit is there to begin the explanation of the gameplay
        title.enabled = true; //we see the title when opening game
        gameplay.enabled = false; //inactive until btnGo clicked or btnback2 clicked
        gameplay2.enabled = false; //inactive until btnnext1 or btnback2 clicked
    }

    public void FirstPage()
    {
        btnGo.SetActive(false); //disabled
        btnQuit.SetActive(false); //disabled
        title.enabled = false; //disabled
        btnback1.SetActive(true); //inactive until btnGo clicked or btnback2 clicked
        btnnext1.SetActive(true); //inactive until btnGo clicked or btnback2 clicked
        btnback2.SetActive(false); //inactive until btnnext1 clicked
        btnStart.SetActive(false); //the button Start Game is inactive at first when the game starts running
        gameplay.enabled = true; //inactive until btnGo clicked or btnback2 clicked
        gameplay2.enabled = false; //inactive until btnnext1 or btnback2 clicked
    }

    public void SecondPage()
    {
        btnback1.SetActive(false); //inactive until btnGo clicked or btnback2 clicked
        btnnext1.SetActive(false); //inactive until btnGo clicked or btnback2 clicked
        gameplay.enabled = false; //inactive until btnGo clicked or btnback2 clicked
        btnback2.SetActive(true); //inactive until btnnext1 clicked
        btnStart.SetActive(true); //inactive until btnnext1 clicked
        gameplay2.enabled = true; //inactive until btnnext1 or btnback2 clicked
    }

    public void Quit()
    {
        Application.Quit(); //Terminate application
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(sceneToReload); //Load the scene SampleScene
    }

    public void ExposeMap()
    {
        mapCamera.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        txtlives.enabled = false; //text hidden
        gameCamera.enabled = false; //camera used when playing the game is disabled in the beginning
        cmfl.SetActive(false); //inactive with the game camera
        mapCamera.enabled = false; //disable the camera used to see map
        btnStart.SetActive(false); //the button Start Game is inactive at first when the game starts running
        btnGo.SetActive(true); //the button Go is there to begin the explanation of the gameplay
        btnQuit.SetActive(true); //the button Quit is there to begin the explanation of the gameplay
        title.enabled = true; //we see the title when opening game
        btnback1.SetActive(false); //inactive until btnGo clicked or btnback2 clicked
        btnback2.SetActive(false); //inactive until btnnext1 clicked
        btnnext1.SetActive(false); //inactive until btnGo clicked or btnback2 clicked
        gameplay.enabled = false; //inactive until btnGo clicked or btnback2 clicked
        gameplay2.enabled = false; //inactive until btnnext1 or btnback2 clicked
        btnReplay.SetActive(false); //inactive until dead or game completed
        gameOver.enabled = false; //inactive unless dead
        congratulations.enabled = false; //active only when win
        diedOnce.enabled = false; //when player died once
        Cursor.lockState = CursorLockMode.None; //Freeze the cursor (mouse) in the center of the screen but not when opening the game
        Cursor.visible = true; //show cursor
        btnQuitP.SetActive(false); //not active unless pausing
        pause.enabled = false; //not activated
        gameCompleted = true;
    }

    //Reference 2 Start
    public void Dead() //when character colliding with objects with Death tag
    {
        lives--;
        diedOnce.enabled = true; //activate message
        Invoke("DiedOff", 3); //message disappears
        txtlives.text = prelives + lives.ToString("D1"); //show lives
        character.transform.position = new Vector3(0, 0, 0);
        if (lives == 0)
        {
            gameCompleted = true;
            Destroy(character); //Destroy the character
            btnReplay.SetActive(true); //inactive until dead or game completed
            btnQuit.SetActive(true); //the button Quit is active
            gameCamera.enabled = false; //camera used when playing the game is disabled in the beginning and when dead
            beginningCam.enabled = true; //disable the camera in the beginning
            gameOver.enabled = true; //active
            diedOnce.enabled = false; //if game over, do not show message
            Cursor.lockState = CursorLockMode.None; //cursor unlocked
            Cursor.visible = true; //show cursor
        }
    } //Reference 2 End

    public void Win()
    {
        gameCompleted = true;
        btnReplay.SetActive(true); //inactive until dead or game completed
        btnQuit.SetActive(true); //the button Quit is active
        gameCamera.enabled = false; //camera used when playing the game is disabled in the beginning and when game finishes
        beginningCam.enabled = true; //disable the camera in the beginning
        congratulations.enabled = true; //show win message
        txtlives.enabled = false; //text hidden
        Cursor.lockState = CursorLockMode.None; //cursor unlocked
        Cursor.visible = true; //show cursor
    }

    public void DiedOff()
    {
        diedOnce.enabled = false; //deactivate
    }

    // Update is called once per frame
    void Update() //Reference 1 Start
    {
        if (Input.GetButtonDown("Cancel") && !gameCompleted) //if I press on "Escape", the game will pause/play
        {
            float prevTime = oldTime; //Hide the saved timeScaled
            oldTime = Time.timeScale; //Permute (alter) the timeScale so we can come back to it
            Time.timeScale = prevTime; //Change timeScale for the hidden value

            Cursor.lockState = (Time.timeScale > 0) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = (Time.timeScale > 0) ? false : true;
            if(Cursor.lockState == CursorLockMode.None && Cursor.visible == true)
            {
                btnQuitP.SetActive(true); //player can quit the game
                pause.enabled = true; //tell player to press escape again to continue playing
            }
            else
            {
                btnQuitP.SetActive(false); //not active unless pausing
                pause.enabled = false; //not activated
            }
            
        }
    } // Reference 1 End
}

//References 
// 1- Script for pausing the game in class project Rebonds.
// 2- Inspired from the script in class project Block Breaker (2D game)