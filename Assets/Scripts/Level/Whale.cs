using UnityEngine;

public class Whale : MonoBehaviour
{
    public Transform StartLocation => startLocation;
    [SerializeField] private Transform startLocation;

    public Transform EndLocation => endLocation;
    [SerializeField] private Transform endLocation;
}
