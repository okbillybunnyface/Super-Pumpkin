using UnityEngine;
using System.Collections;

public class PumpkinCannon : MonoBehaviour {

	public int spawnnDistance = 10, fontSize = 35;
	public static float timeLeft = 15 ;
	public static int score = 0, combo = 1;
	public int fireSpeed = 1000;
	public GameObject pumpkin;
    public AudioClip throwSound, gameOverSound;
	private Camera cam;
    public GameObject cameraPosStart, cameraPosOne, cameraPosTwo, cameraPosThree, cameraPosFour;
    public CircularQueue<GameObject> cameras;
    private GameObject playerCar;
    public Texture2D crosshair, gameOverImage, pausedImage;
    public Font font;
	GUIStyle newFont, titleFont;
    //private bool comboGoing = false;
    public static bool pause = false, gameOver = false, mainMenu = true;
    private float screenScale, screenScaleX, screenScaleY, timeSurvived;

    void Awake()
    {
        cameras = new CircularQueue<GameObject>();
        cameras.EnQueue(cameraPosStart);
        cameras.EnQueue(cameraPosOne);
        cameras.EnQueue(cameraPosTwo);
        cameras.EnQueue(cameraPosThree);
        cameras.EnQueue(cameraPosFour);

    }
	// Use this for initialization
	void Start () {

        StartCoroutine(changeCameraPosition());
        Time.timeScale = 1;
		cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		playerCar = GameObject.FindGameObjectWithTag("Player");
		newFont = new GUIStyle();
        newFont.font = font;
        titleFont = new GUIStyle();
        titleFont.font = font;
        pause = false;
        gameOver = false;
        timeLeft = 15;
        timeSurvived = 0;
        
	}
	
	// Update is called once per frame
	void Update () {
        //timer 
        if (mainMenu == false)
        {
            timeLeft -= Time.deltaTime;
            timeSurvived += Time.deltaTime;

            fireShot();
            pauseCheck();
            newGame();
            checkForFailState();

        }else if (mainMenu == true)
        {
            mainMenuFunctions(false);
        }
       
	}

