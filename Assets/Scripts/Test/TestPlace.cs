using UnityEngine;

public class TestPlace : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // draw x on the position of the object
        Gizmos.DrawLine(transform.position + Vector3.left, transform.position + Vector3.right);
        Gizmos.DrawLine(transform.position + Vector3.down, transform.position + Vector3.up);
    }
}
