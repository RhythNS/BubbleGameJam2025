using UnityEngine;

public class Level : MonoBehaviour
{
    public Rect Bounds => bounds;
    [SerializeField]
    private Rect bounds;

    public Vector2 Size => size;
    [SerializeField]
    private Vector2 size;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
