using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Transform toTrack;

    [SerializeField]
    private List<Biome> biomes;
    private int atBiome = 0;

    private List<Level> toLoadLevels = new List<Level>(); // These are prefabs
    private List<Level> currentLevels = new List<Level>(); // These are instantiated objects

    [SerializeField]
    private float levelChangeDistance = 20.0f;

    [SerializeField]
    private float levelDeleteDistance = 20.0f;

    [SerializeField] private float bannerChangeDistance = 20.0f;
    private List<BackgroundObject> backgroundObjects = new List<BackgroundObject>();
    private BackgroundObject lastBanner;
    private int atBanner;

    [SerializeField] private float bannerDistanceMultipler = 2;

    private bool nextBannerRightSide = false;

    public static LevelLoader Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Already anohter level loader");
        }
        Instance = this;
    }

    public float RemainingHeight => toLoadLevels.Sum(level => level.Size.y);


    private float gradThingMin = 0;
    private float gradThingMax = 0;
    [SerializeField] private float gradientStepper = 0.25f;

    public float GetThingForGradient()
    {
        if (enabled == false || atBiome == 0)
        {
            return 0;
        }

        float maxHeight = biomes.Sum(x => x.GetHeight());
        float minHeight = -5;
        //float min = Mathf.Max(gradThingMin, toTrack.position.y);
        //float thing = (gradientStepper * (atBiome - 1) + (gradientStepper * (toTrack.position.y - min) / (gradThingMax - min)));
        float gradient = (toTrack.position.y - minHeight) / (maxHeight - minHeight);
        //Debug.Log("gradient: " + gradient);
        return gradient;
    }

    public void Begin(Transform player)
    {
        enabled = true;
        toTrack = player;
        atBiome = 0;
        LoadNextLevel();
    }

    public void DeleteAllLevels()
    {
        foreach (Level level in currentLevels)
        {
            Destroy(level.gameObject);
        }
        currentLevels.Clear();
        toLoadLevels.Clear();
        lastBanner = null;


        foreach (BackgroundObject backgroundObject in backgroundObjects)
        {
            Destroy(backgroundObject.gameObject);
        }
        backgroundObjects.Clear();
    }

    public void DeleteAllBackgrounds()
    {

        foreach (BackgroundObject backgroundObject in backgroundObjects)
        {
            Destroy(backgroundObject.gameObject);
        }
        backgroundObjects.Clear();
    }

    private void LoadNextLevel()
    {
        if (toLoadLevels.Count == 0)
        {
            LoadNextBiome();
            return;
        }

        Level lastLevel = currentLevels.Count == 0 ? null : currentLevels[0];
        Level levelPrefab = toLoadLevels[0];

        Vector3 pos;
        if (lastLevel != null)
        {
            pos = lastLevel.transform.position + new Vector3(0, lastLevel.Size.y * 0.5f + levelPrefab.Size.y * 0.5f, 0);
        }
        else
        {
            pos = toTrack.position + new Vector3(0.0f, 20.0f, 0.0f);
        }

        gradThingMin = pos.y;
        gradThingMax = pos.y + biomes[atBiome - 1].GetHeight();

        Level createdLevel = Instantiate(levelPrefab, pos, Quaternion.identity);
        toLoadLevels.RemoveAt(0);
        currentLevels.Insert(0, createdLevel);
        CreateDensityBackground();
    }

    private void LoadNextBiome()
    {
        if (atBiome >= biomes.Count)
        {
            Debug.Log("No more biomes");
            enabled = false;
            return;
        }
        Debug.Log("Loading biome " + atBiome);

        RandomUtil.Shuffle(biomes[atBiome].BannerObjects);
        toLoadLevels.AddRange(biomes[atBiome].Levels);
        RandomUtil.Shuffle(toLoadLevels);
        atBiome++;

        atBanner = 0;

        LoadNextLevel();
    }

    private void Update()
    {
        if (toTrack == null)
        {
            return;
        }

        if (currentLevels.Count == 0)
        {
            return;
        }
        CheckLevels();
        CheckBackgroundObjects();
    }

    private void CheckLevels()
    {
        Level maxLevel = currentLevels.Count == 0 ? null : currentLevels[0];
        float maxLevelPosY = maxLevel.transform.position.y + maxLevel.Size.y;
        if (maxLevelPosY - toTrack.position.y < levelChangeDistance)
        {
            LoadNextLevel();
        }

        Level minLevel = currentLevels[currentLevels.Count - 1];
        float minLevelPosY = minLevel.transform.position.y + minLevel.Size.y;
        if (minLevelPosY - toTrack.position.y < -levelDeleteDistance)
        {
            currentLevels.RemoveAt(currentLevels.Count - 1);
            Destroy(minLevel.gameObject);
        }
    }

    private void CreateDensityBackground()
    {
        if (biomes[atBiome - 1].Level1Objects.Count > 0 && biomes[atBiome - 1].Level1Density != 0)
        {
            CreateBackgroundObjects(biomes[atBiome - 1].Level1Objects, Vector2.zero, biomes[atBiome - 1].Level1Density, biomes[atBiome - 1].Level1MinDistance, 1);
        }
        if (biomes[atBiome - 1].Level2Objects.Count > 0 && biomes[atBiome - 1].Level2Density != 0)
        {
            CreateBackgroundObjects(biomes[atBiome - 1].Level2Objects, Vector2.zero, biomes[atBiome - 1].Level2Density, biomes[atBiome - 1].Level2MinDistance, 2);
        }
    }

    private void CreateBackgroundObjects(List<BackgroundObject> objects, Vector2 position, int density, float minDistance, int level)
    {
        Vector2[] points = GetRandomPoints(density, minDistance,
            new Rect((Vector2)currentLevels[0].transform.position - currentLevels[0].Size, currentLevels[0].Size * 2));

        for (int i = 0; i < points.Length; i++)
        {
            BackgroundObject backgroundObject = Instantiate(RandomUtil.Element(objects), position + points[i], Quaternion.identity);
            backgroundObject.Init(this);
            backgroundObjects.Add(backgroundObject);
        }
    }

    public Vector2[] GetRandomPoints(int count, float minDistance, Rect rect)
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

    private void CheckBackgroundObjects()
    {
        if (biomes[atBiome - 1].BannerObjects.Count != 0)
        {
            TryCreateBanner();
        }
    }

    private void TryCreateBanner()
    {
        BackgroundObject nextBanner;
        if (nextBannerRightSide)
        {
            nextBanner = biomes[atBiome - 1].BannerObjects[atBanner].Right;
        }
        else
        {
            nextBanner = biomes[atBiome - 1].BannerObjects[atBanner].Left;
        }

        if (lastBanner != null)
        {
            if (RemainingHeight < nextBanner.SizeRect.y * bannerDistanceMultipler)
            {
                return;
            }
            if (lastBanner.transform.position.y + lastBanner.SizeRect.height - bannerChangeDistance > toTrack.position.y)
            {
                return;
            }
        }
        float toY;
        if (lastBanner != null)
        {
            toY = lastBanner.transform.position.y + lastBanner.SizeRect.height + Random.Range(5, 10);
        }
        else
        {
            toY = toTrack.position.y + 20.0f;
        }
        Vector3 pos = new Vector3(nextBanner.transform.position.x - 7.3f, toY, nextBanner.transform.position.z);
        BackgroundObject createdBanner = Instantiate(nextBanner, pos, Quaternion.identity);
        createdBanner.Init(this);
        backgroundObjects.Add(createdBanner);

        atBanner++;
        nextBannerRightSide = !nextBannerRightSide;
        if (atBanner >= biomes[atBiome - 1].BannerObjects.Count)
        {
            atBanner = 0;
        }

        lastBanner = createdBanner;
    }

    public void RemoveBackgroundObject(BackgroundObject backgroundObject)
    {
        backgroundObjects.Remove(backgroundObject);
    }
}
