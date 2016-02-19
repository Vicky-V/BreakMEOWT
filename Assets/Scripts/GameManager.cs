using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public GameObject LeftWall;
	public GameObject RightWall;
	public GameObject TopWall;

    public GameObject Kitty;
    public BallController Ball;
    public PaddleController Paddle;
    public TilesGenerator Tiles;

    [Header("UI Screens")]

    [SerializeField]
    Canvas m_GameOverScr;
    [SerializeField]
    Canvas m_GameWonScr;
    [SerializeField]
    Canvas m_ReadyScr;
    [SerializeField]
    Canvas m_MainMenuScr;
    [SerializeField]
    Canvas m_OptionsMenuScr;
    [SerializeField]
    Canvas m_PauseMenuScr;

    [SerializeField]
    UnityEngine.UI.Text m_LifeCounter;

    private static GameManager m_Instance;
	public static GameManager Instance 
	{
		get
		{ 
			GameManager gm = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
			if (gm != null)
			{
				m_Instance = gm;
			}
			else
			{
				GameObject newManager = new GameObject("Game Manager");
				//DontDestroyOnLoad(newManager);
				m_Instance = newManager.AddComponent<GameManager>();
			}
			return m_Instance;
		}
	}

    int m_Lives;
    const int MAX_LIVES = 3;

    Vector2 m_BallVelocity;

    bool m_TransitionOn = false;

    void Start()
    {
        Ball.CanUpdate = false;
        Paddle.CanUpdate = false;
        m_Lives = MAX_LIVES;
        m_LifeCounter.text = m_Lives.ToString();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
    }

    public void OnGameExit()
    {
        Application.Quit();
    }

    public void OnGameOptions()
    {
        m_MainMenuScr.enabled = false;
        m_OptionsMenuScr.enabled = true;
    }

    public void SwitchBallImage(Sprite sprite)
    {
        Ball.BallSprite = sprite;
    }

    public void SetBallSpeed(UnityEngine.UI.Slider slider)
    {
        Ball.StartingForce = slider.value;
    }

    public void OnBackToMenu()
    {
        Ball.CanUpdate = false;
        Paddle.CanUpdate = false;
        m_OptionsMenuScr.enabled = false;
        m_GameOverScr.enabled = false;
        m_GameWonScr.enabled = false;
        m_MainMenuScr.enabled = true;
    }

    public void TogglePauseGame()
    {
        if (m_TransitionOn)
            return;

        m_PauseMenuScr.enabled = !m_PauseMenuScr.enabled;
        PauseObjects(m_PauseMenuScr.enabled);
    }
    
    void PauseObjects(bool aPaused)
    {
        if(Kitty!=null)
        {
            Rigidbody2D charRB = Kitty.GetComponent<Rigidbody2D>();
            charRB.isKinematic = aPaused;
        }

        Rigidbody2D ballRB = Ball.GetComponent<Rigidbody2D>();

        if (ballRB.velocity.sqrMagnitude < m_BallVelocity.sqrMagnitude && ballRB.velocity.sqrMagnitude!=0)
        {
            ballRB.velocity = m_BallVelocity;
        }
        else
        {
            m_BallVelocity = ballRB.velocity;
        }

        Paddle.CanUpdate = !aPaused;
        Ball.CanUpdate = !aPaused;
        ballRB.isKinematic = aPaused;
    }

    public void OnGameStart()
    {
        Ball.CanUpdate = true;
        Paddle.CanUpdate = true;
        m_MainMenuScr.enabled = false;

        m_LifeCounter.text = MAX_LIVES.ToString();

        OnResetGame(true);
    }

    public void OnGameWon()
    {
        m_TransitionOn = true;
        m_GameWonScr.enabled = true;
        PauseObjects(true);
    }

    public void OnResetGame(bool aResetTiles)
    {
        m_OptionsMenuScr.enabled = false;
        m_GameOverScr.enabled = false;
        m_GameWonScr.enabled = false;
        StartCoroutine(getReady_cr(aResetTiles));
    }
    

    public void OnGameOver()
    {
        m_Lives--;
        m_LifeCounter.text = m_Lives.ToString();
        if (m_Lives==0)
        {
            m_TransitionOn = true;
            m_GameOverScr.enabled = true;
            PauseObjects(true);
        }
        else
        {
            m_TransitionOn = true;
            StartCoroutine(getReady_cr(false));
        }

        if(m_Lives == 0)
        {
            m_Lives = MAX_LIVES;
        }
    }

    IEnumerator getReady_cr(bool aWasRestarted)
    {
        PauseObjects(true);

        m_ReadyScr.enabled = true;

        if (aWasRestarted)
        {
            if (Kitty != null)
                Destroy(Kitty);

            Tiles.ResetTiles();
        }

        Paddle.ResetPaddle();

        while (Input.anyKeyDown == false)
            yield return null;

        m_ReadyScr.enabled = false;

        m_TransitionOn = false;

        PauseObjects(false);

        Ball.ResetBall();

        m_LifeCounter.text = m_Lives.ToString();
    }
    

}
