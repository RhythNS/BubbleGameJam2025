using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Rect rect;

    [SerializeField] private int count;
    [SerializeField] private float minDistance;

    private void Start()
    {
        DoTheThing();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoTheThing();
        }
    }

    private void DoTheThing()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Vector2[] points = GetRandomPoints(count, minDistance);
        foreach (Vector2 point in points)
        {
            GameObject test = new GameObject("Point");
            test.transform.position = point;
            test.transform.SetParent(transform);
            test.AddComponent<TestPlace>();
        }
    }

    public Vector2[] GetRandomPoints(int count, float minDistance)
    {
        Vector2[] points = new Vector2[count];
        int emergencyExit = 0;
        for (int i = 0; i < count; i++)
        {
            Vector2 point = new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
            bool valid = true;
            foreach (Vector2 p in points)
            {
                if (Vector2.Distance(point, p) < minDistance)
                {
                    valid = false;
                    break;
                }
            }
            if (valid)
            {
                points[i] = point;
                emergencyExit = 0;
            }
            else
            {
                emergencyExit++;
                if (emergencyExit > 100)
                {
                    Debug.LogError("Emergency exit");
                    break;
                }
                i--;
            }
        }
        return points;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rect.center, rect.size);
    }
}
