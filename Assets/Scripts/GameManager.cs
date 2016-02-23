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

    [SerializeField]
    SpriteRenderer m_Background;
    Color m_DefaultBGColor;
    [SerializeField]
    Color m_TintBGColor;

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
    UnityEngine.UI.Text m_PaswordInputText;

    [SerializeField]
    UnityEngine.UI.Text m_LifeCounter;

    [SerializeField]
    UnityEngine.UI.Text m_Pasword;

    const string SECRET_WORD = "stripes";
    string m_CurrentPassword ="";

    bool ModeUnlocked = false;

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
    public bool InTransition
    {
        get { return m_TransitionOn; }
    }

    void Start()
    {
        Ball.CanUpdate = false;
        Paddle.CanUpdate = false;
        m_Lives = MAX_LIVES;
        m_LifeCounter.text = m_Lives.ToString();
        AudioManager.Instance.PlayMusic(AudioManager.Instance.MenuMusic);
        m_DefaultBGColor = m_Background.color;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (m_MainMenuScr.enabled == false || m_OptionsMenuScr.enabled == false))
        {
            TogglePauseGame();
        }
    }

    public void CheckPassword()
    {
        if(m_PaswordInputText.text.ToLower()==SECRET_WORD)
        {
            m_PaswordInputText.color = Color.green;
            
            if(ModeUnlocked==false)
                OnUnlockMode();
        }
        else
        {
            m_PaswordInputText.color = Color.red;
        }
    }

    public void ResetPasswordColor()
    {
        m_PaswordInputText.color = Color.black;
    }

    void OnUnlockMode()
    {
        ModeUnlocked = true;
        Debug.Log("Mode triggered");
        Tiles.UnlockTheSuperDuperSecretSprite();
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

    public void FlashBackground()
    {
        StartCoroutine(flashBG_cr(0.5f));
    }

    public void OnBackToMenu()
    {
        Ball.CanUpdate = false;
        Paddle.CanUpdate = false;
        m_OptionsMenuScr.enabled = false;
        m_GameOverScr.enabled = false;
        m_GameWonScr.enabled = false;
        m_MainMenuScr.enabled = true;
        m_PauseMenuScr.enabled = false;
        
        AudioManager.Instance.PlayMusic(AudioManager.Instance.MenuMusic);
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
        if (Kitty != null)
        {
            Rigidbody2D charRB = Kitty.GetComponent<Rigidbody2D>();
            charRB.isKinematic = aPaused;
        }

        Rigidbody2D ballRB = Ball.GetComponent<Rigidbody2D>();

        if (ballRB.velocity.sqrMagnitude < m_BallVelocity.sqrMagnitude)
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

        AudioManager.Instance.StopMusic();

        AudioManager.Instance.PlayMusic(AudioManager.Instance.GameMusic);

        OnResetGame(true);

        m_TransitionOn = true;

    }

    public void OnGameWon()
    {
        
        m_TransitionOn = true;
        m_GameWonScr.enabled = true;
        PauseObjects(true);
        Ball.ResetBall();
        AudioManager.Instance.StopMusic();

        if (m_CurrentPassword != SECRET_WORD)
            m_CurrentPassword = SECRET_WORD.Substring(0,m_CurrentPassword.Length+1);

        m_Pasword.text = m_CurrentPassword;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.VictorySound);
    }

    public void OnResetGame(bool aResetTiles)
    {
        m_OptionsMenuScr.enabled = false;
        m_GameOverScr.enabled = false;
        m_GameWonScr.enabled = false;
        m_Lives = MAX_LIVES;
        m_LifeCounter.text = m_Lives.ToString();


        AudioManager.Instance.PlayMusic(AudioManager.Instance.GameMusic);
        

        StartCoroutine(getReady_cr(aResetTiles));
    }


    public void OnGameOver()
    {
        m_Lives--;
        m_LifeCounter.text = m_Lives.ToString();
        if (m_Lives == 0)
        {
            m_TransitionOn = true;
            m_GameOverScr.enabled = true;
            PauseObjects(true);
            AudioManager.Instance.StopMusic();

        }
        else
        {
            m_TransitionOn = true;
            StartCoroutine(getReady_cr(false));
        }

        if (m_Lives == 0)
        {
            m_Lives = MAX_LIVES;
        }
    }

    IEnumerator flashBG_cr(float time)
    {
        float t = 0;
        Color tint = m_TintBGColor;//new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        float numOFlashes = 1;

        while(t<time)
        {
            float x = t / time;
            float value = Mathf.Cos((x + Mathf.PI / 2) * 2 * numOFlashes * Mathf.PI) * 0.5f + 0.5f;

            m_Background.color = m_DefaultBGColor * (1 - value) + tint * value;
            
            t += Time.deltaTime;

            yield return null;
        }

        m_Background.color = m_DefaultBGColor;
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

        AudioManager.Instance.PlaySFX(AudioManager.Instance.KittyFallSound);

        while (Input.anyKeyDown == false)
            yield return null;

        m_ReadyScr.enabled = false;

        m_TransitionOn = false;

        PauseObjects(false);

        Ball.ResetBall();

        m_LifeCounter.text = m_Lives.ToString();
    }


}