using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    
    public PlayerController Player => player;
    [SerializeField] private PlayerController player;

    public Whale Whale => whale;
    [SerializeField] private Whale whale;

    [System.Serializable]
    public enum State { MainMenu, Game, GameOver, Win }
    [SerializeField] private State currentState;

    public LevelLoader LevelLoader => levelLoader;
    [SerializeField] private LevelLoader levelLoader;

    public ButtonCalls ButtonCalls => buttonCalls;
    [SerializeField] private ButtonCalls buttonCalls;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Instance already exists");
            Destroy(gameObject);

        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        player.transform.position = whale.StartLocation.position;
        player.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        FadeUI.Instance.FadeToClear(() => { });
    }

    public void RequestStart()
    {
        // check for correct state
        player.GetComponent<PlayerStarter>().DoTheThing();
        SwitchToGame();
    }

    public void SwitchToGame()
    {
        player.Activate();
        levelLoader.Begin(player.transform);
    }

    public void SwitchToGameOver()
    {
        player.Deactivate();
        FadeUI.Instance.FadeToBlack(SwitchToGameOver2);
    }

    private void SwitchToGameOver2()
    {
        levelLoader.DeleteAllLevels();
        // Move camera to whale
        player.transform.position = whale.StartLocation.position;
        player.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        FadeUI.Instance.FadeToClear(SwitchToGameOver3);
    }

    private void SwitchToGameOver3()
    {
        buttonCalls.OnClickMainMenu();
    }
}
