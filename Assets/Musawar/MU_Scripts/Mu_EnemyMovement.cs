using UnityEngine;

public class Mu_EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform enemyTransform;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float speed = 2f;

    [Header("Flip Sprites Only For 2 Waypoints")]
    [SerializeField] bool flip;

    //local
    private int currentWaypointIndex = 0;

    private void Update()
    {
        if (waypoints.Length == 0) return;

        Movement();
        NextPoint();
    }

    private void Movement()
    {
        enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);
    }

    private void NextPoint()
    {
        if (Vector3.Distance(enemyTransform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            FlipSprite();
        }
    }

    private void FlipSprite() //only applicable for a gameObject that has 2 way movement
    {
        if(!flip) { return; }
        if (currentWaypointIndex == 1)
        {
            enemyTransform.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            enemyTransform.GetComponent<SpriteRenderer>().flipX = false;

        }
    }
}


