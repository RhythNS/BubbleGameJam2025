using UnityEngine;

[CreateAssetMenu(menuName = "Level/BannerStorage")]
public class BannerStorage : ScriptableObject
{
    public BackgroundObject Left => left;
    [SerializeField] private BackgroundObject left;

    public BackgroundObject Right => right;
    [SerializeField] private BackgroundObject right;
}
