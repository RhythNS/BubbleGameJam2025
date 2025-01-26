using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/Biome")]
public class Biome : ScriptableObject
{
    [SerializeField]
    private List<Level> levels;
    public List<Level> Levels => levels;

    public float Level1MinDistance => level1MinDistance;
    [SerializeField] private float level1MinDistance = 2;
    public int Level1Density => level1Density;
    [SerializeField] private int level1Density = 10;
    [SerializeField]
    private List<BackgroundObject> level1Objects;
    public List<BackgroundObject> Level1Objects => level1Objects;

    public float Level2MinDistance => level2MinDistance;
    [SerializeField] private float level2MinDistance = 2;
    public int Level2Density => level2Density;
    [SerializeField] private int level2Density = 10;
    [SerializeField]
    private List<BackgroundObject> level2Objects;
    public List<BackgroundObject> Level2Objects => level2Objects;

    public List<BannerStorage> BannerObjects => bannerObjects;
    [SerializeField]
    private List<BannerStorage> bannerObjects;

    public float GetHeight() => levels.Sum(level => level.Size.y);
}