	public void fireShot()
	{
		if (Input.GetMouseButtonDown(0) && pause == false && gameOver == false && mainMenu == false)
		{
			GameObject clone = (GameObject)Instantiate(pumpkin, cam.transform.position + cam.transform.forward * 2, 
			                                           Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
			clone.rigidbody.velocity = playerCar.transform.right * GoCarGo.actualSpeed;
            clone.rigidbody.AddTorque(Random.Range(0, 50) - 50, Random.Range(0, 50) - 50, Random.Range(0, 50) - 50);
			clone.rigidbody.AddForce(cam.transform.forward * fireSpeed);
            audio.PlayOneShot(throwSound);

		}
	}

    //reload level on failure
    public void newGame()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gameOver == true || mainMenu == true)
        {
            GoCarGo.canPlay = true;
            GoCarGo.speed = GoCarGo.startSpeed;
            LotGen.carChance = LotGen.startCarChance;
            LotGen.mailboxChance = LotGen.startMailboxChance;
            LotGen.trashcanChance = LotGen.startTrashcanChance;
            LotSpawn.houseChance = LotSpawn.startHouseChance;
            score = 0;
            Application.LoadLevel(0);
        }
        mainMenu = false;
    }

    public void pauseCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            pause = !pause;

            if (pause == true)
            {
                Time.timeScale = 0;
                GoCarGo.canPlay = false; //Stops music and junk
            }
            if (pause == false)
            {
                Time.timeScale = 1;
                GoCarGo.canPlay = true; //Starts music and junk
            }
        }
    }

    //Changes factors on failing, mouse etc
    public void checkForFailState()
    {
        if (pause || gameOver || mainMenu)
        {
            Screen.showCursor = true;
        }
        else 
        {
            Screen.showCursor = false;
        }
    }

    public void mainMenuFunctions(bool escapeToMenu)
    {
        //Escape to main menu
        if (escapeToMenu == true)
        {
            mainMenu = true;
            Application.LoadLevel(0);
        }

        //Change Camera 
        if (Input.GetMouseButtonDown(0)) {

            nextCamera();
        }

        //Start GAme
        if (Input.GetKeyDown(KeyCode.Space) && mainMenu == true)
        {
            newGame();
        }

    }

    public void nextCamera()
    {
        cam.transform.position = cameras.Forward().transform.position;
    }


    IEnumerator changeCameraPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (mainMenu)
            {
                nextCamera();
            }
        }

    }

	void OnGUI()
	{
        screenScale = (Screen.width / 1920f) / (Screen.height / 1080f); 
        screenScaleX = (Screen.width / 1920f);
        screenScaleY = (Screen.height / 1080f);
		newFont.fontSize = (int)(fontSize * screenScaleX);
		newFont.normal.textColor = Color.white;
        titleFont.fontSize = (int)(3f / 2f * fontSize * screenScaleX); ;
        titleFont.normal.textColor = Color.cyan;

        if (mainMenu == true)
        { 
            //GameName
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 2, Screen.height / 2), "Super Pumpkin!", titleFont);
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 + titleFont.fontSize + 5, Screen.width / 2, Screen.height / 2), "Press space to play", newFont);

            if (GUI.Button(new Rect(Screen.width - Screen.width / 6, Screen.height - 60, Screen.width / 6, 60), "Exit", newFont))
            {
                Application.Quit();
            }
        }

        if (mainMenu == false)
        {
            //Score
            GUI.Label(new Rect(0, 0, Screen.width / 2, Screen.height / 4), "Score: " + score.ToString(), newFont);

            //Combo
            GUI.Label(new Rect(0, fontSize + 10, 0, 0), "Combo: " + combo.ToString() + "x", newFont);

            //Timer
            GUI.Label(new Rect(Screen.width - 350 * screenScaleX, 0, 150, Screen.height / 4), "Time: " + ((int)(timeLeft)).ToString(), newFont);

            //Cross hair
            if (gameOver == false && pause == false)
            {
                float crosshairXMin = (Screen.width / 2) - (crosshair.width / 2);
                float crosshairYMin = (Screen.height / 2) - (crosshair.height / 2);
                GUI.DrawTexture(new Rect(crosshairXMin, crosshairYMin, crosshair.width, crosshair.height), crosshair);
            }
        }

        //Gameover
        if (timeLeft < 0)
        {
            float gameOverXMin = (Screen.width / 2) - 100 * screenScaleX;
            float gameOverYMin = (Screen.height / 2) - (gameOverImage.height / 2);
            GUI.Label(new Rect(gameOverXMin, gameOverYMin, gameOverImage.width, gameOverImage.height), "Game Over!", titleFont);//, ScaleMode.ScaleToFit, true, screenScale);
            Time.timeScale = 0;
            if (!gameOver)
            {
                GoCarGo.canPlay = false;
                audio.PlayOneShot(gameOverSound);
            }
            gameOver = true;
            pause = true;

            GUI.Label(new Rect(gameOverXMin, (gameOverYMin + gameOverImage.height), gameOverImage.width, fontSize), "Distance Traveled: " + (((int)GoCarGo.distanceTraveled)/2).ToString() + " m", newFont);
            GUI.Label(new Rect(gameOverXMin, (gameOverYMin + gameOverImage.height + fontSize + 5), gameOverImage.width, fontSize), "Time Survived: " + ((int)timeSurvived) + " seconds", newFont);
            GUI.Label(new Rect(gameOverXMin, (gameOverYMin + gameOverImage.height + fontSize + fontSize + 5), gameOverImage.width, fontSize), "Play again? Press Space", newFont);

        }

        //Paused
        if (pause == true)
        {
            float pausedXMin = (Screen.width / 2) - 175 * screenScaleX;
            float pausedYMin = (Screen.height / 2) - (pausedImage.height / 2);
            if(!gameOver)GUI.Label(new Rect(pausedXMin, pausedYMin, pausedImage.width, pausedImage.height), "Paused", titleFont);

            if (GUI.Button(new Rect((Screen.width - (Screen.width/6)), Screen.height-60, Screen.width/6, 60), "Menu", newFont)) 
            {
                GoCarGo.canPlay = true;
                mainMenuFunctions(true);
            }
        }
	}
}
