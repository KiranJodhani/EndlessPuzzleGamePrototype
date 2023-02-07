using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public UIManager uIManager;
    public ScoreManager scoreManager;

    [Header("Game settings")]
    [Space(5)]
    public GameObject camObject;
    public GameObject player;
    public Rigidbody2D playerRigidbody2D;
    public Material trailMaterial;
    public GameObject startPlatform;
    [Space(5)]
    public Color[] colorTable;
    [Space(5)]
    [Header("Obstacles settings")]
    public GameObject obstaclePrefab;
    [Space(5)]
    public float obstacleHeight = .2f;
    public float minObstacleWidth = .75f;
    public float maxObstacleWidth = 2f;
    public float verticalGapBetweenLines = 2f;
    [Range(.5f, 1.5f)]
    public float minHorizontalGap = 1f;
    [Range(1.5f, 2.5f)]
    public float maxHorizontalGap = 2f;
    [Range(1f, 2f)]
    public float minHorizontalSpeed = 1.5f;
    [Range(2f, 3f)]
    public float maxHorizontalSpeed = 2.5f;
    [Range(.5f, 1.5f)]
    public float minVerticalSpeed = 1f;
    [Range(2, 3)]
    public float maxVerticalSpeed = 2.5f;
    [Space(5)]
    public bool started;
    public int lineNumber;
    public bool inAir;

    Vector3 screenSize; //for storing screen size
    GameObject lastObstacle; //for storing last created obstacle

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Physics2D.gravity = new Vector2(0, -9.81f);

        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        CreateScene();
    }

    void Update()
    {
        if (uIManager.gameState == GameState.PLAYING)
        {
            if (uIManager.IsButton())
                return;

            if (Input.GetMouseButton(0))
            {
                if (started && !inAir)
                {
                    playerRigidbody2D.velocity = Vector2.zero;
                    playerRigidbody2D.AddForce(new Vector2(0,190));
                    inAir = true;
                }

                if (!started)
                {
                    started = true;
                    player.GetComponent<TrailRenderer>().enabled = true;
                    ScoreManager.Instance.StartCounting();
                    startPlatform.SetActive(false);
                }

            }

            //when player goes bellow the screen trigger game over
            if (player.transform.position.y < camObject.transform.position.y - screenSize.y)
            {
                GameOver();
            }
            else if (player.transform.position.x > screenSize.x || player.transform.position.x < -screenSize.x) // if player go off screen on the left or right side
            {
                GameOver();
            }
        }
    }

    //spawn first obstacle
    public void SpawnObstacle()
    {
        lastObstacle = Instantiate(obstaclePrefab);
        lastObstacle.GetComponent<Obstacle>().InitOBstacle(new Vector2(0, 0), Random.Range(minHorizontalSpeed, maxHorizontalSpeed), GetRandomColor(), screenSize.x, screenSize.y, minHorizontalGap, maxHorizontalGap, verticalGapBetweenLines, obstacleHeight, Random.Range(minObstacleWidth, maxObstacleWidth), false);
        lineNumber++;
    }

    //create new scene
    public void CreateScene()
    {
        camObject.GetComponent<CameraFollowTarget>().ResetCameraPosition();
        lineNumber = 0;
        inAir = false;
        ShowPlayer();
        SpawnObstacle();
    }

    //set player and platform
    public void ShowPlayer()
    {
        Color tempColor = GetRandomColor();

        started = false;
        player.SetActive(true);
        trailMaterial.color = tempColor;
        player.GetComponent<SpriteRenderer>().color = tempColor;
        player.transform.position = new Vector2(0, -2f);

        startPlatform.SetActive(true);
        startPlatform.transform.position = new Vector2(0, -2.4f);
        startPlatform.GetComponent<SpriteRenderer>().color = tempColor;
    }

    public Color GetRandomColor()
    {
        return colorTable[Random.Range(0, colorTable.Length)];
    }

    //restart game, reset score, show player, open sides
    public void RestartGame()
    {
        if (uIManager.gameState == GameState.PAUSED)
            Time.timeScale = 1;

        ClearScene();
        CreateScene();
        scoreManager.ResetCurrentScore();
        started = false;

        uIManager.ShowGameplay();
    }

    //clear all blocks from scene and reset camera position
    public void ClearScene()
    {
        started = false;
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject item in obstacles)
        {
            Destroy(item);
        }
    }

    //show game over gui
    public void GameOver()
    {
        if (uIManager.gameState == GameState.PLAYING)
        {
            ScoreManager.Instance.StopCounting();
            AudioManager.Instance.PlayEffects(AudioManager.Instance.gameOver);
            AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
            uIManager.ShowGameOver();
            player.SetActive(false);
            player.GetComponent<TrailRenderer>().enabled = false;
            scoreManager.UpdateScoreGameover();
        }
    }
}
