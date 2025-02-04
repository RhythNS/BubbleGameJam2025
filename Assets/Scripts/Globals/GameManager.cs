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

    public GradientBackground GradientBackground => gradientBackground;
    [SerializeField] private GradientBackground gradientBackground;

    public MusicHandler MusicHandler => musicHandler;
    [SerializeField] private MusicHandler musicHandler;

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
        MusicHandler.DoStart();

        // check for correct state
        player.GetComponent<PlayerStarter>().DoTheThing();
    }

    public void SwitchToGame()
    {
        player.Activate();
        levelLoader.Begin(player.transform);
    }

    public void FinishGame()
    {
        SwitchToGameOver1();
    }

    public void SwitchToGameOver()
    {
        // maybe death sounds or something
        MusicHandler.DoStop();
        SwitchToGameOver1();
    }

    public void SwitchToGameOver1()
    {
        // NO DEATH THINGS HERE
        player.Deactivate();
        FadeUI.Instance.FadeToBlack(SwitchToGameOver2);
    }

    private void SwitchToGameOver2()
    {
        gradientBackground.Underwater();
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
