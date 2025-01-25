using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private PlayerController player;

    [System.Serializable]
    public enum State { MainMenu, Game, GameOver, Win }
    [SerializeField] private State currentState;

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
        player.Deactivate();
    }

    public void RequestStart()
    {
        // check for correct state
        // play intro scene
        SwitchToGame();
    }

    public void SwitchToGame()
    {
        player.Activate();
    }

    public void SwitchToGameOver()
    {
        player.Deactivate();
    }
}
