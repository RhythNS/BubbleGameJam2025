using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Biome")]
public class Biome : ScriptableObject
{
    [SerializeField]
    private List<Level> levels;
    public List<Level> Levels => levels;

    [SerializeField]
    private List<BackgroundObject> backgroundObjects;
    public List<BackgroundObject> BackgroundObjects => backgroundObjects;

    public List<BannerStorage> BannerObjects => bannerObjects;
    [SerializeField]
    private List<BannerStorage> bannerObjects;

    public float GetHeight() => levels.Sum(level => level.Size.y);
}
