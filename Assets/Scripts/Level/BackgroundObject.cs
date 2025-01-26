using UnityEngine;

[RequireComponent(typeof(ParallaxMover))]
public class BackgroundObject : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; private set; }

    public Rect SizeRect => sizeRect;
    [SerializeField] private Rect sizeRect = new Rect(0, 0, 10, 10);

    public Vector2 SpawnableRange => spawnableRange;
    [SerializeField]
    private Vector2 spawnableRange = new Vector2(-20, 20);

    public ParallaxMover ParallaxMover { get; private set; }

    private LevelLoader levelLoader;

    private void Awake()
    {
        ParallaxMover = GetComponent<ParallaxMover>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(LevelLoader loader)
    {
        levelLoader = loader;
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (levelLoader != null)
        {
            levelLoader.RemoveBackgroundObject(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, sizeRect.size);
    }
}
